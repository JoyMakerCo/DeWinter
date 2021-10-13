using System;
using System.Collections.Generic;
namespace EasyECS
{
    public class GameEntity
    {
        public int ID;
        public List<IComponent> Components = new List<IComponent>();

        public T[] GetComponents<T>() where T:IComponent
        {
            List<IComponent> results = Components.FindAll(c => c is Component<T>);
            T[] result = new T[results.Count];
            for (int i = results.Count - 1; i >= 0; --i)
                result[i] = ((Component<T>)results[i]).Data;
            return result;
        }

        public T GetComponent<T>() where T:IComponent
        {
            IComponent result = Components.Find(c => c is Component<T>);
            return ((Component<T>)result).Data;
        }

        public void AddComponent<T>(T component) where T:IComponent
        {
            Components.Add(new Component<T>(component));
        }

        public bool RemoveComponent<T>(T component) where T : IComponent
        {
            if (!Components.Remove(component)) return false;
            (component as IDisposable)?.Dispose();
            return true;
        }
    }
}
