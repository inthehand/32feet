//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private CBPeripheral _peripheral;

        private BluetoothDevice(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
        }

        private void Bluetooth_DisconnectedPeripheral(object sender, CBPeripheralErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static implicit operator BluetoothDevice(CBPeripheral peripheral)
        {
            return peripheral == null ? null : new BluetoothDevice(peripheral);
        }

        public static implicit operator CBPeripheral(BluetoothDevice device)
        {
            return device._peripheral;
        }

        public override bool Equals(object obj)
        {
            BluetoothDevice device = obj as BluetoothDevice;
            if (device != null)
            {
                return _peripheral == device._peripheral;
            }

            return base.Equals(obj);
        }

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            try
            {
                NSUuid nativeIdentifier = new NSUuid(id);
                var devices = Bluetooth._manager.RetrievePeripheralsWithIdentifiers(nativeIdentifier);

                if (devices != null && devices.Length > 0)
                    return devices[0];

            }
            catch(FormatException)
            {
                throw new ArgumentException("Invalid Id", nameof(id));
            }

            return null;
        }


        public override int GetHashCode()
        {
            return _peripheral.GetHashCode();
        }

        private string GetId()
        {
            return _peripheral.Identifier.ToString();
        }

        private string GetName()
        {
            return _peripheral.Name;
        }

        private RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        private bool GetIsPaired()
        {
            return false;
        }

        private Task PlatformPairAsync()
        {
            throw new PlatformNotSupportedException();
        }

        private Task PlatformPairAsync(string pairingCode)
        {
            throw new PlatformNotSupportedException();
        }

#if __IOS__
        /// <summary>
        /// Specifies whether to request Apple Notification Center Services to allow access to notifications from the remote device when connecting.
        /// </summary>
        public bool RequiresAncs
        {
            get;set;
        }
#endif
    }
}