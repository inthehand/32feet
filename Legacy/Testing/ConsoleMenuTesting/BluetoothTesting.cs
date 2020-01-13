//
//-
#define RADIO_MODESSS
//
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using System.Threading;
using System.Diagnostics;
using Console = System.ValueType; // Block the use of Console, must use "console".
using System.Reflection;
using InTheHand.Net.Bluetooth.AttributeIds;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Globalization;
using System.Net;
using System.ComponentModel;
#if !ANDROID
using System.IO.Ports;
#endif
using InTheHand.Net.Bluetooth.Factory;

namespace ConsoleMenuTesting
{
    partial class BluetoothTesting
    {
        public MenuSystem console; //HACK public MenuSystem console;
        Stream _peer;

        public BluetoothTesting()
        {
        }

        //----

        WeakReferenceT<BluetoothClient> _cli;

        class WeakReferenceT<T> : WeakReference
        {
            public WeakReferenceT(T target)
                : base(target)
            {
            }

            public new T Target
            {
                get { return (T)base.Target; }
                set { base.Target = value; }
            }
        }

        //--------

#if false && ANDROID
        [MenuItem, SubMenu("PLAYING")]
        public void CheckAndroidNetworkStream()
        {
            var tmp = new MemoryStream();
            var ns = new InTheHand.Net.Bluetooth.Droid.AndroidBthClient.AndroidNetworkStream(tmp);
            console.WriteLine("Done create AndroidNetworkStream.");
            ns.WriteByte(11);
            console.WriteLine("Done write to AndroidNetworkStream.");
            ns.Position = 0;
            var bi = ns.ReadByte();
            console.WriteLine("Done read from AndroidNetworkStream.");
        }
#endif

#if true || DEBUG
        [MenuItem, SubMenu("TEST")]
        public void TestPause()
        {
            console.WriteLine("1111");
            console.WriteLine("2222");
            console.WriteLine("3333");
            console.WriteLine("4444");
            console.WriteLine("55555");
            console.WriteLine("Before Pause");
            console.Pause("Message");
            console.WriteLine("After Pause");
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadLine()
        {
            var i = console.ReadLine("foo\r\nbar");
            if (i == null) {
                console.WriteLine("you input: <null>");
            } else {
                console.WriteLine("you input: [[{0}]], len: {1}", i, i.Length);
            }
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadBluetoothAddress()
        {
            var a = console.ReadBluetoothAddress("foo\r\nbar");
            if (a == null) {
                console.WriteLine("you input: <null>");
            } else {
                console.WriteLine("you input: [[{0}]]", a);
            }
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadOptionalBluetoothAddress()
        {
            var a = console.ReadOptionalBluetoothAddress("foo\r\nbar");
            if (a == null) {
                console.WriteLine("you input: <null>");
            } else {
                console.WriteLine("you input: [[{0}]]", a);
            }
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadOptionalInteger()
        {
            var i = console.ReadOptionalInteger("foobar");
            if (i == null) {
                console.WriteLine("you input: <null>");
            } else {
                console.WriteLine("you input: [[{0}]]", i.Value);
            }
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadYesNo()
        {
            bool i = console.ReadYesNo("foobarYN (default: true)", true);
            console.WriteLine("you input: {0}", i);
            i = console.ReadYesNo("foobarYN (default: false)", false);
            console.WriteLine("you input: {0}", i);
        }

        [MenuItem, SubMenu("TEST")]
        public void TestReadYesNoCancel()
        {
            bool? i = console.ReadYesNoCancel("foobarYNC (default: true)", true);
            console.WriteLine("you input: {0}", i);
            i = console.ReadYesNoCancel("foobarYNC (default: false)", false);
            console.WriteLine("you input: {0}", i);
            i = console.ReadYesNoCancel("foobarYNC (default: cancel)", null);
            console.WriteLine("you input: {0}", i);
        }

        [MenuItem, SubMenu("TEST")]
        public void TestGetFilename()
        {
            var a = console.GetFilename();
            if (a == null) {
                console.WriteLine("you input: <null>");
            } else {
                console.WriteLine("you input: [[{0}]]", a);
            }
        }
#endif

        [MenuItem, SubMenu("Playing")]
        public void DoBthReadRemoteVersion()
        {
            var addr = console.ReadBluetoothAddress("remote");
            var ba = addr.ToByteArray();
            //
            byte lmpVersion;
            ushort lmpSubVersion, manuf0;
            byte[] lmpFeatures = new byte[20];
            int ret = BthReadRemoteVersion(ba,
                out lmpVersion, out lmpSubVersion, out manuf0, lmpFeatures);
            console.WriteLine("ret: {0}=0x{0:X}", ret);
            var manuf = (Manufacturer)manuf0;
            console.WriteLine("lmpVersion: {0}, lmpSubVersion: {1},"
                + " manuf {2} {3}, lmpFeatures[0]: 0x{4:X}",
                lmpVersion, lmpSubVersion, manuf, manuf, lmpFeatures[0]);
        }

        private const string btdrtDll = "btdrt.dll";
        [DllImport(btdrtDll, SetLastError = true)]
        public static extern int BthReadRemoteVersion(byte[] pba, out byte plmp_version,
            out ushort plmp_subversion, out ushort pmanufacturer, byte[] plmp_features);


        [MenuItem, SubMenu("BtLsnr")]
        public void SomeCustomUuids()
        {
            var list = new Guid[] {
                new Guid("{7630F141-6361-4e60-A8EE-237180DA9CE8}"),
                new Guid("{C7CB927D-8DBE-44f2-B671-3DF8D50C4E2F}"),
                new Guid("{59438E67-6094-4bbc-B500-511DE96BB535}"),
                new Guid("{C50AFF29-3C38-4fb7-AB32-D541AD835C34}"),
            };
            foreach (var g in list) {
                console.WriteLine(g);
            }
        }

        //[MenuItem, SubMenu("TEST")]
        //TODO public void ExceptionNative()
        //{
        //    IntPtr p = new IntPtr(1);
        //    var o = Marshal.PtrToStructure(p, typeof(Int64));
        //}

        [MenuItem, SubMenu("TEST")]
        public void ExceptionManaged()
        {
            if (!console.ReadYesNo("Throw exception", false)) return;
            throw new RankException("test");
        }

        [MenuItem, SubMenu("TEST")]
        public void ExceptionManagedOnDelegateAsyncInvoke()
        {
            Action<Version> dlgt = delegate(Version fake)
            {
                console.WriteLine("(Thread gonna throw...)");
                throw new RankException("test");
            };
            if (!console.ReadYesNo("Throw exception", false)) return;
            var ar = DelegateExtension.BeginInvoke(dlgt, null, null, null);
            DelegateExtension.EndInvoke(dlgt, ar);
        }

        //----
        [MenuItem, SubMenu("Playing")]
        public void Base64Decode()
        {
            string s = console.ReadLine("Base64 string");
            console.WriteLine("Input Base64 string: '{0}'", s);
            byte[] arr = Convert.FromBase64String(s);
            var asStr = GetEncoding_forHTC().GetString(arr, 0, Math.Min(20, arr.Length));
            console.WriteLine("asStr: '{0}'", asStr);
        }

        private static Encoding GetEncoding_forHTC()
        {
            var enc = (Encoding)Encoding.ASCII.Clone();
#if !NETCF
            enc.DecoderFallback = new System.Text.DecoderReplacementFallback(".");
            enc.EncoderFallback = new System.Text.EncoderReplacementFallback(".");
#endif
            return enc;
        }

        [MenuItem, SubMenu("Playing")]
        public void Base64Encode()
        {
            string s = console.ReadLine("Input String");
            console.WriteLine("Input string: '{0}'", s);
            var arr = GetEncoding_forHTC().GetBytes(s);
            var b64str = Convert.ToBase64String(arr);
            console.WriteLine("b64str: '{0}'", b64str);
        }

        //----
        [MenuItem, SubMenu("Device Discovery")]
        public void Ebap_NotImplHandlers()
        {
            var bco2 = new BluetoothComponent();
            bco2.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(bco2_DiscoverDevicesComplete);
            bco2.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(bco2_DiscoverDevicesProgress);
            bco2.DiscoverDevicesAsync(255, true, true, true, false, null);
        }

        void bco2_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            throw new NotImplementedException();
        }

        void bco2_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            throw new NotImplementedException();
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void _EbapAsyncDiscoDevices()
        {
            bool discoOnly, auth, rembd, unk;
            GetDiscoveryFlags(console, out discoOnly, out auth, out rembd, out unk);
            //
            var cli = Create_BluetoothClient();
            //cli.InquiryLength = TimeSpan.FromMinutes(1);
            BluetoothComponent bco = new BluetoothComponent(cli);
#if NETCF
            //bco.SetSynchronizeInvoke((System.Windows.Forms.Control)console.InvokeeControl);
#endif
            bco.DiscoverDevicesProgress += _DiscoDevicesAsync_ProgressCallback;
            bco.DiscoverDevicesComplete += _DiscoDevicesAsync_CompleteCallback;
            EventHandler startAction = delegate
            {
                bco.DiscoverDevicesAsync(255, auth, rembd, unk, discoOnly, 99);
            };
            bool startOnUi = console.ReadYesNo("Start from UI thread", true);
            if (startOnUi) {
                console.UiInvoke(startAction);
            } else {
                startAction(null, EventArgs.Empty);
            }
            console.WriteLine("Started at {0} (UTC {1}).", GetTimeWallclockHR(), DateTime.UtcNow.ToLongTimeString());
            console.Pause("After completion un-pause to continue");
        }

        void _DiscoDevicesAsync_CompleteCallback(object sender, DiscoverDevicesEventArgs e)
        {
            Debug.Assert((int)e.UserState == 99);
            CheckSyncCtx();
            console.WriteLine("At {0} {1}, ", GetTimeWallclockHR(), "Complete");
            if (WriteAsyncCompletedState("DiscoDevicesAsync", e)) {
                console.WriteLine("DiscoDevicesAsync found {0} devices.", e.Devices.Length);
                _DiscoDevicesAsync_ShowDevices(e);
            }
            console.WriteLine("Complete.");
        }

        void _DiscoDevicesAsync_ProgressCallback(object sender, DiscoverDevicesEventArgs e)
        {
            var nowUtc = DateTime.UtcNow;
            Debug.Assert((int)e.UserState == 99);
            CheckSyncCtx();
            console.WriteLine("At {0}, {1}, {2} devices:",
                GetTimeWallclockHR(), "async progress",
                e.Devices.Length);
            _DiscoDevicesAsync_ShowDevices(e);
        }

        private void _DiscoDevicesAsync_ShowDevices(DiscoverDevicesEventArgs e)
        {
            foreach (var dev in e.Devices) {
                console.WriteLine("* {0} {1}, last seen: {2}", dev.DeviceAddress, ToStringQuotedOrNull(dev.DeviceName),
                    dev.LastSeen == DateTime.MinValue ? "NEVER" : dev.LastSeen.ToString("o"));
            }
        }

        private bool WriteAsyncCompletedState(string name,
            AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) {
                console.WriteLine("Cancelled: " + name);
            } else if (e.Error != null) {
                console.WriteLine("Error in: " + name + ", error: {0}.", Exception_FirstLine(e.Error));
            } else {
                console.WriteLine("Successful Completion of: " + name);
                return true;
            }
            return false;
        }

        private void CheckSyncCtx()
        {
            bool? bad = console.InvokeRequired;
            if (bad == true) {
                console.WriteLine("Event not called on the UI thread!!!!");
            }
        }

        //--------

        BluetoothRadio m_factoryRadio;
        BluetoothPublicFactory m_factory;

        // TODO Should BluetoothPublicFactory have a Primary/AllRadios property?
        /// <summary>
        /// --can return null--
        /// </summary>
        /// <returns></returns>
        BluetoothRadio Get_BluetoothRadio()
        {
            if (m_factory == null)
                return BluetoothRadio.GetPrimaryRadio();
            BluetoothRadio radio = m_factoryRadio;
            if (radio == null)
                throw new InvalidOperationException("No active radios.");
            //ClassOfDevice checkNonNull = radio.ClassOfDevice;
            return radio;
        }

        BluetoothDeviceInfo Create_BluetoothDeviceInfo(BluetoothAddress addr)
        {
            if (m_factory == null)
                return new BluetoothDeviceInfo(addr);
            return m_factory.CreateBluetoothDeviceInfo(addr);
        }

        BluetoothClient Create_BluetoothClient()
        {
            if (m_factory == null)
                return new BluetoothClient();
            return m_factory.CreateBluetoothClient();
        }

        private BluetoothClient Create_BluetoothClient(BluetoothEndPoint localEndPoint)
        {
            if (m_factory == null)
                return new BluetoothClient(localEndPoint);
            return m_factory.CreateBluetoothClient(localEndPoint);
        }

        BluetoothListener Create_BluetoothListener(Guid svc)
        {
            if (m_factory == null)
                return new BluetoothListener(svc);
            return m_factory.CreateBluetoothListener(svc);
        }

        BluetoothListener Create_BluetoothListener(Guid svc, ServiceRecord record)
        {
            if (m_factory == null)
                return new BluetoothListener(svc, record);
            return m_factory.CreateBluetoothListener(svc, record);
        }

        BluetoothListener Create_BluetoothListener(BluetoothEndPoint ep)
        {
            if (m_factory == null)
                return new BluetoothListener(ep);
            return m_factory.CreateBluetoothListener(ep);
        }

        BluetoothListener Create_BluetoothListener(BluetoothEndPoint ep, ServiceRecord record)
        {
            if (m_factory == null)
                return new BluetoothListener(ep, record);
            return m_factory.CreateBluetoothListener(ep, record);
        }

        ObexWebRequest Create_ObexWebRequest(Uri requestUri)
        {
            if (m_factory == null)
                return new ObexWebRequest(requestUri);
            return m_factory.CreateObexWebRequest(requestUri);
        }

        ObexWebRequest Create_ObexWebRequest(string scheme, BluetoothAddress target, string path)
        {
            if (m_factory == null)
                return new ObexWebRequest(target, path);
            return m_factory.CreateObexWebRequest(scheme, target, path);
        }

        ObexListener Create_ObexListener_Bluetooth()
        {
            if (m_factory == null)
                return new ObexListener(ObexTransport.Bluetooth);
            return m_factory.CreateObexListener();
        }

#if !ANDROID
        L2CapListener Create_L2CapListener(Guid svc)
        {
            //if (m_factory == null)
            return new L2CapListener(svc);
            //return m_factory.CreateL2CapListener(svc);
        }

        private L2CapListener Create_L2CapListener(BluetoothEndPoint localEndPoint)
        {
            //if (m_factory == null)
            return new L2CapListener(localEndPoint);
            //return m_factory.CreateL2CapListener(localEndPoint);
        }
#endif


        [MenuItem]
        public void SetStack_Factory()
        {
            BluetoothRadio[] list = BluetoothRadio_GetAllAndDump();
            if (list.Length == 0) {
                console.WriteLine("No radios found!");
                m_factory = null;
                m_factoryRadio = null;
            }
            int? choice;
            do {
                choice = console.ReadOptionalInteger(string.Format("Which stack for factory (1-{0}), 0 for \"new\"-direct", list.Length));
                if (choice == null) choice = 0;
            } while (choice < 0 || choice > list.Length);
            if (choice == 0) {
                m_factory = null;
                m_factoryRadio = null;
                console.WriteLine("Selected #{0} 'new-direct'.", choice);
            } else {
                m_factoryRadio = list[choice.Value - 1];
                m_factory = m_factoryRadio.StackFactory;
                console.WriteLine("Selected #{0}, stack: '{1}' address: {2}.", choice,
                    m_factoryRadio.SoftwareManufacturer, m_factoryRadio.LocalAddress);
            }
        }


        //--------
        private string GetTimeWallclockHR()
        {
            return DateTime.Now.TimeOfDay.ToString();
        }

        private string GetTime()
        {
            return Environment.TickCount.ToString();
        }

        void ErrorFromMiscCallback(IAsyncResult ar)
        {
            MiscCallback(ar);
            throw new RankException();
        }

        void MiscCallback(IAsyncResult ar)
        {
            console.WriteLine("{1}: Callback called for \"{0}\"", ar.AsyncState, GetTime());
        }

        void EmptyCallback(IAsyncResult ar)
        {
            if (ar.CompletedSynchronously) {
            } else {
            }
        }

        bool VerifyConnectionWrite()
        {
            if (_peer == null) {
                console.WriteLine("No connection!");
                return false;
            }
            if (!_peer.CanWrite) {
                if (!console.ReadYesNo("NOT CanWrite, do operation anyway", false))
                    return false;
            }
            return true;
        }

        bool VerifyConnectionRead()
        {
            if (_peer == null) {
                console.WriteLine("No connection!");
                return false;
            }
            if (!_peer.CanRead) {
                if (!console.ReadYesNo("NOT CanRead, do operation anyway", false))
                    return false;
            }
            return true;
        }

        //--------
        [MenuItem, SubMenu("Data")]
        public void PrintStreamState()
        {
            if (_peer == null) {
                console.WriteLine("Stream is null.");
            } else {
                console.WriteLine("Stream has CanRead: {0}, CanWrite: {1}.",
                    _peer.CanRead, _peer.CanWrite);
            }
            if (_cli == null) {
                console.WriteLine("BtClient is null.");
            } else {
                var cli = _cli.Target;
                if (cli == null) {
                    console.WriteLine("BtClient has been GC'd away.");
                } else {
                    console.WriteLine("BtClient has Connected: {0}.",
                        cli.Connected);
                }
            }
        }

        [MenuItem, SubMenu("Data")]
        public void CloseStream()
        {
            if (_peer != null) {
                _peer.Close();
                _peer = null;
            }
        }

        [MenuItem, SubMenu("Data")]
        public void CloseStream_ButDontNullIt()
        {
            if (_peer != null) {
                _peer.Close();
            }
        }

        [MenuItem, SubMenu("Data")]
        public void NullStreamAndClient()
        {
            _peer = null;
            _cli = null;
        }

        [MenuItem, SubMenu("Data")]
        public void CloseClientIfExists()
        {
            if (_cli != null) {
                BluetoothClient cli = _cli.Target;
                if (cli != null) {
                    cli.Close();
                    _cli = null;
                } else {
                    console.WriteLine("The reference has been GC'd.");
                }
            }
        }

        private BluetoothListener ListenInit()
        {
            Guid svcClass = BluetoothService.Wap;
            console.WriteLine("Default UUID : {0}", svcClass);
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClass);
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("Port number");
            bool auth = console.ReadYesNo("Authenticate", false);
            bool encpt = console.ReadYesNo("Encrypt", false);
            bool setPin1, setPin2;
            string passcode;
            BluetoothAddress pinAddr;
            PromptForSetPin(console, out setPin1, out setPin2, out passcode, out pinAddr);
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
                lep.Address, lep.Service, lep.Port, auth, encpt);
            //
            ServiceRecord sr = null;
            if (lep.Service == BluetoothService.DialupNetworking) {
                var bldr = new ServiceRecordBuilder();
                bldr.AddServiceClass(lep.Service);
                bldr.AddServiceClass(BluetoothService.GenericNetworking);
                bldr.AddBluetoothProfileDescriptor(lep.Service, 1, 0);
                sr = bldr.ServiceRecord;
                console.WriteLine("Adding custom record: "
                    + ServiceRecordUtilities.Dump(sr));
            }
            BluetoothListener lsnr;
            if (sr == null) {
                lsnr = Create_BluetoothListener(lep);
            } else {
                lsnr = Create_BluetoothListener(lep, sr);
            }
            if (svcName != null)
                lsnr.ServiceName = svcName;
            // Assume false by default, so if NotImpl fails only if user says true.
            if (auth)
                lsnr.Authenticate = true;
            if (encpt)
                lsnr.Encrypt = true;
            if (setPin1) {
                Debug.Assert(pinAddr == null, "pinAddr == null");
                //lsnr.SetPin(passcode);
                console.WriteLine("Soooorrrrry we don't support SetPin(String) on listener!!!!");
                throw new NotSupportedException("MenuTesting");
            } else if (setPin2) {
                Debug.Assert(pinAddr != null, "pinAddr != null");
                lsnr.SetPin(pinAddr, passcode);
            }
            console.WriteLine("Starting Listener...");
            lsnr.Start();
            BluetoothEndPoint lepLsnr = (BluetoothEndPoint)lsnr.LocalEndPoint;
            console.WriteLine("Listening on endpoint: {0}", lepLsnr);
            return lsnr;
        }

        //[MenuItem, SubMenu("BtLsnr")]
        //public void ListenStartStopStart()
        //{
        //    Guid svcClass = BluetoothService.Wap;
        //    console.WriteLine("Default UUID : {0}", svcClass);
        //    Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID");
        //    if (inputGuid.HasValue)
        //        svcClass = inputGuid.Value;
        //    BluetoothListener lsnr = Create_BluetoothListener(svcClass);
        //    lsnr.Start();
        //    lsnr.Stop();
        //    lsnr.Start();
        //    //
        //    lsnr = null;
        //    GC.Collect();
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //}

        [MenuItem, SubMenu("BtLsnr")]
        public void Listen()
        {
            BluetoothListener lsnr = ListenInit();
            BluetoothClient cli;
            bool startStopStartEtc = console.ReadYesNo("Start-Stop-Start-Stop-Start-Finalize", false);
            if (startStopStartEtc) {
                lsnr.Stop();
                lsnr.Start();
                console.WriteLine("Stopped and restarted.");
            }
            bool @async = console.ReadYesNo("Async Accept", false);
            if (!@async) {
                bool callPending = console.ReadYesNo("Loop on Pending()", false);
                if (callPending) {
                    int i = 0;
                    while (!lsnr.Pending()) {
                        console.WriteLine("Not Pending" + new string('.', (i + 3) % 3));
                        const int HalfSecond = 500;
                        Thread.Sleep(HalfSecond);
                        ++i;
                    }
                    console.WriteLine("Is Pending...");
                }
                //bool closeNow = console.ReadYesNo("Hit return to proceed to EndConnect.  Enter Y to call Close first.", false);
                console.WriteLine("calling Accept...");
                cli = lsnr.AcceptBluetoothClient();
            } else {
                console.WriteLine("Waiting for connection...");
                IAsyncResult ar = lsnr.BeginAcceptBluetoothClient(MiscCallback, "BeginAcceptBluetoothClient");
                console.WriteLine("(BeginAccept returned)");
                bool startStopStartEtcAfter = console.ReadYesNo("Start-Stop-Start-Stop-Start-Finalize (will break *us*)", false);
                if (startStopStartEtcAfter) {
                    lsnr.Stop();
                    lsnr.Start();
                    console.WriteLine("Stopped and restarted.");
                }
#if !NETCF
                ManualResetEvent quit = console.NewManualResetEvent(false);
                int signalledIdx = WaitHandle.WaitAny(new WaitHandle[] { ar.AsyncWaitHandle, quit });
                if (signalledIdx == 1) {
                    // ?Do we want to test disposal or Finalization here?
                    // lsnr.Stop()
                    _peer = null;
                    return;
                }
#endif
                cli = lsnr.EndAcceptBluetoothClient(ar);
            }
            //
            _peer = cli.GetStream();
            _cli = new WeakReferenceT<BluetoothClient>(cli);
            console.WriteLine("Got connection from: '{0}' {1}", cli.RemoteMachineName, cli.RemoteEndPoint);
            PrintLocalEndpointIfAvailable(cli);
            console.Pause("Un-pause to continue; and close listener");
            if (!startStopStartEtc) {
                lsnr.Stop();
            } else {
                lsnr.Stop();
                lsnr.Start();
                //
                bool x = console.ReadYesNo("Do Async Accept (before Finalization)", false);
                IAsyncResult arLast = null;
                if (x) {
                    arLast = lsnr.BeginAcceptBluetoothClient(MiscCallback, "BeginAcceptBluetoothClient");
                    console.WriteLine("(BeginAccept returned)");
                }
                lsnr = null;
                GC.Collect();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                console.WriteLine("Stopped, restarted, and Finalized.");
                if (arLast != null) {
                    console.WriteLine("arLastL: {0}", arLast.IsCompleted);
                    // Can't call EndAccept as no lsnr reference!
                }
            }
        }

        [MenuItem, SubMenu("BtLsnr")]
        public void ListenAndImmediatelyClose()
        {
            ListenAndImmediatelySendAndClose_(null);
        }

        [MenuItem, SubMenu("BtLsnr")]
        public void ListenAndImmediatelySendOneByteAndClose()
        {
            ListenAndImmediatelySendAndClose_(new byte[] { (byte)'a' });
        }

        void ListenAndImmediatelySendAndClose_(byte[] data)
        {
            BluetoothListener lsnr = ListenInit();
            console.WriteLine("Waiting for connection...");
            BluetoothClient cli = lsnr.AcceptBluetoothClient();
            console.WriteLine("Got connection from: {0}!", cli.RemoteMachineName);
            _peer = cli.GetStream();
            if (data != null)
                _peer.Write(data, 0, data.Length);
            _peer.Close();
            _peer = null;
            console.WriteLine("Got connection from ({2}and closed it!): '{0}' {1}",
                cli.RemoteMachineName, cli.RemoteEndPoint,
                (data != null ? "sent " : string.Empty));
            lsnr.Stop();
        }

        [MenuItem, SubMenu("Data")]
        public void SendOneByteAndImmediatelyClose()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] data = { (byte)'a' };
            _peer.Write(data, 0, data.Length);
            _peer.Close();
        }

