// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.NetworkStream
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.IO;

namespace InTheHand.Net.Sockets
{
    public abstract class NetworkStream : Stream
    {
        /// <summary>
        /// Gets a value that indicates whether data is available on the <see cref="NetworkStream"/> to be read.
        /// </summary>
        public virtual bool DataAvailable { get; }
    }
}
