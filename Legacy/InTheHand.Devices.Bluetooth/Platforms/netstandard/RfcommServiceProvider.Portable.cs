using System;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommServiceProvider
    {
        
        private static async Task<RfcommServiceProvider> DoCreateAsync(RfcommServiceId serviceId)
        {
            return null;
        }
        
        private void DoStartAdvertising()
        {
            throw new PlatformNotSupportedException();
        }

        private void DoStopAdvertising()
        {
            throw new PlatformNotSupportedException();
        }
    }
}
