using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth;
using System.Diagnostics;


class MiscFeatureTestCs
{
    static void Main(string[] args)
    {
        Documentation_SRB_ToByteArray_Pbap();
        new MiscFeatureTestCsXXXX.Documentation().Discovery2F();
        Documentation_SRB_Simple();
        chaosaSrb2();
        //xxxTrySockets();
        Parse();
        //
        Trace.Fail("continue?");
        SocketBehaviour.MyMain();
        //
        jarmhartWorkee();
        jarmhartOrig();
        ServiceElement e;
        e = new ServiceElement(ElementType.TextString, "Hello world");
        e = new ServiceElement(ElementType.TextString, new byte[] { (byte)'h', (byte)'i', });
        e = new ServiceElement(ElementType.Uuid16, (UInt16)0x1101);
    }

    static void Documentation_SRB_Simple()
    {
        ServiceRecord r = Documentation_SRB_Simple_();
        ServiceRecordUtilities.Dump(Console.Out, r);
        ServiceRecordCreator ctr = new ServiceRecordCreator();
        byte[] bs = ctr.CreateServiceRecord(r);
        ServiceRecordParser psr = new ServiceRecordParser();
        ServiceRecord r2 = psr.Parse(bs);
        ServiceRecordUtilities.Dump(Console.Out, r2);
    }
    static ServiceRecord Documentation_SRB_Simple_()
    {
        ServiceRecordBuilder bldr = new ServiceRecordBuilder();
        bldr.AddServiceClass(BluetoothService.SerialPort);
        bldr.ServiceName = "Alan's SPP service";
        bldr.AddCustomAttribute(new ServiceAttribute(0x8001,
            ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0xFEDC)));
        //
        ServiceRecord record = bldr.ServiceRecord;
        return record;
    }
    static void Documentation_SRB_ToByteArray_Pbap()
    {
        ServiceRecordBuilder bldr = new ServiceRecordBuilder();
        bldr.AddServiceClass(BluetoothService.PhonebookAccessPse);
        bldr.ServiceName = "Phonebook Access PSE";
        bldr.AddBluetoothProfileDescriptor(BluetoothService.PhonebookAccess, 1, 0); // v1.0
        const ushort SupportedRepositories = 0x0314;
        bldr.AddCustomAttribute(new ServiceAttribute(SupportedRepositories,
            ServiceElement.CreateNumericalServiceElement(ElementType.UInt8, 0x03)));
        //
        ServiceRecord record = bldr.ServiceRecord;
        //
        var txt = ServiceRecordUtilities.Dump(record);
        Console.WriteLine(txt);
        //
        var byteArr = record.ToByteArray();
        var txtBA = BitConverter.ToString(byteArr);
        Console.WriteLine(txtBA);
    }

    static void chaosaSrb2()
    {
        ushort u = 0x37; // the mobile can get the number of  '10' using this
        ServiceElement se = new ServiceElement(ElementType.Int32, 10);
        ServiceAttribute sa = new ServiceAttribute(u, se);
        ServiceRecordBuilder srb = new ServiceRecordBuilder();
        srb.AddCustomAttribute(sa);
        ServiceRecord rcd = srb.ServiceRecord;
    }

    static void jarmhartOrig()
    {
        // Serial port profile

        ServiceElement[] pSerialPortElements = new ServiceElement[2];

        pSerialPortElements[0] = new ServiceElement(ElementType.Uuid16, 0x1101);  // CRASHES
        /*
            System.ArgumentException was unhandled
              Message="CLR type 'Int32' not valid type for element type 'Uuid16'."
              Source="InTheHand.Net.Personal"
              StackTrace:
                   at InTheHand.Net.Bluetooth.ServiceElement.SetValue(Object value)
                   at InTheHand.Net.Bluetooth.ServiceElement..ctor(ElementTypeDescriptor etd, ElementType type, Object value)
                   at InTheHand.Net.Bluetooth.ServiceElement..ctor(ElementType type, Object value)
                   at MiscFeatureTestCs.Program.jarmhartOrig() in D:\Documents and Settings\alan\My Documents\Visual Studio 2005\Projects\32feet Top CodePlex\InTheHand.Net.Personal\Testing\MiscFeatureTestCs\Program.cs:line 24
                   at MiscFeatureTestCs.Program.Main(String[] args) in D:\Documents and Settings\alan\My Documents\Visual Studio 2005\Projects\32feet Top CodePlex\InTheHand.Net.Personal\Testing\MiscFeatureTestCs\Program.cs:line 13
                   at System.AppDomain._nExecuteAssembly(Assembly assembly, String[] args)
                   at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
                   at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
                   at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
                   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
                   at System.Threading.ThreadHelper.ThreadStart()
            */
        pSerialPortElements[1] = new ServiceElement(ElementType.UInt16, 0x100);
        /*
            System.ArgumentException was unhandled
              Message="CLR type 'Int32' not valid type for element type 'UInt16'."
              Source="InTheHand.Net.Personal"
              StackTrace:
                   at InTheHand.Net.Bluetooth.ServiceElement.SetValue(Object value)
                   at InTheHand.Net.Bluetooth.ServiceElement..ctor(ElementTypeDescriptor etd, ElementType type, Object value)
                   at InTheHand.Net.Bluetooth.ServiceElement..ctor(ElementType type, Object value)
                   at MiscFeatureTestCs.Program.jarmhartOrig()
                   at MiscFeatureTestCs.Program.Main(String[] args)
                   at System.AppDomain._nExecuteAssembly(Assembly assembly, String[] args)
                   at System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
                   at Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
                   at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
                   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
                   at System.Threading.ThreadHelper.ThreadStart()
        */

        ServiceElement Serial = new ServiceElement(ElementType.ElementSequence, pSerialPortElements);
    }

    static void jarmhartWorkee()
    {
        // Serial port profile

        ServiceElement[] pSerialPortElements = new ServiceElement[2];
        pSerialPortElements[0] = new ServiceElement(ElementType.Uuid16, (UInt16)0x1101);  // CRASHES

        pSerialPortElements[1] = new ServiceElement(ElementType.UInt16, (UInt16)0x100);
        // -or-
        pSerialPortElements[1] = ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 0x100);

        ServiceElement Serial = new ServiceElement(ElementType.ElementSequence, pSerialPortElements);
    }

    private static void TrySockets()
    {
        System.Net.Sockets.AddressFamily AddressFamily_Bluetooth = (System.Net.Sockets.AddressFamily)32;
        // #define BTHPROTO_RFCOMM  0x0003
        // #define BTHPROTO_L2CAP   0x0100
        System.Net.Sockets.ProtocolType ProtocolType_Rfcomm = (System.Net.Sockets.ProtocolType)0x0003;
        System.Net.Sockets.ProtocolType ProtocolType_L2Cap = (System.Net.Sockets.ProtocolType)0x0100;
        System.Net.Sockets.Socket sckRfcomm = new System.Net.Sockets.Socket(
            AddressFamily_Bluetooth, System.Net.Sockets.SocketType.Stream,
            ProtocolType_Rfcomm);
        System.Net.Sockets.Socket sckL2Cap = new System.Net.Sockets.Socket(
            AddressFamily_Bluetooth, System.Net.Sockets.SocketType.Stream,
            ProtocolType_L2Cap);
    }

    static void Parse()
    {
        byte[] raw = {  0x35, 0x3b, 0x09, 0x00, 0x01, 0x35, 0x06, 0x19,   
                        0x11, 0x12, 0x19, 0x12, 0x03, 0x09, 0x00, 0x04,    
                        0x35, 0x0c, 0x35, 0x03, 0x19, 0x01, 0x00, 0x35,   
                        0x05, 0x19, 0x00, 0x03, 0x08, 0x0a, 0x09, 0x00,   
                        0x09, 0x35, 0x08, 0x35, 0x06, 0x19, 0x11, 0x08,   
                        0x09, 0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x0d,   
                        0x56, 0x6f, 0x69, 0x63, 0x65, 0x20, 0x47, 0x61,   
                        0x74, 0x65, 0x77, 0x61, 0x79 
        };
        //====
        byte jPortNo = 99;
        byte[] rgbSdpRecord = {
            0x35, 0x00, // Note: The size is filled below. 
            0x09, 
            // ServiceClassIDList 
            0x00, 0x01, 
            // Service Classes 
            0x35, 0x03, // 3 bytes 
                    // OBEXObjectPush 0x1105 
                    0x19, 0x11, 0x05, 
            0x09, 
            // ProtocolDescriptorList 
            0x00, 0x04, 
            // Protocol ID List 
            0x35, 0x11, // 17 bytes 
                    // L2CAP (0x0100) 
                    0x35, 0x03, // 3 bytes 
                            0x19, 0x01, 0x00, 
                    // RFCOMM (0x0003) 
                    0x35, 0x05, // 5 bytes 
                            0x19, 0x00, 0x03, // ID 
                            0x08, jPortNo, 
                            //0x05,           // Channel Number is (5) 
                    // OBEX (0x0008)         
                    0x35, 0x03, // 3 bytes 
                            0x19, 0x00, 0x08, 
            0x09, 
            // BluetoothProfileDescriptorList 
            0x00, 0x09, 
            0x35, 0x08, // 8 bytes 
                  0x35, 0x06, // 6 bytes 
                            0x19, 0x11, 0x05, // OBEXObjectPush 
                            0x09, 0x01, 0x00, // Version 
            0x09, 
            // Service Name 
            0x01, 0x00, 
            0x25, 0x13, // Type - String, size - 19 bytes (Object Push Profile) 
                    0x4f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x20, 0x50, 0x75, 0x73, 0x68, 0x20, 0x53, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 
            0x09, 
            // Supported Format List 
            0x03, 0x03, 
            0x35, 0x0e, // 14 bytes 
                    0x08, 0x01,     // vCard 2.1 
                    0x08, 0x02, // vCard 3.0 
                    0x08, 0x03, // vCard 1.0 
                    0x08, 0x04, // iCal 2.0 
                    0x08, 0x05, // vNote 
                    0x08, 0x06, // vMessage 
                    0x08, 0xff      // Any type of object 
        };
        //const size_t iRecSize = sizeof(rgbSdpRecord) / sizeof(BYTE);
        int iRecSize = rgbSdpRecord.Length;
        rgbSdpRecord[1] = checked((byte)(iRecSize - 2)); // -2 for excluding first 2 bytes 
        //====
        ServiceRecordParser psr = new ServiceRecordParser();
        ServiceRecord sr = psr.Parse(rgbSdpRecord);
        string txt = ServiceRecordUtilities.Dump(sr);
    }

}//class
