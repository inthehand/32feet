#if BLUETOPIA
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InTheHand.Net.Bluetooth.StonestreetOne;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    class SppEventCreator : IDisposable
    {
        List<IntPtr> _listHGlobalToFree = new List<IntPtr>();

        public SppEventCreator()
        {
        }

        //----
        public Structs.SPP_Event_Data CreateOpenConfirmation(uint portId,
            StackConsts.SPP_OPEN_PORT_STATUS connConfStatusCode)
        {
            var data = new Structs.SPP_Open_Port_Confirmation_Data(
                portId, connConfStatusCode);
            return Create(StackConsts.SPP_Event_Type.Port_Open_Confirmation, ref data);
        }

        public Structs.SPP_Event_Data CreateCloseConfirmation(uint portId)
        {
            var data = new Structs.SPP_Close_Port_Indication_Data(
                portId);
            return Create(StackConsts.SPP_Event_Type.Port_Close_Port_Indication, ref data);
        }

        internal Structs.SPP_Event_Data CreateWriteEmpty(uint portId)
        {
            var data = new Structs.SPP_Transmit_Buffer_Empty_Indication_Data(
                portId);
            return Create(StackConsts.SPP_Event_Type.Port_Transmit_Buffer_Empty_Indication, ref data);
        }

        internal Structs.SPP_Event_Data CreateDataIndication(uint portId, int length)
        {
            return CreateDataIndication(portId, checked((ushort)length));
        }

        internal Structs.SPP_Event_Data CreateDataIndication(uint portId, ushort length)
        {
            var data = new Structs.SPP_Data_Indication_Data(
                portId, length);
            return Create(StackConsts.SPP_Event_Type.Port_Data_Indication, ref data);
        }

        private Structs.SPP_Event_Data Create<T>(StackConsts.SPP_Event_Type code,
            ref T eventDataData) where T : struct
        {
            var eventData = new Structs.SPP_Event_Data(code, Alloc(ref eventDataData));
            return eventData;
        }

        #region Resource control
        private IntPtr Alloc<T>(ref T eventDataData) where T : struct
        {
            IntPtr pData = Marshal.AllocHGlobal(Marshal.SizeOf(eventDataData));
            Debug.Assert(pData != IntPtr.Zero);
            _listHGlobalToFree.Add(pData);
            Marshal.StructureToPtr(eventDataData, pData, false);
            return pData;
        }

        public void Dispose()
        {
            foreach (var cur in _listHGlobalToFree) {
                Marshal.FreeHGlobal(cur);
            }
        }
        #endregion
    }
}
#endif
