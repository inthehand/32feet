using InTheHand.Nfc;

namespace NfcReaderMaui
{
    public partial class MainPage : ContentPage
    {
        NdefReader _reader;
        private CancellationTokenSource _cts;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Reader_Error(object sender, EventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                DisplayAlert("NDEF", "Error", "OK");
            });
        }

        private void Reader_Reading(object sender, NdefReadingEventArgs e)
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                _cts.Cancel();
            }

            Dispatcher.Dispatch(async () =>
            {
                foreach (var record in e.Message.Records)
                {
                    if (record.RecordType == NdefRecordType.Mime)
                    {
                        await DisplayAlert("NDEF", $"{record.MediaType} {((byte[])record.Data).Length}", "OK");
                    }
                    else
                    {
                        await DisplayAlert("NDEF", $"{record.RecordType} {record.Data}", "OK");
                    }
                }

                if (!_cts.IsCancellationRequested)
                {
                    await _cts.CancelAsync();
                }
                
            });
        }

        private async void ReadButton_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += Reader_Reading;
            _reader.Error += Reader_Error;

           _cts = new CancellationTokenSource();

            await _reader.ScanAsync(_cts.Token);
        }

        private async void Writer_Reading(object sender, NdefReadingEventArgs e)
        {
            var message = new NdefMessage(NdefRecord.CreateText("My first NFC tag!"),
                NdefRecord.CreateUri(new Uri("https://32feet.net")));
            await ((NdefReader)sender).WriteAsync(message);
            await _cts.CancelAsync();
        }

        private async void WriteButton_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += Writer_Reading;
            _reader.Error += Reader_Error;

            _cts = new CancellationTokenSource();

            await _reader.ScanAsync(_cts.Token);
        }

        protected override void OnDisappearing()
        {
            _cts?.Dispose();
            _reader.Dispose();
            base.OnDisappearing();
        }
    }

}
