//-----------------------------------------------------------------------
// <copyright file="GattDescriptorsResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// The result of descriptor operations like <see cref="GattCharacteristic.GetDescriptorsAsync()"/>.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class GattDescriptorsResult
    {
        /// <summary>
        /// Gets a vector of the GATT descriptors.
        /// </summary>
        public IReadOnlyList<GattDescriptor> Descriptors
        {
            get
            {
                return GetDescriptors();
            }
        }

        /*
        /// <summary>
        /// Gets the GATT protocol error.
        /// </summary>
        public byte? ProtocolError
        {
            get
            {
                return GetProtocolError();
            }
        }*/

        /// <summary>
        /// Gets the status of the operation.
        /// </summary>
        public GattCommunicationStatus Status
        {
            get
            {
                return GetStatus();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", typeof(GattDescriptorsResult).Name, Status);
        }
    }
}