using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelToolkit
{
    /// <summary>
    /// Represents raycast hit info for the voxel object
    /// </summary>
    public struct RaycastHit
    {
        /// <summary>
        /// The voxel coordinate that was hit
        /// </summary>
        public readonly int3 Location;
        
        /// <summary>
        /// The world position of the hit
        /// </summary>
        public readonly float3 WorldPoint;
        
        /// <summary>
        /// The info of the voxel that was hit
        /// </summary>
        public readonly Voxel Voxel;

        public RaycastHit(int3 location, float3 worldPoint, Voxel voxel)
        {
            Location = location;
            WorldPoint = worldPoint;
            Voxel = voxel;
        }
    }
    
    /// <summary>
    /// Holds part related info for setup
    /// </summary>
    public struct PartInfo
    {
        /// <summary>
        /// The descriptor of the mesh that part was created from
        /// </summary>
        public readonly MeshDescriptor MeshDescriptor;

        /// <summary>
        /// Creates new part info object
        /// </summary>
        /// <param name="meshDescriptor">The mesh descriptor used to generate the part</param>
        public PartInfo(MeshDescriptor meshDescriptor)
        {
            MeshDescriptor = meshDescriptor;
        }
    }
    
    /// <summary>
    /// Interface that lets user customize the part game object when dynamic voxel object stups it
    /// </summary>
    public interface IVoxelObjectPartSetupHandler
    {
        /// <summary>
        /// Called when the part is ready for the setup process
        /// </summary>
        /// <param name="target">The part game object</param>
        /// <param name="partInfo">The mesh descriptor that was used to create the voxel object part</param>
        void SetupVoxelObjectPart(GameObject target, PartInfo partInfo);
    }

    /// <summary>
    /// Interface that lets user customize the process of game object release
    /// </summary>
    public interface IVoxelObjectPartReleaseHandler
    {
        /// <summary>
        /// Called when the part is going to be released into object pool
        /// </summary>
        /// <param name="target">The object to be released</param>
        void OnVoxelObjectPartReleased(GameObject target);
    }
    
    [ExecuteAlways]
    public class DynamicVoxelObject : MonoBehaviour
    {
        private static List<IVoxelObjectPartSetupHandler> SetupHandlers = new List<IVoxelObjectPartSetupHandler>();
        private static List<IVoxelObjectPartReleaseHandler> ReleaseHandlers = new List<IVoxelObjectPartReleaseHandler>();
        private static UnityEngine.Material defaultOpaqueMaterial;
        private static UnityEngine.Material defaultTransparentMaterial;
        
        private static List<string> ObjectNames = new List<string>();

        private VoxelObject voxelObject;
        
        private Queue<GameObject> partsPool = new Queue<GameObject>();
        private List<GameObject> children = new List<GameObject>();

        [SerializeField] private IndexFormat indexFormat;
        [SerializeField] private UnityEngine.Material opaqueMaterial;
        [SerializeField] private UnityEngine.Material transparentMaterial;
        [SerializeField] private int3 size = new int3(64, 64, 64);
        [SerializeField] private int chunkSize = 16;
        [SerializeField] private float scale = 0.1f;

        /// <summary>
        /// The local bounds of the voxel object
        /// </summary>
        public Bounds LocalBounds => new Bounds(Vector3.zero, (float3)size * scale);

        /// <summary>
        /// The opaque material to be used for the meshes
        /// </summary>
        public UnityEngine.Material OpaqueMaterial
        {
            get => opaqueMaterial;
            set
            {
                opaqueMaterial = value; 
                OnValidate();
            }
        }

        /// <summary>
        /// The transparent material to be used for the meshes
        /// </summary>
        public UnityEngine.Material TransparentMaterial
        {
            get => transparentMaterial;
            set
            {
                transparentMaterial = value; 
                OnValidate();
            }
        }

        /// <summary>
        /// Gets/sets the index format used for the mesh
        /// </summary>
        public IndexFormat IndexFormat
        {
            get => indexFormat;
            set
            {
                if (indexFormat == value)
                    return;
                
                indexFormat = value;
                Resetup();
            }
        }

        /// <summary>
        /// Gets/sets the size of the voxel object which should be strictly larger than 0
        /// </summary>
        /// <exception cref="ArgumentException">Throws if one of the dimensions is smaller or equal to zero</exception>
        public int3 Size
        {
            get => size;
            set
            {
                if (math.all(size == value))
                    return;
                
                size = value;
                if (size.x <= 0 || size.y <= 0 || size.z <= 0)
                    throw new ArgumentException("Can't set size less than 1 on each dimension");
                
                OnValidate();
            }
        }

        /// <summary>
        /// The scale of the object
        /// </summary>
        public float Scale
        {
            get => scale;
            set
            {
                scale = value; 
                OnValidate();
            }
        }

        /// <summary>
        /// Provides/sets the chunk size which should be strictly larger than 4
        /// </summary>
        /// <exception cref="ArgumentException">Throws the exception if the chunk size is less than 4</exception>
        public int ChunkSize
        {
            get => chunkSize;
            set
            {
                if (chunkSize == value)
                    return;
                
                chunkSize = value;
                if (chunkSize < 4)
                    throw new ArgumentException("Can't set chunk size to be less than 4");
                
                OnValidate();
            }
        }
        
        /// <summary>
        /// Checks if the voxel position belongs to the voxel object volume
        /// </summary>
        /// <param name="value">The position to check</param>
        /// <returns>True if the position is inside the voxel object volume. False if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInVolume(Vector3Int value)
        {
            return voxelObject.IsInVolume(value);
        }

        /// <summary>
        /// Checks if the voxel position belongs to the voxel object volume
        /// </summary>
        /// <param name="value">The position to check</param>
        /// <returns>True if the position is inside the voxel object volume. False if not.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInVolume(int3 value)
        {
            return voxelObject.IsInVolume(value);
        }

        /// <summary>
        /// Sets/gets the voxel at/from a given position
        /// </summary>
        /// <param name="position">The position of the voxel to be gotten/set</param>
        public Voxel this[Vector3Int position]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => voxelObject[position];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => voxelObject[position] = value;
        }
        
        /// <summary>
        /// Sets/gets the voxel at/from a given position
        /// </summary>
        /// <param name="position">The position of the voxel to be gotten/set</param>
        public Voxel this[int3 position]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => voxelObject[position];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => voxelObject[position] = value;
        }

        /// <summary>
        /// Changes the material at a given index
        /// </summary>
        /// <param name="index">The index the material has to be set</param>
        /// <param name="material">The material to be set</param>
        public void SetMaterial(byte index, Material material)
        {
            voxelObject.SetMaterial(index, material);
        }

        private void Awake()
        {
            Resetup();
            
            for (var index = 0; index < 256; index++)
                voxelObject.SetMaterial((byte)index, Material.Base);
        }
        
