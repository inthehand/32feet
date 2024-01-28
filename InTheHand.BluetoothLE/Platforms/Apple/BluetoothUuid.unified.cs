//-----------------------------------------------------------------------
// <copyright file="BluetoothUuid.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using System;
using System.Runtime.InteropServices;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Provides conversion support for CBUUID.
    /// </summary>
    partial struct BluetoothUuid
    {
        public static implicit operator CBUUID(BluetoothUuid uuid)
        {
            return CBUUID.FromString(uuid.Value.ToString());
        }

        public static implicit operator BluetoothUuid(CBUUID uuid)
        {
            byte[] b = new byte[16];
            System.Diagnostics.Debug.WriteLine(uuid.ToString());

            switch (uuid.Data.Length)
            {
                case 2:
                    b = BluetoothBase.ToByteArray();
                    b[0] = uuid.Data[1];
                    b[1] = uuid.Data[0];
                    break;

                case 4:
                    b = BluetoothBase.ToByteArray();
                    b[0] = uuid.Data[3];
                    b[1] = uuid.Data[2];
                    b[2] = uuid.Data[1];
                    b[3] = uuid.Data[0];
                    break;

                case 16:
                    b[0] = uuid.Data[3];
                    b[1] = uuid.Data[2];
                    b[2] = uuid.Data[1];
                    b[3] = uuid.Data[0];
                    b[4] = uuid.Data[5];
                    b[5] = uuid.Data[4];
                    b[6] = uuid.Data[7];
                    b[7] = uuid.Data[6];
                    Marshal.Copy(uuid.Data.Bytes + 8, b, 8, 8);
                    break;

                default:
                    return Guid.Empty;
            }

            return new Guid(b);
        }
    }
}