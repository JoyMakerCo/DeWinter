using System;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace UGraph
{
    [Serializable]
    public class DirectedGraph : IDisposable
    {
        public Vector2Int[] Links;

        public DirectedGraph()
        {
            Links = new Vector2Int[0];
        }

        public DirectedGraph(DirectedGraph graph)
        {
            Links = DeepCopy(graph.Links);
        }

        public int Link(int fromIndex, int toIndex)
        {
            int index = GetLinkIndex(fromIndex, toIndex);
            if (index >= 0) return index;
            List<Vector2Int> list = new List<Vector2Int>(Links);
            list.Add(new Vector2Int(fromIndex, toIndex));
            Links = list.ToArray();
            return Links.Length - 1;
        }

        public bool Unlink(int fromIndex, int toIndex)
        {
            int index = GetLinkIndex(fromIndex, toIndex);
            if (index < 0) return false;
            List<Vector2Int> list = new List<Vector2Int>(Links);
            list.RemoveAt(index);
            Links = list.ToArray();
            return true;
        }

        public int[] GetNeighbors(int node)
        {
            List<int> result = new List<int>();
            foreach(Vector2Int link in Links)
            {
                if (link.x == node)
                {
                    result.Add(link.y);
                }
            }
            return result.ToArray();
        }

        public int GetLinkIndex(int fromIndex, int toIndex)
        {
            return Array.FindIndex(Links, l => l.x == fromIndex && l.y == toIndex);
        }

        public virtual void DeleteNode(int nodeIndex)
        {
            if (Links == null) return;

            List<Vector2Int> links = new List<Vector2Int>();
            foreach(Vector2Int link in Links)
            {
                if (link.x != nodeIndex && link.y != nodeIndex)
                {
                    links.Add(link);
                }
            }
            Links = links.ToArray();
        }

        protected K[] DeepCopy<K>(K[] array)
        {
            if (array == null) return new K[0];
            K[] result = new K[array.Length];
            Array.Copy(array, result, array.Length);
            return result;
        }

        public virtual void Dispose()
        {
            Links = null;
        }
    }

    [Serializable]
    public class DirectedGraph<T> : DirectedGraph
	{
        public T[] Nodes;

        public DirectedGraph() : base()
        {
            Nodes = new T[0];
        }

        // Copy Constructor
        public DirectedGraph(DirectedGraph<T> graph) : base (graph as DirectedGraph)
        {
            Nodes = DeepCopy(graph.Nodes);
        }

        public int Link(T from, T to)
        {
            return Link(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
        }

        public bool Unlink(T from, T to)
        {
            return Unlink(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
        }

        public int GetNodeIndex(T node)
        {
            return Nodes != null ? Array.IndexOf(Nodes, node) : -1;
        }

		public T[] GetNeighbors(T node)
		{
            int index = Array.IndexOf(Nodes, node);
            List<T> result = new List<T>();
            foreach (Vector2Int link in Links)
            {
                if (link.x == index)
                {
                    result.Add(Nodes[link.y]);
                }
            }
            return result.ToArray();
		}

		public int GetLinkIndex(T from, T to)
		{
			return GetLinkIndex(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
		}

        public override void DeleteNode(int nodeIndex)
        {
            if (Nodes != null && nodeIndex >= 0 && nodeIndex < Nodes.Length)
            {
                if (Nodes[nodeIndex] is IDisposable)
                    ((IDisposable)Nodes[nodeIndex]).Dispose();

                List<T> nodes = new List<T>(Nodes);
                nodes.RemoveAt(nodeIndex);
                Nodes = nodes.ToArray();
                base.DeleteNode(nodeIndex);
            }
        }

        public virtual void DeleteNode(T node)
        {
            if (Nodes != null)
            {
                int index = Array.IndexOf(Nodes, node);
                DeleteNode(index);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Nodes != null)
            {
                foreach (T node in Nodes)
                {
                    (node as IDisposable)?.Dispose();
                }
                Nodes = null;
            }
        }
    }

    [Serializable]
    public class DirectedGraph<T, U> : DirectedGraph<T>
    {
        public U[] LinkData;

        public DirectedGraph() : base()
        {
            LinkData = new U[0];
        }

        public DirectedGraph(DirectedGraph<T, U> graph) : base(graph as DirectedGraph<T>)
        {
            LinkData = DeepCopy(graph.LinkData);
        }

		public U[] GetLinks(T node) => GetLinks(Array.IndexOf(Nodes, node));

        public U[] GetLinks(int nodeIndex)
        {
            if (LinkData == null) return new U[0];

            List<U> result = new List<U>();
            for(int i=0;  i<Links.Length; i++)
            {
                if (Links[i].x == nodeIndex && i<LinkData.Length)
                {
                    result.Add(LinkData[i]);
                }
            }
            return result.ToArray();
        }

		public U GetLink(T from, T to) => GetLink(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));

		public U GetLink(int fromIndex, int toIndex)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			return (index >= 0) ? LinkData[index] : default(U);
		}

		public int Link(T from, T to, U linkData)
		{
			return Link(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to), linkData);
		}

        public T GetNextNode(U linkData)
        {
            if (LinkData == null || Links == null || Nodes == null) return default;
            int index = Array.IndexOf(LinkData, linkData);
            if (index < 0 || index >= Links.Length) return default;
            return Links[index].y < Nodes.Length ? Nodes[Links[index].y] : default;
        }

        public int Link(int fromIndex, int toIndex, U linkData)
		{
			int index = GetLinkIndex(fromIndex, toIndex);
			if (index >= 0)
			{
				LinkData[index] = linkData;
				return index;
			}

            List<U> links = new List<U>(LinkData);
            index = base.Link(fromIndex, toIndex);
            links.Insert(index, linkData);
            LinkData = links.ToArray();
			return index;
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

        public override void DeleteNode(int nodeIndex)
        {
            if (Links != null && LinkData != null)
            {
                List<U> linkData = new List<U>();
                for (int i = Math.Min(Links.Length, LinkData.Length) - 1; i >= 0; i--)
                {
                    if (Links[i].x != nodeIndex && Links[i].y != nodeIndex)
                    {
                        linkData.Add(LinkData[i]);
                    }
                    else if (LinkData[i] is IDisposable)
                    {
                        (LinkData[i] as IDisposable).Dispose();
                    }
                }
                LinkData = linkData.ToArray();
            }
            base.DeleteNode(nodeIndex);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (LinkData != null)
            {
                foreach(U link in LinkData)
                {
                    (link as IDisposable)?.Dispose();
                }
                LinkData = null;
            }
        }
    }
}
