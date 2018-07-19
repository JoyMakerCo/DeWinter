using System;
using Util;
using System.Collections.Generic;

namespace UFlow
{
    public class UMachineGraph : DirectedGraph<UStateNode, UGraphLink>
    {
        public int[] Exits = new int[0];
        public Dictionary<int,int[]> Aggregates = new Dictionary<int, int[]>();

        public UMachineGraph() : base() {}
        public UMachineGraph(DirectedGraph<UStateNode, UGraphLink> graph)
        {
            this.Nodes = graph.Nodes;
            this.Links = graph.Links;
            this.LinkData = graph.LinkData;
        }
    }
}
