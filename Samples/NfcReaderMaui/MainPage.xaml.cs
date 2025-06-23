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

        private void Reader_Error(object sender, NdefErrorEventArgs e)
        {
            DisplayAlert("NDEF", e.Error.Message, "OK");

            _cts.Cancel();
        }

        private void Reader_Reading(object sender, NdefReadingEventArgs e)
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                _cts.Cancel();
            }

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

            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }

        private async void ReadButton_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += Reader_Reading;
            _reader.Error += Reader_Error;

           _cts = new CancellationTokenSource();

           try
           {
               await _reader.ScanAsync(_cts.Token);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex);
           }

           _reader.Reading -= Reader_Reading;
            _reader.Error -= Reader_Error;
        }

        private async void Writer_Reading(object sender, NdefReadingEventArgs e)
        {
            var message = new NdefMessage(NdefRecord.CreateText("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin vitae neque congue felis hendrerit tristique sed et augue. Donec et viverra lacus, quis facilisis lectus. Vivamus dictum faucibus lorem."));
            try
            {
                await ((NdefReader)sender).WriteAsync(message, _cts.Token);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Cannot write to tag.", "OK");
            }
            finally
            {
                await _cts.CancelAsync();
            }
            
        }
        
        private async void ReadOnly_Reading(object sender, NdefReadingEventArgs e)
        {
            try
            {
                await ((NdefReader)sender).MakeReadOnlyAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Cannot make read-only, probably already protected.", "OK");
            }
            finally
            {
                await _cts.CancelAsync();
            }
        }

        private async void WriteButton_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += Writer_Reading;

            _cts = new CancellationTokenSource();
            try
            {
                await _reader.ScanAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
                

            _reader.Reading -= Writer_Reading;
            _reader.Dispose();
        }

        private async void ReadOnlyButton_Clicked(object sender, EventArgs e)
        {
            _reader = new NdefReader();
            _reader.Reading += ReadOnly_Reading;

            _cts = new CancellationTokenSource();

            try
            {
                await _reader.ScanAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _reader.Reading -= ReadOnly_Reading;
            _reader.Dispose();
        }

        protected override void OnDisappearing()
        {
            _cts?.Dispose();
            _reader?.Dispose();
            base.OnDisappearing();
        }
    }

}
