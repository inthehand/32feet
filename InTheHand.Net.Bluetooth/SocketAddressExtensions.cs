// 32feet.NET - Personal Area Networking for .NET
//
// System.Net.Sockets.SocketAddressExtensions
// 
// Copyright (c) 2003-23 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace System.Net.Sockets
{
    /// <summary>
    /// Extension methods for <see cref="SocketAddress"/>.
    /// </summary>
    public static class SocketAddressExtensions
    {
        /// <summary>
        /// Returns a SocketAddress as a raw byte array.
        /// </summary>
        /// <param name="sa">The <see cref="SocketAddress"/>.</param>
        /// <returns>The <see cref="SocketAddress"/> contents as a byte array.</returns>
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
