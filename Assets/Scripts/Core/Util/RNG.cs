using System;
using System.Security.Cryptography;

namespace Util
{
    public static class RNG
    {
        private static readonly RNGCryptoServiceProvider _generator=new RNGCryptoServiceProvider();
        private const double RNG_MULT = 1d/255d;

        public static int Generate(int i)
        {
            return Generate(0, i);
        }

        public static int Generate(int i, int j)
        {
            byte[] n = new byte[1];
            _generator.GetBytes(n);
            double mult = (Convert.ToDouble(n[0]) * RNG_MULT) - 0.00000000001d;
            return mult > 0 ? (i + (int)(Math.Floor(mult * (j - i)))) : i;
        }
    }
}
