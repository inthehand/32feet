using System;
using System.Collections.Generic;
using InTheHand.Net.Bluetooth.BlueSoleil;

namespace InTheHand.Net.Tests.BlueSoleil
{
    class BsTestNullApi : IBluesoleilApi
    {
        public bool Btsdk_IsServerConnected()
        {
            return true;
        }





        protected internal BsTestNullApi()
        {
        }

        bool _inited;
        UInt32? _hConn_OneOf;

        internal void SetIsOpen()
        {
            _hConn_OneOf = 99;
        }

        internal void SetIsNotOpen()
        {
            _hConn_OneOf = null;
        }

        #region IBluesoleilApi Members

        BtSdkError IBluesoleilApi.Btsdk_Init()
        {
            _inited = true;
            return BtSdkError.OK;
        }

        bool IBluesoleilApi.Btsdk_IsSDKInitialized()
        {
            return _inited;
        }

        BtSdkError IBluesoleilApi.Btsdk_Done()
        {
            if (!_inited)
                return BtSdkError.SDK_UNINIT;
            _inited = false;
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_RegisterCallback4ThirdParty(ref Structs.BtSdkCallbackStru call_back)
        {
            throw new NotImplementedException();
        }

        void IBluesoleilApi.Btsdk_FreeMemory(IntPtr memblock)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StartBluetooth()
        {
            if (!_inited)
                return BtSdkError.SDK_UNINIT;
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_StopBluetooth()
        {
            throw new NotImplementedException();
        }

        bool IBluesoleilApi.Btsdk_IsBluetoothReady()
        {
            if (!_inited)
                return false;
            return true;
        }

        bool IBluesoleilApi.Btsdk_IsBluetoothHardwareExisted()
        {
            if (!_inited)
                return false;
            return true;
        }

        BtSdkError IBluesoleilApi.Btsdk_SetDiscoveryMode(StackConsts.DiscoveryMode mode)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetDiscoveryMode(out StackConsts.DiscoveryMode mode)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceAddress(byte[] bd_addr)
        {
            if (!_inited)
                return BtSdkError.SDK_UNINIT;
            byte[] addr = { 0x00, 0x20, 0x30, 0x40, 0x50, 0x60 };
            addr.CopyTo(bd_addr, 0);
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalName(byte[] name, ref ushort len)
        {
            if (!_inited)
                return BtSdkError.SDK_UNINIT;
            byte[] nameArr = { (byte)'f', (byte)'o', (byte)'o', (byte)'o', (byte)'o', (byte)'o', 0, 0, 0, 0, };
            nameArr.CopyTo(name, 0);
            len = checked((ushort)nameArr.Length);
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalDeviceClass(out uint device_class)
        {
            device_class = unchecked((uint)(-1));
            if (!_inited) {
                return BtSdkError.SDK_UNINIT;
            }
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_GetLocalLMPInfo(ref Structs.BtSdkLocalLMPInfoStru lmp_info)
        {
            lmp_info = new Structs.BtSdkLocalLMPInfoStru();
            if (!_inited) {
                return BtSdkError.SDK_UNINIT;
            }
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_StartDeviceDiscovery(uint device_class, ushort max_num, ushort max_seconds)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_StopDeviceDiscovery()
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_UpdateRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_IsDevicePaired(uint dev_hdl, out bool pis_paired)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_PairDevice(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_PinCodeReply(uint dev_hdl, byte[] pin_code, ushort size)
        {
            throw new NotImplementedException();
        }

        bool IBluesoleilApi.Btsdk_IsDeviceConnected(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteRSSI(uint device_handle, out sbyte prssi)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteLinkQuality(uint device_handle, out ushort plink_quality)
        {
            throw new NotImplementedException();
        }

        uint IBluesoleilApi.Btsdk_GetRemoteDeviceHandle(byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        uint IBluesoleilApi.Btsdk_AddRemoteDevice(byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_DeleteRemoteDeviceByHandle(uint dev_hdl)
        {
            throw new NotImplementedException();
        }

        int IBluesoleilApi.Btsdk_GetStoredDevicesByClass(uint dev_class, uint[] pdev_hdl, int max_dev_num)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceAddress(uint dev_hdl, byte[] bd_addr)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceName(uint dev_hdl, byte[] name, ref ushort plen)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceClass(uint dev_hdl, out uint pdevice_class)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteDeviceProperty(uint dev_hdl, out Structs.BtSdkRemoteDevicePropertyStru rmt_dev_prop)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_BrowseRemoteServicesEx(uint dev_hdl, Structs.BtSdkSDPSearchPatternStru[] psch_ptn, int ptn_num, uint[] svc_hdl, ref int svc_count)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_GetRemoteServiceAttributes(uint svc_hdl, ref Structs.BtSdkRemoteServiceAttrStru attribute)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_Disconnect(uint handle)
        {
            if (!_inited)
                return BtSdkError.SDK_UNINIT;
            // TO-DO Should really check we're getting the correct handle.
            if (!_hConn_OneOf.HasValue)
                return BtSdkError.HANDLE_NOT_EXIST;
            _hConn_OneOf = null;
            return BtSdkError.OK;
        }

        BtSdkError IBluesoleilApi.Btsdk_SearchAppExtSPPService(uint dev_hdl, ref Structs.BtSdkAppExtSPPAttrStru psvc)
        {
            throw new NotImplementedException();
        }

        BtSdkError IBluesoleilApi.Btsdk_ConnectAppExtSPPService(uint dev_hdl, ref Structs.BtSdkAppExtSPPAttrStru psvc, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_RegisterGetStatusInfoCB4ThirdParty(ref NativeMethods.Func_ReceiveBluetoothStatusInfo statusCBK)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_SetStatusInfoFlag(ushort usMsgType)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_StartEnumConnection()
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_EnumConnection(uint enum_handle, ref Structs.BtSdkConnectionPropertyStru conn_prop)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_EndEnumConnection(uint enum_handle)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_ConnectEx(uint dev_hdl, ushort service_class, uint lParam, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBluesoleilApi Members


        public BtSdkError Btsdk_ConnectEx(uint dev_hdl, ushort service_class, ref Structs.BtSdkSPPConnParamStru lParam, out uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_InitCommObj(byte com_idx, ushort svc_class)
        {
            throw new NotImplementedException();
        }

        public BtSdkError Btsdk_DeinitCommObj(byte com_idx)
        {
            throw new NotImplementedException();
        }

        public short Btsdk_GetClientPort(uint conn_hdl)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_CommNumToSerialNum(int comportNum)
        {
            throw new NotImplementedException();
        }

        public void Btsdk_PlugOutVComm(uint serialNum, StackConsts.COMM_SET flag)
        {
            throw new NotImplementedException();
        }

        public bool Btsdk_PlugInVComm(uint serialNum, out uint comportNumber, uint usageType, StackConsts.COMM_SET flag, uint dwTimeout)
        {
            throw new NotImplementedException();
        }

        public uint Btsdk_GetASerialNum()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
