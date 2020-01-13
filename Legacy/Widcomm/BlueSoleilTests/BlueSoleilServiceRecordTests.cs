using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestContext = System.Version; //dummy
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.BlueSoleil;
using InTheHand.Net.Bluetooth.AttributeIds;
using System.Runtime.InteropServices;
using NMock2;
using System.Diagnostics;

namespace BlueSoleilTests.AlanJMcF.BlueSoleil.Tests
{
    /// <summary>
    /// Summary description for BlueSoleilServiceRecordTests
    /// </summary>
    [TestClass]
    public class BlueSoleilServiceRecordTests
    {
        public BlueSoleilServiceRecordTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Manual mock native interface
        [TestMethod]
        public void OneNoExtAttr_ManualMock()
        {
            var hackApi = new TestSdBluesoleilApi();
            byte[] setSvcName = Encoding.UTF8.GetBytes("aabbccdd");
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, 0x1112, setSvcName, IntPtr.Zero);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'aabbccdd'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1112, svcC, "svcC");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("aabbccdd", sn, "sn");
            //----
            hackApi.AssertActionsNoMore();
        }

        [TestMethod]
        public void OneSppExtAttr_ManualMock()
        {
            var hackApi = new TestSdBluesoleilApi();
            //
            const byte Port = 23;
            var ext = new Structs.BtSdkRmtSPPSvcExtAttrStru(Port);
            IntPtr pExt = Marshal.AllocHGlobal(Marshal.SizeOf(ext));
            Marshal.StructureToPtr(ext, pExt, false);
            //
            byte[] setSvcName = Encoding.UTF8.GetBytes("aabbccdd");
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, 0x1112, setSvcName, pExt);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + NewLine
                + NewLine
                + "AttrId: 0x0004 -- ProtocolDescriptorList" + NewLine
                + "ElementSequence" + NewLine
                + "    ElementSequence" + NewLine
                + "        Uuid16: 0x100 -- L2CapProtocol" + NewLine
                + "    ElementSequence" + NewLine
                + "        Uuid16: 0x3 -- RFCommProtocol" + NewLine
                + "        UInt8: 0x17" + NewLine
                + "( ( L2Cap ), ( Rfcomm, ChannelNumber=23 ) )" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'aabbccdd'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1112, svcC, "svcC");
            //
            e = sr.GetAttributeById(UniversalAttributeId.ProtocolDescriptorList).Value;
            ServiceElement listRfcomm = e.GetValueAsElementArray()[1];
            ServiceElement eScn = listRfcomm.GetValueAsElementArray()[1];
            byte scn = (byte)eScn.Value;
            Assert.AreEqual(Port, scn, "scn");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("aabbccdd", sn, "sn");
            //----
            hackApi.AssertActionsInOrder("Btsdk_FreeMemory", pExt);
            hackApi.AssertActionsNoMore();
        }
