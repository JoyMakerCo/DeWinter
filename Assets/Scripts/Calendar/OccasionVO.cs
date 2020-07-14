using System;
using Newtonsoft.Json;
namespace Ambition
{
    [Serializable]
    public struct OccasionVO
    {
        public string ID;
        public bool IsComplete;
        public OccasionType Type;
        public int Day;
    }
}
