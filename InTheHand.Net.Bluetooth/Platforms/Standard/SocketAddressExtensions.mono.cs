using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Sockets
{
    public static class SocketAddressExtensions
    {
        public static byte[] ToByteArray(this SocketAddress sa)
        {
            byte[] array = new byte[sa.Size];
            for (int i = 0; i < sa.Size; i++)
            {
                array[i] = sa[i];
            }

            return array;
        }
    }
}