        [MenuItem, SubMenu("BtLsnr")]
        public void ListenAcceptMultiple()
        {
            BluetoothListener lsnr = ListenInit();
            ManualResetEvent quit = console.NewManualResetEvent(false);
            ListenAcceptMultipleState state = new ListenAcceptMultipleState();
            state.lsnr = lsnr;
            state.quit = quit;
            bool keepOpen = console.ReadYesNo("Keep reference to all connections?", false);
            state.KeepAllConnections = keepOpen;
            state.ReadToEnd = console.ReadYesNo("ReadToEnd of all connections?", false);
            lsnr.BeginAcceptBluetoothClient(ListenAcceptMultiple_Callback, state);
            console.Pause("Un-pause to stop accepting");
            quit.Set();
            lsnr.Stop();
        }
        struct ListenAcceptMultipleState
        {
            public BluetoothListener lsnr;
            public ManualResetEvent quit;
            public int num;
            //
            bool storeAllConnections;
            public List<BluetoothClient> connList;
            public bool ReadToEnd;

            public bool KeepAllConnections
            {
                get { return storeAllConnections; }
                set
                {
                    storeAllConnections = value;
                    if (storeAllConnections && connList == null) {
                        connList = new List<BluetoothClient>();
                    }
                }
            }
        }

        struct ListenAcceptMultipleConnectionState
        {
            public BluetoothClient Cli;
            public int Id;
        }

        void ListenAcceptMultiple_Callback(IAsyncResult arCb)
        {
            ListenAcceptMultipleState state = (ListenAcceptMultipleState)arCb.AsyncState;
            BluetoothClient cli;
            try {
                cli = state.lsnr.EndAcceptBluetoothClient(arCb);
                if (state.KeepAllConnections) {
                    state.connList.Add(cli);
                }
            } catch (ArgumentException ex) {
                console.WriteLine("Accept failed, due to quit?  Exiting.  "
                    + Exception_FirstLine(ex));
                return;
            } catch (ObjectDisposedException ex) {
                console.WriteLine("Accept failed, due to quit?  Exiting.  "
                    + Exception_FirstLine(ex));
                return;
            }
            Stream peer = cli.GetStream();
            console.WriteLine("At " + DateTime.Now.ToLongTimeString() + " got connection #" + (state.num + 1) + " from: '{0}' {1}", cli.RemoteMachineName, cli.RemoteEndPoint);
            ++state.num;
            if (!IsSet(state.quit))
                state.lsnr.BeginAcceptBluetoothClient(ListenAcceptMultiple_Callback, state);
            if (state.ReadToEnd) {
                ThreadPool.QueueUserWorkItem(ReadToEnd_Runner,
                    new ListenAcceptMultipleConnectionState { Cli = cli, Id = state.num });
            }
            if (_peer == null) {
                _peer = peer;
                console.WriteLine("Connection now active.");
            }
        }

        void ReadToEnd_Runner(object state)
        {
            var state2 = (ListenAcceptMultipleConnectionState)state;
            var conn = state2.Cli.GetStream();
            var buf = new byte[1000];
            while (true) {
                try {
                    int readLen = conn.Read(buf, 0, buf.Length);
                    console.WriteLine("Conn#{0}: Read '{1}' bytes",
                        state2.Id, readLen);
                    if (readLen == 0)
                        break;
                } catch (Exception ex) {
                    console.WriteLine("Conn#{0}: exception: {1}",
                        state2.Id, Exception_FirstLine(ex));
                    break;
                }
            }//while
            console.WriteLine("Conn#{0}: Exit at {1}",
                state2.Id, DateTime.Now.ToLongTimeString());
        }


        [MenuItem, SubMenu("SmokeTest")]
        public void NewBt_Endpoint()
        {
            var ep = new BluetoothEndPoint(BluetoothAddress.None, Guid.Empty);
            console.WriteLine("BluetoothEndPoint created successfully.");
        }

        [MenuItem, SubMenu("SmokeTest")]
        public void NewBtEndpoint_fromSocketAddress()
        {
            var ep = new BluetoothEndPoint(BluetoothAddress.Parse("002233445566"), BluetoothService.AttProtocol);
            console.WriteLine("BluetoothEndPoint created successfully.");
            var sa = ep.Serialize();
            console.WriteLine("SocketAddress created successfully.");
            var ep2 = ep.Create(sa);
            console.WriteLine("BluetoothEndPoint created successfully.");
        }

        [MenuItem, SubMenu("SmokeTest")]
        public void NewBt_Cli()
        {
            using (BluetoothClient cli = Create_BluetoothClient()) {
                console.WriteLine("BluetoothClient created successfully.");
                console.ReadLine("BluetoothClient created.  Continue to dispose");
            }
        }

        [MenuItem, SubMenu("SmokeTest")]
        public void NewBt_Lsnr()
        {
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.Wap);
            try {
                console.WriteLine("BluetoothListener created, now calling Start.");
                lsnr.Start();
                console.ReadLine("Started.  Continue to dispose");
            } finally {
                if (lsnr != null)
                    lsnr.Stop();
            }
        }

        BluetoothAddress _simpleConnectAddress;

        [MenuItem, SubMenu("BtClient")]
        public void Connect_SimpleClosingFirst()
        {
            if (_peer != null && console.ReadYesNo("Close (possible) connection first", false)) {
                CloseStream();
            }
            //
            BluetoothAddress addr;
            if (_simpleConnectAddress != null
                    && console.ReadYesNo("Use server address: " + _simpleConnectAddress, true)) {
                addr = _simpleConnectAddress;
            } else {
                addr = console.ReadBluetoothAddress("Server's BluetoothAddress");
            }
            ConnectSimpleClosingFirstPart2(addr);
        }

        void ConnectSimpleClosingFirstPart2(BluetoothAddress addr)
        {
            Guid svcClass = BluetoothService.Wap;
            BluetoothEndPoint rep = new BluetoothEndPoint(addr, svcClass);
            BluetoothClient cli = Create_BluetoothClient();
            _simpleConnectAddress = addr;
            //
            console.WriteLine("Connecting to: {0}:{1}:{2} ...", rep.Address, rep.Service, rep.Port);
            cli.Connect(rep);
            console.WriteLine("Connected to : '{0}' {1}", cli.RemoteMachineName, cli.RemoteEndPoint);
            PrintLocalEndpointIfAvailable(cli);
            _peer = cli.GetStream();
            _cli = new WeakReferenceT<BluetoothClient>(cli);
        }

        [MenuItem, SubMenu("BtClient")]
        public void Connect()
        {
            BluetoothClient cli;
            Connect_(out cli);
            _peer = cli.GetStream();
            _cli = new WeakReferenceT<BluetoothClient>(cli);
        }

#if NETCF
        [MenuItem, SubMenu("Playing")]
        public void BtpsScoWith_Connect()
        {
            var p = new InTheHand.Net.Bluetooth.StonestreetOne.BluetopiaPlaying();
            Action<BluetoothAddress> makeConnect = delegate(BluetoothAddress address) {
                p.HciCreateConnection(address);
            };
            //
            BluetoothClient cli;
            Connect_(out cli, makeConnect);
            _peer = cli.GetStream();
            _cli = new WeakReferenceT<BluetoothClient>(cli);
            ThreadPool.QueueUserWorkItem(state => Read_(40, false));
            //
            p.HciAddScoConnection();
            //
            var buf = Encoding.ASCII.GetBytes("ATD 07981152473\r");
            _peer.Write(buf, 0, buf.Length);
        }

        [MenuItem, SubMenu("Playing")]
        public void BtpsRawHci()
        {
            var p = new InTheHand.Net.Bluetooth.StonestreetOne.BluetopiaPlaying();
            Byte ogf = 0x3f;
            ushort ocf = 0;
            const byte FirstFragment = 0x80;
            const byte LastFragment = 0x40;
            const byte BccmdChannelId = 2;
            const byte PayloadDescriptor = BccmdChannelId | FirstFragment | LastFragment;
            //
            //const ushort xxBccmdGetReqxx = 0x0000;
            //const ushort xxBuildIdxx = 0x28_19;
            //const ushort xxBuildIdLoaderxx = 0x28_38;
            //
            int seqNum = Interlocked.Increment(ref csrSeqNum);
            Byte[] command = { PayloadDescriptor, 
                //cmd
                0x00, 0x00,
                //len
                16,0,
                //SeqNum
                (byte)seqNum,0,
                // VarId
                0x19,0x28,
                // Status
                0,0,
                // Payload
                0,0,0,0, 0,0,0,0,
            };
            Byte lenCommand = checked((byte)command.Length);
            //
            int ret = p.HciSendRawCommand(ogf, ocf, lenCommand, command);
            console.WriteLine("HCI_Send_Raw_Command ret: {0}", ret);
            /*
                Frame 1: 23 bytes on wire (184 bits), 23 bytes captured (184 bits)
                01:00:fc:13!c2:00:00:10: 00:01:00:19:28:00:00:00:
                00:00:00:00:00:00:00

                Frame 2: 36 bytes on wire (288 bits), 36 bytes captured (288 bits)
                04:ff:21!c2:01:00:10:00: 01:00:19:28:00:00:7e:0d:
                00:00:00:00:00:00:00:00: 00:00:00:00:00:00:00:00:
                00:00:00:00
             * Status = 0
             * BuildID = 0x0d7e = 3454
             * Which is the same as the value reported in Radio info. :-)
             */
        }

        static int csrSeqNum;
#endif

        readonly TimeSpan ConnectDuringDiscoveryPreConnectDelayMs
            = TimeSpan.FromSeconds(4);

        [MenuItem, SubMenu("BtClient")]
        public void Connect_WithInquiryRunning_APM()
        {
            TimeSpan delay = ConnectDuringDiscoveryPreConnectDelayMs;
            var givenDelay = console.ReadTimeSecondsOptional(" (default " + delay + ")");
            if (givenDelay != null) {
                delay = givenDelay.Value;
            }
            BluetoothClient cliDD = Create_BluetoothClient();
            IAsyncResult arDD = null;
            Action<BluetoothAddress> startDisco = delegate
            {
                arDD = cliDD.BeginDiscoverDevices(255, false, false, false, true,
                    MiscCallback, "BeginDiscoverDevices");
                Thread.Sleep(delay.Seconds);
            };
            BluetoothClient cli;
            DateTime startT = DateTime.UtcNow;
            try {
                Connect_(out cli, startDisco);
                _peer = cli.GetStream();
                _cli = new WeakReferenceT<BluetoothClient>(cli);
            } finally {
                DateTime connectedT = DateTime.UtcNow;
                console.WriteLine("Started at: {0}", startT);
                console.WriteLine("Connect completed at: {0}, waiting for device discovery...", connectedT);
                var devices = cliDD.EndDiscoverDevices(arDD);
                DateTime endDdT = DateTime.UtcNow;
                console.WriteLine("End of DD at: {0}, with {1} devices.", endDdT, devices.Length);
            }
        }

        [MenuItem, SubMenu("BtClient")]
        public void Connect_WithInquiryRunning_EBAP()
        {
            TimeSpan delay = ConnectDuringDiscoveryPreConnectDelayMs;
            var givenDelay = console.ReadTimeSecondsOptional(" (default " + delay + ")");
            if (givenDelay != null) {
                delay = givenDelay.Value;
            }
            BluetoothClient cliDD = Create_BluetoothClient();
            var ddComplete = new ManualResetEvent(false);
            var btco = new BluetoothComponent(cliDD);
            btco.DiscoverDevicesProgress += _DiscoDevicesAsync_ProgressCallback;
            btco.DiscoverDevicesComplete += _DiscoDevicesAsync_CompleteCallback;
            btco.DiscoverDevicesComplete += (s, e) => ddComplete.Set();
            Action<BluetoothAddress> startDisco = delegate
            {
                btco.DiscoverDevicesAsync(255, false, false, false, true, 99);
                Thread.Sleep(delay.Seconds);
            };
            BluetoothClient cli;
            DateTime startT = DateTime.UtcNow;
            try {
                Connect_(out cli, startDisco);
                _peer = cli.GetStream();
                _cli = new WeakReferenceT<BluetoothClient>(cli);
            } finally {
                DateTime connectedT = DateTime.UtcNow;
                console.WriteLine("Started at: {0}", startT);
                console.WriteLine("Connect completed at: {0}, waiting for device discovery...", connectedT);
                ddComplete.WaitOne();
                DateTime endDdT = DateTime.UtcNow;
                //console.WriteLine("End of DD at: {0}, with {1} devices.", endDdT, devices.Length);
                console.WriteLine("End of DD at: {0}.", endDdT);
                ddComplete.Close();
            }
        }

        [MenuItem, SubMenu("BtClient")]
        public void Connect_FromInquiryCallback()
        {
            BluetoothClient cliDd = Create_BluetoothClient();
            cliDd.BeginDiscoverDevices(255, true, true, true, false,
                Connect_FromInquiryCallback_DdCallback, cliDd);
            console.WriteLine("Running async DiscoverDevices...");
        }

        void Connect_FromInquiryCallback_DdCallback(IAsyncResult ar)
        {
            console.WriteLine("DiscoverDevices completed...");
            BluetoothClient cliDd = (BluetoothClient)ar.AsyncState;
            cliDd.EndDiscoverDevices(ar);
            //
            try {
                BluetoothClient cli;
                Connect_(out cli);
                _peer = cli.GetStream();
                _cli = new WeakReferenceT<BluetoothClient>(cli);
            } catch (Exception ex) {
                console.WriteLine("Connect_FromInquiryCallback error: " + ex);
            }
        }

#if TEST_EARLY
        [MenuItem, SubMenu("BtClient")]
        public void Connect_GetStream2()
        {
            BluetoothClient cli;
            Connect_(out cli);
            peer = cli.GetStream2();
            s_cli = new WeakReferenceT<BluetoothClient>(cli);
        }
#endif

        [MenuItem, SubMenu("BtClient")]
        public void ConnectSuccessfullyAnd_ImmediatelyClose()
        {
            ConnectSuccessfullyAndImmediatelySendAndClose_(null);
        }

        [MenuItem, SubMenu("BtClient")]
        public void ConnectSuccessfullyAndImmediatelySend_OneByteAndClose()
        {
            ConnectSuccessfullyAndImmediatelySendAndClose_(new byte[] { (byte)'b' });
        }

        void ConnectSuccessfullyAndImmediatelySendAndClose_(byte[] data)
        {
            BluetoothClient cli;
            Connect_(out cli);
            _peer = cli.GetStream();
            if (data != null)
                _peer.Write(data, 0, data.Length);
            _peer.Close();
            _peer = null;
            _cli = null;
        }

        [MenuItem, SubMenu("BtClient")]
        public void ConnectMultiple_Devices()
        {
            int num = console.ReadInteger("How many connects?");
            bool oaat = console.ReadYesNo("Do one-at-a-time (e.g. Widcomm SDP sync)?", false);
            BluetoothAddress[] addrList = new BluetoothAddress[num];
            Guid[] svcClassList = new Guid[num];
            int?[] portList = new int?[num];
            BluetoothClient[] cliList = new BluetoothClient[num];
            IAsyncResult[] arList = new IAsyncResult[num];
            //
            for (int i = 0; i < num; ++i) {
                addrList[i] = console.ReadBluetoothAddress("Server #" + (i + 1) + "'s BluetoothAddress");
                svcClassList[i] = BluetoothService.Wap;
                Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClassList[i]);
                if (inputGuid.HasValue)
                    svcClassList[i] = inputGuid.Value;
                portList[i] = console.ReadOptionalInteger("Port number");
            }
            //
            IAsyncResult waitForLast = null;
            for (int i = 0; i < num; ++i) {
                if (waitForLast != null) {
                    console.Write("Waiting previous.  ");
                    bool signalled = waitForLast.AsyncWaitHandle.WaitOne(60 * 1000, false);
                    if (!signalled) {
                        throw new InvalidOperationException("Timed-out waiting for previous connect to complete.");
                    }
                }
                BluetoothEndPoint rep;
                if (portList[i] == null)
                    rep = new BluetoothEndPoint(addrList[i], svcClassList[i]);
                else
                    rep = new BluetoothEndPoint(addrList[i], svcClassList[i], portList[i].Value);
                console.WriteLine("{4}: Connecting #{3} to: {0}:{1}:{2} ...", rep.Address, rep.Service, rep.Port,
                    (i + 1), GetTime());
                //
                cliList[i] = Create_BluetoothClient();
                try {
                    arList[i] = cliList[i].BeginConnect(rep,
                        MiscCallback, "BeginConnect #" + (i + 1));
                    console.WriteLine("{0}: (BeginConnect returned)", GetTime());
                    if (oaat) {
                        waitForLast = arList[i];
                    }
                } catch (System.Net.Sockets.SocketException ex) {
                    console.WriteLine("{0}: BeginConnect failed: {1}", GetTime(), Exception_FirstLine(ex));
                }
            }//for
            //
            WaitAll(arList);
            console.WriteLine("All signalled");
            //
            for (int i = 0; i < cliList.Length; i++) {
                try {
                    cliList[i].EndConnect(arList[i]);
                } catch (Exception ex) {
                    console.WriteLine("Error for #{3}: ", (i + 1), Exception_FirstLine(ex));
                }
            }
            console.WriteLine("All completed");
        }

        private void WaitAll(IList<IAsyncResult> arList)
        {
            WaitHandle[] whList = GetWaitHandles(arList);
#if !NETCF
            bool signalled = WaitHandle.WaitAll(whList);
#else
            bool signalled = WaitHandle_WaitAll_Hack(whList);
#endif
            Debug.Assert(signalled, "Given infinite timeout, but returned NON-signalled!?!");
        }

        private WaitHandle[] GetWaitHandles(IList<IAsyncResult> arList)
        {
            Debug.Assert(arList != null, "ArgNullEx");
            List<WaitHandle> output = new List<WaitHandle>(arList.Count);
            for (int i = 0; i < arList.Count; ++i) {
                if (arList[i] != null)
                    output.Add(arList[i].AsyncWaitHandle);
            }
            return output.ToArray();
        }

#if NETCF
        bool WaitHandle_WaitAll_Hack(WaitHandle[] list)
        {
            bool sum = true;
            foreach (WaitHandle cur in list) {
                bool signalled = cur.WaitOne();
                Debug.Assert(signalled, "NOT signalled");
                sum = sum && signalled;
            }
            Debug.Assert(sum, "NOT signalled->sum");
            return sum;
        }
#endif

        void Connect_(out BluetoothClient cli)
        {
            Connect_(out cli, null);
        }

        void Connect_(out BluetoothClient cli, Action<BluetoothAddress> codeToRunJustBeforeConnect)
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Server's BluetoothAddress");
            Guid svcClass = BluetoothService.Wap;
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClass);
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("Port number");
            bool auth = console.ReadYesNo("Authenticate", false);
            bool encpt = console.ReadYesNo("Encrypt", false);
            bool setPin1, setPin2;
            string passcode;
            BluetoothAddress pinAddr;
            PromptForSetPin(console, out setPin1, out setPin2, out passcode, out pinAddr);
            //
            BluetoothEndPoint lep;
            BluetoothAddress localAddrOptional = console.ReadOptionalBluetoothAddress("Local Bind() BluetoothAddress");
            if (localAddrOptional != null) {
                const int DefaultLocalPort = 0;
                var localPort = console.ReadOptionalInteger("Local port (default: " + DefaultLocalPort + ")");
                if (!localPort.HasValue) { localPort = DefaultLocalPort; }
                lep = new BluetoothEndPoint(localAddrOptional, BluetoothService.Empty, localPort.Value);
            } else {
                lep = null;
            }
            //
#if FX4
            bool taskAsync = console.ReadYesNo("Task-Async Connect?", false);
#else
            bool taskAsync = false;
