using UnityEngine;
using System.Collections;

namespace Core
{
    public class ValueService : IAppService
    {
        public void SetValue<T>(T value)
        {
        }

        public void SetValue<T>(string ValueID, T value)
        {
        }

        public T TrackValue<T>()
        {
            return default(T);
        }

        public T TrackValue<T>(string ValueID)
        {
            return default(T);
        }

        public T ModifyValue<T>(T modifier)
        {
            return default(T);
        }

        public T ModifyValue<T>(string ValueID, T modifier)
        {
            return default(T);
        }

        public T MultiplyValue<T>(T modifier)
        {
            return default(T);
        }

        public T MultiplyValue<T>(string ValueID, T modifier)
        {
            return default(T);
        }


    }
}
