using System;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using System.Net.Sockets;

namespace ConsoleMenuTesting
{
    partial class BluetoothTesting
    {
        static readonly Guid SvcClassForL2CapTests = BluetoothService.WapClient; // 0x1114
        //
        WeakReferenceT<L2CapClient> _cliL2Cap;

        //--------
        //[MenuItem, SubMenu("L2CAP Play")]
        //public void L2CapNetworkStream()
        //{
        //    var x = new InTheHand.Net.Bluetooth.BlueZ.L2CapNetworkStream(
        //        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified),
        //        true);
        //    console.WriteLine("success");
        //}

        [MenuItem, SubMenu("L2CAP Play")]
        public void L2CapConnect()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Server's BluetoothAddress");
            Guid svcClass = SvcClassForL2CapTests;
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClass);
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("Port number (L2CAP PSM)");
            // to-do: auth/encrypt/setpin
            bool@async = true; //console.ReadYesNo("Async Connect?", false);
            bool asyncCallback = true;
            //if @async) {
            //    asyncCallback = console.ReadYesNo("Async Callback?", true);
            //}
            //
            BluetoothEndPoint rep;
            if (port == null)
                rep = new BluetoothEndPoint(addr, svcClass);
            else
                rep = new BluetoothEndPoint(addr, svcClass, port.Value);
            console.WriteLine("Connecting to: {0}:{1}:{2} ...", rep.Address, rep.Service, rep.Port);
            //
            var cli = new L2CapClient();
            if (@async) {
                AsyncCallback cb = null;
                if (asyncCallback)
                    cb = MiscCallback;
                IAsyncResult ar = cli.BeginConnect(rep,
                    cb, "BeginConnect");
                console.WriteLine("(BeginConnect returned)");
#if NETCF // ReadYesNo will block the screen. :-(
                console.Pause("Paused after BeginConnect");
#endif
                bool closeNow = console.ReadYesNo("Respond when we should proceed to EndConnect.  Enter Y to call Close first.", false);
                if (closeNow) {
                    cli.Close();
                }
                cli.EndConnect(ar);
            } else {
                cli.Connect(rep);
            }
            console.WriteLine("Connected to : {0}", cli.RemoteEndPoint);
            //PrintLocalEndpointIfAvailable(cli);
            PrintMtuEtc(cli);
            //----
            _peer = cli.GetStream();
            _cliL2Cap = new WeakReferenceT<L2CapClient>(cli);
        }

        //void L2CapConnect_Callback(IAsyncResult ar)
        //{
        //    console.WriteLine("Completed...");
        //    var cli = (L2CapClient)ar.AsyncState;
        //    try {
        //        cli.EndConnect(ar);
        //        console.WriteLine("Successfully :-)");
        //    } catch (SocketException soex) {
        //        console.WriteLine("Connect failed: " + Exception_FirstLine(soex));
        //        return;
        //    }
        //    //PrintLocalEndpointIfAvailable(cli);
        //    //
        //    peer = cli.GetStream();
        //    _cliL2Cap = new WeakReferenceT<L2CapClient>(cli);
        //}

        [MenuItem, SubMenu("L2CAP Play")]
        public void L2CapListen()
        {
            L2CapListener lsnr = null;
            try {
                lsnr = L2CapListen_();
                var cli = lsnr.AcceptClient();
                console.WriteLine("Accepted! :-)");
                //
                _peer = cli.GetStream();
                _cliL2Cap = new WeakReferenceT<L2CapClient>(cli);
                console.WriteLine("Got connection from: {0}", cli.RemoteEndPoint);
                //PrintLocalEndpointIfAvailable(cli);
                PrintMtuEtc(cli);
                console.Pause("Un-pause to continue; and close listener");
            } finally {
                if (lsnr != null) { lsnr.Stop(); }
            }
        }

        private L2CapListener L2CapListen_()
        {
            Guid svcClass = SvcClassForL2CapTests;
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClass);
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("PSM");
#if AUTH_ENCR_PIN
            bool auth = console.ReadYesNo("Authenticate", false);
            bool encpt = console.ReadYesNo("Encrypt", false);
            bool setPin1, setPin2;
            string passcode;
            BluetoothAddress pinAddr;
            PromptForSetPin(console, out setPin1, out setPin2, out passcode, out pinAddr);
#endif
            string svcName = console.ReadLine("Service Name (optional)");
            if (svcName != null && svcName.Length == 0)
                svcName = null;
            //
            BluetoothEndPoint lep;
            if (port == null)
                lep = new BluetoothEndPoint(BluetoothAddress.None, svcClass);
            else
                lep = new BluetoothEndPoint(BluetoothAddress.None, svcClass, port.Value);
            console.WriteLine("Going to listen on: {0}:{1}:{2}; auth: {3}, encrypt: {4}.",
                lep.Address, lep.Service, lep.Port, false/*auth*/, false/*encpt*/);
            //
            var lsnr = new L2CapListener(lep);
            if (svcName != null)
                lsnr.ServiceName = svcName;
#if AUTH_ENCR_PIN
            // Assume false by default, so if NotImpl fails only if user says true.
            if (auth)
                lsnr.Authenticate = true;
            if (encpt)
                lsnr.Encrypt = true;
            if (setPin1) {
                Debug.Assert(pinAddr == null, "pinAddr == null");
                //lsnr.SetPin(passcode);
                console.WriteLine("Soooorrrrry we don't support SetPin(String) on listener!!!!");
                throw new NotSupportedException("ConsoleMenuTesting");
            } else if (setPin2) {
                Debug.Assert(pinAddr != null, "pinAddr != null");
                lsnr.SetPin(pinAddr, passcode);
            }
#endif
            console.WriteLine("Starting Listener...");
            lsnr.Start();
            BluetoothEndPoint lepLsnr = (BluetoothEndPoint)lsnr.LocalEndPoint;
            console.WriteLine("Listening on endpoint: {0}", lepLsnr);
            return lsnr;
        }

        //--------
        private void PrintMtuEtc(L2CapClient cli)
        {
            var mtu = cli.GetMtu();
            console.WriteLine("MTU (their MRU): {0}.", mtu);
        }

    }
}