#endif
            bool @async = taskAsync ? false : console.ReadYesNo("Async (APM) Connect?", false);
            bool asyncCallback = true;//not used but init anyway
            if (taskAsync || @async) {
                asyncCallback = console.ReadYesNo("Async Callback?", true);
            }
            //
            BluetoothEndPoint rep;
            if (port == null)
                rep = new BluetoothEndPoint(addr, svcClass);
            else
                rep = new BluetoothEndPoint(addr, svcClass, port.Value);
            console.WriteLine("Connecting to: {0}:{1}:{2} ...", rep.Address, rep.Service, rep.Port);
            //
            if (lep != null) {
                console.WriteLine("Binding to: {0}::{2} ...", lep.Address, lep.Service, lep.Port);
                cli = Create_BluetoothClient(lep);
            } else {
                cli = Create_BluetoothClient();
            }
            //
            // For Auth & Encrypt assume false by default, so if NotImpl fails only if user says true.
            if (auth)
                cli.Authenticate = true;
            if (encpt)
                cli.Encrypt = true;
            if (setPin1) {
                Debug.Assert(pinAddr == null, "pinAddr == null");
                cli.SetPin(passcode);
            } else if (setPin2) {
                Debug.Assert(pinAddr != null, "pinAddr != null");
                cli.SetPin(pinAddr, passcode);
            }
            if (codeToRunJustBeforeConnect != null) {
                codeToRunJustBeforeConnect(addr);
                console.WriteLine("(Have run the code before connect).");
            }
            if (taskAsync) {
#if FX4
                var task = cli.ConnectAsync(rep, "ConnectAsync");
                if (asyncCallback)
                    task.ContinueWith(MiscCallback);
                console.WriteLine("(ConnectAsync returned)");
                bool closeNow = console.ReadYesNo("Respond when we should proceed to Wait.  Enter Y to call Close first.", false);
                if (closeNow) {
                    cli.Close();
                }
                task.Wait();
#else
                throw new NotSupportedException("Task is FX4");
#endif
            } else if (@async) {
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
            console.WriteLine("Connected to : '{0}' {1}", cli.RemoteMachineName, cli.RemoteEndPoint);
            PrintLocalEndpointIfAvailable(cli);
        }

        private void PrintLocalEndpointIfAvailable(BluetoothClient cli)
        {
            System.Net.Sockets.Socket sock = null;
            try {
                sock = cli.Client;
            } catch (NotSupportedException) {
            } catch (NotImplementedException) {
            }
            if (sock != null) {
                console.WriteLine("Local endpoint : {0}", sock.LocalEndPoint);
            }
        }

        private void PromptForSetPin(MenuSystem console, out bool setPin1, out bool setPin2,
            out string passcode, out BluetoothAddress pinAddr)
        {
            passcode = null;
            pinAddr = null;
            setPin1 = console.ReadYesNo("SetPin(string) [not for BtLsnr!]", false);
            if (setPin1) {
                setPin2 = false;
            } else {
                setPin2 = console.ReadYesNo("SetPin(address,string)", false);
            }
            if (setPin1 || setPin2) {
                passcode = console.ReadLine("Passcode");
            }
            if (setPin2) {
                pinAddr = console.ReadBluetoothAddress("PIN address");
            }
        }

        [MenuItem, SubMenu("BtClient")]
        public void ConnectMultiple_Times()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Server's BluetoothAddress");
            Guid svcClass = BluetoothService.Wap;
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID", svcClass);
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("Port number");
            //
            BluetoothEndPoint rep;
            if (port == null)
                rep = new BluetoothEndPoint(addr, svcClass);
            else
                rep = new BluetoothEndPoint(addr, svcClass, port.Value);
            console.WriteLine("Will connect to: {0}:{1}:{2} ...", rep.Address, rep.Service, rep.Port);
            //
            int count = console.ReadInteger("Connect how many times");
            int? delay = console.ReadOptionalInteger("Delay between each (ms)");
            //
            BluetoothClient cli = null;
            byte[] buf = { 0x61, 0x62, 0x63, 0x64 };
            int countSuccess = 0;
            int countFail = 0;
            for (int i = 0; i < count; ++i) {
                try {
                    if (delay.HasValue) {
                        Thread.Sleep(delay.Value);
                    }
                    console.Write("Connecting #" + (i + 1) + "...");
                    cli = Create_BluetoothClient();
                    cli.LingerState = new System.Net.Sockets.LingerOption(true, 10);
                    cli.Connect(rep);
                    ++countSuccess;
                    console.WriteLine("Connected");
                    using (Stream peerA = cli.GetStream()) {
                        peerA.Write(buf, 0, buf.Length);
                    }
                } catch (Exception ex) {
                    ++countFail;
                    console.Write("Failed: " + Exception_FirstLine(ex));
                    Debug.WriteLine("ConnMulti: " + ex);
                    var exitOnError = false;
                    if (exitOnError) {
                        break;
                    }
                } finally {
                    if (cli != null) {
                        cli.Close();
                    }
                }
            }//for
            console.WriteLine("Count success: {0}, fail {1}",
                countSuccess, countFail);
        }

        [MenuItem, SubMenu("Data")]
        public void SendForever()
        {
            if (!VerifyConnectionWrite())
                return;
            int interSendPeriod = console.ReadInteger("Inter-send period in milliseconds");
            byte[] buf = Encoding.ASCII.GetBytes("____abcdefghijklmnopqrstuvwxyz");
            ManualResetEvent quit = console.NewManualResetEvent(false);
            Action<Stream> send = delegate(Stream peer2)
            {
                int num = 0;
                while (true) {
                    if (IsSet(quit))
                        break;
                    string numString = num.ToString("d4");
                    Debug.Assert(numString.Length == 4, "numString.Length: " + numString.Length);
                    unchecked {
                        buf[0] = (byte)numString[0];
                        buf[1] = (byte)numString[1];
                        buf[2] = (byte)numString[2];
                        buf[3] = (byte)numString[3];
                    }
                    peer2.Write(buf, 0, buf.Length);
                    console.Write(".");
                    if (IsSet(quit))
                        break;
                    Thread.Sleep(interSendPeriod);
                    ++num;
                }//while
            };
            IAsyncResult ar = DelegateExtension.BeginInvoke(send, _peer, null, null);
            console.Pause("Un-pause to stop sending");
            quit.Set();
            DelegateExtension.EndInvoke(send, ar);
        }

        [MenuItem, SubMenu("Data")]
        public void ZeroLengthSend()
        {
            if (_peer == null)
                return;
            byte[] buf = new byte[2];
            _peer.Write(buf, 0, 0);
            console.WriteLine("ZeroLengthSend");
        }

        [MenuItem, SubMenu("Data")]
        public void ZeroLengthRead()
        {
            if (_peer == null)
                return;
            byte[] buf = new byte[2];
            IAsyncResult ar = null;
            bool doNormalReadFirst = console.ReadYesNo("Do normal async read first", false);
            if (doNormalReadFirst)
                ar = _peer.BeginRead(buf, 0, 1, null, null);
            int readLen = _peer.Read(buf, 0, 0);
            console.WriteLine("Did ZeroLengthRead, got length: {0}", readLen);
            if (ar != null) {
                int readLen0 = _peer.EndRead(ar);
                console.WriteLine("The first read got {0} bytes.", readLen0);
            }
        }

        [MenuItem, SubMenu("Data")]
        public void Flush()
        {
            if (_peer == null)
                return;
            _peer.Flush();
        }

        [MenuItem, SubMenu("Data")]
        public void SendOneAtATime()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] buf = Encoding.ASCII.GetBytes("____abcdefghijklmnopqrstuvwxyz");
            Action<Stream> send = delegate(Stream peer2)
            {
                int num = 0;
                while (true) {
                    string numString = num.ToString("d4");
                    Debug.Assert(numString.Length == 4, "numString.Length: " + numString.Length);
                    unchecked {
                        buf[0] = (byte)numString[0];
                        buf[1] = (byte)numString[1];
                        buf[2] = (byte)numString[2];
                        buf[3] = (byte)numString[3];
                    }
                    peer2.Write(buf, 0, buf.Length);
                    console.Write(".");
                    if (!console.ReadYesNo("Send another", true))
                        break;
                }//while
            };
            send.Invoke(_peer);
        }

        private static bool IsSet(ManualResetEvent quit)
        {
            bool signalled = quit.WaitOne(0, false);
            return signalled;
        }

        [MenuItem, SubMenu("Data")]
        public void SendCrLf()
        {
            if (_peer == null || !_peer.CanWrite) {
                console.WriteLine("No connection!");
                return;
            }
            byte[] buf = Encoding.ASCII.GetBytes("\r\n");
            _peer.Write(buf, 0, buf.Length);
        }

        [MenuItem, SubMenu("Playing")]
        public void Write_HCI_ReadLocalName_toStream()
        {
            byte[] ReadLocalName = { 0x01, /*hci*/ 0x14, 0x0c, 0x00 };
            if (_peer == null || !_peer.CanWrite) {
                console.WriteLine("No connection!");
                return;
            }
            byte[] buf = ReadLocalName;
            _peer.Write(buf, 0, buf.Length);
        }



        [MenuItem, SubMenu("Data")]
        public void SendAmountOneAtATime()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] srcBuf = Encoding.ASCII.GetBytes("____abcdefghijklmnopqrstuvwxyz");
            Action<Stream> send = delegate(Stream peer2)
            {
                int num = 0;
                int lastAmount = 4096;
                byte[] buf = new byte[0];
                while (true) {
                    int? amount = console.ReadOptionalInteger("How big send (default: " + lastAmount + ")");
                    amount = amount ?? lastAmount;
                    lastAmount = amount.Value;
                    //
                    if (buf.Length < amount) {
                        buf = new byte[amount.Value];
                    }
                    //
                    string numString = num.ToString("d4");
                    Debug.Assert(numString.Length == 4, "numString.Length: " + numString.Length);
                    unchecked {
                        buf[0] = (byte)numString[0];
                        buf[1] = (byte)numString[1];
                        buf[2] = (byte)numString[2];
                        buf[3] = (byte)numString[3];
                    }
                    peer2.Write(buf, 0, amount.Value);
                    console.Write(".");
                    if (!console.ReadYesNo("Send another", true))
                        break;
                }//while
            };
            send.Invoke(_peer);
        }

        //--------
        [MenuItem, SubMenu("Data")]
        public void Read()
        {
            if (!VerifyConnectionRead())
                return;
            int? dumpTruncate = console.ReadOptionalInteger("Dump how much of receive (default None)");
            bool binary = false;
            if (dumpTruncate == null) {
                dumpTruncate = 0;
            }
            if (dumpTruncate > 0) {
                binary = console.ReadYesNo("Dump in hex", false);
            }
            Read_(dumpTruncate, binary);
        }

        void Read_(int? dumpTruncate, bool binary)
        {
            byte[] buf = new byte[100];
            ManualResetEvent quit = console.NewManualResetEvent(false);
            object lastReturn = "-never set-";
            int totalRead = 0;
            Action<Stream> dlgt = delegate(Stream peer2)
            {
                while (true) {
                    if (IsSet(quit))
                        break;
                    bool success = false;
                    try {
                        int readLen = peer2.Read(buf, 0, buf.Length);
                        console.Write(" len='{0}'", readLen);
                        success = true;
                        lastReturn = readLen;
                        totalRead += readLen;
                        if (readLen == 0) {
                            console.WriteLine(" EoS.");
                            break;
                        }
                        if (dumpTruncate > 0) {
                            string txt;
                            if (!binary) {
                                txt = Encoding.ASCII.GetString(buf, 0,
                                    Math.Min(readLen, dumpTruncate.Value));
                            } else {
                                txt = BitConverter.ToString(buf, 0,
                                    Math.Min(readLen, dumpTruncate.Value));
                                txt = txt.Substring(0,
                                    Math.Min(txt.Length, dumpTruncate.Value));
                            }
                            console.WriteLine("\"{0}\"", txt);
                        }
                    } finally {
                        if (!success) {
                            lastReturn = "Error";
                            console.WriteLine("Completed by Error!!!");
                        }
                    }
                }//while
            };
            IAsyncResult ar = DelegateExtension.BeginInvoke(dlgt, _peer, null, null);
            console.Pause("Un-pause to stop after next Read");
            quit.Set();
            DelegateExtension.EndInvoke(dlgt, ar);
            console.WriteLine("Last read returned: {0}", lastReturn);
            console.WriteLine("Total read : {0}", totalRead);
        }

        [MenuItem, SubMenu("Data")]
        public void ReadAndReflect()
        {
            if (!VerifyConnectionRead())
                return;
            byte[] buf = new byte[100];
            byte[] rsp = new byte[100];
            ManualResetEvent quit = console.NewManualResetEvent(false);
            Action<Stream> dlgt = delegate(Stream peer2)
            {
                while (true) {
                    if (IsSet(quit))
                        break;
                    int readLen = peer2.Read(buf, 0, buf.Length);
                    console.Write(" len='{0}'", readLen);
                    if (readLen == 0) {
                        console.WriteLine(" EoS.");
                        break;
                    }
                    SwapCase(buf, rsp, readLen);
                    peer2.Write(rsp, 0, readLen);
                }//while
            };
            IAsyncResult ar = DelegateExtension.BeginInvoke(dlgt, _peer, null, null);
            console.Pause("Un-pause to stop after next Read");
            quit.Set();
            DelegateExtension.EndInvoke(dlgt, ar);
        }

        private static void SwapCase(byte[] src, byte[] dst, int len)
        {
            for (int i = 0; i < len; ++i) {
                byte b = src[i];
                char ch = (char)b;
                char upper = char.ToUpper(ch, System.Globalization.CultureInfo.InvariantCulture);
                char lower = char.ToLower(ch, System.Globalization.CultureInfo.InvariantCulture);
                char upperBack = char.ToLower(ch, System.Globalization.CultureInfo.InvariantCulture);
                char lowerBack = char.ToUpper(ch, System.Globalization.CultureInfo.InvariantCulture);
                if (lowerBack != ch && upperBack != ch) {
                    // didn't round-trip, not a displayable(?) char, return as is.
                    dst[i] = (byte)ch;
                } else if (lower == ch && upper == ch) {
                    // didn't change, not a alphabetical char, return as is.
                    dst[i] = (byte)ch;
                } else if (lower == ch) { // Is lower case, so return as upper case
                    Debug.Assert(upper != ch, ((ushort)upper).ToString("X") + " != " + ((ushort)ch).ToString("X"));
                    dst[i] = (byte)upper;
                } else {
                    Debug.Assert(upper == ch);
                    Debug.Assert(lower != ch, ((ushort)upper).ToString("X") + " != " + ((ushort)ch).ToString("X"));
                    dst[i] = (byte)lower;
                }
            }//for
        }

        [MenuItem, SubMenu("Data")]
        public void SendObexPutInOnePacket()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] bufValid = {
                /*PutFinal*/0x82,  0,41,
                   /*Name*/0x01, 0,19, 0,(byte)'a', 0,(byte)'a', 0,(byte)'a', 
                        0,(byte)'.', 0,(byte)'t', 0,(byte)'x', 0,(byte)'t', 0,0,
                   /*EoB*/0x49, 0,19, 0,(byte)'a', 0,(byte)'a', 0,(byte)'a', 
                        0,(byte)'.', 0,(byte)'t', 0,(byte)'x', 0,(byte)'t', 0,0
            };
            byte[] bufTruncatedInFront = {
                /*PutFinal*/0x82,  0, // !!!!
            };
            byte[] bufTruncatedInRear = {
                /*PutFinal*/0x82,  0,41,
                   /*Name*/0x01, 0,19, 0,(byte)'a', // !!!!!
            };
            byte[] bufTruncateBeforeFinal = {
                /*Put-NON-Final*/0x02,  0,41,
                   /*Name*/0x01, 0,19, 0,(byte)'a', 0,(byte)'a', 0,(byte)'a', 
                        0,(byte)'.', 0,(byte)'t', 0,(byte)'x', 0,(byte)'t', 0,0,
                   /*Body*/0x48, 0,19, 0,(byte)'a', 0,(byte)'a', 0,(byte)'a', 
                        0,(byte)'.', 0,(byte)'t', 0,(byte)'x', 0,(byte)'t', 0,0
            };
            int? choice;
            while (true) {
                choice = console.ReadOptionalInteger(
                    "1. Complete (one packet), 2. Truncated in front, 3. Truncated in back, 4. Truncated before final packet");
                if (!choice.HasValue || (choice.Value >= 1 && choice.Value <= 4))
                    break;
                console.WriteLine("Invalid selection.");
            }
            byte[] buf;
            switch (choice) {
                case null:
                case 1:
                    buf = bufValid;
                    break;
                case 2:
                    buf = bufTruncatedInFront;
                    break;
                case 3:
                    buf = bufTruncatedInRear;
                    break;
                case 4:
                    buf = bufTruncateBeforeFinal;
                    break;
                default:
                    throw new ArgumentException("Invalid selection: " + choice);
            }
            _peer.Write(buf, 0, buf.Length);
            _peer.Flush(); // just in case
            console.ReadLine("Waiting before closing the connection");
            _peer.Close();
        }

        //----
        [MenuItem, SubMenu("Device Discovery")]
        public void AllDiscover()
        {
            BluetoothClient cli = Create_BluetoothClient();
            DateTime startTime = DateTime.Now;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
            DateTime endTime = DateTime.Now;
            DumpDeviceInfo(devices, startTime, endTime);
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void AllApmDiscover()
        {
            BluetoothClient cli = Create_BluetoothClient();
            const int DefaultTimeout = 30;
            TimeSpan? timeout = console.ReadTimeSecondsOptional("Set InquiryLength (default " + DefaultTimeout + "secs)");
            if (timeout == null) {
                timeout = TimeSpan.FromSeconds(DefaultTimeout);
            }
            cli.InquiryLength = timeout.Value;
            EventHandler dlgt = delegate
            {
                DateTime startTime = DateTime.Now;
                DiscoState1 state = new DiscoState1(cli, startTime);
                IAsyncResult ar = cli.BeginDiscoverDevices(255,
                    true, true, true, false,
                    RestartDiscoCallback, state);
            };
#if NETCF
            bool fromUi = console.ReadYesNo("Start first discovery from UI thread?", false);
#else
            bool fromUi = false;
#endif
            if (fromUi) {
                console.UiInvoke(dlgt);
            } else {
                dlgt(null, null);
            }
            //
            console.Pause("Un-pause when discovery has completed");
        }

        class DiscoState1
        {
            public readonly BluetoothClient cli;
            public readonly DateTime startTime;

            public DiscoState1(BluetoothClient cli, DateTime startTime)
            {
                this.cli = cli;
                this.startTime = startTime;
            }
        }

        void SimpleDiscoCallback(IAsyncResult ar)
        {
            DiscoState1 state = (DiscoState1)ar.AsyncState;
            try {
                BluetoothDeviceInfo[] devices = state.cli.EndDiscoverDevices(ar);
                DateTime endTime = DateTime.Now;
                DumpDeviceInfoBrief(devices, state.startTime, endTime);
            } catch (Exception ex) {
                console.WriteLine("Discovery failed with: " + ex);
            }
        }

        void RestartDiscoCallback(IAsyncResult ar)
        {
            SimpleDiscoCallback(ar);
            bool startAgainInCallback = console.ReadYesNo("Start discovery again from callback", false);
            if (startAgainInCallback) {
                DiscoState1 state0 = (DiscoState1)ar.AsyncState;
                DiscoState1 state = new DiscoState1(state0.cli, DateTime.Now);
                IAsyncResult ar2 = state.cli.BeginDiscoverDevices(255,
                    true, true, true, false,
                    SimpleDiscoCallback, state);
            }
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void ConcurrentlyTwoDiscoverApm()
        {
            var cli1 = Create_BluetoothClient();
            var cli2 = Create_BluetoothClient();
            IAsyncResult ar1 = cli1.BeginDiscoverDevices(255, true, true, true, false, null, null);
            IAsyncResult ar2 = cli2.BeginDiscoverDevices(255, true, true, true, false, null, null);
            console.WriteLine("Two started...");
            bool signalled = _WaitAll(new WaitHandle[] { ar1.AsyncWaitHandle, ar2.AsyncWaitHandle }, 30 * 1000);
            if (!signalled) {
                console.WriteLine("Something horrible happened, at least one never/took too long to complete.");
            } else {
                var rslt1 = cli1.EndDiscoverDevices(ar1);
                var rslt2 = cli2.EndDiscoverDevices(ar2);
                console.WriteLine("They returned respectively: {0} and {1} devices.",
                    rslt1.Length, rslt2.Length);
            }
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void _RememberedDiscover()
        {
            BluetoothClient cli = Create_BluetoothClient();
            DateTime startTime = DateTime.Now;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices(255, false, true, false);
            DateTime endTime = DateTime.Now;
            DumpDeviceInfo(devices, startTime, endTime);
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void _FlagsDiscover()
        {
            DiscoverFlags_(false);
        }

        [MenuItem, SubMenu("Device Discovery")]
        void FlagsA_pmDiscover()
        {
            DiscoverFlags_(true);
        }

        private void GetDiscoveryFlags(MenuSystem console, out bool discoOnly, out bool auth, out bool rembd, out bool unk)
        {
            console.WriteLine("Discover which type of remote devices?");
            discoOnly = console.ReadYesNo("discoverableOnly", false);
            unk = console.ReadYesNo("Unknown", false);
            rembd = console.ReadYesNo("Remembered", false);
            auth = console.ReadYesNo("Authenticated", false);
        }

        public void DiscoverFlags_(bool @async)
        {
            BluetoothClient cli = Create_BluetoothClient();
            bool discoOnly, auth, rembd, unk;
            GetDiscoveryFlags(console, out discoOnly, out auth, out rembd, out unk);
            BluetoothDeviceInfo[] devices;
            //
            TimeSpan? timeout = console.ReadTimeSecondsOptional("InquiryLength");
            if (timeout != null) {
                cli.InquiryLength = timeout.Value;
            }
            console.WriteLine("cli.InquiryLength is: {0}", cli.InquiryLength);
            int? maxDevices = console.ReadOptionalInteger("maxDevices (default 255)");
            if (maxDevices == null) maxDevices = 255;
            console.WriteLine("maxDevices is: {0}", maxDevices);
            //
            DateTime startTime = DateTime.Now;
            if (@async) {
                bool doPause = console.ReadYesNo("Pause after BeginDD", false);
                bool errorCallback = console.ReadYesNo("Throw an Error from the Callback", false);
                AsyncCallback cb = errorCallback ? new AsyncCallback(ErrorFromMiscCallback) : MiscCallback;
                IAsyncResult ar = cli.BeginDiscoverDevices(255, auth, rembd, unk, discoOnly,
                    cb, "BeginDiscoverDevices");
                console.WriteLine("(BeginDiscoverDevices returned)");
                if (doPause) console.Pause("Un-pause to continue to EndDD");
                devices = cli.EndDiscoverDevices(ar);
            } else {
                devices = cli.DiscoverDevices(maxDevices.Value, auth, rembd, unk, discoOnly);
            }
            DateTime endTime = DateTime.Now;
            DumpDeviceInfo(devices, startTime, endTime);
            bool dispose = console.ReadYesNo("Dispose BtCli", true);
            if (dispose && cli != null) {
                cli.Dispose();
            }
        }

        [MenuItem, SubMenu("Device Discovery")]
        public void _RunAllFlagCombinations()
        {
            BluetoothClient cli = Create_BluetoothClient();
            DateTime startTime = DateTime.Now;
            bool discoOnly, authenticated, remembered, unknown;
            int flagSource = 0;
            while (true) {
                authenticated = (flagSource & 1) != 0;
                remembered = (flagSource & 2) != 0;
                unknown = (flagSource & 4) != 0;
                discoOnly = (flagSource & 8) != 0;
                //
                BluetoothDeviceInfo[] devices = cli.DiscoverDevices(255,
                    authenticated, remembered, unknown, discoOnly);
                console.WriteLine("Got {0} results", devices.Length);
                //
                if (authenticated && remembered && unknown && discoOnly)
                    break; // Have tested all cases of the original four flags.
                ++flagSource;
                Thread.Sleep(2000); // This sequence hangs on my W2k+Widcomm box -- does delaying help...?
            }//while
            //DateTime endTime = DateTime.Now;
            //DumpDeviceInfo(devices, startTime, endTime);
            console.WriteLine("All DiscoverDevices flag combinations run.");
        }

        const string vbCrLf = "\r\n";
        void DumpDeviceInfo(BluetoothDeviceInfo[] devices,
            DateTime startTime, DateTime endTime)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DateTime localTime = startTime.ToLocalTime();
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "Discovery process started at {0} UTC, {1} local, and ended at {2} UTC.",
                startTime, localTime, endTime);
            sb.Append(vbCrLf + vbCrLf);
            console.WriteLine(sb.ToString());
            foreach (BluetoothDeviceInfo curDevice in devices) {
                sb.Length = 0;
                DumpDeviceInfo(sb, curDevice);
                console.WriteLine(sb.ToString());
            }
        }

        void DumpDeviceInfoBrief(BluetoothDeviceInfo[] devices,
            DateTime startTime, DateTime endTime)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            DateTime localTime = startTime.ToLocalTime();
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "Discovery process started at {0} UTC, {1} local, and ended at {2} UTC.",
                startTime, localTime, endTime);
            console.WriteLine(sb.ToString());
            foreach (BluetoothDeviceInfo curDevice in devices) {
                sb.Length = 0;
                DumpDeviceInfoBrief(sb, curDevice);
                console.WriteLine(sb.ToString());
            }
        }

        private static void DumpDeviceInfoBrief(StringBuilder sb, BluetoothDeviceInfo curDevice)
        {
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "* {0}", ToStringQuotedOrNull(curDevice.DeviceName));
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                ", Address: {0}", curDevice.DeviceAddress);
        }

        private static string DumpDeviceInfoBrief(BluetoothDeviceInfo curDevice)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            DumpDeviceInfoBrief(sb, curDevice);
            return sb.ToString();
        }

        private static void DumpDeviceInfo(System.Text.StringBuilder sb, BluetoothDeviceInfo curDevice)
        {
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "* {0}", ToStringQuotedOrNull(curDevice.DeviceName));
            sb.Append(vbCrLf);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "Address: {0}", curDevice.DeviceAddress);
            sb.Append(vbCrLf);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "Remembered: {2}, Authenticated: {0}, Connected: {1}",
                curDevice.Authenticated, curDevice.Connected, curDevice.Remembered);
            sb.Append(vbCrLf);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "LastSeen: {0} ({2}), LastUsed: {1} ({3})",
                curDevice.LastSeen, curDevice.LastUsed,
                curDevice.LastSeen.Kind, curDevice.LastUsed.Kind);
            sb.Append(vbCrLf);
            DumpCodInfo(curDevice.ClassOfDevice, sb);
            Int32 rssi = curDevice.Rssi;
            if (rssi != Int32.MinValue) {
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "Rssi: {0} (0x{0:X})", rssi);
            } else {
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "Rssi: failed");
            }
            sb.Append(vbCrLf);
        }

        private static string DumpDeviceInfo(BluetoothDeviceInfo curDevice)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            DumpDeviceInfo(sb, curDevice);
            return sb.ToString();
        }

        static void DumpCodInfo(ClassOfDevice cod, System.Text.StringBuilder sb)
        {
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "CoD: (0x{0:X6})", cod.Value, cod);
            sb.Append(vbCrLf);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                " Device:  {0} (0x{1:X2}) / {2} (0x{3:X4})", cod.MajorDevice, (int)cod.MajorDevice, cod.Device, (int)cod.Device);
            sb.Append(vbCrLf);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                " Service: {0} (0x{1:X2})", cod.Service, (int)cod.Service);
            sb.Append(vbCrLf);
        }

