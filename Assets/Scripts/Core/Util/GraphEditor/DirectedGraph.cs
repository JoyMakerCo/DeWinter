using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Util
{
	[Serializable]
	public class DirectedGraph<T>
	{
		public DirectedGraph()
		{
			Nodes = new T[0];
			Links = new Vector2Int[0];
		}

		public T[] Nodes = new T[0];
		public Vector2Int[] Links = new Vector2Int[0];
		public int Link(T from, T to)
		{
			return Link(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}
		public int Link(int fromIndex, int toIndex)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			if (index >= 0) return index;

			if (fromIndex < 0 || fromIndex >= Nodes.Length)
				throw new System.ArgumentException("Error: Node index is out of range", "fromIndex");
			if (toIndex < 0 || toIndex >= Nodes.Length)
				throw new System.ArgumentException("Error: Node index is out of range", "toIndex");

			Links = Links.Append(new Vector2Int(fromIndex, toIndex)).ToArray();
			return Links.Length-1;
		}
		public bool Unlink(T from, T to)
		{
			return Unlink(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}
		public bool Unlink(int fromIndex, int toIndex)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			if (index >= 0) Links = Links.Take(index).Concat(Links.Skip(index+1)).ToArray();
			return index >= 0;
		}
		public T[] GetNeighbors(T node)
		{
			int index = Array.IndexOf(Nodes, node);
			return index < 0 ? null : (
				from l in Links
				where l.x == index
				select Nodes[l.y]).ToArray();
		}

		public int GetLinkIndex(T from, T to)
		{
			return GetLinkIndex(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}

		public int GetLinkIndex(int fromIndex, int toIndex)
		{
			return Array.FindIndex(Links, l=>l.x == fromIndex && l.y == toIndex);
		}
	}

	[Serializable]
	public class DirectedGraph<T,U> : DirectedGraph<T>
	{
		public DirectedGraph() : base()
		{
			LinkData = new U[0];
		}

		public U[] LinkData;
		public U[] GetLinkData(T node)
		{
			int index = Array.IndexOf(Nodes, node);
			return GetLinkData(index);
		}

		public U[] GetLinkData(int nodeIndex)
		{
			List<U> result = new List<U>();
             for (int i=0; i<Links.Length; i++)
			{
				if (Links[i].x == nodeIndex)
                    result.Add(LinkData[i]);
			}
			return result.ToArray();
		}

		public U GetLinkData(T from, T to)
		{
			return GetLinkData(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}

		public U GetLinkData(int fromIndex, int toIndex)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			return (index >= 0) ? LinkData[index] : default(U);
		}

		public int Link(T from, T to, U linkData)
		{
			return Link(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to), linkData);
		}

		public int Link(int fromIndex, int toIndex, U linkData)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			if (index >= 0)
			{
				LinkData[index] = linkData;
				return index;
			}
			if (fromIndex < 0 || fromIndex >= Nodes.Length)
				throw new System.ArgumentException("Error: Index is out of range", "fromIndex");
			if (toIndex < 0 || toIndex >= Nodes.Length)
				throw new System.ArgumentException("Error: Index is out of range", "toIndex");

			Vector2Int link = new Vector2Int(fromIndex, toIndex);
			Links = Links.Append(link).ToArray();
			LinkData = LinkData.Append(linkData).ToArray();
			return LinkData.Length-1;
		}

		public U ExtractLinkData(T from, T to)
		{
			return ExtractLinkData(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}

		public U ExtractLinkData(int fromIndex, int toIndex)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			if (index >= 0)
			{
				U result = LinkData[index];
				Unlink(fromIndex, toIndex);
				return result;
			}
			return default(U);
		}
	}
}
