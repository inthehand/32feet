using System;
using Java.Util;

namespace InTheHand.Devices.Bluetooth
{
    public static class UUIDExtensions
    {
        public static Guid ToGuid(this UUID uuid)
        {
            return new Guid(uuid.ToString());
        }

    }
}