#if !NO_WINFORMS
        [MenuItem, SubMenu("Device Discovery")]
        public void UiDiscovery()
        {
            bool discoOnly, auth, rembd, unk;
            GetDiscoveryFlags(console, out discoOnly, out auth, out rembd, out unk);
            InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dlg = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
            dlg.ShowAuthenticated = auth;
            dlg.ShowRemembered = rembd;
            dlg.ShowUnknown = unk;
            dlg.ShowDiscoverableOnly = discoOnly;
            if (console.ReadYesNo("set filter (always returns true)", false)) {
                dlg.DeviceFilter = FilterDevice;
                if (console.ReadYesNo("set filter to null now(!)", false)) {
                    dlg.DeviceFilter = null;
                }
            }
            System.Windows.Forms.DialogResult rslt = System.Windows.Forms.DialogResult.None;
            EventHandler action = delegate
            {
                rslt = dlg.ShowDialog();
            };
            console.UiInvoke(action);
            console.WriteLine("DialogResult: {0}, Selected: {1}", rslt,
                dlg.SelectedDevice == null ? "(null)" : ("'" + dlg.SelectedDevice.DeviceName + "' " + dlg.SelectedDevice.DeviceAddress.ToString("C")));
        }

        void X()
        {
            var dlg = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
            dlg.DeviceFilter = FilterDevice;
            var rslt = dlg.ShowDialog();
            //......
        }

        bool FilterDevice(BluetoothDeviceInfo dev)
        {
            var ret = true;
            ret = console.ReadYesNo("Include this device " + dev.DeviceAddress + " " + dev.DeviceName, ret);
            return ret;
        }
#endif


        //----
#if TEST_EARLY
        [MenuItem, SubMenu("SDP")]
        public void HackServiceDiscovery()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("SDP server BluetoothAddress");
            try {
                BluetoothClient cli0 = new BluetoothClient();
                // Don't dispose this to check its Finalization.
                InTheHand.Net.Bluetooth.Widcomm.ISdpDiscoveryRecordsBuffer recs
                    = cli0.WidcommHack__GetServiceRecordsUnparsed(addr, BluetoothService.L2CapProtocol);
                console.WriteLine("recordCount: {0}", recs.RecordCount);
                //
                int[] ports = recs.Hack_GetPorts();
#if !NETCF
                string[] txtArr = Array.ConvertAll<int, string>(ports,
                    delegate(int cur) { return cur.ToString(); });
#else
                string[] txtArr = new string[ports.Length];
                for (int i = 0; i < ports.Length; ++i)
                    txtArr[i] = ports[i].ToString();
#endif
                string txt = string.Join(", ", txtArr);
                console.WriteLine("Ports: {0}", txt);
                //
                ServiceRecord[] recordArr = recs.GetServiceRecords();
                foreach (ServiceRecord cur in recordArr) {
                    console.WriteLine(
                        ServiceRecordUtilities.Dump(cur));
                }
            } catch (Exception ex) {
                console.WriteLine(ex);
            }
        }
#endif

        [MenuItem, SubMenu("SDP")]
        public void Sdp_Query()
        {
            SdpQuery_();
        }

        void SdpQuery_()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("SDP server BluetoothAddress");
            Guid? svcClass1 = console.ReadOptionalBluetoothUuid("Search UUID (default L2CAP->all)", null);
            if (svcClass1 == null) {
                svcClass1 = BluetoothService.L2CapProtocol;
            }
#if FX4
            bool taskAsync = console.ReadYesNo("Task-Async Get", false);
#else
            bool taskAsync = false;
#endif
            bool @async = taskAsync ? false : console.ReadYesNo("Async Get", false);
            //
            BluetoothDeviceInfo bdi = Create_BluetoothDeviceInfo(addr);
            ServiceRecord[] recordArr;
            var svcClassList = new List<Guid> { (Guid)svcClass1 };
            var foo = false;
            if (foo) {
                var from = 0x1000;
                var to = 0x1100;
                svcClassList.Clear();
                for (int cur = from; cur <= to; ++cur) {
                    svcClassList.Add(BluetoothService.CreateBluetoothUuid(cur));
                }
            }
            foreach (var svcClass in svcClassList) {
                string target = string.Format(CultureInfo.InvariantCulture,
                    "{0} on {1}", (Guid)svcClass, bdi.DeviceAddress);
                if (taskAsync) {
#if FX4
                    var task = bdi.GetServiceRecordsAsync((Guid)svcClass, "GetServiceRecordsAsync");
                    task.ContinueWith(MiscCallback);
                    console.WriteLine("(GetServiceRecordsAsync returned)");
                    console.Pause("Un-pause when should continue to get Result");
                    recordArr = task.Result;
                    task.Wait();
#else
                    throw new NotSupportedException("Task is FX4");
#endif
                } else if (@async) {
                    console.WriteLine("Calling BeginGetServiceRecords for " + target);

                    IAsyncResult ar = bdi.BeginGetServiceRecords((Guid)svcClass,
                            MiscCallback, "BeginGetServiceRecords");
                    console.WriteLine("BeginGetServiceRecords returned");
                    console.Pause("Un-pause when should continue to EndGetServiceRecords");
                    recordArr = bdi.EndGetServiceRecords(ar);
                } else {
                    console.WriteLine("Calling GetServiceRecords for " + target);
                    recordArr = bdi.GetServiceRecords((Guid)svcClass);
                }
                //
                SdpRecordDump(recordArr);
            }
        }

        private void SdpRecordDump(ICollection<ServiceRecord> recordArr)
        {
            console.WriteLine("recordCount: {0}", recordArr.Count);
            int i = 1;
            foreach (ServiceRecord cur in recordArr) {
                console.WriteLine("{0})", i);
                try {
                    console.WriteLine(
                        ServiceRecordUtilities.Dump(cur));
                } catch (Exception ex) {
                    console.WriteLine("Error in Dump, trying DumpRaw ({0})", ex);
                    console.WriteLine(
                        ServiceRecordUtilities.DumpRaw(cur));
                }
                console.WriteLine("----");
                ++i;
            }
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpQuery_Repeatedly()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("SDP server BluetoothAddress");
            Guid? svcClass = console.ReadOptionalBluetoothUuid("Search UUID (default L2CAP->all)", null);
            if (svcClass == null) {
                svcClass = BluetoothService.L2CapProtocol;
            }
            //
            BluetoothDeviceInfo bdi = Create_BluetoothDeviceInfo(addr);
            string target = string.Format(CultureInfo.InvariantCulture,
                "{0} on {1}", (Guid)svcClass, bdi.DeviceAddress);
            int InterDelay = 500;
            //
            MyAction startQuery = null;
            bool stop = false;
            AsyncCallback _Callback = delegate(IAsyncResult ar)
            {
                try {
                    bdi.EndGetServiceRecords(ar);
                    console.Write("|");
                    Thread.Sleep(InterDelay);
                    if (stop) {
                        console.WriteLine("]");
                    } else {
                        startQuery();
                    }
                } catch (Exception ex) {
                    console.WriteLine(ex);
                }
            };
            startQuery = delegate
            {
                bdi.BeginGetServiceRecords((Guid)svcClass,
                    _Callback, null);
                console.Write("-");
            };
            //
            console.WriteLine("Calling first BeginGetServiceRecords for " + target);
            while (true) {
                stop = false;
                startQuery();
                console.Pause("When to stop");
                stop = true;
                var startAgain = console.ReadYesNo("Start again?", false);
                if (!startAgain) {
                    break;
                }
            }
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpQuery_Concurrent()
        {
            BluetoothAddress addrA = console.ReadBluetoothAddress("SDP server BluetoothAddress");
            BluetoothAddress addrB;
            var another = console.ReadYesNoCancel("Different device | dupl. DBI | same BDI", null);
            BluetoothDeviceInfo bdiA = Create_BluetoothDeviceInfo(addrA);
            BluetoothDeviceInfo bdiB;
            if (another == true) {
                console.Write("Another target. ");
                addrB = console.ReadBluetoothAddress("SDP server BluetoothAddress");
                bdiB = Create_BluetoothDeviceInfo(addrB);
            } else if (another == false) {
                console.Write("Dupl.BDI. ");
                bdiB = Create_BluetoothDeviceInfo(addrA);
            } else {
                console.Write("Same BDI. ");
                bdiB = bdiA;
            }
            //
            Guid? svcClass = console.ReadOptionalBluetoothUuid("Search UUID (default L2CAP->all)", null);
            if (svcClass == null) {
                svcClass = BluetoothService.L2CapProtocol;
            }
            //
            string target = string.Format(CultureInfo.InvariantCulture,
                "{0} on {1} and {2}", (Guid)svcClass, bdiA.DeviceAddress, bdiB.DeviceAddress);
            console.WriteLine(target);
            //
            bool repeat;
            do {
                console.Write("Starting...");
                IAsyncResult arA, arB;
                try {
                    arA = bdiA.BeginGetServiceRecords(svcClass.Value, MiscCallback, "A");
                } catch (Exception ex) {
                    arA = null;
                    console.WriteLine("A @Begin " + " ex: " + ex);
                }
                try {
                    arB = bdiB.BeginGetServiceRecords(svcClass.Value, MiscCallback, "B");
                } catch (Exception ex) {
                    arB = null;
                    console.WriteLine("B @Begin " + " ex: " + ex);
                }
                console.Pause("when complete");
                MyFunc<IAsyncResult, string> doGetIsCompleted = delegate(IAsyncResult arX)
                {
                    if (arX == null) return "NULL";
                    return arX.IsCompleted.ToString();
                };
                console.WriteLine("IsCompleted: A: {0}, B: {1}",
                    doGetIsCompleted(arA), doGetIsCompleted(arB));
                //--
                MyAction<string, BluetoothDeviceInfo, IAsyncResult> doComplete
                    = delegate(string name, BluetoothDeviceInfo devX, IAsyncResult arX)
                {
                    ServiceRecord[] rs;
                    try {
                        rs = devX.EndGetServiceRecords(arX);
                        console.WriteLine(name + " #r: " + rs.Length);
                    } catch (Exception ex) {
                        console.WriteLine(name + " ex: " + ex);
                    }
                };
                // Complete if they started
                if (arA != null) {
                    doComplete("A", bdiA, arA);
                }
                if (arB != null) {
                    doComplete("B", bdiB, arB);
                }
                //
                repeat = console.ReadYesNo("Do again on same BDIs?", false);
            } while (repeat);
        }

        delegate void MyAction();
        delegate void MyAction<T0>(T0 p0);
        delegate void MyAction<T0, T1>(T0 p0, T1 p1);
        delegate void MyAction<T0, T1, T2>(T0 p0, T1 p1, T2 p2);

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateCustom()
        {
            ServiceRecord record = CreateAVariousRecord();
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record);
            lsnr.Start();
            console.Pause("Un-pause to stop the server");
            lsnr.Stop();
        }

