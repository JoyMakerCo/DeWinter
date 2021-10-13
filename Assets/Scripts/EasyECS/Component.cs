using System;
namespace EasyECS
{
    public interface IComponent {}

    public struct Component<T> : IComponent
    {
        public T Data;
        public Component(T data) => Data = data;
    }
}
