using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Widcomm;
using System.Reflection;
using System.Diagnostics;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommExceptionsTest
    {

        const int RcValue = 2;
        const string Location = "HERE";

        private void DoTest(Exception ex, Type cur, string curTypeName)
        {
            DoTest_(ex, cur, curTypeName);
            //
#if !NETCF
            TestSerialization(ex, cur, curTypeName);
#endif
        }

#if !NETCF
        private void TestSerialization(Exception ex, Type cur, string curTypeName)
        {
            System.IO.MemoryStream strm = new System.IO.MemoryStream();
            System.Runtime.Serialization.IFormatter fmtr
                = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            fmtr.Context = new System.Runtime.Serialization.StreamingContext(
                System.Runtime.Serialization.StreamingContextStates.Persistence);
            fmtr.Serialize(strm, ex);
            //
            strm.Position = 0;
            object o = fmtr.Deserialize(strm);
            DoTest_((Exception)o, cur, curTypeName);
        }
#endif

        private void DoTest_(Exception ex, Type cur, string curTypeName)
        {
            Debug.Assert((cur == null) == (curTypeName == null));
            string expected;
            if (cur == null) {
                expected = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "InTheHand.Net.Bluetooth.Widcomm.{0}WidcommSocketException: ; {1}", "NoResultCode", Location);
            } else {
#if !NETCF
                string rcName = Enum.GetName(cur, RcValue);
#else
                object ev = Enum.Parse(cur, RcValue.ToString(), false);
                string rcName = ev.ToString();
#endif
                expected = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "InTheHand.Net.Bluetooth.Widcomm.{0}_WidcommSocketException: {0}={1}=0x{2:X}; {3}", curTypeName, rcName, RcValue, Location);
            }
            //
            string msg = ExceptionMessage(ex);
            //Console.WriteLine(msg);
            Assert.AreEqual(expected, msg, "Message -- " + curTypeName);
        }

        [Test]
        public void NoReturnCodeException()
        {
            Exception ex = new NoResultCodeWidcommSocketException(1, Location);
            DoTest(ex, null, null);
        }

        [Test]
        public void ReturnCodeExceptions()
        {
            Type[] list = {
                typeof(DISCOVERY_RESULT/*_SocketException*/),
                typeof(PORT_RETURN_CODE/*_WidcommSocketException*/),
                typeof(REM_DEV_INFO_RETURN_CODE/*_WidcommSocketException*/),
                typeof(SdpService.SDP_RETURN_CODE/*_WidcommSocketException*/),
            };
            //
            Type baseType = typeof(WidcommSocketException);
            Assembly baseAssm = baseType.Assembly;
            foreach (Type cur in list) {
                //Exception ex = (Exception)Activator.CreateInstance(cur, 1, RcValue, Location);
                string curTypeName = cur.Name;
                string exTypeName = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "InTheHand.Net.Bluetooth.Widcomm.{0}_WidcommSocketException", curTypeName);
                Type exType = baseAssm.GetType(exTypeName, true);
                System.Reflection.ConstructorInfo[] ciList = exType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                System.Reflection.ConstructorInfo ci = exType.GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                    new Type[] { typeof(int), cur, typeof(string) }, null);
#if !NETCF
                int rcValue2 = RcValue;
#else
                object rcValue2 = Enum.Parse(cur, RcValue.ToString(), false);
#endif
                Exception ex = (Exception)ci.Invoke(new object[] { 1, rcValue2, Location });
                //
                DoTest(ex, cur, curTypeName);
                //
            }//for
        }

        private string ExceptionMessage(Exception ex)
        {
            string all = ex.ToString();
            using (System.IO.StringReader rdr = new System.IO.StringReader(all)) {
                string r = rdr.ReadLine();
                return r;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenericExTypeMustTakeAnEnum()
        {
            Exception ex = new NON_ENUM_WidcommSocketException(1, 2, Location);
        }

        class NON_ENUM_WidcommSocketException
        : GenericReturnCodeWidcommSocketException<Decimal>
        {
            internal NON_ENUM_WidcommSocketException(int errorCode, Decimal ret, string location)
                : base(errorCode, ret, location)
            {
            }

            #region Serializable
#if !NETCF
            protected NON_ENUM_WidcommSocketException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
                : base(info, context)
            {
            }
#endif
            #endregion
        }

    }
}