#if false
        [MenuItem, SubMenu("SDP")]
        public void SdpCreateCustomSimpleElements_Btps()
        {
            ServiceRecord record = CreateAVariousRecordWithSimpleElements();
            //
            var c = InTheHand.Net.Bluetooth.StonestreetOne
                .BluetopiaSdpCreator.CreateTestable();
            c.CreateServiceRecord(record);
            console.Pause("Un-pause to stop the server");
            c.DeleteServiceRecord();
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateCustom_Btps()
        {
            ServiceRecord record = CreateAVariousRecord();
            //
            var c = InTheHand.Net.Bluetooth.StonestreetOne
                .BluetopiaSdpCreator.CreateTestable();
            c.CreateServiceRecord(record);
            console.Pause("Un-pause to stop the server");
            c.DeleteServiceRecord();
        }
#endif

        private static ServiceRecord CreateAVariousRecord()
        {
            MyFunc<ElementType, ServiceAttributeId> createId = delegate(ElementType etX)
            {
                return (ServiceAttributeId)0x4000 + checked((byte)etX);
            };
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.HealthDevice);
            var longquotes = false;
            if (longquotes) {
                bldr.ServiceName = new string('"', 40);
                // cmd> sdptool search --bdaddr 00:0a:3a:68:65:bb --xml ftp
                bldr.AddServiceClass(BluetoothService.ObexFileTransfer);
            } else {
                bldr.ServiceName = "alan";
            }
            IList<ServiceAttribute> attrList = new List<ServiceAttribute>();
            ElementType et_;
            CreateVariousSimpleElements(createId, attrList);
            et_ = ElementType.Uuid16;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, (UInt16)et_)));
            et_ = ElementType.Uuid32;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, (UInt32)et_)));
            et_ = ElementType.Uuid128;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, BluetoothService.CreateBluetoothUuid((int)et_))));
            bldr.AddCustomAttributes(attrList);
            bldr.AddCustomAttributes(ElementsAndVariableAndFixedInDeepTree1());
            ServiceRecord record = bldr.ServiceRecord;
            return record;
        }

        private static ServiceRecord CreateAVariousRecordWithSimpleElements()
        {
            MyFunc<ElementType, ServiceAttributeId> createId = delegate(ElementType etX)
            {
                return (ServiceAttributeId)0x4000 + checked((byte)etX);
            };
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.HealthDevice); // not optional :-(
            IList<ServiceAttribute> attrList = new List<ServiceAttribute>();
            CreateVariousSimpleElements(createId, attrList);
            bldr.AddCustomAttributes(attrList);
            ServiceRecord record = bldr.ServiceRecord;
            return record;
        }

        private static void CreateVariousSimpleElements(MyFunc<ElementType, ServiceAttributeId> createId,
            IList<ServiceAttribute> attrList)
        {
            ElementType et_;
#if SUPPORT_NIL
            et_ = ElementType.Nil;
            attrList.Add(new ServiceAttribute(createId(et_), new ServiceElement(et_, null)));
#endif
            et_ = ElementType.Boolean;
            attrList.Add(new ServiceAttribute(createId(et_), new ServiceElement(et_, true)));
            ElementType[] weee = {
                ElementType.UInt8,  ElementType.UInt16, ElementType.UInt32, ElementType.UInt64, //UInt128,
                ElementType.Int8,   ElementType.Int16,  ElementType.Int32,  ElementType.Int64,  //Int128,
            };
            foreach (ElementType et in weee) {
                attrList.Add(new ServiceAttribute(
                    createId(et),
                    ServiceElement.CreateNumericalServiceElement(et, (uint)et)));
            }
        }

        public const String RecordBytes_OneString_StringValue = "abcd\u00e9fgh\u012dj";
        public static IList<ServiceAttribute> ElementsAndVariableAndFixedInDeepTree1()
        {
            IList<ServiceAttribute> attrs = new List<ServiceAttribute>();
            //
            String str = RecordBytes_OneString_StringValue;
            ServiceElement itemStr1 = new ServiceElement(ElementType.TextString, str);
            ServiceElement itemStr2 = new ServiceElement(ElementType.TextString, str);
            //
            Uri uri = new Uri("http://example.com/foo.txt");
            ServiceElement itemUrl = new ServiceElement(ElementType.Url, uri);
            //
            ServiceElement itemF1 = new ServiceElement(ElementType.UInt16, (UInt16)0xfe12);
            ServiceElement itemF2 = new ServiceElement(ElementType.UInt16, (UInt16)0x1234);
            //
            IList<ServiceElement> leaves2 = new List<ServiceElement>();
            leaves2.Add(itemStr1);
            leaves2.Add(itemUrl);
            leaves2.Add(itemF1);
            ServiceElement e2 = new ServiceElement(ElementType.ElementSequence, leaves2);
            //
            ServiceElement e1 = new ServiceElement(ElementType.ElementSequence, e2);
            //
            IList<ServiceElement> leaves0 = new List<ServiceElement>();
            leaves0.Add(e1);
            leaves0.Add(itemStr2);
            leaves0.Add(itemF2);
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementAlternative,
                        leaves0)));
            return attrs;
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateHidAndPnpRecords()
        {
            ServiceRecord record = CreateAHumanInputDeviceRecord();
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record);
            ServiceRecord record2 = CreateAHumanInputDeviceRecordB();
            BluetoothListener lsnr2 = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record2);
            ServiceRecord record3 = CreateAPnpRecord();
            BluetoothListener lsnr3 = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record3);
            lsnr.Start();
            lsnr2.Start();
            lsnr3.Start();
            console.Pause("Un-pause to stop the server");
            lsnr.Stop();
            lsnr2.Stop();
            lsnr3.Stop();
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreatePnpRecord()
        {
            ServiceRecord record = CreateAPnpRecord();
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record);
            lsnr.Start();
            console.Pause("Un-pause to stop the server");
            lsnr.Stop();
        }

        private ServiceRecord CreateAPnpRecord()
        {
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            // Listener needs this. :-(
            bldr.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
            bldr.AddServiceClass(BluetoothService.PnPInformation);
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.SpecificationId,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 1)));
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.VendorId,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 2)));
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.ProductId,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 3)));
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.Version,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 4)));
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.PrimaryRecord,
                new ServiceElement(ElementType.Boolean, true)));
            bldr.AddCustomAttribute(new ServiceAttribute(DeviceIdProfileAttributeId.VendorIdSource,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 6)));
            return bldr.ServiceRecord;
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateHumanInputDeviceRecord()
        {
            ServiceRecord record = CreateAHumanInputDeviceRecord();
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record);
            ServiceRecord record2 = CreateAHumanInputDeviceRecordB();
            BluetoothListener lsnr2 = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                record2);
            lsnr.Start();
            lsnr2.Start();
            console.Pause("Un-pause to stop the servers");
            lsnr.Stop();
            lsnr2.Stop();
        }

        private ServiceRecord CreateAHumanInputDeviceRecord()
        {
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            // Listener needs this. :-(
            bldr.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
            bldr.AddServiceClass(BluetoothService.HumanInterfaceDevice);
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceReleaseNumber,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x1000)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ParserVersion,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 1)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceSubclass,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 2)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.CountryCode,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 3)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.VirtualCable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 4)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ReconnectInitiate,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 5)));
            //bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DescriptorList,
            //    new ServiceElement(ElementType.ElementSequence,
            //        ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 6))));
            //bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.LangIdBaseList,
            //    new ServiceElement(ElementType.ElementSequence,
            //        ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 7))));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.SdpDisable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 8)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.BatteryPower,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 9)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.RemoteWake,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 10)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.BootDevice,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 14)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.SupervisionTimeout,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 12)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.NormallyConnectable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 13)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ProfileVersion,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 11)));
            return bldr.ServiceRecord;
        }
        private ServiceRecord CreateAHumanInputDeviceRecordB()
        {
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            // Listener needs this. :-(
            bldr.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
            bldr.AddServiceClass(BluetoothService.HumanInterfaceDevice);
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceReleaseNumber,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x1FFF)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ParserVersion,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0xFF1F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceSubclass,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x2F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.CountryCode,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x3F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.VirtualCable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x4F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ReconnectInitiate,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x5F)));
            //bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DescriptorList,
            //    new ServiceElement(ElementType.ElementSequence,
            //        ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 6))));
            //bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.LangIdBaseList,
            //    new ServiceElement(ElementType.ElementSequence,
            //        ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 7))));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.SdpDisable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x8F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.BatteryPower,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x9F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.RemoteWake,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x1F)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.BootDevice,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 14)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.SupervisionTimeout,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 12)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.NormallyConnectable,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 13)));
            bldr.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.ProfileVersion,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 11)));
            return bldr.ServiceRecord;
        }


        [MenuItem, SubMenu("SDP")]
        private void SdpCreateAHumanInputDeviceRecordsAllTwoOfThree()
        {
            var create = new Converter<object, ServiceRecordBuilder>(delegate
            {
                ServiceRecordBuilder bldrF = new ServiceRecordBuilder();
                // Listener needs this. :-(
                bldrF.ProtocolType = BluetoothProtocolDescriptorType.GeneralObex;
                bldrF.AddServiceClass(BluetoothService.HumanInterfaceDevice);
                return bldrF;
            });
            var attrA = new Action<ServiceRecordBuilder>(delegate(ServiceRecordBuilder bldrF)
            {
                bldrF.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceReleaseNumber,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x1FFF)));
            });
            var attrC = new Action<ServiceRecordBuilder>(delegate(ServiceRecordBuilder bldrF)
            {
                bldrF.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.DeviceSubclass,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x2F)));
            });
            var attrB = new Action<ServiceRecordBuilder>(delegate(ServiceRecordBuilder bldrF)
            {
                bldrF.AddCustomAttribute(new ServiceAttribute(HidProfileAttributeId.CountryCode,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x3F)));
            });
            //
            ServiceRecordBuilder bldr;
            bldr = create(null);
            attrA(bldr);
            attrB(bldr);
            BluetoothListener lsnr = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                bldr.ServiceRecord);
            bldr = create(null);
            attrA(bldr);
            attrC(bldr);
            BluetoothListener lsnr2 = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                bldr.ServiceRecord);
            bldr = create(null);
            attrB(bldr);
            attrC(bldr);
            BluetoothListener lsnr3 = Create_BluetoothListener(BluetoothService.HealthDeviceSink,
                bldr.ServiceRecord);
            //
            lsnr.Start();
            lsnr2.Start();
            lsnr3.Start();
            console.Pause("Un-pause to stop the server");
            lsnr.Stop();
            lsnr2.Stop();
            lsnr3.Stop();
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateTwoSppRecordsOneWithAvailAndBrowse()
        {
            var bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.WapClient);
            bldr.AddCustomAttribute(new ServiceAttribute(
                UniversalAttributeId.ServiceAvailability,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 255)));
            bldr.AddCustomAttribute(new ServiceAttribute(
                UniversalAttributeId.BrowseGroupList,
                new ServiceElement(ElementType.ElementSequence,
                    new ServiceElement(ElementType.Uuid128, BluetoothService.PublicBrowseGroup))));
            var sr = bldr.ServiceRecord;
            //DEBUGconsole.Write(ServiceRecordUtilities.Dump(sr));
            var lsnr = Create_BluetoothListener(
                new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort, 22),
                sr);
            var lsnr2 = Create_BluetoothListener(
                new Guid("{E665C159-4721-4e73-8E97-9A8BE42423D9}"));
            //
            lsnr.Start();
            lsnr2.Start();
            console.Pause("Un-pause to stop the server");
            lsnr.Stop();
            lsnr2.Stop();
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpCreateAndDumpAnAvcrpRecord()
        {
            ServiceRecord record = CreateAnAvcrpCtRecord();
            //
            string dump = ServiceRecordUtilities.Dump(record);
            console.WriteLine(dump);
        }

        internal static ServiceRecord CreateAnAvcrpCtRecord()
        {
            //const UInt16 SvcClass16Avcrp = 0x110E;
            const UInt16 SvcClass16AvcrpController_ = 0x110F;
            Guid SvcClass128AvcrpController = BluetoothService.CreateBluetoothUuid(SvcClass16AvcrpController_);
            //
            const UInt16 SvcClass16ProtocolL2CAP = 0x0100;
            Debug_Assert_CorrectShortUuid(SvcClass16ProtocolL2CAP, BluetoothService.L2CapProtocol);
            const UInt16 SvcClass16ProtocolAvctp = 0x0017;
            Debug_Assert_CorrectShortUuid(SvcClass16ProtocolAvctp, BluetoothService.AvctpProtocol);
            //
            const UInt16 PsmAvcrp = 0x0017;
            const UInt16 PsmAvcrpBrowsing = 0x001B;
            //
            const UInt16 Version = 0x0103; // 1.3.......
            //
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolL2CAP),
                new ServiceElement(ElementType.UInt16, PsmAvcrp));
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolAvctp),
                new ServiceElement(ElementType.UInt16, Version));
            ServiceElement pdl = new ServiceElement(ElementType.ElementSequence,
                layer0, layer1);
            //
            var apdlLayer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolL2CAP),
                new ServiceElement(ElementType.UInt16, PsmAvcrpBrowsing));
            var apdlLayer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, SvcClass16ProtocolAvctp),
                new ServiceElement(ElementType.UInt16, Version));
            ServiceElement additionalPdlSubList0 = new ServiceElement(ElementType.ElementSequence,
                apdlLayer0, apdlLayer1);
            ServiceElement additionalPdl = new ServiceElement(ElementType.ElementSequence,
                additionalPdlSubList0);
            //
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.AVRemoteControl);
            bldr.AddServiceClass(SvcClass128AvcrpController);
            //
            bldr.ProtocolType = BluetoothProtocolDescriptorType.None;
            bldr.AddCustomAttributes(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl),
                new ServiceAttribute(UniversalAttributeId.AdditionalProtocolDescriptorLists,
                    additionalPdl));
            //
            bldr.AddBluetoothProfileDescriptor(BluetoothService.AVRemoteControl, 1, 4);
            //
            bldr.AddCustomAttribute(new ServiceAttribute(HandsFreeProfileAttributeId.SupportedFeatures,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16,
                (int)(AvcrpSupportedFeatures.Category1 | AvcrpSupportedFeatures.Category2)
                )));
            //
            bldr.ProviderName = "alanjmcf!!!!!!";
            bldr.ServiceName = "avcrp tc!!!!!!";
            return bldr.ServiceRecord;
        }

        [Conditional("DEBUG")]
        private static void Debug_Assert_CorrectShortUuid(UInt16 svcClass16, Guid svcClass)
        {
            Debug.Assert(svcClass.Equals(BluetoothService.CreateBluetoothUuid(svcClass16)),
                "Wrong integer value 0x" + svcClass16.ToString("X4") + ", extracted from: " + svcClass);
        }

        [Conditional("DEBUG")]
        private static void Debug_Assert_CorrectShortUuid(UInt32 svcClass32, Guid svcClass)
        {
            Debug.Assert(svcClass.Equals(BluetoothService.CreateBluetoothUuid(svcClass32)),
                "Wrong integer value 0x" + svcClass32.ToString("X8") + ", extracted from: " + svcClass);
        }

        [Flags]
        enum AvcrpSupportedFeatures : ushort
        {
            /// <summary>
            /// Category 1
            /// </summary>
            Category1 = 0x01,
            /// <summary>
            /// Category 2
            /// </summary>
            Category2 = 0x02,
            /// <summary>
            /// Category 3
            /// </summary>
            Category3 = 0x04,
            /// <summary>
            /// Category 4
            /// </summary>
            Category4 = 0x08,
            /// <summary>
            /// Player Application Settings
            /// </summary>
            /// -
            /// <remarks>"Bit 0 should be set for this bit to be set."
            /// </remarks>
            PlayerApplicationSettings = 0x10,
            /// <summary>
            /// Group Navigation
            /// </summary>
            /// -
            /// <remarks>"Bit 0 should be set for this bit to be set."
            /// </remarks>
            GroupNavigation = 0x20,
            /// <summary>
            /// Supports Browsing
            /// </summary>
            SupportsBrowsing = 0x40,
            /// <summary>
            /// Supports multiple media player applications 
            /// </summary>
            SupportsMultipleMediaPlayerApplications = 0x20,
            // RFA
        }

        internal static ServiceRecord CreateAnAvcrpCtRecord_GuidForUuid16()
        {
            //Guid SvcClass128Avcrp = /*0x110E*/BluetoothService.AVRemoteControl;
            const UInt16 _SvcClass16AvcrpController = 0x110F;
            Guid SvcClass128AvcrpController = BluetoothService.CreateBluetoothUuid(_SvcClass16AvcrpController);
            //
            Guid SvcClass128ProtocolL2CAP = /*0x0100*/BluetoothService.L2CapProtocol;
            Guid SvcClass128ProtocolAvctp = /*0x0017*/BluetoothService.AvctpProtocol;
            //
            const UInt16 PsmAvcrp = 0x0017;
            const UInt16 PsmAvcrpBrowsing = 0x001B;
            //
            const UInt16 Version = 0x0103; // 1.3.......
            //
            var layer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, GetBluetoothUuid16(SvcClass128ProtocolL2CAP)),
                new ServiceElement(ElementType.UInt16, PsmAvcrp));
            var layer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, GetBluetoothUuid16(SvcClass128ProtocolAvctp)),
                new ServiceElement(ElementType.UInt16, Version));
            ServiceElement pdl = new ServiceElement(ElementType.ElementSequence,
                layer0, layer1);
            //
            var apdlLayer0 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, GetBluetoothUuid16(SvcClass128ProtocolL2CAP)),
                new ServiceElement(ElementType.UInt16, PsmAvcrpBrowsing));
            var apdlLayer1 = new ServiceElement(ElementType.ElementSequence,
                new ServiceElement(ElementType.Uuid16, GetBluetoothUuid16(SvcClass128ProtocolAvctp)),
                new ServiceElement(ElementType.UInt16, Version));
            ServiceElement additionalPdlSubList0 = new ServiceElement(ElementType.ElementSequence,
                apdlLayer0, apdlLayer1);
            ServiceElement additionalPdl = new ServiceElement(ElementType.ElementSequence,
                additionalPdlSubList0);
            //
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.AVRemoteControl);
            bldr.AddServiceClass(SvcClass128AvcrpController);
            //
            bldr.ProtocolType = BluetoothProtocolDescriptorType.None;
            bldr.AddCustomAttributes(
                new ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl),
                new ServiceAttribute(UniversalAttributeId.AdditionalProtocolDescriptorLists, additionalPdl));
            //
            bldr.AddBluetoothProfileDescriptor(BluetoothService.AVRemoteControl, 1, 4);
            //
            bldr.AddCustomAttribute(new ServiceAttribute(HandsFreeProfileAttributeId.SupportedFeatures,
                ServiceElement.CreateNumericalServiceElement(ElementType.UInt16,
                (int)(AvcrpSupportedFeatures.Category1 | AvcrpSupportedFeatures.Category2)
                )));
            //
            bldr.ProviderName = "alanjmcf!!!!!!";
            bldr.ServiceName = "avcrp tc!!!!!!";
            return bldr.ServiceRecord;
        }

        static UInt16 GetBluetoothUuid16(Guid sigServiceClass)
        {
            Int64 uuid32 = GetBluetoothUuid32(sigServiceClass);
            UInt16 uuid16;
            try {
                UInt32 uuid32Real = (UInt32)uuid32;
                uuid16 = (UInt16)uuid32Real;
                return uuid16;
            } catch (OverflowException oex) {
                throw new ArgumentException("The Service Class Guid does not have a UUID16 short form.", oex);
            }
        }

        static UInt32 GetBluetoothUuid32(Guid sigServiceClass)
        {
            byte[] ba = sigServiceClass.ToByteArray();
            UInt32 uuid32 = BitConverter.ToUInt32(ba, 0);
            Guid recreated = BluetoothService.CreateBluetoothUuid(uuid32);
            if (!recreated.Equals(sigServiceClass))
                throw new ArgumentException("The Service Class Guid is not a Bluetooth standard class.");
            return uuid32;
        }

        [MenuItem, SubMenu("SDP")]
        public void SdpDumpGiven()
        {
            console.WriteLine("Enter the record bytes, e.g. 0x36, 0x00, 0x53, 0x09, 0x00, 0x00, 0x0a ...");
            console.WriteLine("Enter an empty line to finish.");
            var lineList = new List<string>();
            while (true) {
                //string line = "0x36, 0x00, 0x56, 0x36, 0x00, 0x53, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01, 0x00, 0x00, 0x09, 0x00, ";
                string line = console.ReadLine(string.Empty);
                if (string.IsNullOrEmpty(line)) {
                    break;
                }
                lineList.Add(line);
            }
            //
            const int GuessMaxLineLen = 16;
            var bytesList = new List<byte>(GuessMaxLineLen * lineList.Count);
            foreach (var curLine in lineList) {
                string[] arr = curLine.Split(',');
                bool sawBlank = false;
                foreach (var cur in arr) {
                    if (sawBlank) {
                        throw new FormatException("Found blank array entry in line.");
                    }
                    string v = cur.Trim();
                    byte b;
                    if (v.Length == 0) { // Empty, allow if after last comma...
                        sawBlank = true;
                        continue;
                    }
                    if (v.StartsWith("0x", StringComparison.Ordinal)) {
                        v = v.Substring(2);
                        b = byte.Parse(v, NumberStyles.AllowHexSpecifier);
                    } else {
                        b = byte.Parse(v);
                    }
                    bytesList.Add(b);
                }
            }
            //
            byte[] raw = bytesList.ToArray();
            var p = new ServiceRecordParser();
            ServiceRecord rcd;
            try {
                rcd = p.Parse(raw);
            } catch (UriFormatException) {
                console.WriteLine("Record parse failed with UriFormatException,"
                    + " trying again with LazyUrlCreation=true.");
                p.LazyUrlCreation = true;
                rcd = p.Parse(raw);
            }
            SdpRecordDump(new ServiceRecord[] { rcd });
        }

        [MenuItem, SubMenu("SDP")]
        public void FromJsr82ServerUri()
        {
            var line = console.ReadLine("JSR-82 uri");
            var bldr = ServiceRecordBuilder.FromJsr82ServerUri(line);
            console.WriteLine(ServiceRecordUtilities.Dump(bldr.ServiceRecord));
        }


        //--------
        delegate TResult MyFunc<T0, TResult>(T0 p0);
        delegate TResult MyFunc<T0, T1, TResult>(T0 p0, T1 p1);

        //----
        [MenuItem, SubMenu("Data")]
        public void SendLots()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] data = MakeLotsData();
            Lots_CheckTheTest(ref data);
            bool @async = console.ReadYesNo("Async", true);
            if (@async) {
                IAsyncResult ar = _peer.BeginWrite(data, 0, data.Length, MiscCallback, "BeginWrite");
                console.WriteLine("Started writing (async), len: {0}.", data.Length);
                _peer.EndWrite(ar);
            } else {
                _peer.Write(data, 0, data.Length);
            }
            console.WriteLine("Finished writing.");
        }

        [MenuItem, SubMenu("Data")]
        public void SendLots_InSizedChunks()
        {
            if (!VerifyConnectionWrite())
                return;
            byte[] data = MakeLotsData();
            Lots_CheckTheTest(ref data);
            IList<byte[]> dataList = SplitIntoChunks(data, 999);
            bool @async = console.ReadYesNo("Async", true);
            if (@async) {
                List<IAsyncResult> arList = new List<IAsyncResult>(dataList.Count);
                int total = 0;
                foreach (byte[] cur in dataList) {
                    IAsyncResult ar = _peer.BeginWrite(cur, 0, cur.Length, EmptyCallback, "BeginWrite");
                    total += cur.Length;
                    console.Write(".");
                    arList.Add(ar);
                }
                console.WriteLine("Did BeginWrite all chunks ({2}, async), len: {0} is {1}.", total, data.Length, arList.Count);
                foreach (IAsyncResult cur in arList) {
                    _peer.EndWrite(cur);
                }
                console.WriteLine("Did EndWrite all chunks.");
            } else {
                foreach (byte[] chunk in dataList) {
                    console.WriteLine("chunk={0}", chunk.Length);
                    _peer.Write(chunk, 0, chunk.Length);
                }
            }
            console.WriteLine("Finished writing.");
        }

        private void Lots_CheckTheTest(ref byte[] data)
        {
            bool testChecking1 = console.ReadYesNo("check the TEST, set one byte wrong", false);
            if (testChecking1) {
                data[1030000] ^= 255;
            }
            int hackOffset = 1040000;
            bool testChecking2 = console.ReadYesNo("check the TEST, add two bytes at " + hackOffset, false);
            if (testChecking2) {
                byte[] dataSlodged = new byte[data.Length + 2];
                Array.Copy(data, 0, dataSlodged, 0, hackOffset);
                Array.Copy(data, hackOffset, dataSlodged, 2 + hackOffset, data.Length - hackOffset);
                data = dataSlodged;
            }
        }

        private IList<byte[]> SplitIntoChunks(byte[] data, int chunkSize)
        {
            int offset = 0;
            List<byte[]> chunkList = new List<byte[]>(data.Length / chunkSize + 1);
            while (offset < data.Length) {
                int curSize = Math.Min(chunkSize, data.Length - offset);
                byte[] cur = new byte[curSize];
                Array.Copy(data, offset, cur, 0, cur.Length);
                offset += cur.Length;
                chunkList.Add(cur);
            }
            if (offset != data.Length)
                throw new InvalidOperationException("NOT offset==data.Length: " + offset + " vs " + data.Length);
            return chunkList;
        }

        private byte[] MakeLotsData()
        {
            byte[] data = new byte[1 * 1024 * 1024];
            bool useRandom = console.ReadYesNo("Random data", true);
            if (useRandom) {
                Random r = new Random(999);
                r.NextBytes(data);
            } else {
                for (int i = 0; i < data.Length; ++i) {
                    data[i] = unchecked((byte)i);
                    if (data[i] == 0) // Don't allow zeros, to se if that bug occurs.
                        data[i] = 255;
                }
            }
            return data;
        }

        [MenuItem, SubMenu("Data")]
        public void ReadLots()
        {
            if (!VerifyConnectionRead())
                return;
            byte[] expectedData = MakeLotsData();
            ReadLotsState state = new ReadLotsState(_peer, expectedData);
            state.data = new byte[expectedData.Length + 20];
            ReadLotsStartNextRead(state);
            console.Pause("Un-pause to stop");
            //lock (readLotsCurAr) {
            //    readLotsCurAr.
            //}
        }

        IAsyncResult readLotsCurAr;

        private void ReadLotsStartNextRead(ReadLotsState state)
        {
            if (state.numCompletedSynchronously > ReadLotsState.MaxCompletedSynchronously) {
                // (There's lots of pending data and thus) We've CompletedSynchronously
                // *many* times, so there's a danger of stack overflow 
                // (BeginRead->callback->BeginRead->callback->BeginRead->callback->...),
                // so start the next operation on a new thread.
                WaitCallback dlgt = delegate(object stateP)
                {
                    ReadLotsState state0 = (ReadLotsState)stateP;
                    state.numCompletedSynchronously = 0;
                    ReadLotsStartNextRead(state);
                };
                ThreadPool.QueueUserWorkItem(dlgt);
                return;
            }
            readLotsCurAr = state.peer.BeginRead(state.data, state.totalLen, state.data.Length - state.totalLen,
                ReadLotsReadCallBack, state);
            console.Write("-");
        }

        class ReadLotsState
        {
            public ReadLotsState(Stream peer, byte[] expectedData)
            {
                this.peer = peer;
                this.expectedData = expectedData;
            }

            public Stream peer;
            public byte[] data;
            public int totalLen;
            //
            public byte[] expectedData;
            //
            public int numCompletedSynchronously;
            public const int MaxCompletedSynchronously = 50;
        }

        void ReadLotsReadCallBack(IAsyncResult ar)
        {
            ReadLotsState state = (ReadLotsState)ar.AsyncState;
            int readLen;
            try {
                readLen = state.peer.EndRead(ar);
            } catch (ObjectDisposedException) {
                console.WriteLine("Connection was closed during ReadLots.");
                console.WriteLine("???, hit return to continue.");
                return;
            }
            console.Write(">{1} ({0}) ", readLen, state.totalLen + readLen);
            ArraysEqual(state.expectedData, state.data, state.totalLen, readLen);
            state.totalLen += readLen;
            if (ar.CompletedSynchronously)
                ++state.numCompletedSynchronously;
            else
                state.numCompletedSynchronously = 0;
            if (state.totalLen == state.expectedData.Length) {
                console.WriteLine("got all!");
                return;
            }
            //
            if (readLen == 0) {
                console.WriteLine("EoF, hit return to continue.");
                return;
            }
            ReadLotsStartNextRead(state);
        }

        private void ArraysEqual(byte[] expected, byte[] actual, int offset, int count)
        {
            for (int i = 0; i < count; ++i) {
                if (offset + i >= expected.Length) {
                    console.WriteLine("!! ({0}+{1}=>) {2} >= {3}", offset, i, (offset + i), expected.Length);
                }
                if (offset + i >= actual.Length) {
                    console.WriteLine("!! ({0}+{1}=>) {2} >= {3}", offset, i, (offset + i), actual.Length);
                }
                if (expected[offset + i] != actual[offset + i]) {
                    console.WriteLine("wrong at {0}+{1}={2}", offset, i, offset + i);
                    break;
                }
            }
        }

        //----
        [MenuItem, SubMenu("Local")]
        public void GetAllRadios_OnUiThread()
        {
            EventHandler dlgt = delegate
            {
#if ANDROID
                InTheHand.Net.Bluetooth.Droid.AndroidBthInquiry
                    .MyActivity = (Android.App.Activity)console.InvokeeControl;
#endif
                var rl = BluetoothRadio.GetAllRadios();
            };
            console.UiInvoke(dlgt);
        }

        [MenuItem, SubMenu("Local")]
        public void GetAllRadios()
        {
            BluetoothRadio_GetAllAndDump();
        }

        [MenuItem, SubMenu("Local")]
        public void GetAllRadiosWith_Features()
        {
            BluetoothRadio_GetAllAndDump(true);
        }

        [MenuItem, SubMenu("Local")]
        public void Get_AllRadiosDumpRepeatedly()
        {
            BluetoothRadio[] list = BluetoothRadio.GetAllRadios();
            do {
                DumpBluetoothRadios(list, false);
            } while (console.ReadYesNo("Dump again", false));
        }

        BluetoothRadio[] BluetoothRadio_GetAllAndDump()
        {
            return BluetoothRadio_GetAllAndDump(false);
        }

        BluetoothRadio[] BluetoothRadio_GetAllAndDump(bool dumpFeatureList)
        {
            BluetoothRadio[] list;
            if (console.ReadYesNo("Get Method rather then Property", true)) {
                list = BluetoothRadio.GetAllRadios();
            } else {
                list = BluetoothRadio.AllRadios;
            }
            DumpBluetoothRadios(list, dumpFeatureList);
            return list;
        }

        void DumpBluetoothRadios(BluetoothRadio[] list, bool dumpFeatureList)
        {
            if (list == null)
                console.WriteLine("AllRadios returned null!!!");
            else if (list.Length == 0)
                console.WriteLine("AllRadios returned empty list.");
            int i = 1;
            foreach (BluetoothRadio r in list) {
                console.WriteLine("{0})  {1}", i, DumpBluetoothRadio(r, dumpFeatureList));
                ++i;
            }
        }

        [MenuItem, SubMenu("Local")]
        public void GetPrimaryRadio()
        {
            GetPrimaryRadio_(false);
        }

        [MenuItem, SubMenu("Local")]
        public void Get_PrimaryRadioAndDumpRepeatedly()
        {
            GetPrimaryRadio_(true);
        }

        void GetPrimaryRadio_(bool offerRepeat)
        {
            BluetoothRadio r;
            if (console.ReadYesNo("Get Method rather then Property", true)) {
                r = BluetoothRadio.GetPrimaryRadio();
            } else {
                r = BluetoothRadio.PrimaryRadio;
            }
            do {
                if (r == null)
                    console.WriteLine("PrimaryRadio returned null.");
                else
                    console.WriteLine(DumpBluetoothRadio(r, false));
            } while (offerRepeat
                && console.ReadYesNo("Dump again", false));
        }

        private static string DumpVersions(RadioVersions v)
        {
            if (v == null)
                throw new ArgumentNullException("radio");
            using (StringWriter wtr = new StringWriter()) {
                wtr.WriteLine("LMP Version: " + v.LmpVersion
                    + ", Subversion: " + v.LmpSubversion);
                wtr.WriteLine("Manufacturer: {0}",
                    v.Manufacturer);
                wtr.WriteLine("LMP Features: {0}",
                    v.LmpSupportedFeatures);
                return wtr.ToString();
            }
        }

        private static string DumpBluetoothRadio(BluetoothRadio radio, bool dumpFeatureList)
        {
            if (radio == null)
                throw new ArgumentNullException("radio");
            using (StringWriter wtr = new StringWriter()) {
                RadioMode mode = radio.Mode;
                // Warning: LocalAddress is null if the radio is powered-off.
                wtr.WriteLine("Radio, address: {0:C}", radio.LocalAddress);
#if RADIO_MODESSS
                wtr.Write("Mode: " + mode.ToString());
                wtr.WriteLine("; Modes: { " + radio.Modes.ToString() + " }");
#else
                wtr.WriteLine("Mode: " + mode.ToString());
#endif
                wtr.WriteLine("Name: " + ToStringQuotedOrNull(radio.Name));
                wtr.WriteLine("HCI Version: " + radio.HciVersion
                    + ", Revision: " + radio.HciRevision);
                wtr.WriteLine("LMP Version: " + radio.LmpVersion
                    + ", Subversion: " + radio.LmpSubversion);
                if (dumpFeatureList) { wtr.WriteLine("LMP Features: " + radio.LmpFeatures); }
                wtr.WriteLine("ClassOfDevice: " + radio.ClassOfDevice.ToString()
                    + ", device: " + radio.ClassOfDevice.Device.ToString()
                    + " / service: " + radio.ClassOfDevice.Service.ToString());
                wtr.WriteLine("Software: {0},  Hardware: {1}, status: {2}",
                    radio.SoftwareManufacturer, radio.Manufacturer, radio.HardwareStatus);
                //try {
                //    wtr.WriteLine("Handle: 0x{0:X}", radio.Handle);
                //} catch (Exception ex) {
                //    wtr.WriteLine("Handle: failed with: '{0}'", ex.GetType().Name);
                //}
                wtr.WriteLine("Remote: '{0}'", radio.Remote);
                return wtr.ToString();
            }
        }

        private static string DumpBluetoothRadioBrief(BluetoothRadio radio)
        {
            if (radio == null)
                throw new ArgumentNullException("radio");
            using (StringWriter wtr = new StringWriter()) {
                RadioMode mode = radio.Mode;
                // Warning: LocalAddress is null if the radio is powered-off.
                //wtr.Write("Address: {0:C}, ", radio.LocalAddress);
                wtr.Write("Mode: " + mode.ToString());
#if RADIO_MODESSS
                wtr.Write("; Modes: { " + radio.Modes.ToString() + " }");
#endif
                //wtr.Write(", Name: " + radio.Name);
                return wtr.ToString();
            }
        }

        [MenuItem, SubMenu("Local")]
        public void RepeatedlyGetPrimaryRadio()
        {
            int? initialDelay = console.ReadOptionalInteger("Initial delay (ms)");
            int? interDelay = 250;
            int? userInterDelay = console.ReadOptionalInteger("Inter delay (ms) (default " + interDelay + ")");
            if (userInterDelay.HasValue) interDelay = userInterDelay;
            int count = 20;
            int? userCount = console.ReadOptionalInteger("Optional count (default " + count + ")");
            if (userCount.HasValue) count = userCount.Value;
            //
            if (initialDelay.HasValue) Thread.Sleep(initialDelay.Value);
            for (int i = 0; i < count; ++i) {
                console.WriteLine(Environment.TickCount.ToString() + ": ");
                try {
                    BluetoothRadio r = BluetoothRadio.PrimaryRadio;
                    if (r == null) {
                        console.WriteLine("PrimaryRadio returned null.");
                    } else {
                        console.WriteLine(DumpBluetoothRadioBrief(r));
                    }
                } catch (Exception ex) {
                    console.WriteLine("Error:" + Exception_FirstLine(ex));
                    Debug.WriteLine("RGPR: " + ex);
                }
                if (interDelay.HasValue) Thread.Sleep(interDelay.Value);
            }//for
        }

        [MenuItem, SubMenu("Local")]
        public void IsSupported()
        {
            bool? f = null;
            EventHandler dlgt = (o, e) =>
                f = BluetoothRadio.IsSupported;
            bool ui = console.ReadYesNo("On UI thread?", false);
            if (!ui)
                dlgt(null, null);
            else
                console.UiInvoke(dlgt);
            console.WriteLine("IsSupported: {0}", f.Value);
        }


        //----
