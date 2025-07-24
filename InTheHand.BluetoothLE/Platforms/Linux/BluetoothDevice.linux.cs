//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private bool _watchingAdvertisements = false;
        internal Device _device;

        private static async Task<BluetoothDevice?> PlatformFromId(string id)
        {
            var linuxDevice = await Bluetooth.Adapter.GetDeviceAsync(id);

            if(linuxDevice != null)
            {
                System.Diagnostics.Debug.WriteLine($"BluetoothDevice.FromIdAsync:{linuxDevice.ObjectPath}");

                var bluetoothDevice = (BluetoothDevice)linuxDevice;
                await bluetoothDevice.Init();
                return bluetoothDevice;
            }

            return null;
        }

        public static implicit operator BluetoothDevice(Device device)
        {
            return new BluetoothDevice(device);
        }

        public static implicit operator Device(BluetoothDevice device)
        {
            return device._device;
        }

        private BluetoothDevice(Device device)
        {
            ArgumentNullException.ThrowIfNull(device, nameof(device));

            _device = device;
            _device.Disconnected += _device_Disconnected;
        }

        internal async Task Init()
        {
            try
            {
                var props = await _device.GetAllAsync();
                _id = props.Address;
                _name = props.Name;
                _isPaired = props.Paired;
                await Gatt.InitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            System.Diagnostics.Debug.WriteLine($"BluetoothDevice.Init:{_id} {_name} {_isPaired}");
        }

        private Task _device_Disconnected(Device sender, BlueZEventArgs eventArgs)
        {
            if(eventArgs.IsStateChange)
            {
                OnGattServerDisconnected();
            }
            return Task.CompletedTask;
        }

        private string? _id;

        private string GetId()
        {
            return _id == null ? string.Empty : _id;
        }

        private string? _name;

        private string GetName()
        {
            return _name == null ? string.Empty : _name;
        }

        private RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        private bool _isPaired;

        private bool GetIsPaired()
        {
            return _isPaired;
        }

        private async Task PlatformPairAsync()
        {
            await _device.PairAsync();
            _isPaired = await _device.GetPairedAsync();
        }

        private async Task PlatformPairAsync(string pairingCode)
        {
            var managers = await DBusMethods.GetProxiesAsync<IAgentManager1>("org.bluez.AgentManager1");
            var manager = managers.FirstOrDefault();
            if (manager is null)
            {
                throw new InvalidOperationException("AgentManager1 not found");
            }

            var agent = new PairingAgent(pairingCode);

            await Connection.System.RegisterObjectAsync(agent);
            await manager.RegisterAgentAsync(agent.ObjectPath, "DisplayOnly");
            await manager.RequestDefaultAgentAsync(agent.ObjectPath);

            try
            {
                await PlatformPairAsync();
            }
            finally
            {
                await manager.UnregisterAgentAsync(agent.ObjectPath);
                Connection.System.UnregisterObject(agent);
            }
        }

        /*
        bool GetWatchingAdvertisements()
        {
            return _watchingAdvertisements;
        }

        async Task DoWatchAdvertisements()
        {
            _watchingAdvertisements = true;
        }

        void DoUnwatchAdvertisements()
        {
            _watchingAdvertisements = false;
        }*/

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            BluetoothDevice device = obj as BluetoothDevice;
            if (device != null)
            {
                return device.Id == Id;
            }

            return base.Equals(obj);
        }

        public void Dispose() {}
    }
}
