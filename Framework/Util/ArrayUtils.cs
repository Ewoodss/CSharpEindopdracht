using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Util
{
    public static class ArrayUtils
    {
        public static byte[] Concat(byte[] b1, byte[] b2, int count)
        {
            byte[] r = new byte[b1.Length + count];
            Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
            Buffer.BlockCopy(b2, 0, r, b1.Length, count);
            return r;
        }

        public static T[] GetSubArray<T>(this T[] array, int offset, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
