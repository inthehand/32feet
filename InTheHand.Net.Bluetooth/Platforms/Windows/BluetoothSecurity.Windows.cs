// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Windows)
// 
// Copyright (c) 2019-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class WindowsBluetoothSecurity : IBluetoothSecurity
    {
        private static Dictionary<string, string> pinMappings = new Dictionary<string, string>();

        public bool PairRequest(BluetoothAddress device, string pin, bool? requireMitmProtection)
        {
            BluetoothDevice bluetoothDevice = null;
            var t = Task<bool>.Run(async () =>
            {
                bluetoothDevice = await BluetoothDevice.FromBluetoothAddressAsync(device.ToUInt64());
                DevicePairingResultStatus status = bluetoothDevice.DeviceInformation.Pairing.IsPaired ? DevicePairingResultStatus.Paired : DevicePairingResultStatus.NotPaired;

                if (status == DevicePairingResultStatus.Paired)
                    return true;

                if (string.IsNullOrEmpty(pin))
                {
                    var result = await bluetoothDevice.DeviceInformation.Pairing.PairAsync();
                    status = result.Status;
                }
                else
                {
                    pinMappings.Add(bluetoothDevice.DeviceId, pin);
                    bluetoothDevice.DeviceInformation.Pairing.Custom.PairingRequested += Custom_PairingRequested;
                    var result = await bluetoothDevice.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ProvidePin);
                    status = result.Status;
                }

                return status == DevicePairingResultStatus.Paired;
            });

            t.Wait();
            return t.Result;
        }

        private static void Custom_PairingRequested(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            sender.PairingRequested -= Custom_PairingRequested;
            args.Accept(pinMappings[args.DeviceInformation.Id]);
            pinMappings.Remove(args.DeviceInformation.Id);
        }

        public bool RemoveDevice(BluetoothAddress device)
        {
            BluetoothDevice bluetoothDevice = null;
            var t = Task<bool>.Run(async () =>
            {
                bluetoothDevice = await BluetoothDevice.FromBluetoothAddressAsync(device.ToUInt64());
                if (!bluetoothDevice.DeviceInformation.Pairing.IsPaired)
                    return true;

                return (await bluetoothDevice.DeviceInformation.Pairing.UnpairAsync()).Status == DeviceUnpairingResultStatus.Unpaired;
            });

            t.Wait();

            return t.Result;
        }  
    }
}
