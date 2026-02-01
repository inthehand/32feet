using System.Runtime.CompilerServices;

namespace IOBluetooth;

public partial class BluetoothDevice
{
    /// <summary>
    /// Get the Bluetooth device address for the target device.
    /// </summary>
    public unsafe BluetoothDeviceAddress Address => Unsafe.AsRef<BluetoothDeviceAddress>(GetAddress().ToPointer());

    /// <summary>
    /// Get the date/time of the last time the device was returned during an inquiry.
    /// </summary>
    /// <value>Returns the date/time of the last time the device was seen during an inquiry.
    /// If the device has never been seen during an inquiry, null is returned.</value>
    public DateTime? LastInquiryUpdate => GetNullableDateTimeForNSDate(GetLastInquiryUpdate());

    /// <summary>
    /// Get the date/time of the last successful remote name request.
    /// </summary>
    /// <value>The last name update.</value>
    public DateTime? LastNameUpdate => GetNullableDateTimeForNSDate(GetLastNameUpdate());
    
    /// <summary>
    /// Get the date/time of the last SDP query.
    /// </summary>
    /// <value>Returns the date/time of the last SDP query.
    /// If an SDP query has never been performed on the device, null is returned.</value>
    public DateTime? LastServicesUpdate => GetNullableDateTimeForNSDate(GetLastServicesUpdate());

    /// <summary>
    /// Returns the date/time of the most recent access of the target device.
    /// </summary>
    /// <value>Returns the date/time of the most recent access of the target device.
    /// If the device has not been accessed, null is returned.</value>
    /// <remarks>This is the date that <see cref="GetRecentDevices"/> uses to sort its list of the most recently accessed devices.</remarks>
    public DateTime? RecentAccessDate => GetNullableDateTimeForNSDate(GetRecentAccessDate());

    public override string ToString()
    {
        return NameOrAddress;
    }

    private static DateTime? GetNullableDateTimeForNSDate(NSDate? value)
    {
        return value == null ? null : (DateTime?)value;
    }
}