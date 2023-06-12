// 32feet.NET - Personal Area Networking for .NET
//
// System.Net.Sockets.SocketAddressExtensions
// 
// Copyright (c) 2003-23 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

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
