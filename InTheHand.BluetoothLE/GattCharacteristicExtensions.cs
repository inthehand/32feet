//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2024-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Additional features for characteristics
    /// </summary>
    public static class GattCharacteristicExtensions
    {
        /// <summary>
        /// Get the user-friendly description for this GattCharacteristic, 
        /// if the User Description Descriptor is present,
        /// otherwise this will be an empty string.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public static async Task<string> GetUserDescriptionAsync(this GattCharacteristic characteristic)
        {
#if WINDOWS
            return characteristic.UserDescription;
#else
            var descriptor = await characteristic.GetDescriptorAsync(GattDescriptorUuids.CharacteristicUserDescription);

            if (descriptor == null)
                return string.Empty;

            var descriptionBytes = await descriptor.ReadValueAsync();
            return descriptionBytes.Length == 0 ? string.Empty : Encoding.UTF8.GetString(descriptionBytes);
#endif
        }
    }
}
