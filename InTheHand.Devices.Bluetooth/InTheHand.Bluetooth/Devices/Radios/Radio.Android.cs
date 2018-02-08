//-----------------------------------------------------------------------
// <copyright file="Radio.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Radios
{
    partial class Radio
    {
        private BluetoothManager _manager;

        internal Radio(BluetoothManager manager)
        {
            _manager = manager;
        }

        private static void DoGetRadiosAsync(List<Radio> radios)
        {
            BluetoothManager manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            if (manager != null)
            {
                radios.Add(new Radio(manager));
            }
        }

        // only supporting Bluetooth radio
        private RadioKind GetKind()
        {
            return RadioKind.Bluetooth;
        }

        // matching the UWP behaviour (although we could have used the radio name)
        private string GetName()
        {
            return RadioKind.Bluetooth.ToString();
        }

        private RadioState GetState()
        {
            try
            {
                return _manager.Adapter.IsEnabled ? RadioState.On : RadioState.Off;
            }
            catch
            {
                return RadioState.Unknown;
            }
        }

        private Task<RadioAccessStatus> DoSetStateAsync(RadioState state)
        {
            bool success = false;
            switch (state)
            {
                case RadioState.On:
                    success = _manager.Adapter.Enable();
                    break;

                default:
                    success = _manager.Adapter.Disable();
                    break;
            }

            return Task<RadioAccessStatus>.FromResult(success ? RadioAccessStatus.Allowed : RadioAccessStatus.DeniedBySystem);
        }
    }
}