//-----------------------------------------------------------------------
// <copyright file="GattDescriptorsResult.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptorsResult
    {
        private IReadOnlyList<GattDescriptor> _descriptors;
        private GattCommunicationStatus _status;

        internal GattDescriptorsResult(GattCommunicationStatus status, IReadOnlyList<GattDescriptor> descriptors)
        {
            _status = status;
            _descriptors = descriptors;
        }
        
        private IReadOnlyList<GattDescriptor> GetDescriptors()
        {
            return _descriptors;
        }
        
        private GattCommunicationStatus GetStatus()
        {
            return _status;
        }
    }
}