// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if ANDROID
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using Android.Bluetooth;

namespace TtfTests.Droid
{

    class SimpleBluetoothClassMock : RealProxy
    {
        readonly DeviceClass _dc;
        readonly ServiceClass _sc;

        internal static BluetoothClass Create(DeviceClass dc, ServiceClass sc)
        {
            var abc = Create<BluetoothClass>(dc, sc);
            return abc;
        }

        private static T Create<T>(DeviceClass dc, ServiceClass sc)
            where T : MarshalByRefObject
        {
            var p = new SimpleBluetoothClassMock(typeof(T), dc, sc);
            return (T)p.GetTransparentProxy();
        }

        //----
        private SimpleBluetoothClassMock(Type t, DeviceClass dc, ServiceClass sc)
            : base(t)
        {
            _dc = dc;
            _sc = sc;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var mcMsg = (IMethodCallMessage)msg;
            object ret;
            string fault = null;
            switch (mcMsg.MethodName) {
                case null:
                    Debug.Fail("Ehhh no method name!");
                    fault = "Ehhh no method name!";
                    ret = null;
                    break;
                default:
                    fault = "Mock doesn't handle method name: '" + mcMsg.MethodName + "'.";
                    ret = null;
                    break;
                //-- BluetoothClass --
                case "get_DeviceClass":
                    ret = _dc;
                    break;
                case "HasService":
                    var checkSc = (ServiceClass)mcMsg.InArgs[0];
                    var cb = CountBits(checkSc);
                    Debug.Assert(cb == 1, "Check bits has more that one bit set, has "
                        + cb + "set, it is: " + checkSc + " i.e. 0x" + ((int)checkSc).ToString("X8"));
                    var retB = (_sc & checkSc) != 0;
                    ret = retB;
                    break;
            }
            //MessageProperty.DumpKeys(msg.Properties);
            if (fault != null) {
                throw new NotImplementedException(fault);
            }
            //
            var retM = new ReturnMessage(ret, null, 0, null, mcMsg);
            return retM;
        }

        private int CountBits(ServiceClass checkSc)
        {
            int v = (int)checkSc;
            int count = 0;
            for (int i = 1; i != 0; ShiftLeftUnchecked(ref i)) {
                if ((v & i) != 0)
                    ++count;
            }
            return count;
        }

        private static int ShiftLeftUnchecked(ref int i)
        {
            unchecked { return i <<= 1; }
        }

    }
}
#endif
