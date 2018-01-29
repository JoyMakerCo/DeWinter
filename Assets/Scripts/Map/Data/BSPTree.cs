using System;

namespace Ambition
{
	public class BSPTree
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;
		public BSPTree[] Children; // Always two.
		public BSPTree Parent;

		public BSPTree()
		{
			X=Y=Width=Height=0f;
			Children = null;
		}

		public BSPTree(float x, float y, float width, float height)
		{
			X=x;
			Y=y;
			Width = width;
			Height = height;
			Children = null;
		}

		public BSPTree Split(float percent, bool horizontal)
		{
			if (Children != null) return this;
			if (horizontal)
			{
				Children = new BSPTree[]{
					new BSPTree(X, Y, Width, Height*percent),
					new BSPTree(X, Y + Height*percent, Width, Height*(1f-percent)) };
			}
			else
			{
				Children = new BSPTree[]{
					new BSPTree(X, Y, Width*percent, Height),
					new BSPTree(X + Width*percent, Y, Width*(1f-percent), Height)};
			}
			Children[0].Parent = this;
			Children[1].Parent = this;
			return this;
		}

		public BSPTree Root
		{
			get
			{
				BSPTree tree = this;
				while (tree.Parent != null)
					tree = tree.Parent;
				return tree;
			}
		}

		public bool IsLeaf
		{
			get { return Children == null; }
		}
	}
}
