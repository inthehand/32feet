// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.NullBluetoothRadio (iOS)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class ExternalAccessoryBluetoothRadio : IBluetoothRadio
    {        
        public string Name { get => string.Empty; }

        public BluetoothAddress LocalAddress { get => BluetoothAddress.None; }

        public RadioMode Mode { get => RadioMode.PowerOff; set => throw new PlatformNotSupportedException(); }

        public CompanyIdentifier Manufacturer { get => CompanyIdentifier.Apple; }

        public void Dispose() { }
    }
}