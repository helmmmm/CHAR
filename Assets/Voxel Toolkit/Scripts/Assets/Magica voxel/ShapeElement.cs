using System.Collections.Generic;
using UnityEngine;

namespace VoxelToolkit.MagicaVoxel
{
	public class ShapeElement : HierarchyElement
	{
		public ModelReference[] Models;

		public ShapeElement(int id, Dictionary<string, string> attributes, ModelReference[] models) : base(id, attributes)
		{
			Models = models;
		}

		public override HierarchyNode AddToAsset(VoxelAsset asset)
		{
			var shape = ScriptableObject.CreateInstance<Shape>();
			shape.name = $"Shape {Id}";

			foreach (var modelReference in Models)
				shape.AddModel(asset.FindModelById(modelReference.Id));

			return shape;
		}
	}
}