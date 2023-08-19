using System.Collections.Generic;
using UnityEngine;

namespace VoxelToolkit.MagicaVoxel
{
	public class GroupElement : HierarchyElement
	{
		public int[] ChildrenIds;

		public HierarchyElement[] Children;

		public GroupElement(int id, Dictionary<string, string> attributes, int[] children) : base(id, attributes)
		{
			ChildrenIds = children;
		}

		public override HierarchyNode AddToAsset(VoxelAsset asset)
		{
			var group = ScriptableObject.CreateInstance<Group>();
			group.name = $"Group {Id}";

			foreach (var hierarchyElement in Children)
				group.AddChild(hierarchyElement.AddToAsset(asset));

			return group;
		}
	}
}