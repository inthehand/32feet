using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace IOBluetooth
{
    // @interface IOBluetoothSDPDataElement : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject), Name = "IOBluetoothSDPDataElement")]
    public interface SdpDataElement : INSCoding, INSSecureCoding
    {
        // +(instancetype)withElementValue:(NSObject *)element;
        [Static]
        [Export("withElementValue:")]
        SdpDataElement WithElementValue(NSObject element);

        // +(instancetype)withType:(BluetoothSDPDataElementTypeDescriptor)type sizeDescriptor:(BluetoothSDPDataElementSizeDescriptor)newSizeDescriptor size:(uint32_t)newSize value:(NSObject *)newValue;
        [Static]
        [Export("withType:sizeDescriptor:size:value:")]
        SdpDataElement WithType(SdpDataElementType type, byte sizeDescriptor, uint size, NSObject value);

        // +(instancetype)withSDPDataElementRef:(IOBluetoothSDPDataElementRef)sdpDataElementRef;
        //[Static]
        //[Export ("withSDPDataElementRef:")]
        //unsafe IOBluetoothSDPDataElement WithSDPDataElementRef (IOBluetoothSDPDataElementRef* sdpDataElementRef);

        // -(instancetype)initWithElementValue:(NSObject *)element;
        [Export("initWithElementValue:")]
        NativeHandle Constructor(NSObject element);

        // -(instancetype)initWithType:(BluetoothSDPDataElementTypeDescriptor)newType sizeDescriptor:(BluetoothSDPDataElementSizeDescriptor)newSizeDescriptor size:(uint32_t)newSize value:(NSObject *)newValue;
        [Export("initWithType:sizeDescriptor:size:value:")]
        NativeHandle Constructor(SdpDataElementType type, byte sizeDescriptor, uint size, NSObject value);

        // -(IOBluetoothSDPDataElementRef)getSDPDataElementRef;
        //[Export ("getSDPDataElementRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothSDPDataElementRef* SDPDataElementRef { get; }

        // -(BluetoothSDPDataElementTypeDescriptor)getTypeDescriptor;
        [Export("getTypeDescriptor")] SdpDataElementType TypeDescriptor { get; }

        // -(BluetoothSDPDataElementSizeDescriptor)getSizeDescriptor;
        [Export("getSizeDescriptor")] byte SizeDescriptor { get; }

        // -(uint32_t)getSize;
        [Export("getSize")] uint Size { get; }

        // -(NSNumber *)getNumberValue;
        [Export("getNumberValue")] NSNumber NumberValue { get; }

        // -(NSData *)getDataValue;
        [Export("getDataValue")] NSData DataValue { get; }

        // -(NSString *)getStringValue;
        [Export("getStringValue")] string StringValue { get; }

        // -(NSArray *)getArrayValue;
        [Export("getArrayValue")] NSObject[] ArrayValue { get; }

        // -(IOBluetoothSDPUUID *)getUUIDValue;
        [Export("getUUIDValue")] SdpUuid UuidValue { get; }

        // -(NSObject *)getValue;
        [Export("getValue")] NSObject Value { get; }

        // -(BOOL)containsDataElement:(IOBluetoothSDPDataElement *)dataElement;
        [Export("containsDataElement:")]
        bool ContainsDataElement(SdpDataElement dataElement);

        // -(BOOL)containsValue:(NSObject *)cmpValue;
        [Export("containsValue:")]
        bool ContainsValue(NSObject cmpValue);
    }

    // @interface IOBluetoothSDPServiceAttribute : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject), Name = "IOBluetoothSDPServiceAttribute")]
    public interface SdpServiceAttribute : INSCoding, INSSecureCoding
    {
        // +(instancetype)withID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElementValue:(NSObject *)attributeElementValue;
        [Static]
        [Export("withID:attributeElementValue:")]
        SdpServiceAttribute WithID(ushort attributeID, NSObject attributeElementValue);

        // +(instancetype)withID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElement:(IOBluetoothSDPDataElement *)attributeElement;
        [Static]
        [Export("withID:attributeElement:")]
        SdpServiceAttribute WithID(ushort attributeID, SdpDataElement attributeElement);

        // -(instancetype)initWithID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElementValue:(NSObject *)attributeElementValue;
        [Export("initWithID:attributeElementValue:")]
        NativeHandle Constructor(ushort attributeID, NSObject attributeElementValue);

        // -(instancetype)initWithID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElement:(IOBluetoothSDPDataElement *)attributeElement;
        [Export("initWithID:attributeElement:")]
        NativeHandle Constructor(ushort attributeID, SdpDataElement attributeElement);

        // -(BluetoothSDPServiceAttributeID)getAttributeID;
        [Export("getAttributeID")] ushort AttributeID { get; }

        // -(IOBluetoothSDPDataElement *)getDataElement;
        [Export("getDataElement")] SdpDataElement DataElement { get; }

        // -(IOBluetoothSDPDataElement *)getIDDataElement;
        [Export("getIDDataElement")] SdpDataElement IDDataElement { get; }
    }

    // @interface IOBluetoothSDPServiceRecord : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject), Name = "IOBluetoothSDPServiceRecord")]
    public interface SdpServiceRecord : INSCoding, INSSecureCoding
    {
        // +(instancetype)publishedServiceRecordWithDictionary:(NSDictionary *)serviceDict;
        [Static]
        [Export("publishedServiceRecordWithDictionary:")]
        SdpServiceRecord PublishedServiceRecordWithDictionary(NSDictionary serviceDict);

        // -(IOReturn)removeServiceRecord;
        [Export("removeServiceRecord")]
        IOReturn RemoveServiceRecord();

        // +(instancetype)withServiceDictionary:(NSDictionary *)serviceDict device:(IOBluetoothDevice *)device;
        [Static]
        [Export("withServiceDictionary:device:")]
        SdpServiceRecord WithServiceDictionary(NSDictionary serviceDict, BluetoothDevice device);

        // -(instancetype)initWithServiceDictionary:(NSDictionary *)serviceDict device:(IOBluetoothDevice *)device;
        [Export("initWithServiceDictionary:device:")]
        NativeHandle Constructor(NSDictionary serviceDict, BluetoothDevice device);

        // +(instancetype)withSDPServiceRecordRef:(IOBluetoothSDPServiceRecordRef)sdpServiceRecordRef;
        //[Static]
        //[Export ("withSDPServiceRecordRef:")]
        //unsafe IOBluetoothSDPServiceRecord WithSDPServiceRecordRef (IOBluetoothSDPServiceRecordRef* sdpServiceRecordRef);

        // -(IOBluetoothSDPServiceRecordRef)getSDPServiceRecordRef;
        //[Export ("getSDPServiceRecordRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothSDPServiceRecordRef* SDPServiceRecordRef { get; }

        // @property (readonly, retain) IOBluetoothDevice * device;
        [Export("device", ArgumentSemantic.Retain)]
        BluetoothDevice Device { get; }

        // @property (readonly, copy) NSDictionary * attributes;
        [Export("attributes", ArgumentSemantic.Copy)]
        NSDictionary Attributes { get; }

        // -(IOBluetoothSDPDataElement *)getAttributeDataElement:(BluetoothSDPServiceAttributeID)attributeID;
        [Export("getAttributeDataElement:")]
        SdpDataElement GetAttributeDataElement(ushort attributeID);

        // -(NSString *)getServiceName;
        [Export("getServiceName")] string ServiceName { get; }

        // -(IOReturn)getRFCOMMChannelID:(BluetoothRFCOMMChannelID *)rfcommChannelID;
        [Export("getRFCOMMChannelID:")]
        IOReturn GetRFCOMMChannelID(out byte rfcommChannelID);

        // -(IOReturn)getL2CAPPSM:(BluetoothL2CAPPSM *)outPSM;
        [Export("getL2CAPPSM:")]
        IOReturn GetL2CAPPSM(out ushort outPSM);

        // -(IOReturn)getServiceRecordHandle:(BluetoothSDPServiceRecordHandle *)outServiceRecordHandle;
        [Export("getServiceRecordHandle:")]
        IOReturn GetServiceRecordHandle(out uint outServiceRecordHandle);

        // -(BOOL)matchesUUID16:(BluetoothSDPUUID16)uuid16;
        [Export("matchesUUID16:")]
        bool MatchesUuid16(ushort uuid16);

        // -(BOOL)matchesUUIDArray:(NSArray *)uuidArray;
        [Export("matchesUUIDArray:")]
        bool MatchesUuidArray(SdpUuid[] uuidArray);

        // -(BOOL)matchesSearchArray:(NSArray *)searchArray;
        [Export("matchesSearchArray:")]
        bool MatchesSearchArray(SdpUuid[] searchArray);

        // -(BOOL)hasServiceFromArray:(NSArray *)array;
        [Export("hasServiceFromArray:")]
        bool HasServiceFromArray(SdpUuid[] array);

        // @property (readonly, copy) NSArray * sortedAttributes;
        [Export("sortedAttributes", ArgumentSemantic.Copy)]
        SdpServiceAttribute[] SortedAttributes { get; }

        // -(uint16_t)handsFreeSupportedFeatures __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFreeSupportedFeatures")]
        HandsFreeDeviceFeatures HandsFreeSupportedFeatures { get; }
    }

    // @interface IOBluetoothSDPUUID : NSData
    [BaseType(typeof(NSData), Name = "IOBluetoothSDPUUID")]
    public interface SdpUuid
    {
        // +(instancetype)uuidWithBytes:(const void *)bytes length:(unsigned int)length;
        [Static]
        [New]
        [Export("uuidWithBytes:length:")]
        SdpUuid FromBytes(IntPtr bytes, nuint length);

        // +(instancetype)uuidWithData:(NSData *)data;
        [Static]
        [New]
        [Export("uuidWithData:")]
        SdpUuid FromData(NSData data);

        // +(instancetype)uuid16:(BluetoothSDPUUID16)uuid16;
        [Static]
        [Export("uuid16:")]
        SdpUuid FromUuid16(ushort uuid16);

        // +(instancetype)uuid32:(BluetoothSDPUUID32)uuid32;
        [Static]
        [Export("uuid32:")]
        SdpUuid FromUuid32(uint uuid32);

        // -(instancetype)initWithUUID16:(BluetoothSDPUUID16)uuid16;
        [Export("initWithUUID16:")]
        NativeHandle Constructor(ushort uuid16);

        // -(instancetype)initWithUUID32:(BluetoothSDPUUID32)uuid32;
        [Export("initWithUUID32:")]
        NativeHandle Constructor(uint uuid32);

        // -(instancetype)getUUIDWithLength:(unsigned int)newLength;
        [Export("getUUIDWithLength:")]
        SdpUuid GetUuidWithLength(uint newLength);

        // -(BOOL)isEqualToUUID:(IOBluetoothSDPUUID *)otherUUID;
        [Export("isEqualToUUID:")]
        bool IsEqualToUuid(SdpUuid otherUuid);

        // -(Class)classForCoder;
        [Export("classForCoder")] Class ClassForCoder { get; }

        // -(Class)classForArchiver;
        [Export("classForArchiver")] Class ClassForArchiver { get; }

        // -(Class)classForPortCoder;
        [Export("classForPortCoder")] Class ClassForPortCoder { get; }
    }
}