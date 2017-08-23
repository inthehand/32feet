//-----------------------------------------------------------------------
// <copyright file="GattReadResult.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic or Descriptor value.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    partial class GattReadResult
    {
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult _result;

        internal GattReadResult(Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult result)
        {
            _result = result;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult(GattReadResult result)
        {
            return result._result;
        }

        public static implicit operator GattReadResult(Windows.Devices.Bluetooth.GenericAttributeProfile.GattReadResult result)
        {
            return new GattReadResult(result);
        }

        private GattCommunicationStatus GetStatus()
        {
            return (GattCommunicationStatus)((int)_result.Status);
        }

        private byte[] GetValue()
        {
            byte[] buffer = new byte[_result.Value.Length];
            Windows.Storage.Streams.DataReader.FromBuffer(_result.Value).ReadBytes(buffer);
            return buffer;
        }
    }
}