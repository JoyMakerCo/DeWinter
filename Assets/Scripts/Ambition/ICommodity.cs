using System;
namespace Ambition
{
    public interface ICommodity
    {
        string ID { get; }
        CommodityType Type { get; }
        int Value { get; }
    }
}