#if TEST_EARLY
        [MenuItem, SubMenu("BtLsnr")]
        public void WidcommHackListen()
        {
            BluetoothAddress addr = BluetoothAddress.None;
            //BluetoothAddress addr = console.ReadBluetoothAddress("Server's local BluetoothAddress");
            Guid svcClass = BluetoothService.WapClient;
            console.WriteLine("Default UUID : {0}", svcClass);
            Guid? inputGuid = console.ReadOptionalBluetoothUuid("UUID");
            if (inputGuid.HasValue)
                svcClass = inputGuid.Value;
            int? port = console.ReadOptionalInteger("Port number");
            //
            BluetoothEndPoint rep;
            if (port == null)
                rep = new BluetoothEndPoint(addr, svcClass);
            else
                rep = new BluetoothEndPoint(addr, svcClass, port.Value);
            console.WriteLine("Connecting to: {0}:{1}:{2}", rep.Address, rep.Service, rep.Port);
            //
            InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothListener lsnr
                = new InTheHand.Net.Bluetooth.Widcomm.WidcommBluetoothListener();
            lsnr.Construct(rep);
            lsnr.Start();
            BluetoothClient conn = lsnr.AcceptBluetoothClient();
            peer = conn.GetStream();
            s_cli = new WeakReferenceT<BluetoothClient>(conn);
        }
#endif

        [MenuItem, SubMenu("GC")]
        public void RunFinalizersAfterGc()
        {
            RunFinalizersAfterGc_();
        }

        internal static void RunFinalizersAfterGc_()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [MenuItem, SubMenu("GC")]
        public void RunGc()
        {
            RunGc_();
        }

        internal static void RunGc_()
        {
            GC.Collect();
        }

#if !NETCF && !ANDROID
        [MenuItem, SubMenu("GC")]
        public void BeepOnFinalization()
        {
            JRichter.GCNotification.StartBeeper();
        }
#endif

        //----------
        [MenuItem, SubMenu("OBEX")]
        public void _ObexWebRequest()
        {
            string addr = console.ReadLine("Address (IrDA / TCP/IP; hit return for BT)");
            bool useUriCtor = true;
            BluetoothAddress addrBth;
            if (addr.Length == 0) {
                addrBth = console.ReadBluetoothAddress("Target Bluetooth device");
                addr = addrBth.ToString();
                useUriCtor = !console.ReadYesNo("Use BluetoothAddress+stringPath constructor", !useUriCtor);
            } else {
                // Often get a space after string on NETCF (remembered string)
                addr = addr.Trim();
                addrBth = null;
            }
            string pathO;
            string fileSource;
            int? contentLen;
            if (console.ReadYesNo("Send dummy content", true)) {
                pathO = "/obexpush1.txt";
                contentLen = console.ReadOptionalInteger("How much content (default ~60 bytes)");
                fileSource = null;
                if (contentLen == null)
                    contentLen = 60;
            } else {
                contentLen = null;
                fileSource = console.GetFilename();
                pathO = Path.GetFileName(fileSource);
                pathO = "/" + pathO;
            }
            //
            int? timeout = console.ReadOptionalInteger("timeout (ms)");
            //
            const string scheme = "obex";
            Debug.Assert(pathO.StartsWith("/"), "NOT path starts with /");
            var s = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0}://{1}{2}",
                scheme, addr, pathO);
            Uri uri = new Uri(s);
            ObexWebRequest req;
            if (useUriCtor) {
                req = Create_ObexWebRequest(uri);
            } else {
                req = Create_ObexWebRequest(scheme, addrBth, pathO);
            }
            if (timeout != null) { req.Timeout = timeout.Value; }
            //
            if (contentLen != null) {
                WriteDummyContent(contentLen.Value, req);
            } else {
                req.ReadFile(fileSource);
            }
            console.WriteLine("Calling GetResponse with " + req.ContentLength + " bytes of content...");
            ObexWebResponse rsp = (ObexWebResponse)req.GetResponse();
            console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode);
        }

        private static void WriteDummyContent(int contentLen, ObexWebRequest req)
        {
            using (Stream content = req.GetRequestStream()) {
                // Using a StreamWriter to write text to the stream...
                using (TextWriter wtr = new StreamWriter(content)) {
                    int i = 0;
                    while (content.Length < contentLen) {
                        ++i;
                        wtr.WriteLine("Hello World GetRequestStream {0}",
                            // Write the time to the first line
                            i == 1 ? DateTime.Now.ToString() : i.ToString());
                        wtr.Flush(); // push through to stream
                    }
                    // Set the Length header value
                    req.ContentLength = content.Length;
                }
            }
        }

#if !NETCF
        bool _touchedOwr;

        [MenuItem, SubMenu("OBEX")]
        public void WebClientPut()
        {
            if (!_touchedOwr) {
                bool ret = console.ReadYesNo("Touch ObexWebRequest to install protocol handler.", true);
                if (ret) {
                    var x = new ObexWebRequest(new Uri("obex://127.0.0.1/foo.txt"));
                    _touchedOwr = true;
                }
            }
            string addr = console.ReadLine("Address (IrDA / TCP/IP; hit return for BT)");
            if (addr.Length == 0) {
                BluetoothAddress addrB = console.ReadBluetoothAddress("Target Bluetooth device");
                addr = addrB.ToString();
            }
            int? contentLen = console.ReadOptionalInteger("How much content (default ~60 bytes)");
            if (contentLen == null)
                contentLen = 60;
            //NOT on NETCF!
            //  UriBuilder bldr = new UriBuilder();
            //  bldr.Scheme = "obex";
            //  bldr.Path = "/obexpush1.txt";
            //  bldr.Host = addr.ToString();
            Uri uri = new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0}://{1}{2}",
                "obex", addr, "/obexpush1.txt"));
            //
            var cli = new WebClient();
            cli.UploadProgressChanged += delegate(object sender, UploadProgressChangedEventArgs e)
            {
                console.WriteLine("Progress: {0}%, {1} of {2}",
                    e.ProgressPercentage, e.BytesSent, e.TotalBytesToSend);
            };
            cli.UploadDataCompleted += delegate(object sender, UploadDataCompletedEventArgs e)
            {
                WriteAsyncCompletedState("UploadData", e);
            };
            byte[] data;
            using (var content = new MemoryStream()) {
                // Using a StreamWriter to write text to the stream...
                using (TextWriter wtr = new StreamWriter(content)) {
                    int i = 0;
                    while (content.Length < contentLen) {
                        ++i;
                        wtr.WriteLine("Hello World GetRequestStream {0}",
                            // Write the time to the first line
                            i == 1 ? DateTime.Now.ToString() : i.ToString());
                        wtr.Flush(); // push through to stream
                    }
                    // 
                    data = content.ToArray();
                }
            }
            // Need method set!
            cli.UploadDataAsync(uri, "PUT", data);
            console.Pause("Un-pause after completion");
            //console.WriteLine("Calling GetResponse with " + req.ContentLength + " bytes of content...");
            //ObexWebResponse rsp = (ObexWebResponse)req.GetResponse();
            //console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode);
        }
#endif

        [MenuItem, SubMenu("OBEX")]
        public void ObexWebRequest_GET()
        {
            string addr = console.ReadLine("Address (IrDA / TCP/IP; hit return for BT)");
            if (addr.Length == 0) {
                BluetoothAddress addrB = console.ReadBluetoothAddress("Target Bluetooth device");
                addr = addrB.ToString();
            }
            string path = console.ReadLine("Path");
            Uri uri = new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0}://{1}/{2}",
                "obex-ftp", addr, path));
            console.WriteLine("Using URL: '{0}'", uri);
            ObexWebRequest req = new ObexWebRequest(uri);
            req.Method = "GET";
            ObexWebResponse rsp = (ObexWebResponse)req.GetResponse();
            console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode);
            var strm = rsp.GetResponseStream();
            console.WriteLine("  ContentLength: {0}, Stream.Length: {1}",
                rsp.ContentLength, strm.Length);
            using (Stream rspData = strm) {
                StreamReader rdr = new StreamReader(rspData);
                console.WriteLine("data, first line: '{0}'", rdr.ReadLine());
            }
        }

        [MenuItem, SubMenu("OBEX")]
        public void Obex_Listener()
        {
            m_lsnrStop = false;
            Interlocked.Exchange(ref m_lsnrId, 0);
            ObexListener lstnr = null;
            ObexTransport? proto = null;
            while (lstnr == null) {
                int? i = console.ReadOptionalInteger("Which ObexListener transport: 1. Bluetooth, 2. IrDA, 3. TCP/IP");
                if (!i.HasValue || i.Value == 1) {
                    proto = ObexTransport.Bluetooth;
                    lstnr = Create_ObexListener_Bluetooth();
                } else {
                    while (!proto.HasValue) {
                        switch (i) {
                            case 2:
                                proto = ObexTransport.IrDA;
                                break;
                            case 3:
                                proto = ObexTransport.Tcp;
                                break;
                            default:
                                // (No Trace.Fail on NETCFv2)
                                Debug.Assert(false, "invalid input: " + i);
                                break;
                        }
                    }
                    lstnr = new ObexListener(proto.Value);
                }
            }
            console.WriteLine("Created {0} ObexListener.", proto);
            //
            var auth = console.ReadYesNoCancel("Authenticate value (or don't set)", null);
            if (auth.HasValue) { lstnr.Authenticate = auth.Value; }
            console.WriteLine("get_Authenticate value: '{0}'", lstnr.Authenticate);
            var encrypt = console.ReadYesNoCancel("Encrypt value (or don't set)", null);
            if (encrypt.HasValue) { lstnr.Encrypt = encrypt.Value; }
            console.WriteLine("get_Encrypt value: '{0}'", lstnr.Encrypt);
            //
            lstnr.Start();
            console.WriteLine("OBEX listening");
            console.WriteLine("get_Authenticate value: '{0}'", lstnr.Authenticate);
            console.WriteLine("get_Encrypt value: '{0}'", lstnr.Encrypt);
            int? count = console.ReadOptionalInteger("How many acceptors threads? (1 by default)");
            if (count == null) {
                count = 1;
            }
            console.WriteLine("Starting acceptor...");
            for (int i = 0; i < count; ++i) {
                ThreadPool.QueueUserWorkItem(ObexListener_Worker, lstnr);
            }
            try {
                console.Pause("Un-pause to continue; and close listener");
            } finally {
                m_lsnrStop = true;
                lstnr.Stop();
            }
        }

        private volatile bool m_lsnrStop;
        private int m_lsnrId;

        void ObexListener_Worker(object state)
        {
            int id = Interlocked.Increment(ref m_lsnrId);
            while (true) {
                ObexListener lstnr = (ObexListener)state;
                if (m_lsnrStop || !lstnr.IsListening) {
                    break;
                }
                ObexListenerContext ctx = null;
                try {
                    ctx = lstnr.GetContext();
                    if (ctx == null) {
                        console.WriteLine("{0}: OBEX null context!", id);
                        continue;
                    }
                    console.WriteLine("{0}: OBEX accepted lContext", id);
                    ObexListenerRequest req = ctx.Request;
                    console.WriteLine("{0}: OBEX lRequest url: {1}, ContentLength64: {2}",
                        id, req.RawUrl, req.ContentLength64);
                    var ms = (MemoryStream)req.InputStream;
                    console.WriteLine("{0}: InputStream.Len: {1}",
                        id, ms.Length);
                } catch (Exception ex) {
                    console.WriteLine("{0}: OBEX ctx/req fault: {1}", id, ex);
                }
                ctx = null;
            }//while
        }


        //--------
#if TEST_EARLY
        [MenuItem, SubMenu("Security etc")]
        public void SetPain()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Address");
            string passphrase = console.ReadLine("Passphrase");
            using (BluetoothClient cli = new BluetoothClient()) {
               InTheHand.Net.Bluetooth.Widcomm.BOND_RETURN_CODE ret = cli.WidcommHack__SetPinX(addr, passphrase);
               console.WriteLine("Bond returned: {0} = 0x{1:X}", ret, (int)ret);
           }
        }
