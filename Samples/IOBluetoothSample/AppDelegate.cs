using System.Diagnostics;
using IOBluetoothUI;

namespace IOBluetoothSample;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    public override void DidFinishLaunching(NSNotification notification)
    {
        // Insert code here to initialize your application
        foreach (var btdev in IOBluetooth.BluetoothDevice.PairedDevices)
        {
            Debug.WriteLine(btdev.Name);
        }

        var controller = new IOBluetoothUI.DeviceSelectorController();
        var result = controller.RunModal();
        if (result == (int)ModalResponse.Success)
        {
            NSAlert alert = new NSAlert(){ MessageText = controller.Results[0].Name};
                    alert.RunModal();
        }

        
    }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }
}