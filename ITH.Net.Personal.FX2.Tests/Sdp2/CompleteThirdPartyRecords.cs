using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class CompleteThirdPartyRecords
    {

        [Test]
        public void Xp1()
        {
            TestRecordParsing.DoTestSkippingUnhandledTypes(Data_CompleteThirdPartyRecords.Xp1_Expected,
                Data_CompleteThirdPartyRecords.Xp1Sdp);
        }

        [Test]
        public void PalmOsOpp()
        {
            TestRecordParsing.DoTestSkippingUnhandledTypes(Data_CompleteThirdPartyRecords.PalmOsOpp_Expected,
                Data_CompleteThirdPartyRecords.PalmOsOpp);
        }

        [Test]
        public void WidcommMiscOpp()
        {
            ServiceRecord result =
                TestRecordParsing.DoTestSkippingUnhandledTypes(Data_CompleteThirdPartyRecords.WidcommMiscOpp_Expected,
                    Data_CompleteThirdPartyRecords.WidcommMiscOpp);
            //----
            ServiceElement elmnt = result.GetAttributeByIndex(7).Value;
            Assert.AreEqual(ElementType.TextString, elmnt.ElementType);
            String str1 = elmnt.GetValueAsStringUtf8();
            Assert.AreEqual(Data_CompleteThirdPartyRecords.WidcommMiscOppString1, str1);
        }

        [Test]
        public void Widcomm0of10Spp()
        {
            TestRecordParsing.DoTestSkippingUnhandledTypes(Data_CompleteThirdPartyRecords.Widcomm0of10Spp_Expected,
                Data_CompleteThirdPartyRecords.Widcomm0of10Spp);
        }

    }//class

}
