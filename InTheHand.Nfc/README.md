# InTheHand.Nfc
### 32feet.NET - personal area networking for .NET

A simple cross-platform NFC tag reader for .NET 8.0 and above. Supports Android, iOS and Mac Catalyst. The API is based on WebNFC and currently supports reading only.

## Usage

Create new NdefReader
```
var reader = new NdefReader();
reader.Reading += Reader_Reading;
```

Start session
```
await reader.ScanAsync(cancellationToken);
```

Example event handler
```
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
```

## 32feet.NET

Further documentation, including the source, samples, current issues and Wiki are available on [the GitHub repository](https://github.com/inthehand/32feet).