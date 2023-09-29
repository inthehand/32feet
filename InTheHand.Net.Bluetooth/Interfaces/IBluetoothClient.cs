// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.IBluetoothClient
// 
// Copyright (c) 2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    internal interface IBluetoothClient : IDisposable
    {
        void Connect(BluetoothAddress address, Guid service);
        void Connect(BluetoothEndPoint remoteEP);
        Task ConnectAsync(BluetoothAddress address, Guid service);

        void Close();

        IEnumerable<BluetoothDeviceInfo> PairedDevices { get; }

        IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices);
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
#endif
        bool Authenticate { get; set; }

        Socket Client { get; }

        bool Connected { get; }

        bool Encrypt { get; set; }

        TimeSpan InquiryLength { get; set; }

        string RemoteMachineName { get; }

        NetworkStream GetStream();
    }
}