#endif

        private BluetoothPublicFactory GetStackFactory()
        {
            var r = Get_BluetoothRadio();
            if (r == null) {
                console.WriteLine("No radios.");
                throw new InvalidOperationException("No radios.");
            }
            return r.StackFactory;
        }

        [MenuItem, SubMenu("Security etc")]
        public void PairRequest()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Address");
            string passphrase = console.ReadLine("Passphrase");
            if (string.IsNullOrEmpty(passphrase)) {
                bool makeNull = console.ReadYesNo("Do you want to use a null/Nothing passphrase", true);
                passphrase = makeNull ? null : string.Empty;
            }
            InTheHand.Net.Bluetooth.Factory.IBluetoothSecurity sec
                = GetStackFactory().BluetoothSecurity;
            bool ret = sec.PairRequest(addr, passphrase);
            console.WriteLine("PairRequest returned: {0}", ret);
        }

        [MenuItem, SubMenu("Security etc")]
        public void RemoveDevice()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Address");
            InTheHand.Net.Bluetooth.Factory.IBluetoothSecurity sec
                = GetStackFactory().BluetoothSecurity;
            bool ret = sec.RemoveDevice(addr);
            console.WriteLine("RemoveDevice returned: {0}", ret);
        }

        [MenuItem, SubMenu("Security etc")]
        public void SetPin()
        {
            string passphrase = console.ReadLine("Passphrase");
            if (string.IsNullOrEmpty(passphrase)) {
                bool makeNull = console.ReadYesNo("Do you want to use a null/Nothing passphrase", true);
                passphrase = makeNull ? null : string.Empty;
            }
            BluetoothAddress addr = console.ReadBluetoothAddress("Address");
            InTheHand.Net.Bluetooth.Factory.IBluetoothSecurity sec
                = GetStackFactory().BluetoothSecurity;
            bool ret = sec.SetPin(addr, passphrase);
            console.WriteLine("SetPin returned: {0}", ret);
        }

        [MenuItem, SubMenu("Security etc")]
        public void RevokePin()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Address");
            string passphrase = console.ReadLine("Passphrase");
            if (string.IsNullOrEmpty(passphrase)) {
                bool makeNull = console.ReadYesNo("Do you want to use a null/Nothing passphrase", true);
                passphrase = makeNull ? null : string.Empty;
            }
            InTheHand.Net.Bluetooth.Factory.IBluetoothSecurity sec
                = GetStackFactory().BluetoothSecurity;
            bool ret = sec.RevokePin(addr);
            console.WriteLine("RevokePin returned: {0}", ret);
        }

        //--------
        [MenuItem, SubMenu("MultiStack")]
        public void ListenerOnStack1ClientOnStack2()
        {
            BluetoothRadio[] radios = BluetoothRadio_GetAllAndDump();
            Debug.Assert(radios != null, "Should't return null!");
            if (radios.Length < 2) {
                console.WriteLine("Fewer that two radios were found ({0}).", radios.Length);
                return;
            }
            //
            ListenerOnStackAClientOnStackB(radios[0], radios[1]);
        }

        [MenuItem, SubMenu("MultiStack")]
        public void ListenerOnStack2ClientOnStack1()
        {
            BluetoothRadio[] radios = BluetoothRadio_GetAllAndDump();
            Debug.Assert(radios != null, "Should't return null!");
            if (radios.Length < 2) {
                console.WriteLine("Fewer that two radios were found ({0}).", radios.Length);
                return;
            }
            //
            ListenerOnStackAClientOnStackB(radios[1], radios[0]);
        }

        public void ListenerOnStackAClientOnStackB(BluetoothRadio radioLsnr, BluetoothRadio radioCli)
        {
            BluetoothListener lsnr = radioLsnr.StackFactory.CreateBluetoothListener(BluetoothService.Wap);
            BluetoothClient conn = null;
            BluetoothClient cli = null;
            try {
                lsnr.Start();
#if APM_LSNR
            IAsyncResult arLsnr = lsnr.BeginAcceptBluetoothClient(MiscCallback, "ListenerOnStack1ClientOnStack2");
#else
                ManualResetEvent accepted = new ManualResetEvent(false);
                WaitCallback dlgt = delegate(object state)
                {
                    BluetoothListener lsnr_ = (BluetoothListener)state;
                    try {
                        conn = lsnr_.AcceptBluetoothClient();
                        console.WriteLine("Server got connection from : '{0}' {1}", conn.RemoteMachineName, conn.RemoteEndPoint);
                        PrintLocalEndpointIfAvailable(conn);
                    } catch (Exception ex) {
                        console.WriteLine("Accept failed with: " + ex);
                    } finally {
                        accepted.Set();
                    }
                };
                conn = null;
                ThreadPool.QueueUserWorkItem(dlgt, lsnr);
#endif
                BluetoothEndPoint lep = lsnr.LocalEndPoint;
                console.WriteLine("Listener active on SCN: {0}", lep.Port);
                //
                cli = radioCli.StackFactory.CreateBluetoothClient();
                BluetoothEndPoint rep = new BluetoothEndPoint(radioLsnr.LocalAddress, BluetoothService.Empty, lep.Port);
                console.WriteLine("Gonna connect to: {0}", rep);
                console.Pause("Un-pause to connect!");
                cli.Connect(rep);
                console.WriteLine("Client Connected to : '{0}' {1}", cli.RemoteMachineName, cli.RemoteEndPoint);
                PrintLocalEndpointIfAvailable(cli);
                //
#if APM_LSNR
                console.WriteLine("Lsnr.Accept.IsCompleted: {0}", arLsnr.IsCompleted);
                conn = lsnr.EndAcceptBluetoothClient(arLsnr);
#else
                console.WriteLine("waiting for Lsnr.Accept completion event.");
                accepted.WaitOne();
                Debug.Assert(conn != null, "Should have veen set by thread!");
#endif
                console.WriteLine("All success");
                console.Pause("Un-pause to exit (and close both)");
            } finally {
                if (lsnr != null)
                    lsnr.Stop();
                if (conn != null)
                    conn.Dispose();
                if (cli != null)
                    cli.Dispose();
            }
        }

        [MenuItem, SubMenu("MultiStack")]
        public void ObexWebRequest_MultiAsync()
        {
            BluetoothRadio[] radios = BluetoothRadio_GetAllAndDump();
            Debug.Assert(radios != null, "Should't return null!");
            if (radios.Length < 1) {
                console.WriteLine("No radios found.");
                return;
            }
            //
            List<BluetoothAddress> addrList = new List<BluetoothAddress>();
            for (int i = 0; i < radios.Length; ++i) {
                BluetoothAddress addr = console.ReadBluetoothAddress(
                    "Target device for radio #" + (i + 1));
                addrList.Add(addr);
            }//for
            bool bigSend = console.ReadYesNo("Send big (1MB) or small?", false);
            byte[] bigBuf = null;
            if (bigSend) {
                bigBuf = new byte[1024 * 1024];
#if !NETCF
                Array.ForEach(bigBuf, delegate(byte cur) { cur = (byte)'a'; });
#endif
            }
            List<IAsyncResult> arList = new List<IAsyncResult>(addrList.Count);
            for (int i = 0; i < radios.Length; ++i) {
                BluetoothAddress addr = addrList[i];
                Uri uri = new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "{0}://{1}{2}",
                    "obex", addr, "/obexpush1.txt"));
                BluetoothPublicFactory factory = radios[i].StackFactory;
                ObexWebRequest req = factory.CreateObexWebRequest(uri);
                using (Stream content = req.GetRequestStream()) {
                    // Using a StreamWriter to write text to the stream...
                    if (bigSend) {
                        content.Write(bigBuf, 0, bigBuf.Length);
                    } else {
                        using (TextWriter wtr = new StreamWriter(content)) {
                            wtr.WriteLine("Hello World GetRequestStream");
                            wtr.WriteLine("Hello World GetRequestStream 2");
                            wtr.Flush();
                            // Set the Length header value
                            req.ContentLength = content.Length;
                        }
                    }
                }
                IAsyncResult ar = req.BeginGetResponse(null, req);
                arList.Add(ar);
            }//for
            //
            console.WriteLine("Waiting for all requests to complete...");
            WaitAll(arList);
            console.WriteLine("All requests completed...");
            //
            for (int i = 0; i < radios.Length; ++i) {
                IAsyncResult cur = arList[i];
                ObexWebRequest req = (ObexWebRequest)cur.AsyncState;
                console.WriteLine("- - - #{1} - - -", null, (i + 1));
                try {
                    ObexWebResponse rsp = (ObexWebResponse)req.EndGetResponse(cur);
                    console.WriteLine("#{1}. Response Code: {0} (0x{0:X})", rsp.StatusCode, (i + 1));
                } catch (Exception ex) {
                    console.WriteLine("#{1}. Failed: {0}", ex, (i + 1));
                }
            }//for
        }

        //----
        [MenuItem, SubMenu("Data")]
        public void SetReadTimeout()
        {
            if (_peer == null) {
                console.WriteLine("no connection");
                return;
            }
            console.WriteLine("ReadTimeout: {0}", _peer.ReadTimeout);
            int? to = console.ReadOptionalInteger("New value");
            if (to.HasValue)
                _peer.ReadTimeout = to.Value;
        }

        [MenuItem, SubMenu("Data")]
        public void SetWriteTimeout()
        {
            if (_peer == null) {
                console.WriteLine("no connection");
                return;
            }
            console.WriteLine("WriteTimeout: {0}", _peer.WriteTimeout);
            int? v = console.ReadOptionalInteger("New value");
            if (v.HasValue)
                _peer.WriteTimeout = v.Value;
        }

        //----
        Stream _tcpConn;

        [MenuItem, SubMenu("SOCKETS")]
        public void TcpConnect()
        {
            string hostname = console.ReadLine("hostname");
            int port = console.ReadInteger("port");
            System.Net.Sockets.TcpClient cli = new System.Net.Sockets.TcpClient(hostname, port);
            _tcpConn = cli.GetStream();
        }

        [MenuItem, SubMenu("SOCKETS")]
        public void TcpClose()
        {
            if (_tcpConn != null) {
                _tcpConn.Close();
            }
            _tcpConn = null;
        }

        //----
        [MenuItem, SubMenu("DeviceInfo")]
        public void GetDeviceInfo()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Device Address");
            BluetoothDeviceInfo device = Create_BluetoothDeviceInfo(addr);
            bool again;
            do {
                console.WriteLine(DumpDeviceInfo(device));
                RadioVersions v;
                try {
#if true
                    v = device.GetVersions();
#elif false
                    var w = device.GetImpl<InTheHand.Net.Bluetooth.Msft.WindowsBluetoothDeviceInfo>();
                    if (w != null) {
                        v = w.GetVersions();
                    }
#endif
                } catch (Exception ex) {
                    console.WriteLine("GetVersions error: " + Exception_FirstLine(ex));
                    var w32ex = ex as Win32Exception;
                    if (w32ex != null) {
                        console.WriteLine("  NativeErrorCode: " + w32ex.NativeErrorCode);
                    }
                    goto afterDisplayVersion;
                }
                if (v == null) {
                    console.WriteLine("GetVersions returned null!!!!!!!!!!!!!");
                } else {
                    console.WriteLine(DumpVersions(v));
                }
            afterDisplayVersion:
                //
                again = console.ReadYesNo("Display same one again", false);
                if (again) {
                    var doSdp = console.ReadYesNo("Do SDP to", false);
                    if (doSdp) {
                        device.BeginGetServiceRecords(BluetoothService.L2CapProtocol, null, null);
                    }
                }
            } while (again);
        }

        [MenuItem, SubMenu("DeviceInfo")]
        public void GetDeviceInfoAndConnect()
        {
            Action<BluetoothAddress> dlgtDevInfo = delegate(BluetoothAddress addrX)
            {
                BluetoothDeviceInfo device = Create_BluetoothDeviceInfo(addrX);
                console.WriteLine("name0: " + ToStringQuotedOrNull(device.DeviceName));
                console.WriteLine("name1: " + ToStringQuotedOrNull(device.DeviceName));
            };
            Action<BluetoothAddress> dlgtConn = delegate(BluetoothAddress addrX)
            {
                Thread.Sleep(1500);
                console.WriteLine("Connecting...");
                ConnectSimpleClosingFirstPart2(addrX);
            };
            //
            BluetoothAddress addrDev0 = console.ReadBluetoothAddress("Device Address for DeviceInfo");
            BluetoothAddress addrConn = console.ReadBluetoothAddress("Device Address for Connect");
            long addrTmpl = addrDev0.ToInt64();
            var arrArBdi = new IAsyncResult[4];
            //
            new BluetoothClient(); // Init the stack in case the user forgot to do so before.
            var ar = DelegateExtension.BeginInvoke(dlgtConn, addrConn, null, null);
            for (int i = 0; i < arrArBdi.Length; ++i) {
                arrArBdi[i] = DelegateExtension.BeginInvoke(dlgtDevInfo, new BluetoothAddress(addrTmpl++), null, null);
            }
            try {
                DelegateExtension.EndInvoke(dlgtConn, ar);
                console.WriteLine("connect success");
            } catch (System.Net.Sockets.SocketException) {
                console.WriteLine("connect fail");
            }
            foreach (var curAr in arrArBdi) {
                DelegateExtension.EndInvoke(dlgtDevInfo, curAr);
            }
        }

        [MenuItem, SubMenu("DeviceInfo")]
        public void SetServiceState()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Device Address");
            Guid? svcClass = console.ReadOptionalBluetoothUuid("Service Class (default SPP)", null);
            if (svcClass == null) svcClass = BluetoothService.SerialPort;
            console.WriteLine("Service: {0}", svcClass);
            BluetoothDeviceInfo device = Create_BluetoothDeviceInfo(addr);
            console.WriteLine(DumpDeviceInfoBrief(device));
            bool state = console.ReadYesNo("Enable/Disable the service", true);
            console.WriteLine("Will set the service to: " + state);
            device.SetServiceState(svcClass.Value, state, true);
        }

        [MenuItem, SubMenu("DeviceInfo")]
        public void GetInstalledServices()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Device Address");
            BluetoothDeviceInfo device = Create_BluetoothDeviceInfo(addr);
            console.WriteLine(DumpDeviceInfoBrief(device));
            var guidList = device.InstalledServices;
            console.WriteLine("{0} of", guidList.Length);
            foreach (var cur in guidList) {
                var name = BluetoothService.GetName(cur);
                //var shortI = BluetoothService.CreateBluetoothUuid(
                console.WriteLine("* {0} aka {1}", cur, name);
            }
        }

#if !ANDROID
        [MenuItem, SubMenu("WidcommPlaying")]
        public void WidcommIsPresentConnected()
        {
            BluetoothAddress addr = console.ReadBluetoothAddress("Device Address");
            InTheHand.Net.Bluetooth.Widcomm.RemoteDeviceState state
                = new InTheHand.Net.Bluetooth.Widcomm.WidcommPlaying().FindIfPresentOrConnected(addr);
            console.WriteLine("state: {0}", state);
        }

        [MenuItem, SubMenu("WidcommPlaying")]
        public void DoPowerDownUpReset()
        {
            var p = new InTheHand.Net.Bluetooth.Widcomm.WidcommPlaying();
            var prev = p.DoPowerDownUpReset;
            var set = console.ReadYesNoCancel(string.Format(
                "DoPowerDownUpReset current: {0}.  Set to", prev), null);
            if (set.HasValue) {
                p.DoPowerDownUpReset = set.Value;
            }
            console.WriteLine("DoPowerDownUpReset set: {0}, was: {1}.",
                set, prev);
        }
#endif

        //----
#if !ANDROID
        [MenuItem, SubMenu("Serial")]
        public void SerialPorts_List()
        {
#if !NETCF
            SerialPortsListWmi();
#else
            SerialPortsListWmRegistry();
#endif
            //----
            console.WriteLine("SerialPort.GetPortNames...");
            string[] nameList = System.IO.Ports.SerialPort.GetPortNames();
            console.WriteLine("SerialPort.GetPortNames: {0}",
                string.Join(", ", nameList));
        }

#if NETCF
        private void SerialPortsListWmRegistry()
        {
            //console.WriteLine("SerialPortsListWmRegistry");
            using (var portsK = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Bluetooth\Serial\Ports", false)) {
                if (portsK == null) {
                    console.WriteLine("Bluetooth COM port configuration is missing.");
                    return;
                }
                foreach (var devKeyName in portsK.GetSubKeyNames()) {
                    BluetoothAddress addr;
                    try {
                        addr = BluetoothAddress.Parse(devKeyName);
                    } catch (FormatException) {
                        console.WriteLine("Error: key name is not a device address: '{0}'", devKeyName);
                        continue;
                    }
                    console.WriteLine("DeviceID:    {0} ", addr);
                    using (var devKey = portsK.OpenSubKey(devKeyName)) {
                        var portN0 = devKey.GetValue("Port");
                        var portN1 = portN0 as string;
                        var portN = TruncateAtZeros(portN1);
                        console.WriteLine("PortName: '{0}'", portN);
                    }
                }//for
            }
        }

        string TruncateAtZeros(string value)
        {
            if (value == null) return null;
            string result = value;
            int idx = value.IndexOf((char)0);
            if (idx != -1) {
                result = value.Substring(0, idx);
            }
#if DUMP_TRUNCATED_STRING_CONTENT
            if (value.Length != result.Length) {
                console.WriteLine("(Zero in string!! ({0}vs{1}))", value.Length, result.Length);
                // (No Array.ForEach etc in NETCF)
                StringBuilder bldr = new StringBuilder();
                foreach (var ch in value.ToCharArray()) {
                    if (char.IsControl(ch))
                        bldr.AppendFormat(CultureInfo.InvariantCulture, "\\x{0:x4}", (ushort)ch);
                    else
                        bldr.Append(ch);
                }
                console.WriteLine("([{0}])", bldr.ToString());
            }
#endif
            return result;
        }
#endif

#if !NETCF
        private void SerialPortsListWmi()
        {
            const string Win32_SerialPort = "Win32_SerialPort";
#if false // Either work
            console.WriteLine("Running query...");
            System.Management.SelectQuery q = new System.Management.SelectQuery(Win32_SerialPort);
            System.Management.ManagementObjectSearcher s = new System.Management.ManagementObjectSearcher(q);
#else
            console.WriteLine("Running WMI query...");
            System.Management.ManagementObjectSearcher s = new System.Management.ManagementObjectSearcher(
                "SELECT DeviceID,PNPDeviceID FROM " + Win32_SerialPort);
#endif
            console.WriteLine("Getting results...");
            System.Management.ManagementObjectCollection list = s.Get();
            console.WriteLine("Enumerating results...");
            foreach (System.Management.ManagementBaseObject cur in list) {
                object id = cur.GetPropertyValue("DeviceID");
                object pnpId = cur.GetPropertyValue("PNPDeviceID");
                console.WriteLine("DeviceID:    {0} ", id);
                console.WriteLine("PNPDeviceID: {0} ", pnpId);
                console.WriteLine("");
            }//for
        }
#endif

        SerialPort SerialPortCreate()
        {
            var nameNum = console.ReadOptionalInteger("Port -- if numerical 'COMx'");
            string name;
            if (nameNum != null) {
                name = "COM" + nameNum.ToString();
            } else {
                name = console.ReadLine("Port name");
            }
            var bitrate = console.ReadOptionalInteger("Bit rate");
            System.IO.Ports.SerialPort port;
            if (bitrate == null) {
                console.WriteLine("Connecting to port '{0}'.", name);
                port = new System.IO.Ports.SerialPort(name);
            } else {
                console.WriteLine("Connecting to port '{0}' at bitrate '{1}'.", name, bitrate.Value);
                port = new System.IO.Ports.SerialPort(name, bitrate.Value);
            }
            return port;
        }

        [MenuItem, SubMenu("Serial")]
        public void SerialPort_GetDefaults()
        {
            string name = console.ReadLine("(Serial) Device Name");
            console.WriteLine("(Serial) Device Name: {0}", name);
            var cc = new Foo.COMMCONFIG(null);
            uint size = checked((uint)cc.dwSize);
            bool success = Foo.NativeMethods.GetDefaultCommConfig(name, ref cc, ref size);
            var gle = Marshal.GetLastWin32Error();
            if (!success) {
                console.WriteLine("Failed: " + gle);
                return;
            }
            console.WriteLine("dwProviderSubType: {0}, baud: {1}, size: {2} (provider offset: {3}, size: {4})",
                cc.dwProviderSubType, cc.dcb.BaudRate,
                cc.dwSize,
                cc.dwProviderOffset, cc.dwProviderSize);
        }

        static class Foo
        {
            internal static class NativeMethods
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                internal static extern bool GetDefaultCommConfig(string lpszName, [In, Out] ref COMMCONFIG lpCC,
                   ref uint lpdwSize);

            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct DCB
            {
                public Int32 DCBLength;
                public Int32 BaudRate;
                public Int32 fBitField;
                public Int16 wReserved;
                public Int16 XonLim;
                public Int16 XoffLim;
                public Byte ByteSize;
                public Byte Parity;
                public Byte StopBits;
                public Char XonChar;
                public Char XoffChar;
                public Char ErrorChar;
                public Char EofChar;
                public Char EvtChar;
                public Int32 wReserved1;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct COMMCONFIG
            {
                public Int32 dwSize;
                public Int16 wVersion;
                public Int16 wReserved;
                public DCB dcb;
                public Int32 dwProviderSubType;
                public Int32 dwProviderOffset;
                public Int32 dwProviderSize;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200/*e.g.*/)]
                public byte[] ProviderData;

                public COMMCONFIG(object fake)
                    : this()
                {
                    this.dwSize = Marshal.SizeOf(this);
                }
            }
        }


        [MenuItem, SubMenu("Serial")]
        public void SerialPortCo_nnect()
        {
            var port = SerialPortCreate();
            port.Open();
            _peer = port.BaseStream;
        }

        [MenuItem, SubMenu("Serial")]
        public void SerialPort_ConnectAlsoUsePortEvents()
        {
            var port = SerialPortCreate();
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.ErrorReceived += new SerialErrorReceivedEventHandler(port_ErrorReceived);
            port.PinChanged += new SerialPinChangedEventHandler(port_PinChanged);
            port.Open();
            _peer = port.BaseStream;
        }

        void port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            console.WriteLine("Port PinChanged: " + e.EventType);
        }

        void port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            console.WriteLine("Port Error: " + e.EventType);
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            console.WriteLine("Port Rcv: " + e.EventType);
        }
#endif

        [MenuItem, SubMenu("Serial")]
        public void OpenFileStream()
        {
            string name = console.ReadLine("File/port name");
            _peer = new FileStream(name, FileMode.Open);
        }

        //----

#if !ANDROID
#if NETCF
        InTheHand.Net.Ports.BluetoothSerialPort _btSerialPort;
#endif
        InTheHand.Net.Bluetooth.Widcomm.WidcommSerialPort _wcSerialPort;

        [MenuItem, SubMenu("Serial")]
        public void BtWcSerialPortCliClose()
        {
#if NETCF
            _btSerialPort.Close();
            _btSerialPort = null;
#endif
            _wcSerialPort.Close();
            _wcSerialPort = null;
        }

        [MenuItem, SubMenu("Serial")]
        public void BtWcSerialPortCliNullNoDispose()
        {
#if NETCF
            _btSerialPort = null;
#endif
            _wcSerialPort = null;
        }

#if NETCF
        [MenuItem, SubMenu("Serial")]
        public void BtSerialPortCliConnect()
        {
            var addr = console.ReadBluetoothAddress("target");
            var addrWithSpp = new BluetoothEndPoint(addr, BluetoothService.SerialPort);
            var spp = InTheHand.Net.Ports
                .BluetoothSerialPort.CreateClient(addrWithSpp);
            console.WriteLine("Success: {0}", spp.PortName);
            _btSerialPort = spp;
        }
#endif

        [MenuItem, SubMenu("Serial")]
        public void WcSerialPortCliConnect()
        {
            var addr = console.ReadBluetoothAddress("Target Device");
            var count = console.ReadOptionalInteger("Repeat count");
            console.WriteLine(null);
            Converter<BluetoothAddress, InTheHand.Net.Bluetooth.Widcomm.WidcommSerialPort> open
                = addrI =>
                {
                    var spp = InTheHand.Net.Bluetooth.Widcomm
                                        .WidcommSerialPort.CreateClient(addrI);
                    console.WriteLine("Success: {0}", spp.PortName);
                    return spp;
                };
            //
            if (count == null) {
                _wcSerialPort = open(addr);
            } else {
                for (int i = 0; i < count; ++i) {
                    using (var spp = open(addr)) {
                    }
                }//for
            }
        }
#endif

        //--------
        [MenuItem, SubMenu("SOCKETS")]
        public void TestsSocketsAndSockaddr()
        {
            Converter<bool, System.Net.Sockets.Socket> createSocket = delegate(bool unspecificProto)
            {
                System.Net.Sockets.Socket s0;
                if (unspecificProto)
                    s0 = new System.Net.Sockets.Socket(
                        AddressFamily32.Bluetooth, System.Net.Sockets.SocketType.Stream,
                        System.Net.Sockets.ProtocolType.Unspecified);
                else
                    s0 = new System.Net.Sockets.Socket(
                        AddressFamily32.Bluetooth, System.Net.Sockets.SocketType.Stream,
                        BluetoothProtocolType.RFComm);
                return s0;
            };
            BluetoothEndPoint ep;
            System.Net.Sockets.Socket s;
            //
            int step = 0;
            console.WriteLine("#{0}, gonna createSocket(false)", ++step);
            s = createSocket(false);
            ep = new BluetoothEndPoint(
                BluetoothAddress.None,
                BluetoothService.ObexFileTransfer);
            console.WriteLine("#{0}, gonna Bind", ++step);
            // Bind     None/FTP/None
            try {
                s.Bind(ep);
            } catch (Exception ex) {
                console.WriteLine(ex);
            }
            ep = new BluetoothEndPoint(
                BluetoothAddress.Parse("12345679abcd"),
                BluetoothService.ObexObjectPush);
            console.WriteLine("#{0}, gonna Connect", ++step);
            // Connect  Foo/OPP/None
            try {
                s.Connect(ep);
            } catch (Exception ex) {
                console.WriteLine(ex);
            }
            //
            console.WriteLine("#{0}, gonna createSocket(false)", ++step);
            s = createSocket(false);
            ep = new BluetoothEndPoint(
                BluetoothAddress.None,
                BluetoothService.ObexFileTransfer,
                unchecked((Int32)0xF2345678));
            console.WriteLine("#{0}, gonna Bind", ++step);
            // Bind     None/FTP/Bar
            try {
                s.Bind(ep);
            } catch (Exception ex) {
                console.WriteLine("Excepted failure");
                console.WriteLine(ex);
            }
            //
            ep = new BluetoothEndPoint(
                BluetoothAddress.None,
                BluetoothService.ObexFileTransfer,
                0x45);
            console.WriteLine("#{0}, gonna Bind", ++step);
            // Bind     None/FTP/Bar
            try {
                s.Bind(ep);
            } catch (Exception ex) {
                console.WriteLine(ex);
            }
            //
            ep = new BluetoothEndPoint(
                BluetoothAddress.Parse("12345679abcd"),
                BluetoothService.Fax, 0x45);
            console.WriteLine("#{0}, gonna Connect", ++step);
            // Connect  Foo/OPP/Bar
            try {
                s.Connect(ep);
            } catch (Exception ex) {
                console.WriteLine(ex);
            }
            //
            console.WriteLine("#{0}, gonna createSocket(true)", ++step);
            try {
                s = createSocket(true);
            } catch (System.Net.Sockets.SocketException ex) {
                const int WSAEPROTOTYPE = 10041;
#if NETCF
                if (ex.ErrorCode != WSAEPROTOTYPE)
                    throw;
#else
                if (ex.SocketErrorCode != System.Net.Sockets.SocketError.ProtocolType)
                    throw;
#endif
                Debug.Assert(ex.NativeErrorCode == WSAEPROTOTYPE);
                console.WriteLine("(Somewhat) expected failure; how to get 'Unspecified' behaviour supported?");
                console.WriteLine(ex);
            }
        }

        [MenuItem, SubMenu("SOCKETS")]
        public void TryAllMsftSocketOptions()
        {
            var wr = _cli;
            BluetoothClient cli = null;
            if (wr == null || (cli = wr.Target) == null) {
                console.WriteLine("No BluetoothClient available, do a Connect/Accept.");
                return;
            }
            var sock = cli.Client;
            // Do one get option manually.
            {
                System.Net.Sockets.SocketOptionName option;
#if NETCF
                option = BluetoothSocketOptionName.GetMtu;
#else
                option = BluetoothSocketOptionName.XPMtu;
#endif
                var v = sock.GetSocketOption(BluetoothSocketOptionLevel.RFComm, option);
                var i = (int)v;
                console.WriteLine("Option MTU value: {0}=0x{0:X}", i);
            }
            // Do all known options.
            var nameFiList = typeof(BluetoothSocketOptionName).GetFields(
                BindingFlags.Static | BindingFlags.Public);
            foreach (var curField in nameFiList) {
                var curOpt = (System.Net.Sockets.SocketOptionName)curField.GetValue(null);
                var curName = curField.Name;
                string txtOpt = (int)curOpt >= 0 ? ((int)curOpt).ToString()
                    : "0x" + ((int)curOpt).ToString("X8");
                console.Write("Option: {0,24} {1,11} : ",
                    curName, txtOpt);
                try {
                    var v = sock.GetSocketOption(BluetoothSocketOptionLevel.RFComm, curOpt);
                    var i = (int)v;
                    console.WriteLine("value: {0}=0x{0:X}", i);
                } catch (System.Net.Sockets.SocketException soex) {
                    console.WriteLine("error: " + SocketException_Codes(soex));
                }
            }//for
        }

        //--------
        [MenuItem, SubMenu("DeviceInfo")]
        public void RssiRepeated()
        {
            console.WriteLine("Stack: {0}", Get_BluetoothRadio().SoftwareManufacturer);
            BluetoothAddress addr = console.ReadBluetoothAddress("Address of device to read RSSI for");
            bool doSdp = console.ReadYesNo("Do concurrent repeated SDP Query", true);
            //
            _quitRepeatRssiing.Reset();
            try {
                ThreadPool.QueueUserWorkItem(RepeatRssiing_ReadRssi, addr);
                if (doSdp) { // Doing SDP queries to force forming a connection.
                    ThreadPool.QueueUserWorkItem(RepeatRssiing_DoSdp, addr);
                }
                console.WriteLine(null);
                console.Pause("Un-pause to stop RSSI-ing");
            } finally {
                _quitRepeatRssiing.Set();
            }
        }

        ManualResetEvent _quitRepeatRssiing = new ManualResetEvent(false);

        void RepeatRssiing_ReadRssi(object state)
        {
            BluetoothAddress addr = (BluetoothAddress)state;
            int delayMs = 1000;
            BluetoothDeviceInfo info = Create_BluetoothDeviceInfo(addr);
            int? lastRssi = null;
            while (!_quitRepeatRssiing.WaitOne(delayMs, false)) {
                int rssi = info.Rssi;
                console.Write("r");
                if (rssi != lastRssi) {
                    lastRssi = rssi;
                    console.WriteLine(null);
                    console.WriteLine("{0:T}: RSSI now: {1}", DateTime.Now, rssi);
                }
            }//while
        }

        void RepeatRssiing_DoSdp(object state)
        {
            Thread.Sleep(5000); // show rssi not available when not connected
            //
            BluetoothAddress addr = (BluetoothAddress)state;
            int delayMs = 1000;
            BluetoothDeviceInfo info = Create_BluetoothDeviceInfo(addr);
            while (!_quitRepeatRssiing.WaitOne(delayMs, false)) {
                ServiceRecord[] records = info.GetServiceRecords(BluetoothService.TcpProtocol);
                console.Write("s");
            }//while
        }

        //--------
