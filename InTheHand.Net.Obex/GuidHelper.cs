// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.GuidHelper
// 
// Copyright (c) 2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net
{
    /// <summary>
    /// Contains logic to switch byte ordering from host to network (Big-endian). If the host is big-endian then no reording is done.
    /// </summary>
    /// <remarks>This class is modelled on IPAddress.HostToNetworkOrder which is used for integer types.</remarks>
    internal static class GuidHelper
    {
        /// <summary>
        /// Converts a <see cref="Guid"/> value from host byte order to network byte order.
        /// </summary>
        /// <param name="host">The <see cref="Guid"/> to convert, expressed in host byte order.</param>
        /// <returns>A <see cref="Guid"/> value, expressed in network byte order.</returns>
        public static Guid HostToNetworkOrder(Guid host)
        {
            return BitConverter.IsLittleEndian ? ReverseEndianness(host) : host;
        }

        /// <summary>
        /// Converts a <see cref="Guid"/> value from network byte order to host byte order.
        /// </summary>
        /// <param name="host">The <see cref="Guid"/> to convert, expressed in network byte order.</param>
        /// <returns>A <see cref="Guid"/> value, expressed in host byte order.</returns>
        public static Guid NetworkToHostOrder(Guid network)
        {
            return HostToNetworkOrder(network);
        }

        private static Guid ReverseEndianness(Guid guid)
        {
            byte[] reversed = new byte[16];
            byte[] guidBytes = guid.ToByteArray();

            for (int i = 8; i < 16; i++)
            {
                reversed[i] = guidBytes[i];
            }

            reversed[0] = guidBytes[3];
            reversed[1] = guidBytes[2];
            reversed[2] = guidBytes[1];
            reversed[3] = guidBytes[0];

            reversed[4] = guidBytes[5];
            reversed[5] = guidBytes[4];

            reversed[6] = guidBytes[7];
            reversed[7] = guidBytes[6];

            return new Guid(reversed);
        }
    }
}
