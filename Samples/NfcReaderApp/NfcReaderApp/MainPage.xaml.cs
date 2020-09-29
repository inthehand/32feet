using InTheHand.Nfc;
using System;
using Xamarin.Forms;

namespace NfcReaderApp
{
    public partial class MainPage : ContentPage
    {
        InTheHand.Nfc.NdefReader reader;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Reader_Error(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("NDEF", "Error", "OK");
            });
        }

        private void Reader_Reading(object sender, InTheHand.Nfc.NdefMessage e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var record in e.Records)
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
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            reader = new NdefReader();
            reader.Reading += Reader_Reading;
            reader.Error += Reader_Error;

            NdefScanOptions options = new NdefScanOptions();
            System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
            options.Signal = cts.Token;
            await reader.ScanAsync(options);

            cts.CancelAfter(30000);
        }
    }
}
