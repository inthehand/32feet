//-----------------------------------------------------------------------
// <copyright file="Radio.wp8.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Windows.Networking.Proximity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace InTheHand.Devices.Radios
{
    partial class Radio
    {
        private static void DoGetRadiosAsync(List<Radio> radios)
        {
            radios.Add(new Radio());
        }
        
        // only supporting Bluetooth radio
        private RadioKind GetKind()
        {
            return RadioKind.Bluetooth;
        }

        // matching the UWP behaviour (although we could have used the radio name)
        private string GetName()
        {
            return GetKind().ToString();
        }

        private RadioState GetState()
        {
            RadioState state = RadioState.Unknown;

            var t = Task.Run(async () =>
            {
                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";

                try
                {
                    var peers = await PeerFinder.FindAllPeersAsync();
                    state = RadioState.On;
                }
                catch (Exception ex)
                {
                    if ((uint)ex.HResult == 0x8007048F)
                    {
                        state = RadioState.Off;
                    }
                }
            });
            t.Wait();

            return state;
        }

    }
}