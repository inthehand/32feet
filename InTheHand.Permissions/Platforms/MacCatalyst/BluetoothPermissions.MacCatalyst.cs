using static Microsoft.Maui.ApplicationModel.Permissions;

namespace InTheHand.Permissions
{
    /// <summary>
    ///	BluetoothPermissions (.NET MAUI on macOS Catalyst).
    /// </summary>
    public partial class BluetoothPermissions : BasePlatformPermission
    {
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            switch(CoreBluetooth.CBManager.Authorization)
            {
                case CoreBluetooth.CBManagerAuthorization.AllowedAlways:
                    return Task.FromResult(PermissionStatus.Granted);

                case CoreBluetooth.CBManagerAuthorization.Denied:
                    return Task.FromResult(PermissionStatus.Denied);

                case CoreBluetooth.CBManagerAuthorization.Restricted:
                    return Task.FromResult(PermissionStatus.Restricted);

                default:
                    return Task.FromResult(PermissionStatus.Unknown);
            }
        }
    }
}