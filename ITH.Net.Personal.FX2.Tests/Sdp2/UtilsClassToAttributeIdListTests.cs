using System;
using NUnit.Framework;
#if FX1_1
using IList_ServiceElement = System.Collections.IList;
using List_ServiceElement = System.Collections.ArrayList;
using IList_ServiceAttribute = System.Collections.IList;
using List_ServiceAttribute = System.Collections.ArrayList;
#else
using IList_ServiceElement = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceElement>;
using List_ServiceElement = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceElement>;
using IList_ServiceAttribute = System.Collections.Generic.IList<InTheHand.Net.Bluetooth.ServiceAttribute>;
using List_ServiceAttribute = System.Collections.Generic.List<InTheHand.Net.Bluetooth.ServiceAttribute>;
#endif
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{
    [TestFixture]
    public class UtilsClassToAttributeIdListTests
    {
        
        //--------------------------------------------------------------
        class GiveAccess_SdpClassToAttributeIdList : MapServiceClassToAttributeIdList
        {
            internal new Type GetAttributeIdEnumType(Guid uuid)
            {
                return base.GetAttributeIdEnumType(uuid);
            }

            public new Type GetAttributeIdEnumType(ServiceElement idElement)
            {
                return base.GetAttributeIdEnumType(idElement);
            }
        }//class

        [Test]
        public void FromRecord()
        {
            ServiceRecord record = new ServiceRecordParser().Parse(Data_CompleteThirdPartyRecords.XpFsquirtOpp);
            MapServiceClassToAttributeIdList mapper = new MapServiceClassToAttributeIdList();
            Type[] enums = mapper.GetAttributeIdEnumTypes(record);
            Assert.IsNotNull(enums);
            Assert.AreEqual(1, enums.Length);
            Assert.IsNotNull(enums[0]);
            Assert.AreEqual("ObexAttributeId", enums[0].Name);
        }

        [Test]
        public void FromUuid_Opp()
        {
            Type enumClass = new GiveAccess_SdpClassToAttributeIdList().GetAttributeIdEnumType(
                BluetoothService.CreateBluetoothUuid(0x1105));
            Assert.IsNotNull(enumClass);
            Assert.AreEqual("ObexAttributeId", enumClass.Name);
        }

        [Test]
        public void FromUuid_AgHandfree()
        {
            Type enumClass = new GiveAccess_SdpClassToAttributeIdList().GetAttributeIdEnumType(
                BluetoothService.CreateBluetoothUuid(0x111F));
            Assert.IsNotNull(enumClass);
            Assert.AreEqual("HandsFreeProfileAttributeId", enumClass.Name);
        }

        [Test]
        public void FromElement()
        {
            ServiceElement element = new ServiceElement(ElementType.Uuid16, (UInt16)0x1105);
            Type enumClass = new GiveAccess_SdpClassToAttributeIdList().GetAttributeIdEnumType(element);
            Assert.IsNotNull(enumClass);
            Assert.AreEqual("ObexAttributeId", enumClass.Name);
        }

        //--------------------------------------------------------------
        const string CrLf = "\r\n";

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null." + CrLf + "Parameter name: idElement")]
        public void BadElementNull()
        {
            ServiceElement element = null;
            new GiveAccess_SdpClassToAttributeIdList().GetAttributeIdEnumType(element);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = "Value cannot be null." + CrLf + "Parameter name: record")]
        public void BadRecordNull()
        {
            ServiceRecord record = null;
            new MapServiceClassToAttributeIdList().GetAttributeIdEnumTypes(record);
        }

        [Test]
        public void BadElementNotUuidChild()
        {
            ServiceElement element = new ServiceElement(ElementType.UInt16, (UInt16)0x1105);
            Type enumClass = new GiveAccess_SdpClassToAttributeIdList().GetAttributeIdEnumType(element);
            Assert.IsNull(enumClass);
        }

        [Test]
        public void BadRecordNotSeq()
        {
            IList_ServiceAttribute listA = new List_ServiceAttribute();
            listA.Add(new ServiceAttribute(UniversalAttributeId.ServiceClassIdList,
                new ServiceElement(ElementType.UInt32, (UInt32)0)));
            ServiceRecord record = new ServiceRecord(listA);
            MapServiceClassToAttributeIdList mapper = new MapServiceClassToAttributeIdList();
            Type[] enums = mapper.GetAttributeIdEnumTypes(record);
            Assert.IsNotNull(enums);
            Assert.AreEqual(0, enums.Length);
        }

        [Test]
        public void BadRecordNoSvcClassAttr()
        {
            IList_ServiceAttribute listA = new List_ServiceAttribute();
            ServiceRecord record = new ServiceRecord(listA);
            MapServiceClassToAttributeIdList mapper = new MapServiceClassToAttributeIdList();
            Type[] enums = mapper.GetAttributeIdEnumTypes(record);
            Assert.IsNotNull(enums);
            Assert.AreEqual(0, enums.Length);
        }

    }//class

}
