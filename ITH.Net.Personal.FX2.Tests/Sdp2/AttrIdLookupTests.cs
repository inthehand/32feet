using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;

namespace InTheHand.Net.Tests.Sdp2
{

    [TestFixture]
    public class DumpAttributeIdLookup
    {

        [Test]
        public void OneSimple()
        {
            String name = AttributeIdLookup.GetName((ServiceAttributeId)0x0001,
                new Type[] { typeof(UniversalAttributeId) });
            Assert.AreEqual("ServiceClassIdList", name);
        }

        [Test]
        public void TwoSimple()
        {
            String name = AttributeIdLookup.GetName((ServiceAttributeId)0x0302,
                new Type[] {
                    typeof(UniversalAttributeId), 
                    typeof(HeadsetProfileAttributeId)});
            Assert.AreEqual("RemoteAudioVolumeControl", name);
        }

    }//class

}