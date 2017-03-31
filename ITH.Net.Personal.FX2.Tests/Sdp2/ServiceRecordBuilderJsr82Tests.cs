using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{
    [TestFixture]
    public class ServiceRecordBuilderJsr82Tests
    {
        // btl2cap://localhost:3B9FA89520078C303355AAA694238F08;name=Aserv
        // btspp://localhost:3B9FA89520078C303355AAA694238F08
        // btgoep://localhost:3B9FA89520078C303355AAA694238F08

#if FX1_1
        /*
#endif
#pragma warning disable 618
#if FX1_1
        */
#endif
        [Test]
        public void L2capOne()
        {
            String url = "btl2cap://localhost:3B9FA89520078C303355AAA694238F08";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooL2CapOne, dump);
        }

        [Test]
        public void L2capTwoWithName()
        {
            String url = "btl2cap://localhost:12af51a9030c4b2937407f8c9ecb238a;name=Aserv";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooL2CapTwoWithName, dump);
        }

        [Test]
        public void RfcommOne()
        {
            String url = "btspp://localhost:102030405060708090A1B1C1D1D1E100;name=SPPEx";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooRfcommOne, dump);
        }

        [Test]
        [Ignore("Arghh Regex not looping")]
        public void RfcommOneNameFollowed()
        {
            String url = "btspp://localhost:102030405060708090A1B1C1D1D1E100;name=SPPEx;encrypt=false";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooRfcommOne, dump);
        }

        [Test]
        [Ignore("Arghh Regex not looping")]
        public void RfcommOneNameSurrounded()
        {
            String url = "btspp://localhost:102030405060708090A1B1C1D1D1E100;auth=false;name=SPPEx;encrypt=false";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooRfcommOne, dump);
        }

        [Test]
        public void GeopOne()
        {
            String url = "btgoep://localhost:12AF51A9030C4B2937407F8C9ECB238A";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooGoepOne, dump);
        }

        [Test]
        public void One()
        {
            String url = "btspp://localhost:0000110100001000800000805F9B34FB";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.One, dump);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Invalid URI format.")]
        public void TruncatedUuid()
        {
            String urlTruncatedUuid = "btspp://localhost:0000110100001000800000805F9";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(urlTruncatedUuid);
        }

#if FX1_1
        /*
#endif
#pragma warning restore 618
#if FX1_1
        */
#endif

        [Test]
        public void FancyName()
        {
            String url = "btspp://localhost:102030405060708090A1B1C1D1D1E100"
                + ";name=aa-bb_cc dd1234";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooRfcommOneWithFancyName, dump);
        }

        [Test]
        [Ignore("Arghh Regex not looping")]
        public void FancyNameFollowed()
        {
            String url = "btspp://localhost:102030405060708090A1B1C1D1D1E100"
                + ";name=aa-bb_cc dd1234;encrypt=false";
            ServiceRecordBuilder bldr = ServiceRecordBuilder.FromJsr82ServerUri(url);
            ServiceRecord record = bldr.ServiceRecord;
            String dump = ServiceRecordUtilities.Dump(record);
            Assert.AreEqual(ServiceRecordBuilderTests_Data.FooRfcommOneWithFancyName, dump);
        }

    }
}
