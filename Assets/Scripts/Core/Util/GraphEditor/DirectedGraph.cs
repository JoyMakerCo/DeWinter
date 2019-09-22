using System;
using System.Collections.Generic;
using System.Linq;
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
            Links = Links.Append(new Vector2Int(fromIndex, toIndex)).ToArray();
            return Links.Length - 1;
        }

        public bool Unlink(int fromIndex, int toIndex)
        {
            int index = GetLinkIndex(fromIndex, toIndex);
            if (index >= 0) Links = Links.Take(index).Concat(Links.Skip(index + 1)).ToArray();
            return index >= 0;
        }

        public int[] GetNeighbors(int node)
        {
            return (from l in Links
                    where l.x == node
                    select l.y).ToArray();
        }

        public int GetLinkIndex(int fromIndex, int toIndex)
        {
            return Array.FindIndex(Links, l => l.x == fromIndex && l.y == toIndex);
        }

        public virtual void DeleteNode(int nodeIndex)
        {
            Links = Links?.Where(l => l.x != nodeIndex && l.y != nodeIndex).ToArray();
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
			return index < 0 ? new T[0] : (
				from l in Links
				where l.x == index
				select Nodes[l.y]).ToArray();
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

                Nodes = Nodes.Where((source, index) => index != nodeIndex).ToArray();
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
                    if (node is IDisposable)
                        ((IDisposable)node).Dispose();
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

        public U[] GetLinks(int nodeIndex) => LinkData.Where((n, i) => Links[i].x == nodeIndex).ToArray();

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
                IDisposable[] disposables = LinkData.OfType<IDisposable>().ToArray();
                Array.ForEach(disposables, l => l.Dispose());
                LinkData = null;
            }
        }
    }
}
