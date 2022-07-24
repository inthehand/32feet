using InTheHand.Maui.ApplicationModel;

namespace RfcommMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            this.Appearing += MainPage_Appearing;
            
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            PermissionStatus status = await Permissions.RequestAsync<BluetoothPermission>();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}