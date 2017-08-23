//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a picker flyout that contains a list of devices for the user to choose from.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DeviceInformation
    {
        /// <summary>
        /// Enumerates DeviceInformation objects matching the specified query string.
        /// </summary>
        /// <param name="aqsFilter"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<DeviceInformation>> FindAllAsync(string aqsFilter)
        {
            List<DeviceInformation> all = new List<DeviceInformation>();
            
            await FindAllAsyncImpl(aqsFilter, all);

            return all.AsReadOnly();
        }

        /// <summary>
        /// A string representing the identity of the device.
        /// </summary>
        public string Id
        {
            get
            {
                return GetId();
            }
        }

        /// <summary>
        /// The name of the device.
        /// </summary>
        public string Name
        {
            get
            {
                return GetName();
            }
        }

#if __ANDROID__ || WINDOWS_UWP || WIN32
        /// <summary>
        /// Gets the information about the capabilities for this device to pair.
        /// </summary>
        /// <value>The pairing information for this device.</value>
        public DeviceInformationPairing Pairing
        {
            get
            {
                return GetPairing();
            }
        }
#endif

        /// <summary>
        /// Returns the Id of the <see cref="DeviceInformation"/>.
        /// </summary>
        /// <returns></returns>
        /// <seealso cref="Id"/>
        public override string ToString()
        {
            return Id;
        }
    }
}