using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
	[Serializable]
	public class DirectedGraphLink
	{
		public int Origin;
		public int Target;
		public DirectedGraphLink() {}
		public DirectedGraphLink(DirectedGraphLink link)
		{
			Origin = link.Origin;
			Target = link.Target;
		}
	}

	public class DirectedGraph<T,U> where U:DirectedGraphLink, new()
	{
		public T[] Nodes;
		public U[] Links;

		public T[] GetNeighbors(T node)
		{
			int index = Array.IndexOf(Nodes, node);
			return index < 0 ? new T[0] : (
				from l in Links
				where l.Origin == index
				select Nodes[l.Target]).ToArray();
		}

		public U[] GetLinks(T node)
		{
			int index = Array.IndexOf(Nodes, node);
			return GetLinks(index);
		}

		public U[] GetLinks(int nodeIndex)
		{
			return (from l in Links
				where l.Origin == nodeIndex
				select l).ToArray();
		}

		public U Link(T origin, T target)
		{
			int l0 = Array.IndexOf(Nodes, origin);
			int l1 = Array.IndexOf(Nodes, target);
			return Link(l0, l1);
		}

		public U Link(int origin, int target)
		{
			if (origin < 0 || target < 0
				|| origin >= Nodes.Length
				|| target >= Nodes.Length) return null;
			U link = new U();
			link.Origin = origin;
			link.Target = target;
			Links.Append(link);
			return link;
		}
	}

	public class DirectedGraph<T> : DirectedGraph<T, DirectedGraphLink> {}
}
