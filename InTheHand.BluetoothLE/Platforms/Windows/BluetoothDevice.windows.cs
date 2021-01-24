//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        internal BluetoothLEDevice NativeDevice;

        internal BluetoothDevice(BluetoothLEDevice device)
        {
            NativeDevice = device;
        }

        ~BluetoothDevice()
        {
            NativeDevice.Dispose();
        }

        public static implicit operator BluetoothLEDevice(BluetoothDevice device)
        {
            return device.NativeDevice;
        }

        public static implicit operator BluetoothDevice(BluetoothLEDevice device)
        {
            return new BluetoothDevice(device);
        }

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            BluetoothLEDevice device = null;

            if (ulong.TryParse(id, System.Globalization.NumberStyles.HexNumber, null, out var parsedId))
            {
                if(Bluetooth.KnownDevices.ContainsKey(parsedId))
                {
                    BluetoothDevice knownDevice = (BluetoothDevice)Bluetooth.KnownDevices[parsedId].Target;
                    if (knownDevice != null)
                        return knownDevice;
                }
                device = await BluetoothLEDevice.FromBluetoothAddressAsync(parsedId);
            }
            else
            {
                device = await BluetoothLEDevice.FromIdAsync(id);
            }

            if (device != null)
            {
                var success = await device.RequestAccessAsync();
                System.Diagnostics.Debug.WriteLine($"RequestAccessAsync {success}");
            }

            return device;
        }

        string GetId()
        {
            return NativeDevice.BluetoothAddress.ToString("X6");
        }

        string GetName()
        {
            if(NativeDevice.Name.StartsWith("Bluetooth "))
            {
                return NativeDevice.DeviceInformation.Name;
            }

            return NativeDevice.Name;
        }

        RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        /*bool GetWatchingAdvertisements()
        {
            return _watchingAdvertisements;
        }

        async Task DoWatchAdvertisements()
        {
            _watchingAdvertisements = true;
            _advertisementRefCount += 1;
            _advertisementWatcher.Received += _advertisementWatcher_Received;

            if(_advertisementWatcher.Status != BluetoothLEAdvertisementWatcherStatus.Started)
            {
                _advertisementWatcher.Start();
            }
        }

        private void _advertisementWatcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if(args.BluetoothAddress == NativeDevice.BluetoothAddress)
            {
                OnAdvertismentReceived(new BluetoothAdvertisingEvent(this, (byte)args.RawSignalStrengthInDBm, args.Advertisement));
            }
        }

        void DoUnwatchAdvertisements()
        {
            _watchingAdvertisements = false;
            _advertisementRefCount -= 1;
            _advertisementWatcher.Received -= _advertisementWatcher_Received;

            if (_advertisementRefCount < 1 && _advertisementWatcher.Status != BluetoothLEAdvertisementWatcherStatus.Stopped)
            {
                _advertisementWatcher.Stop();
            }
        }*/
    }
}
