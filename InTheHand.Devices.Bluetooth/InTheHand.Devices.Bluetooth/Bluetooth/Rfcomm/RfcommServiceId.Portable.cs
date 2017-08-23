//-----------------------------------------------------------------------
// <copyright file="RfcommServiceId.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommServiceId
    {      
        private static RfcommServiceId FromShortIdImpl(uint shortId)
        {
            Guid g = BluetoothUuidHelper.FromShortId(shortId);
            return new RfcommServiceId(g);
        }
        
        private static RfcommServiceId FromUuidImpl(Guid uuid)
        { 
            return new RfcommServiceId(uuid);
        }


        private Guid _uuid;

        private RfcommServiceId(Guid uuid)
        {
            _uuid = uuid;
        }

          
        private Guid GetUuid()
        {
            return _uuid;
        }

        private uint AsShortIdImpl()
        {
            var shortId = BluetoothUuidHelper.TryGetShortId(_uuid);
            return shortId.HasValue ? shortId.Value: 0;
        }
        
        private string AsStringImpl()
        {
            return _uuid.ToString();
        }
    }
}