#endregion


        #region Using Mockery
        private void SetupNoSearchAppExtSPPService(IBluesoleilApi hackApi)
        {
            Expect.Once.On(hackApi).Method("Btsdk_SearchAppExtSPPService")
                .WithAnyArguments().Will(Return.Value(BtSdkError.NO_SERVICE));
        }

        [TestMethod]
        public void OneNoExtAttr()
        {
            var mocks = new Mockery();
            var hackApi = mocks.NewMock<IBluesoleilApi>();
            SetupNoSearchAppExtSPPService(hackApi);
            Expect.Never.On(hackApi).Method("Btsdk_FreeMemory");
            //
            byte[] setSvcName = { (byte)'a', /* \u00E4 */ 0xC3, 0xA4, (byte)'b', (byte)'b', (byte)'c', (byte)'c', (byte)'d', (byte)'d' };
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, 0x1112, setSvcName, IntPtr.Zero);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'a\u00E4bbccdd'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1112, svcC, "svcC");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("a\u00E4bbccdd", sn, "sn");
            //----
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void OneSppExtAttr_FullLenServiceName()
        {
            var mocks = new Mockery();
            var hackApi = mocks.NewMock<IBluesoleilApi>();
            SetupNoSearchAppExtSPPService(hackApi);
            //
            const byte Port = 23;
            var ext = new Structs.BtSdkRmtSPPSvcExtAttrStru(Port);
            IntPtr pExt = Marshal.AllocHGlobal(Marshal.SizeOf(ext));
            Marshal.StructureToPtr(ext, pExt, false);
            Expect.Once.On(hackApi).Method("Btsdk_FreeMemory").With(pExt);
            //
            Debug.Assert(80 == StackConsts.BTSDK_SERVICENAME_MAXLENGTH, "BTSDK_SERVICENAME_MAXLENGTH: " + StackConsts.BTSDK_SERVICENAME_MAXLENGTH);
            byte[] setSvcName = new byte[StackConsts.BTSDK_SERVICENAME_MAXLENGTH];
            for (int i = 0; i < (setSvcName.Length - 1); ++i) setSvcName[i] = (byte)'a';
            setSvcName[setSvcName.Length - 1] = (byte)'b';
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, 0x1112, 
                setSvcName, pExt);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + NewLine
                + NewLine
                + "AttrId: 0x0004 -- ProtocolDescriptorList" + NewLine
                + "ElementSequence" + NewLine
                + "    ElementSequence" + NewLine
                + "        Uuid16: 0x100 -- L2CapProtocol" + NewLine
                + "    ElementSequence" + NewLine
                + "        Uuid16: 0x3 -- RFCommProtocol" + NewLine
                + "        UInt8: 0x17" + NewLine
                + "( ( L2Cap ), ( Rfcomm, ChannelNumber=23 ) )" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1112, svcC, "svcC");
            //
            e = sr.GetAttributeById(UniversalAttributeId.ProtocolDescriptorList).Value;
            ServiceElement listRfcomm = e.GetValueAsElementArray()[1];
            ServiceElement eScn = listRfcomm.GetValueAsElementArray()[1];
            byte scn = (byte)eScn.Value;
            Assert.AreEqual(Port, scn, "scn");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", sn, "sn");
            //----
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void OneNoExtAttr_PanuLeUnicodeSvcName()
        {
            var mocks = new Mockery();
            var hackApi = mocks.NewMock<IBluesoleilApi>();
            SetupNoSearchAppExtSPPService(hackApi);
            Expect.Never.On(hackApi).Method("Btsdk_FreeMemory");
            //
            byte[] setSvcName = {
                0x50, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00,
                0x6F, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x6C, 0x00,
                0x20, 0x00, 0x41, 0x00, 0x64, 0x00, 0x2D, 0x00,
                0x68, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x20, 0x00,
                0x55, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00,
                0x20, 0x00, 0x53, 0x00, 0x65, 0x00, 0x72, 0x00,
                0x76, 0x00, 0x69, 0x00, 0x63, 0x00, 0x65, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            ushort SvcClassPanu = 0x1115;
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, SvcClassPanu, setSvcName, IntPtr.Zero);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1115 -- Panu" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'Personal Ad-hoc User Service'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1115, svcC, "svcC");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("Personal Ad-hoc User Service", sn, "sn");
            //----
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        //Not supported!  Can't tell between little-endian and big-endian Unicode!
        //[TestMethod]
        public void OneNoExtAttr_PanuBeUnicodeSvcName()
        {
            var mocks = new Mockery();
            var hackApi = mocks.NewMock<IBluesoleilApi>();
            SetupNoSearchAppExtSPPService(hackApi);
            Expect.Never.On(hackApi).Method("Btsdk_FreeMemory");
            //
            byte[] setSvcName = {
          0x00, 0x50, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00,
                0x6F, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x6C, 0x00,
                0x20, 0x00, 0x41, 0x00, 0x64, 0x00, 0x2D, 0x00,
                0x68, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x20, 0x00,
                0x55, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72, 0x00,
                0x20, 0x00, 0x53, 0x00, 0x65, 0x00, 0x72, 0x00,
                0x76, 0x00, 0x69, 0x00, 0x63, 0x00, 0x65, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            ushort SvcClassPanu = 0x1115;
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, SvcClassPanu, setSvcName, IntPtr.Zero);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1115 -- Panu" + NewLine
                + NewLine
                + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    UInt16: 0x656E" + NewLine
                + "    UInt16: 0x6A" + NewLine
                + "    UInt16: 0x100" + NewLine
                + NewLine
                + "AttrId: 0x0100 -- ServiceName" + NewLine
                + "TextString: [en] 'Personal Ad-hoc User Service'" + NewLine
                + NewLine
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr);
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1115, svcC, "svcC");
            //
            string sn = sr.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName);
            Assert.AreEqual("Personal Ad-hoc User Service", sn, "sn");
            //----
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [TestMethod]
        public void DeviceIdExtAttr_NoServiceName()
        {
            var mocks = new Mockery();
            var hackApi = mocks.NewMock<IBluesoleilApi>();
            SetupNoSearchAppExtSPPService(hackApi);
            //
            var ext = new Structs.BtSdkRmtDISvcExtAttrStru(1, 2, 3, 4, true, 6, 0);
            IntPtr pExt = Marshal.AllocHGlobal(Marshal.SizeOf(ext));
            Marshal.StructureToPtr(ext, pExt, false);
            Expect.Once.On(hackApi).Method("Btsdk_FreeMemory").With(pExt);
            //
            byte[] setSvcName = new byte[0];
            const int SvcClassPnp = 0x1200;
            var attrs = new Structs.BtSdkRemoteServiceAttrStru(0, SvcClassPnp, 
                setSvcName, pExt);
            ServiceRecord sr = BluesoleilDeviceInfo.CreateServiceRecord(ref attrs, hackApi);
            //
            const string NewLine = "\r\n";
            const string expectedDump
                = "AttrId: 0x0001 -- ServiceClassIdList" + NewLine
                + "ElementSequence" + NewLine
                + "    Uuid16: 0x1200 -- PnPInformation" + NewLine
                + NewLine
                //
                + "AttrId: 0x0200 -- SpecificationId" + NewLine
                + "UInt16: 0x1" + NewLine
                + NewLine
                + "AttrId: 0x0201 -- VendorId" + NewLine
                + "UInt16: 0x2" + NewLine
                + NewLine
                + "AttrId: 0x0202 -- ProductId" + NewLine
                + "UInt16: 0x3" + NewLine
                + NewLine
                + "AttrId: 0x0203 -- Version" + NewLine
                + "UInt16: 0x4" + NewLine
                + NewLine
                + "AttrId: 0x0204 -- PrimaryRecord" + NewLine
                + "Boolean: True" + NewLine
                + NewLine
                + "AttrId: 0x0205 -- VendorIdSource" + NewLine
                + "UInt16: 0x6" + NewLine
                + NewLine
                //
                + "AttrId: 0xFFFF" + NewLine
                + "TextString (guessing UTF-8): '<partial BlueSoleil decode>'" + NewLine
                ;
            string dump = ServiceRecordUtilities.Dump(sr, typeof(DeviceIdProfileAttributeId));
            Assert.AreEqual(expectedDump, dump, "dump");
            //
            ServiceElement e;
            //
            e = sr.GetAttributeById(UniversalAttributeId.ServiceClassIdList).Value;
            ServiceElement eSvcClass = e.GetValueAsElementArray()[0];
            UInt16 svcC = (UInt16)eSvcClass.Value;
            Assert.AreEqual(0x1200, svcC, "svcC");
            //
            //----
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        #endregion

    }
}
