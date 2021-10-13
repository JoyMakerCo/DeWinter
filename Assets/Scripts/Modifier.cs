using System;
namespace Ambition
{
    [Serializable]
    public struct SerializableHash<K,V>
    {
        public K Key;
        public V Value;

        public SerializableHash(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
