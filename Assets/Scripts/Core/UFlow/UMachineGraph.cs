using Util;
using System.Collections.Generic;

namespace UFlow
{
    public class UMachineGraph : DirectedGraph<UStateNode, UGraphLink>
    {
        public int[] Exits = new int[0];
        public Dictionary<int,int[]> Aggregates = new Dictionary<int, int[]>();
    }
}