#if RADIO_MODESSS
        [MenuItem, SubMenu("Local")]
        public void SetModesss()
        {
            BluetoothRadio r = Get_BluetoothRadio();
            if (r == null) {
                console.WriteLine("No radio(?)");
                return;
            }
            console.WriteLine("Radio stack is: {0}", r.SoftwareManufacturer);
            var conno = console.ReadYesNoCancel("Connectable (on/off/leave)", null);
            var disco = console.ReadYesNoCancel("Discoverable (on/off/leave)", null);
            bool doit = console.ReadYesNo("Set radio mode to: c=" + conno + " d=" + disco, true);
            if (doit) {
                console.WriteLine("Setting Modes({0},{1})", conno, disco);
                r.SetMode(conno, disco);
            }
        }
#endif

        [MenuItem, SubMenu("Local")]
        public void SetMode()
        {
            BluetoothRadio r = Get_BluetoothRadio();
            if (r == null) {
                console.WriteLine("No radio(?)");
                return;
            }
            console.WriteLine("Radio stack is: {0}", r.SoftwareManufacturer);
            int choice = console.ReadInteger("0. PowerOff, 1. Connectable, 2. Discoverable");
            RadioMode mode = (RadioMode)choice;
            bool doit = console.ReadYesNo("Set radio mode to: " + mode, true);
            if (doit) {
                r.Mode = mode;
            }
        }

        [MenuItem, SubMenu("Local")]
        public void SetAllModes()
        {
            BluetoothRadio r_ = Get_BluetoothRadio();
            if (r_ == null) {
                console.WriteLine("No radio(?)");
                return;
            }
            console.WriteLine("Radio stack is: {0}", r_.SoftwareManufacturer);
            var doIt = console.ReadYesNo("Do Set All Modes", false);
            if (!doIt) return;
            //
            // o->c, o->d; c->o, c->d; d->o, d->c.
            var stateList = new RadioMode[] {
                RadioMode.PowerOff, RadioMode.Connectable,  RadioMode.Connectable,  RadioMode.Discoverable,
                RadioMode.PowerOff, RadioMode.Discoverable, RadioMode.Discoverable, RadioMode.Connectable,
                RadioMode.PowerOff, RadioMode.PowerOff,
            };
            int i;
            var originalMode = r_.Mode;
            console.WriteLine("Running test, using same radio instance each time.");
            i = 0;
            var r0 = r_;
            foreach (var cur in stateList) {
                console.Write("   Set {0,12}, ", cur);
                var ts = RunAndTime(delegate { r0.Mode = cur; });
                RadioMode? got = null;
                var tg = RunAndTime(delegate { got = r0.Mode; });
                if (got != cur)
                    console.Write("error at {0} expected {1} was {2}, ",
                        i, cur, got);
                console.WriteLine("set: {0,-16}, get: {1,-16}", ts, tg);
                ++i;
            }//for
            console.WriteLine("Done {0} tests.", i);
            console.WriteLine("Do test again, using new radio instance each time.");
            i = 0;
            foreach (var cur in stateList) {
                var rx = Get_BluetoothRadio();
                if (rx == null) {
                    console.WriteLine("error at {0} radio is null!", i);
                    goto next;
                }
                console.Write("   Set {0,12}, ", cur);
                var ts = RunAndTime(delegate { rx.Mode = cur; });
                RadioMode? got = null;
                var tg = RunAndTime(delegate { got = rx.Mode; });
                if (got != cur)
                    console.Write("error at {0} expected {1} was {2}, ",
                        i, cur, got);
                console.WriteLine("set: {0,-16}, get: {1,-16}", ts, tg);
            next:
                ++i;
            }//for
            console.WriteLine("Done {0} tests.", i);
            r_.Mode = originalMode;
        }

        [MenuItem, SubMenu("Local")]
        public void SetName()
        {
            BluetoothRadio r = Get_BluetoothRadio();
            if (r == null) {
                console.WriteLine("No radio(?)");
                return;
            }
            console.WriteLine("Radio stack is: {0}", r.SoftwareManufacturer);
            var name = console.ReadLine("New Name");
            bool doit = console.ReadYesNo("Set radio name to: " + name, true);
            if (doit) {
                r.Name = name;
            }
        }


        //--------
        [MenuItem, SubMenu("Data")]
        public void CloseLinger()
        {
            if (!VerifyConnectionWrite())
                return;
            BluetoothClient cli = _cli.Target;
            if (cli == null) {
                console.WriteLine("No BtCli to set LingerState on!.");
                return;
            }
            cli.LingerState = new System.Net.Sockets.LingerOption(true, 30);
            // Write until buffers full
            List<IAsyncResult> arList = new List<IAsyncResult>();
            byte[] buf = new byte[16 * 1024];
            bool exitAfterNext = false;
            while (true) {
                IAsyncResult ar = _peer.BeginWrite(buf, 0, buf.Length, MiscCallback, "BeginWrite");
                arList.Add(ar);
                console.Write(".");
                if (exitAfterNext)
                    break;
                if (!ar.IsCompleted) {
                    bool signalled = ar.AsyncWaitHandle.WaitOne(10000, false);
                    if (!signalled) {
                        exitAfterNext = true;
                    }
                }
            }//while
            console.WriteLine("Did BeginWrite until full ({0} times).", arList.Count);
            //
            console.WriteLine("Now going to Close.  Release receiver to not linger timeout.");
            console.Pause("Un-pause to Close");
            console.WriteLine("Closing at {0}", GetTime());
            DateTime startT = DateTime.UtcNow;
            try {
                _peer.Close();
                string end = GetTime();
                console.WriteLine("Close exited cleanly at {0} after {1}.", end, DateTime.UtcNow - startT);
            } catch (Exception ex) { // We throw Exception currently!
                string end = GetTime();
                console.WriteLine("Close failed at {0} after {1} with: {2}", end, DateTime.UtcNow - startT, ex);
            }
            //
            int i = 0;
            console.Write("Write statuses (. or x, where x=exception): ");
            foreach (IAsyncResult cur in arList) {
                if (!cur.IsCompleted) {
                    console.WriteLine("Write #{0} is not completed!", i + 1);
                }
                try {
                    _peer.EndWrite(cur);
                    console.Write(".");
                } catch (IOException) {
                    console.Write("x");
                }
                ++i;
            }//for
            console.WriteLine(null);
            console.WriteLine("Finished.");
        }

        //--------
        [MenuItem, SubMenu("Playing")]
        public void HackShutdownAll()
        {
            InTheHand.Net.Bluetooth.Factory.BluetoothFactory.HackShutdownAll();
            console.WriteLine("Done HackShutdownAll");
        }

        //--------
#if NETCF
        [MenuItem, SubMenu("Playing")]
        public void HidEnable_NotCompletedTesting()
        {
            IntPtr h = CreateFile("BHI0:", 0, 0, IntPtr.Zero, CreateFile__OPEN_EXISTING, 0, IntPtr.Zero);
            if (h == IntPtr.Zero || h == (IntPtr)(-1)) {
                int err = Marshal.GetLastWin32Error();
                console.WriteLine("CreateFile(BHI0:) returned: {0}, gle: {1} = 0x{1:X}", h, err);
                return;
            }
            console.WriteLine("CreateFile(BHI0:) SUCCESS");
            //console.WriteLine("Skipping for now!!!!!!");
            //return;
            //----
            int tmp;
            BluetoothAddress addr = console.ReadBluetoothAddress("Device address");
            byte[] addrBytes = addr.ToByteArray();
            bool ret = DeviceIoControl(h, BTHHID_IOCTL_HIDConnect, addrBytes, addrBytes.Length,
                IntPtr.Zero, 0, out tmp, IntPtr.Zero);
            int err2 = Marshal.GetLastWin32Error();
            console.WriteLine("DeviceIoControl(h, BTHHID_IOCTL_HIDConnect) ret: {0}", ret);
            if (!ret) {
                console.WriteLine("DeviceIoControl(h, BTHHID_IOCTL_HIDConnect) gle: {0} = 0x{0:X}", err2);
            }
        }

        // http://read.pudn.com//downloads123/sourcecode/embed/523853/BLUETOOTH/PROFILES/HID/BASE/hidioctl.h__.htm
        const int BTHHID_IOCTL_HIDConnect = 1;
        const int BTHHID_IOCTL_HIDVerify = 2;

        const int CreateFile__OPEN_EXISTING = 3;

        [System.Runtime.InteropServices.DllImport("coredll.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, UInt32 dwDesiredAccess,
            UInt32 dwShareMode, IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition,
            UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        [System.Runtime.InteropServices.DllImport("coredll.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);
        [System.Runtime.InteropServices.DllImport("coredll.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);
        [System.Runtime.InteropServices.DllImport("coredll.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);
#endif

#if NETCF
        [System.Runtime.InteropServices.DllImport("coredll.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int EnumDevices([Out]char[] devList, ref int bufLen);

        [MenuItem, SubMenu("Playing")]
        public void EnumDevices()
        {
            var buf = new char[4096];
            int bufLen = buf.Length; // Docs don't say measured in bytes or wchars!  Be conservative
            //const int ERROR_MORE_DATA = 234;
            var ret = EnumDevices(buf, ref bufLen);
            if (ret != 0) {
                console.WriteLine("!! EnumDevices failed: {0}", ret);
            }
            int dbgCharsUsed;
            var list = ParseDoubleNullTerminatedStrings(buf, out dbgCharsUsed);
            Debug.Assert(dbgCharsUsed * 2 == bufLen, "is measured in bytes");
            var x = list.Count;
            const string Sep = ", ";
            var all = string.Join(Sep, list.ToArray());
            console.WriteLine(all);
            //
            //
            TestParseDoubleNullTerminatedStrings();
        }

        private static List<string> ParseDoubleNullTerminatedStrings(char[] buf, out int dbgCharsUsed)
        {
            int idx = 0;
            var list = new List<string>();
            while (true) {
                if (buf[idx] == 0) {
                    break;
                }
                var idxEnd = Array.IndexOf(buf, (char)0, idx);
                var len = idxEnd - idx;
                var str = new string(buf, idx, len);
                list.Add(str);
                idx = idxEnd + 1;
            }
            dbgCharsUsed = idx + 1;
            return list;
        }

        static void TestParseDoubleNullTerminatedStrings()
        {
            var buf = new char[30];
            List<string> list;
            const char NullChar = (char)0;
            //
            list = DoTestParse(new char[]{
                'a','b','c',NullChar,
                'e','f',NullChar,
                'm','n','o','p',NullChar,
                NullChar });
            Assert_AreEqual(3, list.Count, "A Count");
            Assert_AreEqual(3, list[0].Length, "A0 Len");
            Assert_AreEqual("abc", list[0], "A0 Count");
            Assert_AreEqual(2, list[1].Length, "A1 Len");
            Assert_AreEqual("ef", list[1], "A1");
            Assert_AreEqual(4, list[2].Length, "A2 Len");
            Assert_AreEqual("mnop", list[2], "A2");
            //
            // Null at start -- (like first is empty)
            list = DoTestParse(new char[]{
                NullChar,
                NullChar });
            Assert_AreEqual(0, list.Count, "00 B list.Count");
            //
            // None -- NOT double null, is valid
            list = DoTestParse(new char[]{
                NullChar,'*','*'});
            Assert_AreEqual(0, list.Count, "C Count");
            //
            // First is empty -- same as all empty
            list = DoTestParse(new char[]{
                NullChar,
                'e','f',NullChar,
                NullChar });
            Assert_AreEqual(0, list.Count, "D Count");
            //
            // Second is empty -- same as termination
            list = DoTestParse(new char[]{
                'a','b','c',NullChar,
                NullChar,
                'm','n','o','p',NullChar,
                NullChar });
            Assert_AreEqual(1, list.Count, "E Count");
            Assert_AreEqual(3, list[0].Length, "E0 Len");
            Assert_AreEqual("abc", list[0], "E0 Count");
        }

        private static void FillWithRubbish(char[] buf)
        {
            for (int i = 0; i < buf.Length; ++i) buf[i] = 'z';
        }

        private static void Assert_AreEqual(int expected, int actual, string name)
        {
            if (expected != actual) throw new RankException(name);
        }

        private static void Assert_AreEqual(string expected, string actual, string name)
        {
            if (expected != actual) throw new RankException(name);
        }

        private static List<string> DoTestParse(char[] content)
        {
            char[] buf;
            //
            buf = new char[content.Length * 2];
            FillWithRubbish(buf);
            content.CopyTo(buf, 0);
            int dbgUsed;
            var list = ParseDoubleNullTerminatedStrings(buf, out dbgUsed);
            //TODO assert on dbgUsed
            return list;
#if false
            //
            buf = new char[content.Length];
            FillWithRubbish(buf);
            content.CopyTo(buf, 0);
            list = ParseDoubleNullTerminatedStrings(buf, buf.Length);
#endif
        }
#endif

        //--------
#if !NETCF
        [MenuItem]
        public void Win32EventsNew()
        {
            var r = Get_BluetoothRadio();
            // Better be MSFT+Win32 radio!!!
            using (var x = new BluetoothWin32Events(r)) {
                x.InRange += HandleWin32InRange;
                x.OutOfRange += HandleWin32OutOfRange;
                console.Pause("Continue to stop getting events");
            }
        }

        [MenuItem]
        public void Win32EventsShared()
        {
            var r = Get_BluetoothRadio();
            // Better be MSFT+Win32 radio!!!
            var x = BluetoothWin32Events.GetInstance();
            try {
                x.InRange += HandleWin32InRange;
                x.OutOfRange += HandleWin32OutOfRange;
                console.Pause("Continue to stop getting events");
            } finally {
                x.InRange -= HandleWin32InRange;
                x.OutOfRange -= HandleWin32OutOfRange;
            }
        }

        void HandleWin32InRange(object sender, BluetoothWin32RadioInRangeEventArgs e)
        {
            console.WriteLine("{0}: InRange {1} '{2}', now '{3}' was '{4}'.",
                GetTimeWallclockHR(), e.Device.DeviceAddress, e.Device.DeviceName,
                e.CurrentState, e.PreviousState);
        }
        void HandleWin32OutOfRange(object sender, BluetoothWin32RadioOutOfRangeEventArgs e)
        {
            console.WriteLine("{0}: OutOfRange {1} '{2}'.",
                GetTimeWallclockHR(), e.Device.DeviceAddress, e.Device.DeviceName);
        }
#endif

#if !NETCF && false
        [MenuItem, SubMenu("Playing")]
        public void AllIoctls()
        {
            console.WriteLine("Output is on Debug.Write...");
            InTheHand.Net.Bluetooth.Msft.MsftPlaying.AllIoctls();
        }

        [MenuItem, SubMenu("Playing")]
        public void AuthDeviceExFakeOob()
        {
            var device = console.ReadBluetoothAddress("target");
            byte[] oobData;
            int ret;
            //
            oobData = new byte[16];
            new Random().NextBytes(oobData);
            ret = InTheHand.Net.Bluetooth.Msft.MsftPlaying.AuthDeviceExFakeOob(
                 device, oobData);
            console.WriteLine("ret: {0} = 0x{0:X}", ret);
        }

        [MenuItem, SubMenu("Playing")]
        public void AuthDeviceExFakeOob_ThreeWays()
        {
            var device = console.ReadBluetoothAddress("target");
            byte[] oobData;
            int ret;
            //
            oobData = new byte[16];
            new Random().NextBytes(oobData);
            ret = InTheHand.Net.Bluetooth.Msft.MsftPlaying.AuthDeviceExFakeOob(
                 device, oobData);
            console.WriteLine("ret: {0} = 0x{0:X}", ret);
            //
            oobData = new byte[32];
            ret = InTheHand.Net.Bluetooth.Msft.MsftPlaying.AuthDeviceExFakeOob(
                 device, oobData);
            console.WriteLine("ret: {0} = 0x{0:X}", ret);
            //
            oobData = null;
            ret = InTheHand.Net.Bluetooth.Msft.MsftPlaying.AuthDeviceExFakeOob(
                 device, oobData);
            console.WriteLine("ret: {0} = 0x{0:X}", ret);
        }
#endif

#if false
        [MenuItem, SubMenu("Playing")]
        public void BlueZSdpDumpSdpBlueZDbus()
        {
            var b = new ServiceRecordBuilder();
            b.AddServiceClass(0x1105);
            b.AddServiceClass(BluetoothService.SerialPort);
            b.AddServiceClass(0x12345);
            //
            b.AddCustomAttribute(0x500, ElementType.Boolean, false);
            b.AddCustomAttribute(0x501, ElementType.Boolean, true);
            b.AddCustomAttribute(0x511, ElementType.Nil, null);
            //
            // UUID128 (AddServiceClass shortens automatically)
            b.AddCustomAttribute(0x1000, ElementType.Uuid128, BluetoothService.SerialPort);
            var g128 = new Guid("{54D39E6A-2AF2-47dd-BB14-59C476CF2848}");
            b.AddCustomAttribute(0x1001, ElementType.Uuid128, g128);
            //
            b.AddCustomAttribute(0x6000 + (ushort)ElementType.UInt8, ElementType.UInt8, (int)ElementType.UInt8);
            b.AddCustomAttribute(0x6000 + (ushort)ElementType.UInt16, ElementType.UInt16, 0xfe00 + (int)ElementType.UInt16);
            b.AddCustomAttribute(0x6000 + (ushort)ElementType.UInt32, ElementType.UInt32, 0xfe00 + (int)ElementType.UInt32);
            b.AddCustomAttribute(0x6000 + (ushort)ElementType.UInt64, ElementType.UInt64, 0xfe00 + (int)ElementType.UInt64);
            var arr128 = new byte[16];
            arr128[0] = 0xfe;
            arr128[15] = (byte)ElementType.UInt64;
            //b.AddCustomAttribute(0x6000 + (ushort)ElementType.UInt128, ElementType.UInt128, arr128);
            //b.AddCustomAttribute(new ServiceAttribute(0x6000 + (ushort)ElementType.UInt128, 
            //    new ServiceElement(ElementType.UInt128, arr128)));
            //
            b.AddCustomAttribute(0x7000 + (ushort)ElementType.Int8, ElementType.Int8, (int)ElementType.Int8);
            b.AddCustomAttribute(0x7000 + (ushort)ElementType.Int16, ElementType.Int16, 0x7e00 + (int)ElementType.Int16);
            b.AddCustomAttribute(0x7000 + (ushort)ElementType.Int32, ElementType.Int32, 0x7e00 + (int)ElementType.Int32);
            b.AddCustomAttribute(0x7000 + (ushort)ElementType.Int64, ElementType.Int64, 0x7e00 + (int)ElementType.Int64);
            //
            b.AddCustomAttribute(new ServiceAttribute(0x7000,
                new ServiceElement(ElementType.ElementAlternative,
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 1),
                    ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 2))));
            var foo = true;
            if (foo) {
                b.ServiceName = "foo";
            }
            //----
            var s = new BluezSdpXmlWriter().Write(b.ServiceRecord);
            console.WriteLine(s);
            //----
            var rOut = new BluezSdpXmlReader().Read(s);
            console.WriteLine(ServiceRecordUtilities.Dump(rOut));
            //================
            //
            new BluezSdpReaderTests().One();
            new BluezSdpReaderTests().Two_UnterminatedEntity();
        }
#endif

        //--------
        public static string ToStringQuotedOrNull<T>(T obj)
        {
            if (obj == null)
                return "(null)";
            else
                return obj.ToString();
        }

        static string Exception_FirstLine(Exception ex)
        {
            string line;
            using (StringReader rdr = new StringReader(ex.ToString())) {
                line = rdr.ReadLine();
            }
            var soex = ex as System.Net.Sockets.SocketException;
            if (soex != null) {
                line += SocketException_Codes(soex);
            }
            return line;
        }

        static string SocketException_Codes(System.Net.Sockets.SocketException soex)
        {
            string line = null;// string.Empty;
            line += " (" + soex.ErrorCode; // int
#if !NETCF
            line += " " + soex.SocketErrorCode; // Enum name 
#endif
            line += ")";
            return line;
        }

        static bool _WaitAll(WaitHandle[] events, int timeout)
        {
#if false && !NETCF
            return WaitHandle.WaitAll(events, timeout);
#else
            var final = DateTime.UtcNow.AddMilliseconds(timeout);
            foreach (var curE in events) {
                TimeSpan curTo = final.Subtract(DateTime.UtcNow);
                if (curTo.CompareTo(TimeSpan.Zero) <= 0) {
                    Debug.Fail("Should we expect to get here?  Normally exit at !signalled below...");
                    return false;
                }
                bool signalled = curE.WaitOne((int)curTo.TotalMilliseconds, false);
                if (!signalled) {
                    return false;
                }
            }//for
            return true;
#endif
        }

        static TimeSpan RunAndTime(ThreadStart method)
        {
            var start = DateTime.UtcNow;
            method();
            var time = DateTime.UtcNow - start;
            return time;
        }

    }//class

}
