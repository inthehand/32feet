using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace MauiBluetoothClassic
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var radio = BluetoothRadio.Default;

            System.Diagnostics.Debug.WriteLine($"{radio.Name} {radio.LmpVersion} {radio.LmpSubversion}");
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            BluetoothDeviceInfo device = null;
            var picker = new BluetoothDevicePicker();
            device = await picker.PickSingleDeviceAsync();
        }
    }
}