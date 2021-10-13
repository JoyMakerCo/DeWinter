using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class RNG
    {
        public static int Generate(int max) => (int)(UnityEngine.Random.value * max * .999999f);

        public static int Generate(int min, int max) => (int)(UnityEngine.Random.Range(min, max * .999999f));

        public static T TakeRandom<T>(IEnumerable<T> seq)
        {
            List<T> ts = seq as List<T> ?? new List<T>(seq);
            return ts.Count > 0
                ? ts[Generate(ts.Count)]
                : default;
        }

        public static bool Chance( float probability ) => UnityEngine.Random.value * .999999f < probability;
        
        public static void Randomize() =>  UnityEngine.Random.InitState((int)(Time.realtimeSinceStartup * 1000.0));

        public static T[] Shuffle<T>(IEnumerable<T> seq)
        {                            
            int k;
            T t;
            T[] result;
            if (seq is T[])
            {
                result = new T[((T[])seq).Length];
                Array.Copy((T[])seq, result, result.Length);
            }
            else result = (seq as List<T>)?.ToArray() ?? new List<T>(seq).ToArray();
            for(int n=result.Length-1; n > 0; --n)
            {
                k = Generate(n);
                t = result[k];
                result[k] = result[n];
                result[n] = t;
            } 
            return result;
        }
    }
}
