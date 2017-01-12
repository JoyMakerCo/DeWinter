using System;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
	public class DirectedGraph<T>
	{
		protected Dictionary<T,HashSet<T>> _edges;

		protected HashSet<T> _nodes;
		public HashSet<T> Nodes
		{
			get { return _nodes; }
		}

		public DirectedGraph ()
		{
			_nodes = new HashSet<T>();
			_edges = new Dictionary<T, HashSet<T>>();
		}

		public void AddNode(T newNode)
		{
			_nodes.Add(newNode);
		}

		public void AddNode(T newNode, T fromNode)
		{
			_nodes.Add(newNode);
			Connect (fromNode, newNode);
		}

		public void Connect(T parent, params T[] children)
		{
			HashSet<T> connections;
			_edges.TryGetValue(parent, out connections);
			if (connections == null)
			{
				_edges[parent] = connections = new HashSet<T>(children);
			}
			else
			{
				connections.UnionWith(children);
			}
		}

		public T[] GetConnectionsFrom(T node)
		{
			HashSet<T> connections;
			_edges.TryGetValue(node, out connections);
			if (connections == null)
			{
				return new T[0];
			}
			T[] result = new T[connections.Count];
			connections.CopyTo(result);
			return result;
		}
	}
}