#if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= OnDestroy;
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += OnDestroy;
        }
#endif

        /// <summary>
        /// Clears the voxel object
        /// </summary>
        public void Clear()
        {
            if (voxelObject == null)
                return;
            
            voxelObject.DeleteMeshes();
            for (var x = 0; x < size.x; x++)
                for (var y = 0; y < size.y; y++)
                    for (var z = 0; z < size.z; z++)
                        voxelObject[new Vector3Int(x, y, z)] = new Voxel();
        }

        /// <summary>
        /// Finds an intersection of the ray and the voxel object
        /// </summary>
        /// <param name="ray">The ray to be casted</param>
        /// <param name="hit">The hit result of the raycast</param>
        /// <returns>Returns true if raycast hit ended up hitting the voxel object</returns>
        public bool Raycast(Ray ray, out RaycastHit hit)
        {
            hit = new RaycastHit();
            var direction = transform.InverseTransformDirection(ray.direction);
            var position = transform.InverseTransformPoint(ray.origin);
            var scaledSize = (float3)size;
            var bounds = new Bounds(-scaledSize * 0.5f, scaledSize);
            var localRay = new Ray(position, direction);
            
            if (!bounds.IntersectRay(localRay, out float distance))
                return false;
            
            position = localRay.GetPoint(distance) + localRay.direction * 0.001f;
            localRay = new Ray(position, direction);

            var iterationsLeft = 1024;
            var one = (float3)1.0f;
            var half = one * 0.5f;
            while (iterationsLeft-- > 0 && bounds.Contains(localRay.origin))
            {
                var currentVoxel = (int3)math.round((float3)localRay.origin + size - half);
                var voxel = this[currentVoxel];
                var voxelBounds = new Bounds((currentVoxel - size) + half, one);

                if (voxel.VoxelKind == VoxelKind.Solid)
                {
                    hit = new RaycastHit(currentVoxel, localRay.origin - localRay.direction * 0.001f, voxel);
                    return true;
                }
                
                var castRay = new Ray(localRay.origin + localRay.direction * 5.0f, -localRay.direction);
                if (!voxelBounds.IntersectRay(castRay, out float cellDistance))
                    break;

                localRay.origin = castRay.GetPoint(cellDistance) + localRay.direction * 0.001f;
            }

            return false;
        }

        /// <summary>
        /// Setting up the dynamic voxel object from the model prototype
        /// </summary>
        /// <param name="model">The model to setup from</param>
        /// <param name="palette">The palette to use to setup the voxel object</param>
        public void SetupFromModel(Model model, ReadonlyArray<Material> palette)
        {
            Resetup();
            if (voxelObject != null)
                voxelObject.Dispose();
            
            size = new int3(model.Size.x, model.Size.y, model.Size.z);
            voxelObject = VoxelObject.CreateFromModel(model, palette, chunkSize);
            voxelObject.Scale = scale;
        }

        /// <summary>
        /// Transforms world point to voxel coordinates
        /// </summary>
        /// <param name="worldPosition">The world position to transform</param>
        /// <returns>Voxel coordinate</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3Int TransformWorldToVoxel(Vector3 worldPosition)
        {
            var local = transform.InverseTransformPoint(worldPosition);
            return Vector3Int.CeilToInt(local + (Vector3)(float3)size);
        }

        /// <summary>
        /// Transforms world point to voxel coordinates
        /// </summary>
        /// <param name="worldPosition">The world position to transform</param>
        /// <returns>Voxel coordinate</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3Int TransformWorldToVoxel(float3 worldPosition)
        {
            var local = transform.InverseTransformPoint(worldPosition);
            return Vector3Int.CeilToInt(local + (Vector3)(float3)size);
        }
        
        /// <summary>
        /// Transforms voxel point to world position
        /// </summary>
        /// <param name="voxelPosition">The position of the voxel to transform</param>
        /// <returns>World coordinates</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 TransformVoxelToWorld(Vector3Int voxelPosition)
        {
            return transform.TransformPoint((voxelPosition - (Vector3)(float3)size) + Vector3.one * 0.5f);
        }
        
        /// <summary>
        /// Transforms voxel point to world position
        /// </summary>
        /// <param name="voxelPosition">The position of the voxel to transform</param>
        /// <returns>World coordinates</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 TransformVoxelToWorld(int3 voxelPosition)
        {
            return transform.TransformPoint((voxelPosition - size) + (float3)0.5f);
        }

        /// <summary>
        /// Replaces the sphere of the voxels at a given position
        /// </summary>
        /// <param name="position">The center position of the sphere</param>
        /// <param name="radius">The radius of the sphere in voxels</param>
        /// <param name="voxel">The voxel prototype to fill the sphere with</param>
        public void SetSphere(Vector3Int position, int radius, Voxel voxel)
        {
            var min = Vector3Int.Max(Vector3Int.zero, position - Vector3Int.one * radius);
            var max = Vector3Int.Min(Vector3Int.one * new Vector3Int(size.x, size.y, size.z), position + Vector3Int.one * radius);

            var radiusSquared = radius * radius;
            for (var x = min.x; x < max.x; x++)
            {
                for (var y = min.y; y < max.y; y++)
                {
                    for (var z = min.z; z < max.z; z++)
                    {
                        var current = new Vector3Int(x, y, z);
                        var difference = position - current;
                        var squaredDistance = 
                            difference.x * difference.x + 
                            difference.y * difference.y +
                            difference.z * difference.z;
                        
                        if (squaredDistance > radiusSquared)
                            continue;

                        voxelObject[current] = voxel;
                    }
                }
            }
        }

        private void Resetup()
        {
            var newVoxelObject = new VoxelObject(new Vector3Int(size.x, size.y, size.z), chunkSize);
            if (voxelObject != null)
            {
                voxelObject.CopyTo(newVoxelObject);
                voxelObject.Dispose();
            }

            voxelObject = newVoxelObject;
            voxelObject.Scale = scale;
        }

        private void UpdateVoxelObject()
        {
            if (voxelObject == null)
                return;
            
            if (defaultOpaqueMaterial == null)
                defaultOpaqueMaterial = Resources.Load<UnityEngine.Material>($"{PathUtility.GetMaterialPath()}/Opaque");
            
            if (defaultTransparentMaterial == null)
                defaultTransparentMaterial = Resources.Load<UnityEngine.Material>($"{PathUtility.GetMaterialPath()}/Transparent");
            
            voxelObject.UpdateMeshes(indexFormat, -(float3)voxelObject.Size * voxelObject.Scale);
            
            children.RemoveAll(x => x == null);

            foreach (var child in children)
                partsPool.Enqueue(child);

            for (var index = voxelObject.MeshesCount; index < children.Count; index++)
            {
                ReleaseHandlers.Clear();
                foreach (var releaseHandler in ReleaseHandlers)
                    releaseHandler.OnVoxelObjectPartReleased(children[index]);
                
                children[index].gameObject.SetActive(false);
            }

            children.Clear();

            for (var index = 0; index < voxelObject.MeshesCount; index++)
            {
                var mesh = voxelObject.GetMesh(index);
                
                if (ObjectNames.Count <= index)
                    ObjectNames.Add($"Part {index}");

                var go = partsPool.Count == 0 ? new GameObject() : partsPool.Dequeue();
                go.hideFlags = HideFlags.DontSave;
                go.SetActive(true);
                go.name = ObjectNames[index];
                var filter = go.GetComponent<MeshFilter>();
                if (filter == null)
                    filter = go.AddComponent<MeshFilter>();
                
                filter.mesh = mesh.Mesh;

                var renderer = go.GetComponent<MeshRenderer>();
                if (renderer == null)
                    renderer = go.AddComponent<MeshRenderer>();
                
                children.Add(go);
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;

                renderer.sharedMaterial = mesh.Kind == MeshKind.Opaque ? 
                    (opaqueMaterial == null ? defaultOpaqueMaterial : opaqueMaterial) :
                    (transparentMaterial == null ? defaultTransparentMaterial : transparentMaterial);
                
                SetupHandlers.Clear();
                GetComponents(SetupHandlers);
                foreach (var setupHandler in SetupHandlers)
                    setupHandler.SetupVoxelObjectPart(go, new PartInfo(mesh));
            }
        }

        protected virtual void Update()
        {
            UpdateVoxelObject();
        }

        protected virtual void OnValidate()
        {
            if (size.x < 1)
                size.x = 1;

            if (size.y < 1)
                size.y = 1;

            if (size.z < 1)
                size.z = 1;

            if (chunkSize < 4)
                chunkSize = 4;
            
            if (Application.isPlaying)
                Resetup();
        }
        
        private void OnDestroy()
        {
            foreach (var child in children)
                DestroyImmediate(child);
            
            children.Clear();
            
            if (voxelObject != null)
                voxelObject.Dispose();

            voxelObject = null;
        }
    }
}