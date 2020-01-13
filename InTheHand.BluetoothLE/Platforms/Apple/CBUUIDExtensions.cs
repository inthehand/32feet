using System;
using System.Runtime.InteropServices;

namespace CoreBluetooth
{
    /// <summary>
    /// Provides conversion support for CBUUID.
    /// </summary>
    internal static class CBUUIDExtensions
    {
        internal static Guid ToGuid(this CBUUID uuid)
        {
            byte[] b = new byte[16];

            switch (uuid.Data.Length)
            {
                case 2:
                    b = new Guid("0x00000000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB").ToByteArray();
                    Marshal.Copy(uuid.Data.Bytes, b, 0, (int)uuid.Data.Length);

                    break;
                default:

                    Marshal.Copy(uuid.Data.Bytes, b, 0, (int)uuid.Data.Length);
                    break;
            }

            return new Guid(b);
        }

        internal static CBUUID ToCBUUID(this Guid guid)
        {
            return CBUUID.FromBytes(guid.ToByteArray());
        }
    }
}
