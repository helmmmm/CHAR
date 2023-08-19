using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelToolkit
{
    public struct ReadonlyArray<T>
    {
        private T[] values;

        public T[] ToArray()
        {
            var result = new T[values.Length];
            Array.Copy(values, result, result.Length);

            return result;
        }
        
        public T this[int index]
        {
            get => values[index];
        }

        public int Length => values.Length;

        public ReadonlyArray(T[] values)
        {
            this.values = values;
        }
        
        public static implicit operator ReadonlyArray<T>(T[] other)
        {
            return new ReadonlyArray<T>(other);
        }
    }
    
    /// <summary>
    /// Represents the voxel asset
    /// </summary>
    [PreferBinarySerialization]
    public class VoxelAsset : ScriptableObject
    {
        [SerializeField] private string version;
        [SerializeField] private string importSource;
        [SerializeField] private HierarchyNode hierarchyRoot;
        [SerializeField] private List<Model> models = new List<Model>();
        [SerializeField] private List<Layer> layers = new List<Layer>();
        [SerializeField] private Material[] palette = new Material[256];

        /// <summary>
        /// Allows access to the palette of the asset
        /// </summary>
        public ReadonlyArray<Material> Palette => new ReadonlyArray<Material>(palette);

        /// <summary>
        /// The root object of the hierarchy
        /// </summary>
        public HierarchyNode HierarchyRoot
        {
            get => hierarchyRoot;
            set => hierarchyRoot = value;
        }
        
        /// <summary>
        /// The version of the asset
        /// </summary>
        public string Version
        {
            get => version;
            set => version = value;
        }
        
        /// <summary>
        /// The way the asset was imported
        /// </summary>
        public string InputSource
        {
            get => importSource;
            set => importSource = value;
        }

        /// <summary>
        /// The count of the models contained
        /// </summary>
        public int ModelsCount => models.Count;

        /// <summary>
        /// The count of layers in the asset
        /// </summary>
        public int LayersCount => layers.Count;
        
        /// <summary>
        /// Sets the material into the palette
        /// </summary>
        /// <param name="index">The index of the materials to be set</param>
        /// <param name="material">The material to be set</param>
        /// <exception cref="Exception">Throws exception if the index is less than 0 or more than 255</exception>
        public void SetPaletteMaterial(int index, Material material)
        {
            if (index < 0 || index > 255)
                throw new Exception("Max palette size is 256");

            palette[index] = material;
        }

        /// <summary>
        /// Returns the material from the palette
        /// </summary>
        /// <param name="index">The index of the material</param>
        /// <returns>Material with the index</returns>
        /// <exception cref="Exception">Throws exception if the index is less than 0 or more than 255</exception>
        public Material GetPaletteMaterial(int index)
        {
            if (index < 0 || index > 255)
                throw new Exception("Max palette size is 256");

            return palette[index];
        }

        /// <summary>
        /// Adds the layer to the asset
        /// </summary>
        /// <param name="layer">The layer to be added</param>
        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }

        /// <summary>
        /// Returns a layer by it's ID
        /// </summary>
        /// <param name="id">The ID of the layer to be found</param>
        /// <returns>The layer with a specified ID</returns>
        public Layer FindLayerById(int id)
        {
            return layers.Find(x => x.ID == id);
        }

        /// <summary>
        /// Returns the layer by its index
        /// </summary>
        /// <param name="index">The index of the layer to return</param>
        /// <returns></returns>
        public Layer GetLayer(int index)
        {
            return layers[index];
        }
        
        /// <summary>
        /// Adds a model to a list of models
        /// </summary>
        /// <param name="model">The model to be added</param>
        public void AddModel(Model model)
        {
            if (model.ParentAsset != this)
                throw new Exception("Can't add the model because it belongs to another asset");
            
            models.Add(model);
        }

        /// <summary>
        /// Finds a model by it's ID
        /// </summary>
        /// <param name="id">The ID of model to be found</param>
        /// <returns>The model with a specified ID</returns>
        public Model FindModelById(int id) => models.Find(x => x.ID == id);
        
        /// <summary>
        /// Returns a model with an index
        /// </summary>
        /// <param name="index">The index of the model</param>
        /// <returns>The model located at the specified index</returns>
        public Model GetModel(int index) => models[index];
    }
}