namespace MauiWebBluetooth
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InTheHand.Bluetooth.Bluetooth.AvailabilityChanged += Bluetooth_AvailabilityChanged;
        }

        private async void Bluetooth_AvailabilityChanged(object sender, EventArgs e)
        {
            var a = await InTheHand.Bluetooth.Bluetooth.GetAvailabilityAsync();
            System.Diagnostics.Debug.WriteLine($"Availability: {a}");
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            InTheHand.Bluetooth.Bluetooth.RequestDeviceAsync();
        }
    }
}