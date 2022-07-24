using Android.Content;
using Android.Hardware.Usb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace InTheHand.Permissions
{
    public partial class UsbPermission : BasePlatformPermission
    {
        private static readonly UsbManager _manager;

        static UsbPermission()
        {
            _manager = Android.App.Application.Context.GetSystemService(Context.UsbService) as UsbManager;
        }

    }
}
