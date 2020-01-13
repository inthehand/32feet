//-----------------------------------------------------------------------
// <copyright file="GattReadResult.Bytes.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattReadResult
    {
        private GattCommunicationStatus _status;
        private byte[] _value;
      
        internal GattReadResult(GattCommunicationStatus status, byte[] value)
        {
            _status = status;
            _value = value;
        }

        private GattCommunicationStatus GetStatus()
        {
            return _status;
        }

        private byte[] GetValue()
        {
            return _value;
        }
    }
}