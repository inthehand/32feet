// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Bluetooth.Factory;

namespace TtfTests.Droid
{
    static class AndroidTestInfra
    {
        internal static BluetoothFactory Init()
        {
            var f = new AndroidBthMockFactory();
            return f;
        }

        internal static BluetoothFactory Init(AndroidMockValues values)
        {
            var f = new AndroidBthMockFactory(values);
            return f;
        }

    }
}
#endif
