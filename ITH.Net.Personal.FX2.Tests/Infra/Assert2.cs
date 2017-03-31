using System;
using NUnit.Framework;

namespace InTheHand.Net.Tests.Infra
{
    static class Assert2
    {
        public static void AreEqual_Buffers(byte[] expectedData, byte[] buffer, int offset, int count, string name)
        {
            if (count > buffer.Length)
                throw new ArgumentException("count");
            for (int i = 0; i < Math.Min(expectedData.Length, count); ++i) {
                Assert.AreEqual(expectedData[i], buffer[i + offset], name + "-- at " + i);
            }
            Assert.AreEqual(expectedData.Length, count, name + "-- lengths");
        }

    }
}
