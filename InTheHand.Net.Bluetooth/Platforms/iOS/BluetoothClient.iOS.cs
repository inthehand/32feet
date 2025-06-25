// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (iOS)
// 
// Copyright (c) 2018-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using ExternalAccessory;
using Foundation;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets.iOS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    internal sealed class ExternalAccessoryBluetoothClient : IBluetoothClient
    {
        private EASession _session;
        private EAAccessory _accessory;
        private ExternalAccessoryNetworkStream _stream;
        private NSObject _connectionObserver;

        public ExternalAccessoryBluetoothClient()
        {
            EAAccessoryManager.SharedAccessoryManager.RegisterForLocalNotifications(); 
            _connectionObserver = EAAccessoryManager.Notifications.ObserveDidConnect((s, e) =>
            {
                Debug.WriteLine($"{e.Notification} {e.Accessory} {e.Selected}");
            });
        }

        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                foreach (EAAccessory accessory in EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories)
                {
                    yield return new BluetoothDeviceInfo(new ExternalAccessoryBluetoothDeviceInfo(accessory));
                }
            }
        }

        IReadOnlyCollection<BluetoothDeviceInfo> IBluetoothClient.DiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

#if NET6_0_OR_GREATER
        async IAsyncEnumerable<BluetoothDeviceInfo> IBluetoothClient.DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield break;
        }
#endif

        public void Connect(BluetoothAddress address, Guid service)
        {
            _accessory = address.Accessory;
            _accessory.Disconnected += _accessory_Disconnected;
            // TODO: provide mapping support for multiple protocol strings
            if (BluetoothServiceProtocolMapping.TryGetProtocolForUuid(service, out var protocol))
            {
                _session = new EASession(_accessory, protocol);
            }
            else
            {
                _session = new EASession(_accessory, _accessory.ProtocolStrings[0]);
            }
            _stream = new ExternalAccessoryNetworkStream(_session);
        }

        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            Connect(remoteEP.Address, remoteEP.Service);
        }

        async Task IBluetoothClient.ConnectAsync(BluetoothAddress address, Guid service)
        {
            Connect(address, service);
        }

        private void _accessory_Disconnected(object sender, EventArgs e)
        {
            Debug.WriteLine($"{_accessory.Name} Disconnected");
            _accessory.Disconnected -= _accessory_Disconnected;
        }

        public void Close()
        {
            _session?.Dispose();
            _session = null;
        }

        bool IBluetoothClient.Authenticate { get => true; set => throw new PlatformNotSupportedException(); }
        Socket IBluetoothClient.Client { get => throw new PlatformNotSupportedException(); }
        bool IBluetoothClient.Encrypt { get => true; set => throw new PlatformNotSupportedException(); }
        TimeSpan IBluetoothClient.InquiryLength { get => TimeSpan.Zero; set => throw new PlatformNotSupportedException(); }
        
        public bool Connected
        {
            get
            {
                if (_accessory is object)
                {
                    return _accessory.Connected;
                }

                return false;
            }
        }

        public string RemoteMachineName
        {
            get
            {
                return _accessory?.Name;
            }
        }

        public NetworkStream GetStream()
        {
            if (Connected)
                return _stream;

            return null;
        }

        void Dispose(bool disposing)
        {
            if (_connectionObserver != null)
            {
                _connectionObserver.Dispose();
                _connectionObserver = null;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}