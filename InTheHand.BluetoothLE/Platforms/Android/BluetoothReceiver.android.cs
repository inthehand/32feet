//-----------------------------------------------------------------------
// <copyright file="BluetoothReceiver.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using Android.Content;

namespace InTheHand.Bluetooth
{
    [BroadcastReceiver(Enabled = true)]
    internal class BluetoothReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if(intent.Action == BluetoothAdapter.ActionStateChanged)
            {
                Bluetooth.OnAvailabilityChanged();
            }
        }
    }
}
