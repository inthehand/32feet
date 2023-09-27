//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        internal BluetoothLEDevice NativeDevice;
        internal readonly ConcurrentDictionary<int, IDisposable> NativeDisposeList = new ConcurrentDictionary<int, IDisposable>();
        private string _cachedId;
        private string _cachedName;
        internal ulong LastKnownAddress;

        internal BluetoothDevice(BluetoothLEDevice device)
        {
            NativeDevice = device;
            AddDisposableObject(this, device);


            if (device == null) return;

            LastKnownAddress = device.BluetoothAddress;

            // this will cache the Id and Name for use in cases where 'NativeDevice' has been disposed. 
            GetId();
            GetName();
        }

        ~BluetoothDevice()
        {
            DisposeAllNativeObjects();
        }

        /// <summary>Adds a native (IDisposable) object to the dispose list</summary>
        /// <param name="container">This is generally the managed object that contains the native object but it can be anything that can serve as a unique key.</param>
        /// <param name="disposableObject">The native object that we will disposed when the user requests a disconnect</param>
        internal void AddDisposableObject(object container, IDisposable disposableObject)
        {
            if (NativeDisposeList.TryGetValue(container.GetHashCode(), out IDisposable existingValue))
            {
                NativeDisposeList.TryUpdate(container.GetHashCode(), disposableObject, existingValue);
            }
            else
            {
                NativeDisposeList.TryAdd(container.GetHashCode(), disposableObject);
            }
        }

        /// <summary>Called in RemoteServer.PlatformCleanup to dispose all of the native object that have been collected.</summary>
        internal void DisposeAllNativeObjects()
        {
            Dictionary<int, IDisposable> itemsDisposed = new Dictionary<int, IDisposable>();
            foreach (var kv in NativeDisposeList)
            {
                try
                {
                    kv.Value?.Dispose();
                }
                catch (TargetInvocationException e) when (e.InnerException is ObjectDisposedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidComObjectException) { }
                itemsDisposed.Add(kv.Key, kv.Value);
            }

            foreach (var kv in itemsDisposed)
            {
                IDisposable val;
                NativeDisposeList.TryRemove(kv.Key, out val);
            }
        }

        /// <summary>
        /// Used to Recreate 'NativeDevice' if it is in a disposed state.
        /// </summary>
        /// <param name="deviceAddress">The bluetooth device address</param>
        /// <returns>True if 'NativeDevice' was recreated</returns>
        internal async Task<bool> CreateNativeInstance()
        {
            if (LastKnownAddress == 0) return false;

            if (NativeDisposeList.TryGetValue(GetHashCode(), out IDisposable existingItem))
            {
                if (existingItem == null)
                {
                    // The native object was disposed as the result of a call to RemoteGattServer.Disconnect.
                    // we need to create another one.
                    BluetoothLEDevice nativeDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(LastKnownAddress);
                    if (nativeDevice != null)
                    {
                        AddDisposableObject(this, nativeDevice);
                        NativeDevice = nativeDevice;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Checks if the native object for this container has been disposed.</summary>
        /// <param name="container">This is generally the managed object that contains the native object but it can be anything that can serve as a unique key.</param>
        /// <returns>True if the container exists and it's native object has been disposed.</returns>
        internal bool IsDisposedItem(object container)
        {
            if (NativeDisposeList.TryGetValue(container.GetHashCode(), out IDisposable existingItem))
            {
                return existingItem == null;
            }

            return false;
        }

        public static implicit operator BluetoothLEDevice(BluetoothDevice device)
        {
            return device.NativeDevice;
        }

        public static implicit operator BluetoothDevice(BluetoothLEDevice device)
        {
            return device == null ? null : new BluetoothDevice(device);
        }

        public override bool Equals(object obj)
        {
            BluetoothDevice device = obj as BluetoothDevice;
            if (device != null)
            {
                return NativeDevice == device.NativeDevice;
            }

            return base.Equals(obj);
        }

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            BluetoothLEDevice device = null;

            if (ulong.TryParse(id, System.Globalization.NumberStyles.HexNumber, null, out var parsedId))
            {
                if (Bluetooth.KnownDevices.ContainsKey(parsedId))
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

        public override int GetHashCode()
        {
            return NativeDevice.GetHashCode();
        }

        internal string GetId()
        {
            if (IsDisposedItem(this)) return _cachedId;
            _cachedId = NativeDevice.BluetoothAddress.ToString("X6");
            return _cachedId;
        }

        internal string GetName()
        {
            if (IsDisposedItem(this)) return _cachedName;
            if (NativeDevice.Name.StartsWith("Bluetooth "))
            {
                _cachedName = NativeDevice.DeviceInformation.Name;
            }
            else
            {
                _cachedName = NativeDevice.Name;
            }

            return _cachedName;
        }

        RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        bool GetIsPaired()
        {
            return NativeDevice.DeviceInformation.Pairing.IsPaired;
        }

        Task PlatformPairAsync()
        {
            return NativeDevice.DeviceInformation.Pairing.PairAsync().AsTask();
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
