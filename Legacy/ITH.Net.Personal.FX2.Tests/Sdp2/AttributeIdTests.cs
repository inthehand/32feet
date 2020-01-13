using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{
    

    [TestFixture]
    public class ServiceAttributeIdTests
    {

        private static ServiceAttributeId  DoTestNonExpected(ServiceAttributeId x, ServiceAttributeId y)
        {
            ServiceAttributeId result = ServiceRecord.CreateLanguageBasedAttributeId(x, y);
            return result;
        }
        private static void DoTest(ServiceAttributeId expected, ServiceAttributeId x, ServiceAttributeId y)
        {
            ServiceAttributeId result = DoTestNonExpected(x, y);
            Assert.AreEqual(expected, result);
        }


        [Test]
        public void Simple()
        {
            DoTest((ServiceAttributeId)0x0101,
                (ServiceAttributeId)1, (ServiceAttributeId)0x0100);
        }

        [Test]
        public void OneMsbSet()
        {
            DoTest(unchecked((ServiceAttributeId)0xF101), 
                (ServiceAttributeId)1, unchecked((ServiceAttributeId)0xF100));
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void OverflowFromBothMsbSet()
        {
            DoTestNonExpected(
                unchecked((ServiceAttributeId)0x8002), unchecked((ServiceAttributeId)0x8100));
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void OverflowFromOneMsbSet()
        {
            DoTestNonExpected(
                (ServiceAttributeId)0x1000, unchecked((ServiceAttributeId)0xF100));
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void OverflowFromNeitherMsbSet()
        {
            DoTestNonExpected(
                (ServiceAttributeId)0x7000, unchecked((ServiceAttributeId)0x7000));
        }

    }//class

}