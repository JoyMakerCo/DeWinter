using System;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

// Directed Graph intended for serialization
// Not designed to be mutable at runtime
namespace UGraph
{
    // Simple Adjacency graph
    [Serializable]
    public class DirectedGraphV2 : IDisposable
    {
        // The first index is the originating node
        // Each array represents the list of nodes adjacent to the originating node
        public int[][] Graph;

        public DirectedGraphV2() => Graph = new int[0][];

        public DirectedGraphV2(DirectedGraphV2 graph)
        {
            Graph = new int[graph.Graph.Length][];
            for (int i = graph.Graph.Length - 1; i >= 0; --i)
            {
                Graph[i] = DeepCopy(graph.Graph[i]);
            }
        }

        public int GetLinkIndex(int fromIndex, int toIndex)
        {
            return fromIndex < Graph.Length
                ? Array.IndexOf(Graph[fromIndex], toIndex)
                : -1;
        }

        public virtual void Dispose() => Graph = null;

        protected K[] DeepCopy<K>(K[] array)
        {
            if (array == null) return new K[0];
            K[] result = new K[array.Length];
            Array.Copy(array, result, array.Length);
            return result;
        }
    }

    // Adjacency graph with an arbitrary node data type
    // Please note that in order to be serialized correctly,
    // A class must be defined that extends the generic type.
    // Otherwise the DirectedGraphV2 will serialize as a simple adjadency graph.
    [Serializable]
    public abstract class DirectedGraphV2<T> : DirectedGraphV2
    {
        // Node indices match up with the index in the adjacency graph
        public T[] Nodes;

        public DirectedGraphV2() : base() => Nodes = new T[0];

        // Copy Constructor
        public DirectedGraphV2(DirectedGraphV2<T> graph) : base(graph as DirectedGraphV2)
        {
            Nodes = DeepCopy(graph.Nodes);
        }

        public T[] GetNeighbors(T node) => GetNeighbors(Array.IndexOf(Nodes, node));

        public T[] GetNeighbors(int nodeIndex)
        {
            if (nodeIndex >= 0 && nodeIndex < Nodes.Length)
            {
                T[] result = new T[Graph[nodeIndex].Length];
                for (int i = Graph[nodeIndex].Length - 1; i >= 0; --i)
                {
                    result[i] = Nodes[Graph[nodeIndex][i]];
                }
                return result;
            }
            return null;
        }

        public int GetLinkIndex(T from, T to)
        {
            return GetLinkIndex(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));
        }

        public override void Dispose()
        {
            base.Dispose();
            if (default(T) is IDisposable)
            {
                foreach (T node in Nodes)
                {
                    (node as IDisposable).Dispose();
                }
            }
            Nodes = null;
        }
    }

    [Serializable]
    public abstract class DirectedGraphV2<T, U> : DirectedGraphV2<T>
    {
        // Linkdata indices 
        public U[][] Links;

        public DirectedGraphV2() : base() => Links = new U[0][];

        public DirectedGraphV2(DirectedGraphV2<T, U> graph) : base(graph as DirectedGraphV2<T>)
        {
            Links = new U[graph.Links.Length][];
            for (int i = graph.Links.Length - 1; i >= 0; i--)
            {
                Links[i] = DeepCopy(graph.Links[i]);
            }
        }

        public U[] GetLinks(T node) => GetLinks(Array.IndexOf(Nodes, node));

        public U[] GetLinks(int nodeIndex)
        {
            return nodeIndex >= 0 && nodeIndex < Nodes.Length
                ? Links[nodeIndex]
                : null;
        }

        public U GetLink(T from, T to) => GetLink(Array.IndexOf(Nodes, from), Array.IndexOf(Nodes, to));

        public U GetLink(int fromIndex, int toIndex)
        {
            int index = GetLinkIndex(fromIndex, toIndex);
            return index >= 0
                ? Links[index][toIndex]
                : default;
        }

        // Returns the index of the node linked to by this link
        public int GetNodeIndex(U link)
        {
            int index;
            foreach (U[] links in Links)
            {
                index = Array.IndexOf(links, link);
                if (index >= 0)
                {
                    return index;
                }
            }
            return -1;
        }

        // Returns the node linked to by this link
        public T GetNode(U link)
        {
            int index = GetNodeIndex(link);
            return index >= 0
                ? Nodes[index]
                : default;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (default(U) is IDisposable)
            {
                foreach (U[] links in Links)
                {
                    foreach (U link in links)
                    {
                        (link as IDisposable).Dispose();
                    }
                }
            }
            Links = null;
        }
    }
}
