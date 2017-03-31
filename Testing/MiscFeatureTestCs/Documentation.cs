using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Windows.Forms;
using System.Windows.Forms;
using System.IO;
using InTheHand.Net.Bluetooth;
using System.Diagnostics;

namespace MiscFeatureTestCsXXXX
{
    class Documentation : IWin32Window
    {
        ListBox listBox1 = new ListBox();
        #region IWin32Window Members

        IntPtr IWin32Window.Handle
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        //----------------
        #region Discovery
        void Discovery1()
        {
            var cli = new BluetoothClient();
            BluetoothDeviceInfo[] peers = cli.DiscoverDevices();
            BluetoothDeviceInfo device = null;///= ... select one of peer()...
            BluetoothAddress addr = device.DeviceAddress;
        }

        void Discovery2()
        {
            ///...and

            var dlg = new SelectBluetoothDeviceDialog();
            DialogResult result = dlg.ShowDialog(this);
            if (result != DialogResult.OK) {
                return;
            }
            BluetoothDeviceInfo device = dlg.SelectedDevice;
            BluetoothAddress addr = device.DeviceAddress;
        }

        public void Discovery2F()
        {
            EventHandler realWork = delegate
            {
                ///...and

                var dlg = new SelectBluetoothDeviceDialog();
                dlg.DeviceFilter = FilterDevice;
                DialogResult result = dlg.ShowDialog();
                if (result != DialogResult.OK) {
                    return;
                }
                BluetoothDeviceInfo device = dlg.SelectedDevice;
                BluetoothAddress addr = device.DeviceAddress;
            };
            /////
            Application.Idle += realWork;
            Application.Run();

        }

        static bool FilterDevice(BluetoothDeviceInfo dev)
        {
            var ret = true;
            var rslt = MessageBox.Show("Include this device " + dev.DeviceAddress + " " + dev.DeviceName, "FilterDevice",
                MessageBoxButtons.YesNo);
            ret = rslt == DialogResult.Yes;
            Debug.Assert(rslt == DialogResult.Yes || rslt == DialogResult.No);
            return ret;
        }

        void Discovery3()
        {
            ///...Finally, the discovered devices can be added to a Windows Form’s control with Data Binding, using code like the following, which uses a drop-down list box.
            var cli = new BluetoothClient();
            BluetoothDeviceInfo[] peers = cli.DiscoverDevices();
            this.listBox1.DisplayMember = "DeviceName";
            this.listBox1.ValueMember = null;
            this.listBox1.DataSource = peers;
        }
        #endregion

        #region General Data
        class MyConsts
        {
            static readonly Guid MyServiceUuid
              = new Guid("{00112233-4455-6677-8899-aabbccddeeff}");
        }
        void GeneralData1()
        {
            BluetoothAddress addr
              = BluetoothAddress.Parse("001122334455");
            Guid serviceClass;
            serviceClass = BluetoothService.SerialPort;
            // - or - etc
            // serviceClass = MyConsts.MyServiceUuid
            //
            var ep = new BluetoothEndPoint(addr, serviceClass);
            var cli = new BluetoothClient();
            cli.Connect(ep);
            Stream peerStream = cli.GetStream();
            // peerStream.Write/Read ...

            // e.g.
            byte[] buf = new byte[1000];
            int readLen = peerStream.Read(buf, 0, buf.Length);
            if (readLen == 0) {
                Console.WriteLine("Connection is closed");
            } else {
                Console.WriteLine("Recevied {0} bytes", readLen);
            }
        }
        #endregion

        #region Bluetooth Server-side
        class MyConstsXXXXXXXXXXXXXXXX
        {
            static readonly Guid MyServiceUuid
              = new Guid("{00112233-4455-6677-8899-aabbccddeeff}");
        }
        void Server1()
        {
            //...
            Guid serviceClass;
            serviceClass = BluetoothService.SerialPort;
            // - or - etc
            // serviceClass = MyConsts.MyServiceUuid
            //
            var lsnr = new BluetoothListener(serviceClass);
            lsnr.Start();


            // Now accept new connections, perhaps using the thread pool to handle each
            BluetoothClient conn = lsnr.AcceptBluetoothClient();
            Stream peerStream = conn.GetStream();
            ///...

            // etc
            conn = lsnr.AcceptBluetoothClient();
            peerStream = conn.GetStream();
            //...
        }
        #endregion

        #region Local Radio Information
        void DisplayBluetoothRadio()
        {
            BluetoothRadio myRadio = BluetoothRadio.PrimaryRadio;
            if (myRadio == null) {
                Console.WriteLine("No radio hardware or unsupported software stack");
                return;
            }
            RadioMode mode = myRadio.Mode;
            /// Warning: LocalAddress can be null if the radio is powered-off.
            Console.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress);
            Console.WriteLine("Mode: " + mode.ToString());
            Console.WriteLine("Name: " + myRadio.Name);
            Console.WriteLine("HCI Version: " + myRadio.HciVersion
                + ", Revision: " + myRadio.HciRevision);
            Console.WriteLine("LMP Version: " + myRadio.LmpVersion
                + ", Subversion: " + myRadio.LmpSubversion);
            // Too Long: Console.WriteLine("LMP Features: {" + myRadio.LmpFeatures + " }"); ;
            Console.WriteLine("ClassOfDevice: " + myRadio.ClassOfDevice.ToString()
                + ", device: " + myRadio.ClassOfDevice.Device.ToString()
                + " / service: " + myRadio.ClassOfDevice.Service.ToString());
            //
            //
            // Enable discoverable mode
            Console.WriteLine();
            myRadio.Mode = RadioMode.Discoverable;
            Console.WriteLine("Radio Mode now: " + myRadio.Mode.ToString());
        }
        #endregion

        #region OBEX
        void Obex1()
        {
            BluetoothAddress addr = BluetoothAddress.Parse("002233445566");
            String path = "HelloWorld.txt";
            //
            var req = new ObexWebRequest(addr, path);
            req.ReadFile("Hello World.txt");
            ObexWebResponse rsp = (ObexWebResponse)req.GetResponse();
            Console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode);
        }

        void ObexSvr1()
        {
            var lsnr = new ObexListener(ObexTransport.Bluetooth);
            lsnr.Start();
            // For each connection
            ObexListenerContext ctx = lsnr.GetContext();
            ObexListenerRequest req = ctx.Request;
            String[] pathSplits = req.RawUrl.Split('/');
            String filename = pathSplits[pathSplits.Length - 1];
            req.WriteFile(filename);
            //
            lsnr.Stop();
        }
        #endregion

    }
}
