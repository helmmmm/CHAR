using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;

namespace VoxelToolkit.Editor
{
    public enum GenerationMode
    {
        Everything,
        EssentialOnly,
        DataOnly
    }

    public abstract class VoxelImporter : ScriptedImporter
    {
        [SerializeField] private float opaqueEdgeShift = 0.0f;
        [SerializeField] private float transparentEdgeShift = 0.0f;
        [SerializeField] private float scale = 0.1f;
        [SerializeField] private IndexFormat indexFormat = IndexFormat.UInt16;
        [SerializeField] private bool generateLightmapUV = false;
        [SerializeField] private bool generateColliders = true;
        [SerializeField] private int chunkSize = 16;
        [SerializeField] private OriginMode originMode;
        [SerializeField] private bool reduceHierarchy = true;
        [SerializeField] private GenerationMode generationMode = GenerationMode.EssentialOnly;
        [HideInInspector][SerializeField] private bool overrideAssetMaterials = false;
        [HideInInspector][SerializeField] private Material[] overrideMaterials = new Material[256];

        private static Dictionary<string, UnityEngine.Material> cachedMaterials = new Dictionary<string, UnityEngine.Material>();

        protected static string ConvertProjectPathToSystemPath(string projectPath)
        {
            var dataPath = Application.dataPath;
            var projectFolder = dataPath.Substring(0, dataPath.Length - "Assets".Length);

            return Path.Combine(projectFolder, projectPath);
        }

        private void Reset()
        {
            for (var index = 0; index < overrideMaterials.Length; index++)
                overrideMaterials[index] = Material.Base;
        }

        private static UnityEngine.Material FindMaterial(string name)
        {
            var replacedName = name.Replace('/', '\\');
            if (cachedMaterials.TryGetValue(replacedName, out UnityEngine.Material material))
                return material;

            var materials = AssetDatabase.FindAssets("t:Material").ToList();
            var found = materials.FindAll(x =>
                                          {
                                                var path  = AssetDatabase.GUIDToAssetPath(x).Replace('/', '\\');
                                                return path.EndsWith(replacedName, StringComparison.Ordinal);
                                          });
            
            if (found.Count == 0)
                Debug.LogError($"No material found for name '{replacedName}'");
            else if (found.Count > 1)
                Debug.LogWarning($"More than one material found for name '{replacedName}'");

            material = found.Count == 0 ? null : AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(found[0]);
            if (found.Count == 1)
                cachedMaterials.Add(replacedName, material);
            
            return material;
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var asset = ImportAsset(ctx);

            ctx.AddObjectToAsset("Voxel data", asset);

            if (overrideAssetMaterials)
            {
                for (var index = 0; index < 256; index++)
                    asset.SetPaletteMaterial(index, overrideMaterials[index]);
            }

            if (generationMode != GenerationMode.DataOnly)
            {
                var gameObjectBuilder = new GameObjectBuilder();

                gameObjectBuilder.OpaqueMaterial = FindMaterial($"{PathUtility.GetMaterialPath()}/VoxelToolkitDefaultOpaque.mat");
                gameObjectBuilder.TransparentMaterial = FindMaterial($"{PathUtility.GetMaterialPath()}/VoxelToolkitDefaultTransparent.mat");

                gameObjectBuilder.Scale = scale;
                gameObjectBuilder.ReduceHierarchy = reduceHierarchy;
                gameObjectBuilder.ChunkSize = chunkSize;
                gameObjectBuilder.IndexFormat = indexFormat;
                gameObjectBuilder.GenerateColliders = generateColliders;
                gameObjectBuilder.OriginMode = originMode;
                gameObjectBuilder.OpaqueEdgeShift = opaqueEdgeShift;
                gameObjectBuilder.TransparentEdgeShift = transparentEdgeShift;
                gameObjectBuilder.GenerateLightmapUV = generateLightmapUV;

                var gameObject = gameObjectBuilder.CreateGameObject(asset);
                ctx.AddObjectToAsset(gameObject.name, gameObject);
                ctx.SetMainObject(gameObject);

                var filters = gameObject.GetComponentsInChildren<MeshFilter>(true);
                foreach (var meshFilter in filters)
                {
                    if (meshFilter.sharedMesh == null)
                        continue;
                    
                    var mesh = meshFilter.sharedMesh;
                    ctx.AddObjectToAsset(mesh.name, mesh);
                }
            }
            
            if (generationMode == GenerationMode.EssentialOnly)
                return;

            for (var index = 0; index < asset.ModelsCount; index++)
                ctx.AddObjectToAsset(asset.GetModel(index).name, asset.GetModel(index));
            
            for (var index = 0; index < asset.LayersCount; index++)
                ctx.AddObjectToAsset(asset.GetLayer(index).name, asset.GetLayer(index));

            AddRelatedObjectsToContext(ctx, asset.HierarchyRoot);
        }

        private void AddRelatedObjectsToContext(AssetImportContext context, HierarchyNode node)
        {
            foreach (var nodeRelatedObject in node.RelatedObjects)
            {
                context.AddObjectToAsset(nodeRelatedObject.name, nodeRelatedObject);
                if (nodeRelatedObject is HierarchyNode hierarchyNode)
                    AddRelatedObjectsToContext(context, hierarchyNode);
            }
        }

        protected abstract VoxelAsset ImportAsset(AssetImportContext ctx);
    }
}