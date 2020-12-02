using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InTheHand.Net
{
    internal static class SocketAddressExtensions
    {
        /// <summary>
        /// Returns the serialized SocketAddress as a raw byte array.
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this SocketAddress sa)
        {
            byte[] array = new byte[sa.Size];
            for(int i = 0; i < sa.Size; i++)
            {
                array[i] = sa[i];
            }

            return array;
        }
    }
}
