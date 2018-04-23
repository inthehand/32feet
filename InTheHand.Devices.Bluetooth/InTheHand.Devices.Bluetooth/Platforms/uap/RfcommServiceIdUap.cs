//-----------------------------------------------------------------------
// <copyright file="RfcommServiceId.uwp.cs" company="In The Hand Ltd">
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
            return Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId.FromShortId(shortId);
        }
        
        private static RfcommServiceId FromUuidImpl(Guid uuid)
        {
            return Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId.FromUuid(uuid);
        }
        
        private Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId _id;

        private RfcommServiceId(Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId id)
        {
            _id = id;
        }

        public static implicit operator Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId(RfcommServiceId id)
        {
            return id._id;
        }

        public static implicit operator RfcommServiceId(Windows.Devices.Bluetooth.Rfcomm.RfcommServiceId id)
        {
            return new RfcommServiceId(id);
        }
        
        private Guid GetUuid()
        {
            return _id.Uuid;
        }
        
        private uint AsShortIdImpl()
        {
            return _id.AsShortId();
        }
        
        private string AsStringImpl()
        {
            return _id.AsString();
        }
    }
}