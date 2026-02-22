namespace IOBluetooth;

public partial class DevicePair
{
    /// <summary>
    /// This is the required reply to the PinCodeRequested event. Set the PIN code to use during pairing if required.
    /// </summary>
    /// <param name="pinCode"></param>
    public void ReplyPinCode(byte[] pinCode)
    {
        var bytes = NSArray.FromObjects(pinCode);
        ReplyPinCode((nuint)pinCode.Length, bytes);
    }
}