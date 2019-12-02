using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Util
{
    public static class RNG
    {
        /* 
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        private const double RNG_MULT = 1d / 255d;

        public static int Generate(int max) =>  Generate(0, max);

        public static int Generate(int min, int max)
        {
            byte[] n = new byte[1];
            _generator.GetBytes(n);
            double mult = (Convert.ToDouble(n[0]) * RNG_MULT) - 0.00000000001d;
            return mult > 0 ? (min + (int)(Math.Floor(mult * (max - min)))) : min;
        }

        public static T TakeRandom<T>(T[] list)
        {
            return list.Length > 0 ? list[Generate(list.Length)] : default(T);
        }

        public static T TakeRandomExcept<T>(T[] list, T except)
        {
            if (list.Length == 0) return default(T);
            T result = list[Generate(1, list.Length)];
            return result.Equals(except) ? list[0] : result;
        }
        */
        public static int Generate(int max)
        {
            return I(max);
        }

        public static int Generate(int min, int max)
        {
            return I(max-min)+min;
        }

        public static T TakeRandom<T>( IEnumerable<T> seq )
        {
            return seq.Count() > 0 ? Choice(seq) : default(T);
        }

        public static T TakeRandomExcept<T>(IEnumerable<T> seq, T except)
        {
            var result = TakeRandom(seq);
            if (!result.Equals(except))
            {
                return result;
            }

            // if we hit the exception on the first try, only then generate a 
            // copy of the sequence without the offender.  
            return TakeRandom( seq.Where( x => !x.Equals(except)));
        }

        public static void Test()
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.LogFormat( "{0:0.000}", Tangential(-100.0f,100.0f) );
            }
        }
        public static int D( int sides )
        {
            return (int)(UnityEngine.Random.value * sides * 0.9999f) + 1;
        }      

        public static int I( int range )
        {
            return (int)(UnityEngine.Random.value * range * 0.9999f);
        }        
    
        // tangential distribution
        public static float Tangential( float min, float max )
        {
            var scale = 0.25f * (max-min);    
            var mid = 0.5f * (max + min);
            var result = scale * Mathf.Tan( Uniform( -1.107f, 1.107f ) ) + mid;

            return Mathf.Clamp( result, min, max  );
            //return result;
        }
        
        public static float Uniform( float min, float max )
        {
            return (UnityEngine.Random.value * (max-min))+min;
        }       
        
        public static bool Chance( float prob )
        {
            return UnityEngine.Random.value < prob;
        }
        
        public static void Randomize() =>  UnityEngine.Random.InitState((int)(Time.realtimeSinceStartup * 1000.0));

        public static T Choice<T>( IEnumerable<T> seq )
        {    
            int index = D(seq.Count())-1;
            return seq.ElementAt(index);
        }
            
        public static T[] Shuffle<T>( T[] seq )
        {                            
            int n = seq.Count();
            while (n > 1)
            {
                n--;
                int k = (int)(UnityEngine.Random.value * (n + 0.999f));
                T t = seq[k];
                seq[k] = seq[n];
                seq[n] = t;
            } 
            
            return seq;    
        }
    }
}
