# InTheHand.Bluetooth.Permissions
### 32feet.NET - personal area networking for .NET

Bluetooth permissions for .NET MAUI 7.0 and Xamarin Forms

This library adds a Bluetooth permission instance for .NET MAUI 7.0 and Xamarin Forms. 
For .NET MAUI 8.0 use the built-in MAUI equivalent instead (they have been designed to match).

## Usage

Check permission using
```
PermissionStatus status = await Permissions.CheckStatusAsync<InTheHand.Bluetooth.Permissions.Bluetooth>();
```

Request permission using
```
PermissionStatus status = await Permissions.RequestAsync<InTheHand.Bluetooth.Permissions.Bluetooth>();
```

Read more about the permission requirements on different platforms [in the Wiki](https://github.com/inthehand/32feet/wiki/Permissions).
## 32feet.NET

Further documentation, including the source, samples, current issues and Wiki are available on [the GitHub repository](https://github.com/inthehand/32feet).