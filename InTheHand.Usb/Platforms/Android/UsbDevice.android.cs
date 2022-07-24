using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Usb
{
    public partial class UsbDevice
    {
        private static readonly UsbManager _manager;
        private Android.Hardware.Usb.UsbDevice _device;
        private UsbDeviceConnection _connection;

        static UsbDevice()
        {
            _manager = Application.Context.GetSystemService(Context.UsbService) as UsbManager;
        }

        internal static async Task<IReadOnlyCollection<UsbDevice>> PlatformGetDevices()
        {
            List<UsbDevice> devices = new List<UsbDevice>();

            foreach (var deventry in _manager.DeviceList)
            {
                devices.Add(deventry.Value);
            }

            return devices.AsReadOnly();
        }

        internal UsbDevice(Android.Hardware.Usb.UsbDevice device)
        {
            _device = device;
            DeviceClass = (int)device.DeviceClass;
            DeviceProtocol = device.DeviceProtocol;
            DeviceSubclass = (int)device.DeviceSubclass;
            DeviceVersion = new Version(device.Version);
            ManufacturerName = device.ManufacturerName;
            ProductId = device.ProductId;
            ProductName = device.ProductName;
            SerialNumber = device.SerialNumber;
            VendorId = device.VendorId;
        }

        public static implicit operator UsbDevice(Android.Hardware.Usb.UsbDevice device)
        {
            return new UsbDevice(device);
        }

        public static implicit operator Android.Hardware.Usb.UsbDevice(UsbDevice device)
        {
            return device._device;
        }

        private Task PlatformOpen()
        {
            _connection  = _manager.OpenDevice(_device);
            IsOpened = _connection != null;
            return Task.CompletedTask;
        }

        private Task PlatformClose()
        {
            if(_connection != null)
            {
                _connection.Close();
            }

            return Task.CompletedTask;
        }

        private Android.Hardware.Usb.UsbInterface GetInterfaceForNumber(int interfaceNumber)
        {
            return _device.GetInterface(interfaceNumber);
        }

        private Task PlatformClaimInterface(int interfaceNumber)
        {
            bool success = _connection.ClaimInterface(GetInterfaceForNumber(interfaceNumber), false);
            return Task.CompletedTask;
        }

        private Task PlatformReleaseInterface(int interfaceNumber)
        {
            bool success = _connection.ReleaseInterface(GetInterfaceForNumber(interfaceNumber));
            return Task.CompletedTask;
        }

        private static UsbAddressing RequestTypeToUsbAddressing(RequestType requestType)
        {
            switch(requestType)
            {
                case RequestType.Class:
                    return (UsbAddressing)UsbConstants.UsbTypeClass;

                case RequestType.Vendor:
                    return (UsbAddressing)UsbConstants.UsbTypeVendor;

                default:
                    return (UsbAddressing)UsbConstants.UsbTypeStandard;
            }
        }

        private async Task<UsbInTransferResult> PlatformControlTransferIn(ControlTransferSetup setup, int length)
        {
            byte[] buffer = new byte[length];
            int read = await _connection.ControlTransferAsync(UsbAddressing.In | RequestTypeToUsbAddressing(setup.RequestType), setup.Request, setup.Value, setup.Index, buffer, length, 0);
            TransferStatus status = (read <= length) ? TransferStatus.Ok : TransferStatus.Babble;
            return new UsbInTransferResult { Data = buffer, Status = status };
        }

        private async Task<UsbOutTransferResult> PlatformControlTransferOut(ControlTransferSetup setup, byte[] data)
{
            int written = await _connection.ControlTransferAsync(UsbAddressing.Out | RequestTypeToUsbAddressing(setup.RequestType), setup.Request, setup.Value, setup.Index, data, data.Length, 0);

            TransferStatus status = (written == 0) ? TransferStatus.Stall : TransferStatus.Ok;
            return new UsbOutTransferResult { BytesWritten = written, Status = status };
        }

        private Task<UsbInTransferResult> PlatformTransferIn(int endpointNumber, int length)
        {
            return (Task<UsbInTransferResult>)Task.CompletedTask;
        }

        private Task PlatformTransferOut(int endpointNumber, byte[] data)
        {
            return Task.CompletedTask;
        }
    }
}