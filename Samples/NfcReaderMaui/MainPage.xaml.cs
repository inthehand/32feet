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
            Dispatcher.Dispatch(() =>
            {
                foreach (var record in e.Message.Records)
                {
                    if (record.RecordType == NdefRecordType.Mime)
                    {
                        DisplayAlert("NDEF", $"{record.MediaType} {((byte[])record.Data).Length}", "OK");
                    }
                    else
                    {
                        DisplayAlert("NDEF", $"{record.RecordType} {record.Data}", "OK");
                    }
                }
                
                _cts.Cancel();
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += Reader_Reading;
            _reader.Error += Reader_Error;

           _cts = new CancellationTokenSource();

            await _reader.ScanAsync(_cts.Token);
        }
    }

}
