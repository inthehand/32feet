using System.Threading.Tasks;
using Tmds.DBus;

namespace InTheHand.Bluetooth;

/// <summary>
/// BlueZ Agent1 interface
/// See https://github.com/pauloborges/bluez/blob/master/doc/agent-api.txt
/// </summary>
[DBusInterface("org.bluez.Agent1")]
internal interface IAgent1 : IDBusObject
{
    Task ReleaseAsync();
    Task<string> RequestPinCodeAsync(ObjectPath device);
    Task DisplayPinCodeAsync(ObjectPath device, string pincode);
    Task<uint> RequestPasskeyAsync(ObjectPath device);
    Task DisplayPasskeyAsync(ObjectPath device, uint passkey, ushort entered);
    Task RequestConfirmationAsync(ObjectPath device, uint passkey);
    Task RequestAuthorizationAsync(ObjectPath device);
    Task AuthorizeServiceAsync(ObjectPath device, string uuid);
    Task CancelAsync();
}

public class PairingAgent(string pairingCode) : IAgent1
{
    public ObjectPath ObjectPath { get; } = new("/InTheHand/CustomAgent");

    public Task ReleaseAsync() => Task.CompletedTask;

    public Task<string> RequestPinCodeAsync(ObjectPath device) => Task.FromResult(pairingCode);

    public Task DisplayPinCodeAsync(ObjectPath device, string pincode)
    {
        throw new System.NotImplementedException();
    }

    public Task<uint> RequestPasskeyAsync(ObjectPath device) => Task.FromResult(ParseIntPairingCode());

    private uint ParseIntPairingCode() => uint.TryParse(pairingCode, out var value) ? value : 0;

    public Task DisplayPasskeyAsync(ObjectPath device, uint passkey, ushort entered)
    {
        throw new System.NotImplementedException();
    }

    public Task RequestConfirmationAsync(ObjectPath device, uint passkey)
    {
        throw new System.NotImplementedException();
    }

    public Task RequestAuthorizationAsync(ObjectPath device)
    {
        throw new System.NotImplementedException();
    }

    public Task AuthorizeServiceAsync(ObjectPath device, string uuid)
    {
        throw new System.NotImplementedException();
    }

    public Task CancelAsync() => Task.CompletedTask;
}