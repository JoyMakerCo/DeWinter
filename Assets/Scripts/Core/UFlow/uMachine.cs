using Util;

namespace UFlow
{
    public class UMachine : DirectedGraph<UStateMap, ULinkMap>
    {
        public int Initial = 0;
        public int[] Exits = new int[0];
    }
}
