//-----------------------------------------------------------------------
// <copyright file="Radio.Uap.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-18 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace InTheHand.Devices.Radios
{
    partial class Radio
    {
        private Windows.Devices.Radios.Radio _radio;

        private Radio(Windows.Devices.Radios.Radio radio)
        {
            _radio = radio;
        }

        public static implicit operator Windows.Devices.Radios.Radio(Radio radio)
        {
            return radio._radio;
        }

        public static implicit operator Radio(Windows.Devices.Radios.Radio radio)
        {
            return new Radio(radio);
        }

        private static async Task<RadioAccessStatus> DoRequestAccessAsync()
        {
            return (RadioAccessStatus)((int)await Windows.Devices.Radios.Radio.RequestAccessAsync());
        }

        private async Task<RadioAccessStatus> DoSetStateAsync(RadioState state)
        {
            return (RadioAccessStatus)((int)await _radio.SetStateAsync((Windows.Devices.Radios.RadioState)((int)state)));
        }

        private static async Task DoGetRadiosAsync(List<Radio> radios)
        {
            foreach (Windows.Devices.Radios.Radio r in await Windows.Devices.Radios.Radio.GetRadiosAsync())
            {
                radios.Add(r);
            }
        }
        
        private RadioKind GetKind()
        {
            return (RadioKind)((int)_radio.Kind);
        }

        private string GetName()
        {
            return _radio.Name;
        }

        private RadioState GetState()
        {
            return (RadioState)((int)_radio.State);
        }
    }
}