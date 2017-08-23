//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(async() =>
            {
                if (s_default == null)
                {
                    s_default = new BluetoothAdapter(await Windows.Devices.Bluetooth.BluetoothAdapter.GetDefaultAsync());
                }

                return s_default;
            });
        }

        private Windows.Devices.Bluetooth.BluetoothAdapter _adapter;

        internal BluetoothAdapter(Windows.Devices.Bluetooth.BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            
            return BluetoothClassOfDevice.FromRawValue(0);
        }

        private bool GetIsClassicSupported()
        {
            return true;
        }

        private bool GetIsLowEnergySupported()
        {
            return true;
        }

        private string GetName()
        {
            return string.Empty;
        }
    }
}