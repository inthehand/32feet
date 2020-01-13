using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Tests.Sdp2
{

#if !FX1_1
    static
#endif
    class Data_CompleteThirdPartyRecords
    {

        public static readonly byte[] UnsupportedCharacterEncoding = { // Copied from Xp1Sdp
            0x35, 0x98, 0x09, 0x00, 0x00, 0x0A, 0x00, 0x00,  0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19,
            0x10, 0x00, 0x09, 0x00, 0x04, 0x35, 0x0D, 0x35,  0x06, 0x19, 0x01, 0x00, 0x09, 0x00, 0x01, 0x35,
            0x03, 0x19, 0x00, 0x01, 0x09, 0x00, 0x05, 0x35,  0x03, 0x19, 0x10, 0x02, 0x09, 0x00, 0x06, 0x35,
            0x09, 0x09, 0x65, 0x6E, 0x09, 0x00, 0x01, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x12, 0x53,
            0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x20, 0x44,  0x69, 0x73, 0x63, 0x6F, 0x76, 0x65, 0x72, 0x79,
            0x00, 0x09, 0x01, 0x01, 0x25, 0x25, 0x50, 0x75,  0x62, 0x6C, 0x69, 0x73, 0x68, 0x65, 0x73, 0x20,
            0x73, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73,  0x20, 0x74, 0x6F, 0x20, 0x72, 0x65, 0x6D, 0x6F,
            0x74, 0x65, 0x20, 0x64, 0x65, 0x76, 0x69, 0x63,  0x65, 0x73, 0x00, 0x09, 0x01, 0x02, 0x25, 0x0A,
            0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66,  0x74, 0x00, 0x09, 0x02, 0x00, 0x35, 0x03, 0x09,
            0x01, 0x00, 0x09, 0x02, 0x01, 0x0A, 0x00, 0x00,  0x00, 0x01 
        };
        public const string UnsupportedCharacterEncodingDump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x0" + CrLf
            + "" + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1000" + " -- ServiceDiscoveryServer" + CrLf
            + "" + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1" + " -- SdpProtocol" + CrLf
            + "( ( L2Cap, PSM=Sdp ), ( Sdp ) )" + CrLf
            + "" + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
            + "" + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x1" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: Failure: Unrecognized character encoding (1); add to LanguageBaseItem mapping table." + CrLf
            + "" + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: Failure: Unrecognized character encoding (1); add to LanguageBaseItem mapping table." + CrLf
            + "" + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: Failure: Unrecognized character encoding (1); add to LanguageBaseItem mapping table." + CrLf
            + "" + CrLf
            + "AttrId: 0x0200 -- VersionNumberList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0201 -- ServiceDatabaseState" + CrLf
            + "UInt32: 0x1" + CrLf;
        public static readonly byte[] Xp1Sdp = {
            0x35, 0x98, 0x09, 0x00, 0x00, 0x0A, 0x00, 0x00,  0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19,
            0x10, 0x00, 0x09, 0x00, 0x04, 0x35, 0x0D, 0x35,  0x06, 0x19, 0x01, 0x00, 0x09, 0x00, 0x01, 0x35,
            0x03, 0x19, 0x00, 0x01, 0x09, 0x00, 0x05, 0x35,  0x03, 0x19, 0x10, 0x02, 0x09, 0x00, 0x06, 0x35,
            0x09, 0x09, 0x65, 0x6E, 0x09, 0x00, 0x6A, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x12, 0x53,
            0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x20, 0x44,  0x69, 0x73, 0x63, 0x6F, 0x76, 0x65, 0x72, 0x79,
            0x00, 0x09, 0x01, 0x01, 0x25, 0x25, 0x50, 0x75,  0x62, 0x6C, 0x69, 0x73, 0x68, 0x65, 0x73, 0x20,
            0x73, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73,  0x20, 0x74, 0x6F, 0x20, 0x72, 0x65, 0x6D, 0x6F,
            0x74, 0x65, 0x20, 0x64, 0x65, 0x76, 0x69, 0x63,  0x65, 0x73, 0x00, 0x09, 0x01, 0x02, 0x25, 0x0A,
            0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66,  0x74, 0x00, 0x09, 0x02, 0x00, 0x35, 0x03, 0x09,
            0x01, 0x00, 0x09, 0x02, 0x01, 0x0A, 0x00, 0x00,  0x00, 0x01 
        };
        public static readonly byte[] Xp1StringBytes1 = {
            /*0x25, 0x12,*/ 0x53,
            0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x20, 0x44,  0x69, 0x73, 0x63, 0x6F, 0x76, 0x65, 0x72, 0x79,
            0x00, 
        };
        public const string Xp1String1 = "Service Discovery\u0000"; //Stupid Xp coder put null-termination!
        public static readonly byte[] Xp1StringBytes2 = {
                                /*0x25, 0x25, */0x50, 0x75,  0x62, 0x6C, 0x69, 0x73, 0x68, 0x65, 0x73, 0x20,
            0x73, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73,  0x20, 0x74, 0x6F, 0x20, 0x72, 0x65, 0x6D, 0x6F,
            0x74, 0x65, 0x20, 0x64, 0x65, 0x76, 0x69, 0x63,  0x65, 0x73, 0x00, 
        };
        public static readonly byte[] Xp1StringBytes3 = {
            //0x25, 0x0A,
            0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66,  0x74, 0x00, 
        };
        public static readonly ExpectedServiceAttribute[] Xp1_Expected = new ExpectedServiceAttribute[] {
            /// (type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            /// (type: 0x0001/0x0210) UInt32: 0x00000000
            new ExpectedServiceAttribute(0, ElementType.UInt32, ElementTypeDescriptor.UnsignedInteger, 0),
            /// (type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0003/0x0130)     UUID16: 0x1000
            new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1000)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0006/0x0000)     Element Sequence:
            /// (type: 0x0003/0x0130)         UUID16: 0x0100
            /// (type: 0x0001/0x0110)         UInt16: 0x0001
            /// (type: 0x0006/0x0000)     Element Sequence:
            /// (type: 0x0003/0x0130)         UUID16: 0x0001
            new ExpectedServiceAttribute(4, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0100),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0001)}),
                new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0001)}),
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0003/0x0130)     UUID16: 0x1002
            new ExpectedServiceAttribute(5, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1002)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0001/0x0110)     UInt16: 0x656e
            /// (type: 0x0001/0x0110)     UInt16: 0x006a
            /// (type: 0x0001/0x0110)     UInt16: 0x0100
            new ExpectedServiceAttribute(6, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x656e),
                new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x006a),
                new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            /// (type: 0x0004/0x0000) (String: Service Discovery)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            new ExpectedServiceAttribute(0x0100, ElementType.TextString, ElementTypeDescriptor.TextString, Xp1StringBytes1),
            /// (type: 0x0001/0x0110) UInt16: 0x0101  -- Unknown
            /// (type: 0x0004/0x0000) (String: Publishes services to remote devices)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            new ExpectedServiceAttribute(0x0101, ElementType.TextString, ElementTypeDescriptor.TextString, Xp1StringBytes2),
            /// (type: 0x0001/0x0110) UInt16: 0x0102  -- Unknown
            /// (type: 0x0004/0x0000) (String: Microsoft)BluetoothSdpGetString0 error: 0x80010106 & 0x277e
            new ExpectedServiceAttribute(0x0102, ElementType.TextString, ElementTypeDescriptor.TextString, Xp1StringBytes3),
            /// (type: 0x0001/0x0110) UInt16: 0x0200  -- Unknown
            /// (type: 0x0006/0x0000) Element Sequence:
            /// (type: 0x0001/0x0110)     UInt16: 0x0100
            new ExpectedServiceAttribute(0x0200, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{
                new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100)
            }),
            /// (type: 0x0001/0x0110) UInt16: 0x0201  -- Unknown
            /// (type: 0x0001/0x0210) UInt32: 0x00000001
            new ExpectedServiceAttribute(0x0201, ElementType.UInt32, ElementTypeDescriptor.UnsignedInteger, 1),
        };
        public const string CrLf = "\r\n";
        public const string Xp1DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x0" + CrLf
            + "" + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1000" + CrLf
            + "" + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + CrLf
            + "        UInt16: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1" + CrLf
            + "" + CrLf
            + "AttrId: 0x0005" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + CrLf
            + "" + CrLf
            + "AttrId: 0x0006" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + "" + CrLf
            + "AttrId: 0x0101" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + "" + CrLf
            + "AttrId: 0x0102" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + "" + CrLf
            + "AttrId: 0x0200" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0201" + CrLf
            + "UInt32: 0x1" + CrLf;
        public const string Xp1Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x0" + CrLf
            + "" + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1000" + " -- ServiceDiscoveryServer" + CrLf
            + "" + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1" + " -- SdpProtocol" + CrLf
            + "( ( L2Cap, PSM=Sdp ), ( Sdp ) )" + CrLf
            + "" + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
            + "" + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Service Discovery'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] 'Publishes services to remote devices'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: [en] 'Microsoft'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0200 -- VersionNumberList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0201 -- ServiceDatabaseState" + CrLf
            + "UInt32: 0x1" + CrLf;
        public const string Xp1DumpWithNullForEnums
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x0" + CrLf
            + "" + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1000" + " -- ServiceDiscoveryServer" + CrLf
            + "" + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1" + " -- SdpProtocol" + CrLf
            + "( ( L2Cap, PSM=Sdp ), ( Sdp ) )" + CrLf
            + "" + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
            + "" + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Service Discovery'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] 'Publishes services to remote devices'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: [en] 'Microsoft'" + CrLf
            + "" + CrLf
            + "AttrId: 0x0200" + " -- VersionNumberList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x100" + CrLf
            + "" + CrLf
            + "AttrId: 0x0201" + " -- ServiceDatabaseState" + CrLf
            + "UInt32: 0x1" + CrLf;


        //--------------------------------------------------------------
        public static readonly byte[] XpB_0of2Sdp = {
            0x35, 0x98, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x00,  0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19, 
            0x10, 0x00, 0x09, 0x00, 0x04, 0x35, 0x0d, 0x35,  0x06, 0x19, 0x01, 0x00, 0x09, 0x00, 0x01, 0x35, 
            0x03, 0x19, 0x00, 0x01, 0x09, 0x00, 0x05, 0x35,  0x03, 0x19, 0x10, 0x02, 0x09, 0x00, 0x06, 0x35, 
            0x09, 0x09, 0x65, 0x6e, 0x09, 0x00, 0x6a, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x12, 0x53, 
            0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x20, 0x44,  0x69, 0x73, 0x63, 0x6f, 0x76, 0x65, 0x72, 0x79, 
            0x00, 0x09, 0x01, 0x01, 0x25, 0x25, 0x50, 0x75,  0x62, 0x6c, 0x69, 0x73, 0x68, 0x65, 0x73, 0x20, 
            0x73, 0x65, 0x72, 0x76, 0x69, 0x63, 0x65, 0x73,  0x20, 0x74, 0x6f, 0x20, 0x72, 0x65, 0x6d, 0x6f, 
            0x74, 0x65, 0x20, 0x64, 0x65, 0x76, 0x69, 0x63,  0x65, 0x73, 0x00, 0x09, 0x01, 0x02, 0x25, 0x0a, 
            0x4d, 0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66,  0x74, 0x00, 0x09, 0x02, 0x00, 0x35, 0x03, 0x09, 
            0x01, 0x00, 0x09, 0x02, 0x01, 0x0a, 0x00, 0x00,  0x00, 0x01, 
         };
        public const string XpB_0of2Sdp_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x0" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1000" + " -- ServiceDiscoveryServer" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1" + " -- SdpProtocol" + CrLf
            + "( ( L2Cap, PSM=Sdp ), ( Sdp ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Service Discovery'" + CrLf
            + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] 'Publishes services to remote devices'" + CrLf
            + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: [en] 'Microsoft'" + CrLf
            + CrLf
            + "AttrId: 0x0200 -- VersionNumberList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0201 -- ServiceDatabaseState" + CrLf
            + "UInt32: 0x1" + CrLf;

        
        //AttrId: 0x0000 -- ServiceRecordHandle
        //UInt32: 0x0

        //AttrId: 0x0001 -- ServiceClassIdList
        //ElementSequence
        //    Uuid16: 0x1000

        //AttrId: 0x0004 -- ProtocolDescriptorList
        //  ( ( L2Cap, PSM=Sdp ), ( Sdp ) )

        //AttrId: 0x0005 -- BrowseGroupList
        //ElementSequence
        //    Uuid16: 0x1002

        //AttrId: 0x0006 -- LanguageBaseAttributeIdList
        //ElementSequence
        //    UInt16: 0x656E
        //    UInt16: 0x6A
        //    UInt16: 0x100

        //AttrId: 0x0100 -- ServiceName
        //TextString: [en] 'Service Discovery'

        //AttrId: 0x0101 -- ServiceDescription
        //TextString: [en] 'Publishes services to remote devices'

        //AttrId: 0x0102 -- ProviderName
        //TextString: [en] 'Microsoft'

        //AttrId: 0x0200
        //ElementSequence
        //    UInt16: 0x100

        //AttrId: 0x0201
        //UInt32: 0x1

        public static readonly byte[] XpB_1of2_1115 = {
            0x35, 0xca, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01,  0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19, 
            0x11, 0x15, 0x09, 0x00, 0x04, 0x35, 0x12, 0x35,  0x06, 0x19, 0x01, 0x00, 0x09, 0x00, 0x0f, 0x35, 
            0x08, 0x19, 0x00, 0x0f, 0x09, 0x01, 0x00, 0x35,  0x00, 0x09, 0x00, 0x05, 0x35, 0x03, 0x19, 0x10, 
            0x02, 0x09, 0x00, 0x06, 0x35, 0x09, 0x09, 0x65,  0x6e, 0x09, 0x03, 0xf7, 0x09, 0x01, 0x00, 0x09, 
            0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 0x19, 0x11,  0x15, 0x09, 0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 
            0x38, 0x50, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73,  0x00, 0x6f, 0x00, 0x6e, 0x00, 0x61, 0x00, 0x6c, 
            0x00, 0x20, 0x00, 0x41, 0x00, 0x64, 0x00, 0x2d,  0x00, 0x68, 0x00, 0x6f, 0x00, 0x63, 0x00, 0x20, 
            0x00, 0x55, 0x00, 0x73, 0x00, 0x65, 0x00, 0x72,  0x00, 0x20, 0x00, 0x53, 0x00, 0x65, 0x00, 0x72, 
            0x00, 0x76, 0x00, 0x69, 0x00, 0x63, 0x00, 0x65,  0x00, 0x09, 0x01, 0x01, 0x25, 0x38, 0x50, 0x00, 
            0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x6f, 0x00,  0x6e, 0x00, 0x61, 0x00, 0x6c, 0x00, 0x20, 0x00, 
            0x41, 0x00, 0x64, 0x00, 0x2d, 0x00, 0x68, 0x00,  0x6f, 0x00, 0x63, 0x00, 0x20, 0x00, 0x55, 0x00, 
            0x73, 0x00, 0x65, 0x00, 0x72, 0x00, 0x20, 0x00,  0x53, 0x00, 0x65, 0x00, 0x72, 0x00, 0x76, 0x00, 
            0x69, 0x00, 0x63, 0x00, 0x65, 0x00, 0x09, 0x03,  0x0a, 0x09, 0x00, 0x00, 
        };
        //AttrId: 0x0000 -- ServiceRecordHandle
        //UInt32: 0x10000

        //AttrId: 0x0001 -- ServiceClassIdList
        //ElementSequence
        //    Uuid16: 0x1115

        //AttrId: 0x0004 -- ProtocolDescriptorList
        //  ( ( L2Cap, PSM=Bnep ), ( 15, ... ) )

        //AttrId: 0x0005 -- BrowseGroupList
        //ElementSequence
        //    Uuid16: 0x1002

        //AttrId: 0x0006 -- LanguageBaseAttributeIdList
        //ElementSequence
        //    UInt16: 0x656E
        //    UInt16: 0x3F7
        //    UInt16: 0x100

        //AttrId: 0x0009 -- BluetoothProfileDescriptorList
        //ElementSequence
        //    ElementSequence
        //        Uuid16: 0x1115
        //        UInt16: 0x100

        //AttrId: 0x0100 -- ServiceName
        //TextString: [en] ''

        //AttrId: 0x0101 -- ServiceDescription
        //TextString: [en] ''

        //AttrId: 0x030A
        //UInt16: 0x0


        public const string XpB_1of2_1115_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10000" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1115" + " -- Panu" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "        UInt16: 0xF" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0xF" + " -- BnepProtocol" + CrLf
            + "        UInt16: 0x100" + CrLf
            + "        ElementSequence" + CrLf
            + "( ( L2Cap, PSM=Bnep ), ( Bnep, ... ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x3F7" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1115" + " -- Panu" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Personal Ad-hoc User Service'" + CrLf
            + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] 'Personal Ad-hoc User Service'" + CrLf
            + CrLf
            + "AttrId: 0x030A -- SecurityDescription" + CrLf
            + "UInt16: 0x0" + CrLf;

        public static readonly byte[] XpFsquirtOpp = {
            0x35, 0x4a, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01,  0x00, 0x0b, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19, 
            0x11, 0x05, 0x09, 0x00, 0x04, 0x35, 0x11, 0x35,  0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 0x00, 
            0x03, 0x08, 0x01, 0x35, 0x03, 0x19, 0x00, 0x08,  0x09, 0x00, 0x05, 0x35, 0x03, 0x19, 0x10, 0x02, 
            0x09, 0x01, 0x00, 0x25, 0x10, 0x4f, 0x42, 0x45,  0x58, 0x20, 0x4f, 0x62, 0x6a, 0x65, 0x63, 0x74, 
            0x20, 0x50, 0x75, 0x73, 0x68, 0x09, 0x03, 0x03,  0x35, 0x02, 0x08, 0xff, 
        };
        public const string XpFsquirtOpp_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x1000B" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1105 -- ObexObjectPush" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=1 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002 -- PublicBrowseGroup" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString (guessing UTF-8): 'OBEX Object Push'" + CrLf
            + CrLf
            + "AttrId: 0x0303 -- SupportedFormatsList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt8: 0xFF" + CrLf;


        //--------------------------------------------------------------
        public static readonly byte[] WidcommMiscOpp = {
            0x36, 0x00, 0x78, 0x09, 0x00, 0x00, 0x0A, 0x00,  0x01, 0x00, 0x04, 0x09, 0x00, 0x01, 0x35, 0x03,
            0x19, 0x11, 0x05, 0x09, 0x00, 0x04, 0x35, 0x11,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19,
            0x00, 0x03, 0x08, 0x03, 0x35, 0x03, 0x19, 0x00,  0x08, 0x09, 0x00, 0x05, 0x35, 0x03, 0x19, 0x10,
            0x02, 0x09, 0x00, 0x06, 0x35, 0x09, 0x09, 0x65,  0x6E, 0x09, 0x00, 0x6A, 0x09, 0x01, 0x00, 0x09,
            0x00, 0x08, 0x08, 0xFF, 0x09, 0x00, 0x09, 0x35,  0x08, 0x35, 0x06, 0x19, 0x11, 0x05, 0x09, 0x01,
            0x00, 0x09, 0x01, 0x00, 0x25, 0x12, 0x50, 0x49,  0x4D, 0x20, 0x49, 0x74, 0x65, 0x6D, 0x20, 0x54,
            0x72, 0x61, 0x6E, 0x73, 0x66, 0x65, 0x72, 0x00,  0x09, 0x03, 0x03, 0x35, 0x0E, 0x08, 0x01, 0x08,
            0x02, 0x08, 0x03, 0x08, 0x04, 0x08, 0x05, 0x08,  0x06, 0x08, 0xFF };
        public static readonly byte[] WidcommMiscOppStringBytes1 = {
            /*0x25, 0x12,*/ 0x50, 0x49,  0x4D, 0x20, 0x49, 0x74, 0x65, 0x6D, 0x20, 0x54,
            0x72, 0x61, 0x6E, 0x73, 0x66, 0x65, 0x72, 0x00,
        };
        public static readonly string WidcommMiscOppString1 = "PIM Item Transfer";
        public static readonly ExpectedServiceAttribute[] WidcommMiscOpp_Expected = new ExpectedServiceAttribute[] {
            //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            //(type: 0x0001/0x0210) UInt32: 0x00010004
            new ExpectedServiceAttribute(0, ElementType.UInt32, ElementTypeDescriptor.UnsignedInteger, 0x00010004),
            //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1105
            new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1105)
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0100
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0003
            //(type: 0x0001/0x0010)         UInt8: 0x03
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0008
            new ExpectedServiceAttribute(4, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{ 
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0100),
                }),
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0003),
                    new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x03),
                }),
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0008),
                }),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1002
            new ExpectedServiceAttribute(5, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1002),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0001/0x0110)     UInt16: 0x656e
            //(type: 0x0001/0x0110)     UInt16: 0x006a
            //(type: 0x0001/0x0110)     UInt16: 0x0100
            new ExpectedServiceAttribute(6, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x656e),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x006a),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
            //(type: 0x0001/0x0010) UInt8: 0xff
            new ExpectedServiceAttribute(8, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0xFF),
            //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x1105
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            new ExpectedServiceAttribute(9, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1105),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100),
                })
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            //(type: 0x0004/0x0000) String: PIM Item Transfer
            new ExpectedServiceAttribute(0x0100, ElementType.TextString, ElementTypeDescriptor.TextString, WidcommMiscOppStringBytes1),
            //(type: 0x0001/0x0110) UInt16: 0x0303  -- SupportedFormatsList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0001/0x0010)     UInt8: 0x01
            //(type: 0x0001/0x0010)     UInt8: 0x02
            //(type: 0x0001/0x0010)     UInt8: 0x03
            //(type: 0x0001/0x0010)     UInt8: 0x04
            //(type: 0x0001/0x0010)     UInt8: 0x05
            //(type: 0x0001/0x0010)     UInt8: 0x06
            //(type: 0x0001/0x0010)     UInt8: 0xff
            new ExpectedServiceAttribute(0x0303, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x01),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x02),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x03),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x04),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x05),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x06),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0xFF),
            })
        };
        //* GetSvcRcds PublicBrowseGroup
        //GetSvcRcds #=10
        public static readonly byte[] Widcomm0of10Spp = {
            0x36, 0x00, 0x64, 0x09, 0x00, 0x00, 0x0A, 0x00,  0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x03,
            0x19, 0x11, 0x01, 0x09, 0x00, 0x04, 0x35, 0x0C,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19,
            0x00, 0x03, 0x08, 0x01, 0x09, 0x00, 0x05, 0x35,  0x03, 0x19, 0x10, 0x02, 0x09, 0x00, 0x06, 0x35,
            0x09, 0x09, 0x65, 0x6E, 0x09, 0x00, 0x6A, 0x09,  0x01, 0x00, 0x09, 0x00, 0x08, 0x08, 0xFF, 0x09,
            0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 0x19, 0x11,  0x01, 0x09, 0x01, 0x00, 0x09, 0x01, 0x00, 0x25,
            0x16, 0x42, 0x6C, 0x75, 0x65, 0x74, 0x6F, 0x6F,  0x74, 0x68, 0x20, 0x53, 0x65, 0x72, 0x69, 0x61,
            0x6C, 0x20, 0x50, 0x6F, 0x72, 0x74, 0x00};
        public static readonly byte[] Widcomm0of10SppString1Bytes = {
            //0x25,
            /*0x16,*/ 0x42, 0x6C,  0x75, 0x65, 0x74, 0x6F, 0x6F,  0x74, 0x68, 0x20,0x53, 0x65, 0x72, 0x69, 0x61,
            0x6C, 0x20,  0x50, 0x6F, 0x72, 0x74, 0x00
        };
        public static readonly ExpectedServiceAttribute[] Widcomm0of10Spp_Expected = new ExpectedServiceAttribute[] {
            //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            //(type: 0x0001/0x0210) UInt32: 0x00010000
            new ExpectedServiceAttribute(0, ElementType.UInt32, ElementTypeDescriptor.UnsignedInteger, 0x00010000),
            //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1101
            new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1101)
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0100
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0003
            //(type: 0x0001/0x0010)         UInt8: 0x01
            new ExpectedServiceAttribute(4, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{ 
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0100),
                }),
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0003),
                    new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x01),
                }),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1002
            new ExpectedServiceAttribute(5, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1002),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0001/0x0110)     UInt16: 0x656e
            //(type: 0x0001/0x0110)     UInt16: 0x006a
            //(type: 0x0001/0x0110)     UInt16: 0x0100
            new ExpectedServiceAttribute(6, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x656e),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x006a),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100),
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
            //(type: 0x0001/0x0010) UInt8: 0xff
            new ExpectedServiceAttribute(8, ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0xFF),
            //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x1101
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            new ExpectedServiceAttribute(9, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1101),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100),
                })
            }),
            //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            //(type: 0x0004/0x0000) String: Bluetooth Serial Port
            new ExpectedServiceAttribute(0x0100, ElementType.TextString, ElementTypeDescriptor.TextString, Widcomm0of10SppString1Bytes),
        };
        public static readonly byte[] Widcomm1of10Gn = {
        // 0: 36-00-6A-09-00-00-0A-00-01-00-01-09-00-01-35-03-19-11-17-09-00-04-35-18-35-06-19-01-00-09-00-0F-35-0E-19-00-0F-09-01-00-35-06-09-08-00-09-08-06-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-09-35-08-35-06-19-11-17-09-01-00-09-01-00-25-0F-4E-65-74-77-6F-72-6B-20-41-63-63-65-73-73-00-09-03-0A-09-00-01
        };
            //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            //(type: 0x0001/0x0210) UInt32: 0x00010001
            //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1117
            //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0100
            //(type: 0x0001/0x0110)         UInt16: 0x000f
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x000f
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            //(type: 0x0006/0x0000)         Element Sequence:
            //(type: 0x0001/0x0110)             UInt16: 0x0800
            //(type: 0x0001/0x0110)             UInt16: 0x0806
            //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1002
            //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0001/0x0110)     UInt16: 0x656e
            //(type: 0x0001/0x0110)     UInt16: 0x006a
            //(type: 0x0001/0x0110)     UInt16: 0x0100
            //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x1117
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            //(type: 0x0004/0x0000) String: Network Access
            //(type: 0x0001/0x0110) UInt16: 0x030a  -- Unknown
            //(type: 0x0001/0x0110) UInt16: 0x0001
        public static readonly byte[] Widcomm2of10Panu = {
            // 0: 36-00-6A-09-00-00-0A-00-01-00-02-09-00-01-35-03-19-11-15-09-00-04-35-18-35-06-19-01-00-09-00-0F-35-0E-19-00-0F-09-01-00-35-06-09-08-00-09-08-06-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-09-35-08-35-06-19-11-15-09-01-00-09-01-00-25-0F-4E-65-74-77-6F-72-6B-20-41-63-63-65-73-73-00-09-03-0A-09-00-01
        };
            //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
            //(type: 0x0001/0x0210) UInt32: 0x00010002
            //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1115
            //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x0100
            //(type: 0x0001/0x0110)         UInt16: 0x000f
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x000f
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            //(type: 0x0006/0x0000)         Element Sequence:
            //(type: 0x0001/0x0110)             UInt16: 0x0800
            //(type: 0x0001/0x0110)             UInt16: 0x0806
            //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0003/0x0130)     UUID16: 0x1002
            //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0001/0x0110)     UInt16: 0x656e
            //(type: 0x0001/0x0110)     UInt16: 0x006a
            //(type: 0x0001/0x0110)     UInt16: 0x0100
            //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
            //(type: 0x0006/0x0000) Element Sequence:
            //(type: 0x0006/0x0000)     Element Sequence:
            //(type: 0x0003/0x0130)         UUID16: 0x1115
            //(type: 0x0001/0x0110)         UInt16: 0x0100
            //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
            //(type: 0x0004/0x0000) String: Network Access
            //(type: 0x0001/0x0110) UInt16: 0x030a  -- Unknown
            //(type: 0x0001/0x0110) UInt16: 0x0001
        public static readonly byte[] Widcomm3of10DN = {
            // 0: 36-00-61-09-00-00-0A-00-01-00-03-09-00-01-35-03-19-11-03-09-00-04-35-0C-35-03-19-01-00-35-05-19-00-03-08-02-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-03-09-01-00-09-01-00-25-13-44-69-61-6C-2D-75-70-20-4E-65-74-77-6F-72-6B-69-6E-67-00
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010003
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1103
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x02
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1103
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: Dial-up Networking
        public static readonly byte[] Widcomm4of10Opp = {
// 0: 36-00-78-09-00-00-0A-00-01-00-04-09-00-01-35-03-19-11-05-09-00-04-35-11-35-03-19-01-00-35-05-19-00-03-08-03-35-03-19-00-08-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-05-09-01-00-09-01-00-25-12-50-49-4D-20-49-74-65-6D-20-54-72-61-6E-73-66-65-72-00-09-03-03-35-0E-08-01-08-02-08-03-08-04-08-05-08-06-08-FF
                };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010004
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1105
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x03
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0008
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1105
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: PIM Item Transfer
        //(type: 0x0001/0x0110) UInt16: 0x0303  -- SupportedFormatsList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0010)     UInt8: 0x01
        //(type: 0x0001/0x0010)     UInt8: 0x02
        //(type: 0x0001/0x0010)     UInt8: 0x03
        //(type: 0x0001/0x0010)     UInt8: 0x04
        //(type: 0x0001/0x0010)     UInt8: 0x05
        //(type: 0x0001/0x0010)     UInt8: 0x06
        //(type: 0x0001/0x0010)     UInt8: 0xff
        public static readonly byte[] Widcomm5of10Ftp = {
        // 0: 36-00-61-09-00-00-0A-00-01-00-05-09-00-01-35-03-19-11-06-09-00-04-35-11-35-03-19-01-00-35-05-19-00-03-08-04-35-03-19-00-08-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-06-09-01-00-09-01-00-25-0E-46-69-6C-65-20-54-72-61-6E-73-66-65-72-00
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010005
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1106
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x04
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0008
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1106
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: File Transfer
        public static readonly byte[] Widcomm6of10Fax = {
        // 0: 36-00-52-09-00-00-0A-00-01-00-06-09-00-01-35-03-19-11-11-09-00-04-35-0C-35-03-19-01-00-35-05-19-00-03-08-05-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-11-09-01-00-09-01-00-25-04-46-61-78-00
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010006
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1111
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x05
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1111
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: Fax
        public static readonly byte[] Widcomm7of10Synch = {
        // 0: 36-00-74-09-00-00-0A-00-01-00-07-09-00-01-35-03-19-11-04-09-00-04-35-11-35-03-19-01-00-35-05-19-00-03-08-06-35-03-19-00-08-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-04-09-01-00-09-01-00-25-14-50-49-4D-20-53-79-6E-63-68-72-6F-6E-69-7A-61-74-69-6F-6E-00-09-03-01-35-08-08-01-08-03-08-05-08-06
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010007
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1104
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x06
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0008
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1104
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: PIM Synchronization
        //(type: 0x0001/0x0110) UInt16: 0x0301  -- Unknown
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0010)     UInt8: 0x01
        //(type: 0x0001/0x0010)     UInt8: 0x03
        //(type: 0x0001/0x0010)     UInt8: 0x05
        //(type: 0x0001/0x0010)     UInt8: 0x06
        public static readonly byte[] Widcomm8of10SynchCmd = {
        // 0: 36-00-63-09-00-00-0A-00-01-00-08-09-00-01-35-03-19-11-07-09-00-04-35-11-35-03-19-01-00-35-05-19-00-03-08-06-35-03-19-00-08-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-09-35-08-35-06-19-11-04-09-01-00-09-01-00-25-15-53-79-6E-63-20-43-6F-6D-6D-61-6E-64-20-53-65-72-76-69-63-65-00
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010008
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1107
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x06
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0008
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1104
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: Sync Command Service
        public static readonly byte[] Widcomm9of10HagAndGa = {
        //0: 36-00-5F-09-00-00-0A-00-01-00-09-09-00-01-35-06-19-11-12-19-12-03-09-00-04-35-0C-35-03-19-01-00-35-05-19-00-03-08-07-09-00-05-35-03-19-10-02-09-00-06-35-09-09-65-6E-09-00-6A-09-01-00-09-00-08-08-FF-09-00-09-35-08-35-06-19-11-12-09-01-00-09-01-00-25-0E-41-75-64-69-6F-20-47-61-74-65-77-61-79-00
        };
        //(type: 0x0001/0x0110) UInt16: 0x0000  -- ServiceRecordHandle
        //(type: 0x0001/0x0210) UInt32: 0x00010009
        //(type: 0x0001/0x0110) UInt16: 0x0001  -- ServiceClassIdList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1112
        //(type: 0x0003/0x0130)     UUID16: 0x1203
        //(type: 0x0001/0x0110) UInt16: 0x0004  -- ProtocolDescriptorList
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0100
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x0003
        //(type: 0x0001/0x0010)         UInt8: 0x07
        //(type: 0x0001/0x0110) UInt16: 0x0005  -- SDP_ATTRIB_BROWSE_GROUP_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0003/0x0130)     UUID16: 0x1002
        //(type: 0x0001/0x0110) UInt16: 0x0006  -- LanguageBaseAttributeId
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0001/0x0110)     UInt16: 0x656e
        //(type: 0x0001/0x0110)     UInt16: 0x006a
        //(type: 0x0001/0x0110)     UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0008  -- SDP_ATTRIB_AVAILABILITY
        //(type: 0x0001/0x0010) UInt8: 0xff
        //(type: 0x0001/0x0110) UInt16: 0x0009  -- SDP_ATTRIB_PROFILE_DESCRIPTOR_LIST
        //(type: 0x0006/0x0000) Element Sequence:
        //(type: 0x0006/0x0000)     Element Sequence:
        //(type: 0x0003/0x0130)         UUID16: 0x1112
        //(type: 0x0001/0x0110)         UInt16: 0x0100
        //(type: 0x0001/0x0110) UInt16: 0x0100  -- ServiceName
        //(type: 0x0004/0x0000) String: Audio Gateway


        public static readonly byte[] PalmOsOpp ={
            0x36, 0x00, 0x56, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x01, 0x09, 0x00, 0x01, 0x35, 0x03,
            0x19, 0x11, 0x05, 0x09, 0x00, 0x04, 0x35, 0x11,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19,
            0x00, 0x03, 0x08, 0x01, 0x35, 0x03, 0x19, 0x00,  0x08, 0x09, 0x00, 0x06, 0x35, 0x09, 0x09, 0x65,
            0x6e, 0x09, 0x08, 0xcc, 0x09, 0x01, 0x00, 0x09,  0x01, 0x00, 0x25, 0x10, 0x4f, 0x42, 0x45, 0x58,
            0x20, 0x4f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x20,  0x50, 0x75, 0x73, 0x68, 0x09, 0x03, 0x03, 0x35,
            0x08, 0x08, 0x01, 0x08, 0x02, 0x08, 0x03, 0x08,  0xff, 
        };
        public static readonly byte[] PalmOsOpp_HackMadeFirstLengthOneByteField ={
            0x35, /*0x36, 0x00,*/ 0x56, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x01, 0x09, 0x00, 0x01, 0x35, 0x03,
            0x19, 0x11, 0x05, 0x09, 0x00, 0x04, 0x35, 0x11,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19,
            0x00, 0x03, 0x08, 0x01, 0x35, 0x03, 0x19, 0x00,  0x08, 0x09, 0x00, 0x06, 0x35, 0x09, 0x09, 0x65,
            0x6e, 0x09, 0x08, 0xcc, 0x09, 0x01, 0x00, 0x09,  0x01, 0x00, 0x25, 0x10, 0x4f, 0x42, 0x45, 0x58,
            0x20, 0x4f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x20,  0x50, 0x75, 0x73, 0x68, 0x09, 0x03, 0x03, 0x35,
            0x08, 0x08, 0x01, 0x08, 0x02, 0x08, 0x03, 0x08,  0xff, 
        };
        public static readonly byte[] PalmOsOppStringBytes1 = {
                                                                     /*0x25, 0x10,*/ 0x4f, 0x42, 0x45, 0x58,
            0x20, 0x4f, 0x62, 0x6a, 0x65, 0x63, 0x74, 0x20,  0x50, 0x75, 0x73, 0x68,
        };
        public const string PalmOsOppDump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10001" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1105" + " -- ObexObjectPush" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3" + " -- RFCommProtocol" + CrLf
            + "        UInt8: 0x1" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8" + " -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=1 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x8CC" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'OBEX Object Push'" + CrLf
            + CrLf
            + "AttrId: 0x0303 -- SupportedFormatsList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt8: 0x1" + CrLf
            + "    UInt8: 0x2" + CrLf
            + "    UInt8: 0x3" + CrLf
            + "    UInt8: 0xFF" + CrLf;
        public const byte PalmOsOpp_RfcommChannelNumber = 0x01;
        public static readonly ExpectedServiceAttribute[] PalmOsOpp_Expected = new ExpectedServiceAttribute[] {
            //AttrId: 0x0000 -- ServiceRecordHandle
            //UInt32: 0x10001
            new ExpectedServiceAttribute(0, ElementType.UInt32, ElementTypeDescriptor.UnsignedInteger, 0x00010001),
            //AttrId: 0x0001 -- ServiceClassIdList
            //ElementSequence
            //    Uuid16: 0x1105
            new ExpectedServiceAttribute(1, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x1105)
            }),
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //  ( ( L2Cap ), ( Rfcomm, CN= 1 ), ( Obex ) )
            new ExpectedServiceAttribute(4, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[]{ 
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0100),
                }),
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0003),
                    new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x01),
                }),
                new ExpectedServiceElement(ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.Uuid16, ElementTypeDescriptor.Uuid, 0x0008),
                }),
            }),
            //AttrId: 0x0006 -- LanguageBaseAttributeIdList
            //ElementSequence
            //    UInt16: 0x656E
            //    UInt16: 0x8CC
            //    UInt16: 0x100
            new ExpectedServiceAttribute(6, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x656e),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x08cc),
                    new ExpectedServiceElement(ElementType.UInt16, ElementTypeDescriptor.UnsignedInteger, 0x0100),
            }),
            //AttrId: 0x0100 -- ServiceName
            //TextString: [en] 'OBEX Object Push'
            new ExpectedServiceAttribute(0x0100, ElementType.TextString, ElementTypeDescriptor.TextString, PalmOsOppStringBytes1),
            //AttrId: 0x0303
            //ElementSequence
            //    UInt8: 0x1
            //    UInt8: 0x2
            //    UInt8: 0x3
            //    UInt8: 0xFF
            new ExpectedServiceAttribute(0x0303, ElementType.ElementSequence, ElementTypeDescriptor.ElementSequence, new ExpectedServiceElement[] {
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x01),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x02),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0x03),
                new ExpectedServiceElement(ElementType.UInt8, ElementTypeDescriptor.UnsignedInteger, 0xFF),
            })
        };

        //--------------------------------------------------------------
        public const string LogitechF0228A_Headset_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10000" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1108 -- Headset" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x1" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=1 ) )" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Logitech HS02-V07'" + CrLf
            + CrLf
            + "AttrId: 0x0302 -- RemoteAudioVolumeControl" + CrLf
            + "Boolean: True" + CrLf;
        public const string LogitechF0228A_Handsfree_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10001" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x111E -- Handsfree" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x2" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=2 ) )" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x111E -- Handsfree" + CrLf
            + "        UInt16: 0x101" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Logitech HS02-V07'" + CrLf
            + CrLf
            + "AttrId: 0x0311 -- SupportedFeatures" + CrLf
            + "UInt16: 0x1A" + CrLf;
        public static readonly byte[] LogitechF0228A_Headset ={
            0x36, 0x00, 0x5a, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x08, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 0x35, 0x0c, 0x35, 0x03, 0x19, 0x01, 0x00, 
            0x35, 0x05, 0x19, 0x00, 0x03, 0x08, 0x01, 0x09,  0x00, 0x06, 0x35, 0x09, 0x09, 0x65, 0x6e, 0x09, 
            0x00, 0x6a, 0x09, 0x01, 0x00, 0x09, 0x00, 0x09,  0x35, 0x08, 0x35, 0x06, 0x19, 0x11, 0x08, 0x09, 
            0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x11, 0x4c,  0x6f, 0x67, 0x69, 0x74, 0x65, 0x63, 0x68, 0x20, 
            0x48, 0x53, 0x30, 0x32, 0x2d, 0x56, 0x30, 0x37,  0x09, 0x03, 0x02, 0x28, 0x01, 
        };
        public static readonly byte[] LogitechF0228A_Handsfree ={
            0x36, 0x00, 0x5b, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x01, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x1e, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 0x35, 0x0c, 0x35, 0x03, 0x19, 0x01, 0x00, 
            0x35, 0x05, 0x19, 0x00, 0x03, 0x08, 0x02, 0x09,  0x00, 0x06, 0x35, 0x09, 0x09, 0x65, 0x6e, 0x09, 
            0x00, 0x6a, 0x09, 0x01, 0x00, 0x09, 0x00, 0x09,  0x35, 0x08, 0x35, 0x06, 0x19, 0x11, 0x1e, 0x09, 
            0x01, 0x01, 0x09, 0x01, 0x00, 0x25, 0x11, 0x4c,  0x6f, 0x67, 0x69, 0x74, 0x65, 0x63, 0x68, 0x20, 
            0x48, 0x53, 0x30, 0x32, 0x2d, 0x56, 0x30, 0x37,  0x09, 0x03, 0x11, 0x09, 0x00, 0x1a, 
        };

        //--------
        //Private ServiceName As New Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}")
        public const string BluetoothListener_DefaultRecord_ChatSample_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x1002D" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid128: e075d486-e23d-4887-8af5-daa1f6a5b172" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x1" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=1 ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002 -- PublicBrowseGroup" + CrLf
            ;
        public static readonly byte[] BluetoothListener_DefaultRecord_ChatSample = {
            0x35, 0x37, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01,  0x00, 0x2d, 0x09, 0x00, 0x01, 0x35, 0x11, 0x1c, 
            0xe0, 0x75, 0xd4, 0x86, 0xe2, 0x3d, 0x48, 0x87,  0x8a, 0xf5, 0xda, 0xa1, 0xf6, 0xa5, 0xb1, 0x72, 
            0x09, 0x00, 0x04, 0x35, 0x0c, 0x35, 0x03, 0x19,  0x01, 0x00, 0x35, 0x05, 0x19, 0x00, 0x03, 0x08, 
            0x01, 0x09, 0x00, 0x05, 0x35, 0x03, 0x19, 0x10,  0x02, 
        };

        //--------------------------------------------------------------
        public const string SonyEricssonMv100_Imaging_hasUint64_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10002" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x111B -- ImagingResponder" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x3" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=3 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x111A -- Imaging" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString (guessing UTF-8): 'BIP'" + CrLf
            + CrLf
            + "AttrId: 0x0310" + CrLf   // SupportedCapabilities (sp!)
            + "UInt8: 0x1" + CrLf       // Bit 0 = Generic imaging
            + CrLf
            + "AttrId: 0x0311" + CrLf   // SupportedFeatures
            + "UInt16: 0x1" + CrLf      // Bit 0 = ImagePush
            + CrLf
            + "AttrId: 0x0312" + CrLf   // SupportedFunctions
            + "UInt32: 0x3" + CrLf      // Bit 0 = GetCapabilities, Bit 1 = PutImage
            + CrLf
            + "AttrId: 0x0313" + CrLf   // TotalImagingDataCapacity
            + "UInt64: 0x8096980000000000" + CrLf // 0x8096980000000000 = 9,265,760,409,128,796,160
            ;
        public static readonly byte[] SonyEricssonMv100_Imaging_hasUint64 = {
            0x35, 0x5a, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01,  0x00, 0x02, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19, 
            0x11, 0x1b, 0x09, 0x00, 0x04, 0x35, 0x11, 0x35,  0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 0x00, 
            0x03, 0x08, 0x03, 0x35, 0x03, 0x19, 0x00, 0x08,  0x09, 0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 0x19, 
            0x11, 0x1a, 0x09, 0x01, 0x00, 0x09, 0x01, 0x00,  0x25, 0x03, 0x42, 0x49, 0x50, 0x09, 0x03, 0x10, 
            0x08, 0x01, 0x09, 0x03, 0x11, 0x09, 0x00, 0x01,  0x09, 0x03, 0x12, 0x0a, 0x00, 0x00, 0x00, 0x03, 
            0x09, 0x03, 0x13, 0x0b, 0x80, 0x96, 0x98, 0x00,  0x00, 0x00, 0x00, 0x00, 
        };

        //--------------------------------------------------------------
        private static ServiceAttribute CreateServiceAttribute(int id, ElementTypeDescriptor etd, ElementType elementType, params ServiceElement[] otherElements)
        {
            return InTheHand.Net.Tests.Sdp2.Data_SdpCreator_CompleteRecords.CreateServiceAttribute(id, etd, elementType, otherElements);
        }
        private static ServiceAttribute CreateServiceAttribute(int id, ElementTypeDescriptor etd, ElementType elementType, object value)
        {
            return InTheHand.Net.Tests.Sdp2.Data_SdpCreator_CompleteRecords.CreateServiceAttribute(id, etd, elementType, value);
        }
        private static ServiceElement CreateServiceElement(ElementTypeDescriptor etd, ElementType type, params ServiceElement[] childElements)
        {
            return InTheHand.Net.Tests.Sdp2.Data_SdpCreator_CompleteRecords.CreateServiceElement(etd, type, childElements);
        }
        private static ServiceElement CreateServiceElement(ElementTypeDescriptor etd, ElementType type, object value)
        {
            return InTheHand.Net.Tests.Sdp2.Data_SdpCreator_CompleteRecords.CreateServiceElement(etd, type, value);
        }
        private static ServiceRecord CreateServiceRecord(ServiceAttribute firstAttribute, params ServiceAttribute[] otherAttributes)
        {
            return InTheHand.Net.Tests.Sdp2.Data_SdpCreator_CompleteRecords.CreateServiceRecord(firstAttribute, otherAttributes);
        }
        //
        public static readonly ServiceRecord SonyEricsson_Hid_Record = CreateServiceRecord(
            //AttrId: 0x0000 -- ServiceRecordHandle
            //UInt32: 0x1000A
            CreateServiceAttribute(0x0000, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt32, 0x1000A),
            //AttrId: 0x0001 -- ServiceClassIdList
            //ElementSequence
            //    Uuid16: 0x1124 -- HumanInterfaceDevice
            CreateServiceAttribute(0x0001, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1124)),
            //AttrId: 0x0004 -- ProtocolDescriptorList
            //ElementSequence
            //    ElementSequence
            //        Uuid16: 0x100 -- L2CapProtocol
            //        UInt16: 0x11
            //    ElementSequence
            //        Uuid16: 0x11
            //( ( L2Cap, PSM=0x11 ), ( Hidp ) )
            CreateServiceAttribute(0x0004, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x100),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x11)),
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x11))),
            //AttrId: 0x0005 -- BrowseGroupList
            //ElementSequence
            //    Uuid16: 0x1002 -- PublicBrowseGroup
            CreateServiceAttribute(0x0005, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1002)),
            //AttrId: 0x0006 -- LanguageBaseAttributeIdList
            //ElementSequence
            //    UInt16: 0x656E
            //    UInt16: 0x6A
            //    UInt16: 0x100
            CreateServiceAttribute(0x0006, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x656E),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x6A),
                CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100)),
            //AttrId: 0x0009 -- BluetoothProfileDescriptorList
            //ElementSequence
            //    ElementSequence
            //        Uuid16: 0x1124 -- HumanInterfaceDevice
            //        UInt16: 0x100
            CreateServiceAttribute(0x0009, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x1124),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100))),
            //AttrId: 0x000D
            //ElementSequence
            //    ElementSequence
            //        ElementSequence
            //            Uuid16: 0x100 -- L2CapProtocol
            //            UInt16: 0x13
            //        ElementSequence
            //            Uuid16: 0x11
            //    ( ( L2Cap, PSM=0x13 ), ( Hidp ) )
            CreateServiceAttribute(0x000D, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                        CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x100),
                        CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x13)),
                    CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                        CreateServiceElement(ElementTypeDescriptor.Uuid, ElementType.Uuid16, 0x11)))),
            //AttrId: 0x0100 -- ServiceName
            //TextString: [en] 'Mouse & Keyboard'
            CreateServiceAttribute(0x0100, ElementTypeDescriptor.TextString, ElementType.TextString, "Mouse & Keyboard"),
            //AttrId: 0x0101 -- ServiceDescription
            //TextString: [en] 'Remote Control'
            CreateServiceAttribute(0x0101, ElementTypeDescriptor.TextString, ElementType.TextString, "Remote Control"),
            //AttrId: 0x0102 -- ProviderName
            //TextString: [en] 'Sony Ericsson'
            CreateServiceAttribute(0x0102, ElementTypeDescriptor.TextString, ElementType.TextString, "Sony Ericsson"),
            //AttrId: 0x0200		---dev rel#
            //UInt16: 0x100
            CreateServiceAttribute(0x0200, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100),
            //AttrId: 0x0201		---prof vers
            //UInt16: 0x111
            CreateServiceAttribute(0x0201, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x111),
            //AttrId: 0x0202		---dev subclass
            //UInt8: 0xC0
            CreateServiceAttribute(0x0202, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0xC0),
            //AttrId: 0x0203		---country code
            //UInt8: 0x30
            CreateServiceAttribute(0x0203, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x30),
            //AttrId: 0x0204		---virtual cable
            //Boolean: False
            CreateServiceAttribute(0x0204, ElementTypeDescriptor.Boolean, ElementType.Boolean, false),
            //AttrId: 0x0205		---reconnect initiate
            //Boolean: True
            CreateServiceAttribute(0x0205, ElementTypeDescriptor.Boolean, ElementType.Boolean, true),
            //AttrId: 0x0206		---descr list!!!
            //ElementSequence
            //    ElementSequence
            //        UInt8: 0x22
            //        TextString: System.Byte[]
            CreateServiceAttribute(0x0206, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt8, 0x22),
                    CreateServiceElement(ElementTypeDescriptor.TextString, ElementType.TextString,
                            // I don't know what the real content was here, just add some dummy bytes...
                            new byte[] { 0x61, 0x62, 0x63, 0x64 }))),
            //AttrId: 0x0207		---langBaseId
            //ElementSequence
            //    ElementSequence
            //        UInt16: 0x409
            //        UInt16: 0x100
            CreateServiceAttribute(0x0207, ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                CreateServiceElement(ElementTypeDescriptor.ElementSequence, ElementType.ElementSequence,
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x409),
                    CreateServiceElement(ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100))),
            //AttrId: 0x0208		---sdp disable
            //Boolean: False
            CreateServiceAttribute(0x0208, ElementTypeDescriptor.Boolean, ElementType.Boolean, false),
            //AttrId: 0x0209		---battery power
            //Boolean: True
            CreateServiceAttribute(0x0209, ElementTypeDescriptor.Boolean, ElementType.Boolean, true),
            //AttrId: 0x020A		---remote wake
            //Boolean: False
            CreateServiceAttribute(0x020A, ElementTypeDescriptor.Boolean, ElementType.Boolean, false),
            //AttrId: 0x020B		---parser vers
            //UInt16: 0x100
            CreateServiceAttribute(0x020B, ElementTypeDescriptor.UnsignedInteger, ElementType.UInt16, 0x100),
            //AttrId: 0x020D		---normally conn'ble
            //Boolean: False
            CreateServiceAttribute(0x020D, ElementTypeDescriptor.Boolean, ElementType.Boolean, false),
            //AttrId: 0x020E		---boot dev
            //Boolean: True
            CreateServiceAttribute(0x020E, ElementTypeDescriptor.Boolean, ElementType.Boolean, true)
        );

        public const String SonyEricsson_Hid_Record_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x1000A" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1124 -- HumanInterfaceDevice" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        UInt16: 0x11" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x11 -- HidpProtocol" + CrLf
            + "( ( L2Cap, PSM=HidControl ), ( Hidp ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002 -- PublicBrowseGroup" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1124 -- HumanInterfaceDevice" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x000D -- AdditionalProtocolDescriptorLists" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "            UInt16: 0x13" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x11 -- HidpProtocol" + CrLf
            + "    ( ( L2Cap, PSM=HidInterrupt ), ( Hidp ) )" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Mouse & Keyboard'" + CrLf
            + CrLf
            + "AttrId: 0x0101 -- ServiceDescription" + CrLf
            + "TextString: [en] 'Remote Control'" + CrLf
            + CrLf
            + "AttrId: 0x0102 -- ProviderName" + CrLf
            + "TextString: [en] 'Sony Ericsson'" + CrLf
            + CrLf
            + "AttrId: 0x0200 -- DeviceReleaseNumber" + CrLf
            + "UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0201 -- ParserVersion" + CrLf
            + "UInt16: 0x111" + CrLf
            + CrLf
            + "AttrId: 0x0202 -- DeviceSubclass" + CrLf
            + "UInt8: 0xC0" + CrLf
            + CrLf
            + "AttrId: 0x0203 -- CountryCode" + CrLf
            + "UInt8: 0x30" + CrLf
            + CrLf
            + "AttrId: 0x0204 -- VirtualCable" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x0205 -- ReconnectInitiate" + CrLf
            + "Boolean: True" + CrLf
            + CrLf
            + "AttrId: 0x0206 -- DescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        UInt8: 0x22" + CrLf
            + "        TextString (guessing UTF-8): 'abcd'" + CrLf
            + CrLf
            + "AttrId: 0x0207 -- LangIdBaseList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        UInt16: 0x409" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0208 -- SdpDisable" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x0209 -- BatteryPower" + CrLf
            + "Boolean: True" + CrLf
            + CrLf
            + "AttrId: 0x020A -- RemoteWake" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x020B -- ProfileVersion" + CrLf
            + "UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x020D -- NormallyConnectable" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x020E -- BootDevice" + CrLf
            + "Boolean: True" + CrLf
            ;

        //--------------------------------------------------------------
        public static readonly byte[] KingSt_d2r1 = { // Has Uuid32s in PDL
            0x36, 0x00, 0x44, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x12, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 0x35, 0x10, 0x35, 0x05, 0x1a, 0x00, 0x00, 
            0x01, 0x00, 0x35, 0x07, 0x1a, 0x00, 0x00, 0x00,  0x03, 0x08, 0x02, 0x09, 0x00, 0x09, 0x35, 0x0a, 
            0x35, 0x08, 0x1a, 0x00, 0x00, 0x11, 0x08, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x08, 0x56, 
            0x6f, 0x69, 0x63, 0x65, 0x20, 0x47, 0x57, 
        };

        public static readonly byte[] KingSt_d2r1_withPdlUuid128s = { // Has Uuid128s in PDL
            0x36, 0x00, 0x5C, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x12, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 
            0x35, 0x28, 
                0x35, 0x11, 
                    0x1c, 0x00, 0x00, 0x01, 0x00, 
                        0x00, 0x00, 0x10, 0x00, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                0x35, 0x13, 
                    0x1c, 0x00, 0x00, 0x00, 0x03, 
                        0x00, 0x00, 0x10, 0x00, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB,
                    0x08, 0x02, 
            0x09, 0x00, 0x09, 0x35, 0x0a, 
            0x35, 0x08, 0x1a, 0x00, 0x00, 0x11, 0x08, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x08, 0x56, 
            0x6f, 0x69, 0x63, 0x65, 0x20, 0x47, 0x57, 
        };

        public const string KingSt_d2r1_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10000" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x2" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=2 ) )" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString (guessing UTF-8): 'Voice GW'" + CrLf
            ;

        public const string KingSt_d2r1_withPdlUuid128s_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10000" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1112 -- HeadsetAudioGateway" + CrLf
            + "    Uuid16: 0x1203 -- GenericAudio" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 00000100-0000-1000-8000-00805f9b34fb -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid128: 00000003-0000-1000-8000-00805f9b34fb -- RFCommProtocol" + CrLf
            + "        UInt8: 0x2" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=2 ) )" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x1108 -- Headset" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString (guessing UTF-8): 'Voice GW'" + CrLf
            ;

        public static readonly byte[] KingSt_d2r1_withPdlUuid128sNonBluetoothBase = { // Has Uuid128s in PDL
            0x36, 0x00, 0x5C, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x00, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x12, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 
            0x35, 0x28, 
                0x35, 0x11, 
                    0x1c, 0x00, 0x00, 0x01, 0x00, 
                        0x01, 0x02, 0x13, 0x04, 0x85, 0x06, 0x07, 0x88, 0x59, 0x9a, 0x3b, 0xFc,
                0x35, 0x13, 
                    0x1c, 0x00, 0x00, 0x00, 0x03, 
                        0x01, 0x02, 0x13, 0x04, 0x85, 0x06, 0x07, 0x88, 0x59, 0x9a, 0x3b, 0xFc,
                    0x08, 0x02, 
            0x09, 0x00, 0x09, 0x35, 0x0a, 
            0x35, 0x08, 0x1a, 0x00, 0x00, 0x11, 0x08, 0x09,  0x01, 0x00, 0x09, 0x01, 0x00, 0x25, 0x08, 0x56, 
            0x6f, 0x69, 0x63, 0x65, 0x20, 0x47, 0x57, 
        };

        public static readonly byte[] KingSt_d2r2 = {
            0x36, 0x00, 0x4e, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x01, 0x09, 0x00, 0x01, 0x35, 0x06, 
            0x19, 0x11, 0x1f, 0x19, 0x12, 0x03, 0x09, 0x00,  0x04, 0x35, 0x0c, 0x35, 0x03, 0x19, 0x01, 0x00, 
            0x35, 0x05, 0x19, 0x00, 0x03, 0x08, 0x03, 0x09,  0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 0x19, 0x11, 
            0x1e, 0x09, 0x01, 0x00, 0x09, 0x01, 0x00, 0x25,  0x0d, 0x56, 0x6f, 0x69, 0x63, 0x65, 0x20, 0x47, 
            0x61, 0x74, 0x65, 0x77, 0x61, 0x79, 0x09, 0x03,  0x01, 0x08, 0x01, 0x09, 0x03, 0x11, 0x09, 0x00, 
            0x01, 
        };

        public static readonly byte[] KingSt_d2r3 = { // Has Uuid32s in PDL
            0x36, 0x00, 0x4b, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x02, 0x09, 0x00, 0x01, 0x35, 0x05, 
            0x1a, 0x00, 0x00, 0x11, 0x03, 0x09, 0x00, 0x04,  0x35, 0x10, 0x35, 0x05, 0x1a, 0x00, 0x00, 0x01, 
            0x00, 0x35, 0x07, 0x1a, 0x00, 0x00, 0x00, 0x03,  0x08, 0x01, 0x09, 0x01, 0x00, 0x25, 0x12, 0x44, 
            0x69, 0x61, 0x6c, 0x2d, 0x75, 0x70, 0x20, 0x6e,  0x65, 0x74, 0x77, 0x6f, 0x72, 0x6b, 0x69, 0x6e, 
            0x67, 0x09, 0x00, 0x09, 0x35, 0x08, 0x35, 0x06,  0x19, 0x11, 0x03, 0x09, 0x01, 0x00, 
        };

        public static readonly byte[] KingSt_d2r4 = {
            0x36, 0x00, 0x31, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x03, 0x09, 0x00, 0x01, 0x35, 0x03, 
            0x19, 0x11, 0x01, 0x09, 0x00, 0x04, 0x35, 0x0c,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 
            0x00, 0x03, 0x08, 0x06, 0x09, 0x01, 0x00, 0x25,  0x0b, 0x53, 0x65, 0x72, 0x69, 0x61, 0x6c, 0x20, 
            0x50, 0x6f, 0x72, 0x74, 
        };

        public static readonly byte[] KingSt_d2r5 = {
            0x36, 0x00, 0x42, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x04, 0x09, 0x00, 0x01, 0x35, 0x03, 
            0x19, 0x11, 0x05, 0x09, 0x00, 0x04, 0x35, 0x11,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 
            0x00, 0x03, 0x08, 0x04, 0x35, 0x03, 0x19, 0x00,  0x08, 0x09, 0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 
            0x19, 0x11, 0x05, 0x09, 0x01, 0x00, 0x09, 0x01,  0x00, 0x25, 0x03, 0x4f, 0x50, 0x50, 0x09, 0x03, 
            0x03, 0x35, 0x02, 0x08, 0xff, 
        };

        public static readonly byte[] KingSt_d2r6 = {
            0x36, 0x00, 0x49, 0x09, 0x00, 0x00, 0x0a, 0x00,  0x01, 0x00, 0x05, 0x09, 0x00, 0x01, 0x35, 0x03, 
            0x19, 0x11, 0x06, 0x09, 0x00, 0x04, 0x35, 0x11,  0x35, 0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 
            0x00, 0x03, 0x08, 0x07, 0x35, 0x03, 0x19, 0x00,  0x08, 0x09, 0x00, 0x09, 0x35, 0x08, 0x35, 0x06, 
            0x19, 0x11, 0x06, 0x09, 0x01, 0x00, 0x09, 0x01,  0x00, 0x25, 0x11, 0x4f, 0x42, 0x45, 0x58, 0x20, 
            0x46, 0x69, 0x6c, 0x65, 0x54, 0x72, 0x61, 0x6e,  0x73, 0x66, 0x65, 0x72, 
        };

        public const String KingSt_d2r1_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10000" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1112" + CrLf
            + "    Uuid16: 0x1203" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x3" + CrLf
            + "        UInt8: 0x2" + CrLf
            + CrLf
            + "AttrId: 0x0009" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x1108" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            ;

        public const String KingSt_d2r2_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10001" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x111F" + CrLf
            + "    Uuid16: 0x1203" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3" + CrLf
            + "        UInt8: 0x3" + CrLf
            + CrLf
            + "AttrId: 0x0009" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x111E" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + CrLf
            + "AttrId: 0x0301" + CrLf
            + "UInt8: 0x1" + CrLf
            + CrLf
            + "AttrId: 0x0311" + CrLf
            + "UInt16: 0x1" + CrLf
            ;

        public const String KingSt_d2r3_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10002" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid32: 0x1103" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid32: 0x3" + CrLf
            + "        UInt8: 0x1" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + CrLf
            + "AttrId: 0x0009" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1103" + CrLf
            + "        UInt16: 0x100"
            ;

        public const String KingSt_d2r4_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10003" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1101" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3" + CrLf
            + "        UInt8: 0x6" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            ;

        public const String KingSt_d2r5_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10004" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1105" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3" + CrLf
            + "        UInt8: 0x4" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8" + CrLf
            + CrLf
            + "AttrId: 0x0009" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1105" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]" + CrLf
            + CrLf
            + "AttrId: 0x0303" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt8: 0xFF"
            ;

        public const String KingSt_d2r6_DumpRaw
            = "AttrId: 0x0000" + CrLf
            + "UInt32: 0x10005" + CrLf
            + CrLf
            + "AttrId: 0x0001" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1106" + CrLf
            + CrLf
            + "AttrId: 0x0004" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3" + CrLf
            + "        UInt8: 0x7" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8" + CrLf
            + CrLf
            + "AttrId: 0x0009" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1106" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString: System.Byte[]"
            ;

        //-------------------------------------------
        // DjVaz @ cardwell

        public static readonly byte[] SemcHla = {
            0x35, 0x4a, 0x09, 0x00, 0x00, 0x0a, 0x40, 0x00,  0x00, 0x01, 0x09, 0x00, 0x01, 0x35, 0x05, 0x1a, 
            0x8e, 0x77, 0x13, 0x01, 0x09, 0x00, 0x04, 0x35,  0x0f, 0x35, 0x06, 0x19, 0x01, 0x00, 0x09, 0xf0, 
            0xf9, 0x35, 0x05, 0x1a, 0x8e, 0x77, 0x03, 0x00,  0x09, 0x00, 0x05, 0x35, 0x03, 0x19, 0x10, 0x02, 
            0x09, 0x00, 0x09, 0x35, 0x0a, 0x35, 0x08, 0x1a,  0x8e, 0x77, 0x13, 0x03, 0x09, 0x01, 0x00, 0x09, 
            0x01, 0x00, 0x25, 0x08, 0x53, 0x45, 0x4d, 0x43,  0x20, 0x48, 0x4c, 0x41, 
        };

        public const String SemcHla_Dump
        = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
        + "UInt32: 0x40000001" + CrLf
        + CrLf
        + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
        + "ElementSequence" + CrLf
        + "    Uuid32: 0x8E771301" + CrLf
        + CrLf
        + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
        + "ElementSequence" + CrLf
        + "    ElementSequence" + CrLf
        + "        Uuid16: 0x100" + " -- L2CapProtocol" + CrLf
        + "        UInt16: 0xF0F9" + CrLf
        + "    ElementSequence" + CrLf
        + "        Uuid32: 0x8E770300" + CrLf
        + "( ( L2Cap, PSM=0xF0F9 ), ( 8e770300-0000-1000-8000-00805f9b34fb ) )" + CrLf
        + CrLf
        + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
        + "ElementSequence" + CrLf
        + "    Uuid16: 0x1002" + " -- PublicBrowseGroup" + CrLf
        + "" + CrLf
        + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
        + "ElementSequence" + CrLf
        + "    ElementSequence" + CrLf
        + "        Uuid32: 0x8E771303" + CrLf
        + "        UInt16: 0x100" + CrLf
        + CrLf
        + "AttrId: 0x0100" + CrLf
        + "TextString (guessing UTF-8): 'SEMC HLA'" + CrLf
        ;

        //-------------------------------------------
        public static readonly byte[] BenqE72ImagingResponder = {
            0x35, 0x5e, 0x09, 0x00, 0x00, 0x0a, 0x00, 0x01,  0x00, 0x0b, 0x09, 0x00, 0x01, 0x35, 0x03, 0x19, 
            0x11, 0x1b, 0x09, 0x00, 0x04, 0x35, 0x11, 0x35,  0x03, 0x19, 0x01, 0x00, 0x35, 0x05, 0x19, 0x00, 
            0x03, 0x08, 0x10, 0x35, 0x03, 0x19, 0x00, 0x08,  0x09, 0x01, 0x00, 0x25, 0x07, 0x49, 0x4d, 0x41, 
            0x47, 0x49, 0x4e, 0x47, 0x09, 0x00, 0x09, 0x35,  0x08, 0x35, 0x06, 0x19, 0x11, 0x1a, 0x09, 0x01, 
            0x00, 0x09, 0x03, 0x10, 0x08, 0x01, 0x09, 0x03,  0x11, 0x09, 0x00, 0x01, 0x09, 0x03, 0x12, 0x0a, 
            0x00, 0x00, 0x00, 0x0f, 0x09, 0x03, 0x13, 0x0b,  0x00, 0x00, 0x00, 0x00, 0x00, 0x8c, 0xa0, 0x00, 
        };

        public const String BenqE72ImagingResponder_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x1000B" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x111B -- ImagingResponder" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x10" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=16 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0100" + CrLf
            + "TextString (guessing UTF-8): 'IMAGING'" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x111A -- Imaging" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0310" + CrLf
            + "UInt8: 0x1" + CrLf
            + CrLf
            + "AttrId: 0x0311" + CrLf
            + "UInt16: 0x1" + CrLf
            + CrLf
            + "AttrId: 0x0312" + CrLf
            + "UInt32: 0xF" + CrLf
            + CrLf
            + "AttrId: 0x0313" + CrLf
            + "UInt64: 0x8CA000" + CrLf
            ;

        //-------------------------------------------
        public static readonly byte[] BppDirectPrinting_TheMajor = {
            54, 2, 201, 9, 0, 0, 10, 0, 
            1, 0, 3, 9, 0, 1, 53, 6,
            25, 17, 35, 25, 17, 24, 9, 0, 
            2, 10, 0, 0, 3, 17, 9, 0,
            4, 53, 17, 53, 3, 25, 1, 0,
            53, 5, 25, 0, 3, 8, 4, 53,
            3, 25, 0, 8, 9, 0, 5, 53,
            3, 25, 16, 2, 9, 0, 6, 53,
            9, 9, 101, 110, 9, 0, 106, 9,
            1, 0, 9, 0, 9, 53, 8, 53,
            6, 25, 17, 34, 9, 1, 0, 9,
            0, 13, 53, 19, 53, 17, 53, 3,
            25, 1, 0, 53, 5, 25, 0, 3,
            8, 5, 53, 3, 25, 0, 8, 9,
            1, 0, 37, 14, 66, 97, 115, 105,
            99, 32, 80, 114, 105, 110, 116, 105,
            110, 103, 9, 3, 80, 37, 157, 97,
            112, 112, 108, 105, 99, 97, 116, 105,
            111, 110, 47, 118, 110, 100, 46, 112,
            119, 103, 45, 120, 104, 116, 109, 108,
            45, 112, 114, 105, 110, 116, 43, 120,
            109, 108, 58, 49, 46, 48, 44, 116,
            101, 120, 116, 47, 112, 108, 97, 105,
            110, 44, 116, 101, 120, 116, 47, 120,
            45, 118, 99, 97, 114, 100, 58, 50,
            46, 49, 44, 116, 101, 120, 116, 47,
            120, 45, 118, 99, 97, 108, 101, 110,
            100, 97, 114, 58, 49, 46, 48, 44,
            97, 112, 112, 108, 105, 99, 97, 116,
            105, 111, 110, 47, 118, 110, 100, 46,
            104, 112, 45, 80, 67, 76, 58, 51,
            67, 44, 105, 109, 97, 103, 101, 47,
            106, 112, 101, 103, 44, 97, 112, 112,
            108, 105, 99, 97, 116, 105, 111, 110,
            47, 118, 110, 100, 46, 112, 119, 103,
            45, 109, 117, 108, 116, 105, 112, 108,
            101, 120, 101, 100, 9, 3, 82, 12,
            0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 17, 243,
            9, 3, 84, 37, 10, 105, 109, 97,
            103, 101, 47, 106, 112, 101, 103, 9,
            3, 86, 40, 1, 9, 3, 88, 37,     // 0x09, 0x03, 0x58, 0x25
            80, 0, 80, 77, 70, 71, 58, 72,  // 0x50, string-content:
            80, 59, 77, 68, 76, 58, 68, 69,
            83, 75, 74, 69, 84, 32, 52, 54,
            48, 59, 67, 77, 68, 58, 80, 67,
            76, 44, 68, 87, 95, 80, 67, 76,
            44, 68, 69, 83, 75, 74, 69, 84,
            44, 68, 89, 78, 59, 67, 76, 83,
            58, 80, 82, 73, 78, 84, 69, 82,
            59, 68, 69, 83, 58, 68, 101, 115,
            107, 106, 101, 116, 32, 52, 54, 48,
            59, 9, 3, 90, 37, 26, 68, 101,
            115, 107, 106, 101, 116, 32, 52, 54,
            48, 32, 83, 47, 78, 32, 77, 89,
            55, 55, 80, 53, 90, 48, 71, 77,
            9, 3, 92, 37, 7, 85, 110, 107,
            110, 111, 119, 110, 9, 3, 94, 40,
            0, 9, 3, 96, 37, 219, 115, 116,
            97, 116, 105, 111, 110, 101, 114, 121,
            44, 115, 116, 97, 116, 105, 111, 110,
            101, 114, 121, 45, 99, 111, 97, 116,
            101, 100, 44, 115, 116, 97, 116, 105,
            111, 110, 101, 114, 121, 45, 105, 110,
            107, 106, 101, 116, 44, 116, 114, 97,
            110, 115, 112, 97, 114, 101, 110, 99,
            121, 44, 101, 110, 118, 101, 108, 111,
            112, 101, 44, 101, 110, 118, 101, 108,
            111, 112, 101, 45, 112, 108, 97, 105,
            110, 44, 101, 110, 118, 101, 108, 111,
            112, 101, 45, 119, 105, 110, 100, 111,
            119, 44, 116, 97, 98, 45, 115, 116,
            111, 99, 107, 44, 112, 114, 101, 45,
            99, 117, 116, 45, 116, 97, 98, 115,
            44, 102, 117, 108, 108, 45, 99, 117,
            116, 45, 116, 97, 98, 115, 44, 108,
            97, 98, 101, 108, 115, 44, 112, 104,
            111, 116, 111, 103, 114, 97, 112, 104,
            105, 99, 44, 112, 104, 111, 116, 111,
            103, 114, 97, 112, 104, 105, 99, 45,
            103, 108, 111, 115, 115, 121, 44, 112,
            104, 111, 116, 111, 103, 114, 97, 112,
            104, 105, 99, 45, 115, 101, 109, 105,
            45, 103, 108, 111, 115, 115, 44, 112,
            104, 111, 116, 111, 103, 114, 97, 112,
            104, 105, 99, 45, 109, 97, 116, 116,
            101, 9, 3, 98, 9, 0, 210, 9,
            3, 100, 9, 1, 24, 9, 3, 102,
            40, 0, 9, 3, 112, 40, 0, 9,
            3, 114, 40, 0,
        };
        public const string BppDirectPrinting_TheMajor_Dump
            = "AttrId: 0x0000 -- ServiceRecordHandle" + CrLf
            + "UInt32: 0x10003" + CrLf
            + CrLf
            + "AttrId: 0x0001 -- ServiceClassIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1123 -- PrintingStatus" + CrLf
            + "    Uuid16: 0x1118 -- DirectPrinting" + CrLf
            + CrLf
            + "AttrId: 0x0002 -- ServiceRecordState" + CrLf
            + "UInt32: 0x311" + CrLf
            + CrLf
            + "AttrId: 0x0004 -- ProtocolDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "        UInt8: 0x4" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "( ( L2Cap ), ( Rfcomm, ChannelNumber=4 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0005 -- BrowseGroupList" + CrLf
            + "ElementSequence" + CrLf
            + "    Uuid16: 0x1002 -- PublicBrowseGroup" + CrLf
            + CrLf
            + "AttrId: 0x0006 -- LanguageBaseAttributeIdList" + CrLf
            + "ElementSequence" + CrLf
            + "    UInt16: 0x656E" + CrLf
            + "    UInt16: 0x6A" + CrLf
            + "    UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x0009 -- BluetoothProfileDescriptorList" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        Uuid16: 0x1122 -- BasicPrinting" + CrLf
            + "        UInt16: 0x100" + CrLf
            + CrLf
            + "AttrId: 0x000D -- AdditionalProtocolDescriptorLists" + CrLf
            + "ElementSequence" + CrLf
            + "    ElementSequence" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x100 -- L2CapProtocol" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x3 -- RFCommProtocol" + CrLf
            + "            UInt8: 0x5" + CrLf
            + "        ElementSequence" + CrLf
            + "            Uuid16: 0x8 -- ObexProtocol" + CrLf
            + "    ( ( L2Cap ), ( Rfcomm, ChannelNumber=5 ), ( Obex ) )" + CrLf
            + CrLf
            + "AttrId: 0x0100 -- ServiceName" + CrLf
            + "TextString: [en] 'Basic Printing'" + CrLf
            + CrLf
            + "AttrId: 0x0350 -- DocumentFormatsSupported" + CrLf
            + "TextString (guessing UTF-8): 'application/vnd.pwg-xhtml-print+xml:1.0,text/plain,text/x-vcard:2.1,text/x-vcalendar:1.0,application/vnd.hp-PCL:3C,image/jpeg,application/vnd.pwg-multiplexed'" + CrLf
            + CrLf
            + "AttrId: 0x0352 -- CharacterRepertoiresSupported" + CrLf
            + "UInt128: 00-00-00-00-00-00-00-00-00-00-00-00-00-00-11-F3" + CrLf
            + CrLf
            + "AttrId: 0x0354 -- XhtmlPrintImageFormatsSupported" + CrLf
            + "TextString (guessing UTF-8): 'image/jpeg'" + CrLf
            + CrLf
            + "AttrId: 0x0356 -- ColorSupported" + CrLf
            + "Boolean: True" + CrLf
            + CrLf
            + "AttrId: 0x0358 -- Model1284Id" + CrLf
            + "TextString (Unknown/bad encoding):" + CrLf
            + "Length: 80, >>00-50-4D-46-47-3A-48-50-3B-4D-44-4C-3A-44-45-53-4B-4A-45-54-20-34-36-30-3B-43-4D-44-3A-50-43-4C-2C-44-57-5F-50-43-4C-2C-44-45-53-4B-4A-45-54-2C-44-59-4E-3B-43-4C-53-3A-50-52-49-4E-54-45-52-3B-44-45-53-3A-44-65-73-6B-6A-65-74-20-34-36-30-3B<<" + CrLf
            + CrLf
            + "AttrId: 0x035A -- PrinterName" + CrLf
            + "TextString (guessing UTF-8): 'Deskjet 460 S/N MY77P5Z0GM'" + CrLf
            + CrLf
            + "AttrId: 0x035C -- PrinterLocation" + CrLf
            + "TextString (guessing UTF-8): 'Unknown'" + CrLf
            + CrLf
            + "AttrId: 0x035E -- DuplexSupported" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x0360 -- MediaTypesSupported" + CrLf
            + "TextString (guessing UTF-8): 'stationery,stationery-coated,stationery-inkjet,transparency,envelope,envelope-plain,envelope-window,tab-stock,pre-cut-tabs,full-cut-tabs,labels,photographic,photographic-glossy,photographic-semi-gloss,photographic-matte'" + CrLf
            + CrLf
            + "AttrId: 0x0362 -- MaxMediaWidth" + CrLf
            + "UInt16: 0xD2" + CrLf
            + CrLf
            + "AttrId: 0x0364 -- MaxMediaLength" + CrLf
            + "UInt16: 0x118" + CrLf
            + CrLf
            + "AttrId: 0x0366 -- EnhancedLayoutSupported" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x0370 -- ReferencePrintingRuiSupported" + CrLf
            + "Boolean: False" + CrLf
            + CrLf
            + "AttrId: 0x0372 -- DirectPrintingRuiSupported" + CrLf
            + "Boolean: False" + CrLf
            ;

        //-------------------------------------------
        public static readonly byte[] SimpleRfcommPdl = {
            0x35, 0x11,
                0x09, 0x00, 0x04, 
                0x35, 0x0c, 
                    0x35, 0x03, 
                        0x19, 0x01, 0x00, 
                    0x35, 0x05, 
                        0x19, 0x00, 0x03, 
                        0x08, 0x05
        };

        public static readonly byte[] SimpleRfcommExplicitPsmSamePdl = {
            0x35, 0x14,
                0x09, 0x00, 0x04, 
                0x35, 0x0f, 
                    0x35, 0x06, 
                        0x19, 0x01, 0x00, 
                        0x09, 0x00, 0x03, 
                    0x35, 0x05, 
                        0x19, 0x00, 0x03, 
                        0x08, 0x19
        };

        public static readonly byte[] SimpleRfcommExplicitPsmDifferentPdl = {
            0x35, 0x14,
                0x09, 0x00, 0x04, 
                0x35, 0x0f, 
                    0x35, 0x06, 
                        0x19, 0x01, 0x00, 
                        0x09, 0x98, 0x03, 
                    0x35, 0x05, 
                        0x19, 0x00, 0x03, 
                        0x08, 0x10
        };

        public static readonly byte[] SimpleAlternativeTwoRfcommPdl = {
            0x35, 0x21,
                0x09, 0x00, 0x04, 
                0x3d, 0x1c,
                    //
                    0x35, 0x0c, 
                        0x35, 0x03, 
                            0x19, 0x01, 0x00, 
                        0x35, 0x05, 
                            0x19, 0x00, 0x03, 
                            0x08, 0x11,
                    //
                    0x35, 0x0c, 
                        0x35, 0x03, 
                            0x19, 0x01, 0x00, 
                        0x35, 0x05, 
                            0x19, 0x00, 0x03, 
                            0x08, 0x12
        };

    }//class

}