// 32feet.NET - Personal Area Networking for .NET
//
// BtIf.cpp
// 
// Copyright (c) 2009-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2009-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#include "stdafx.h"
#include "32feetWidcomm.h"


typedef void (CALLBACK* FnDeviceResponded)(BD_ADDR bdAddr, DEV_CLASS devClass, BD_NAME deviceName, BOOL connected);
typedef void (CALLBACK* FnInquiryComplete)(BOOL success, UINT16 numResponses);
typedef void (CALLBACK* FnDiscoveryComplete)();
typedef void (CALLBACK* FnStackStatusChange)(int/*CBtIf::STACK_STATUS*/ new_status);


class BtIf : public CBtIf
{
public:
	FnDeviceResponded  pHandleDeviceResponded;
	FnInquiryComplete pHandleInquiryComplete;
	FnDiscoveryComplete pHandleDiscoveryComplete;
	FnStackStatusChange pHandleStackStatusChange;

private:
	//virtual void OnStackStatusChange(CBtIf::STACK_STATUS new_status) {}

	virtual void OnDeviceResponded (BD_ADDR bda, DEV_CLASS devClass, BD_NAME bdName, BOOL bConnected)
	{
		//printf("BtIf::OnDeviceResponded\n");
		pHandleDeviceResponded(bda, devClass, bdName, bConnected);
	}
	virtual void OnInquiryComplete (BOOL success, short num_responses)
	{
		//printf("BtIf::OnInquiryComplete\n");
		pHandleInquiryComplete (success, num_responses);
	}

	//----------
	virtual void OnDiscoveryComplete ()
	{
		//printf("BtIf::OnDiscoveryComplete\n");
		pHandleDiscoveryComplete ();
	}

	virtual void OnDiscoveryComplete (UINT16 nRecs, long lResultCode)
	{
		//printf("BtIf::OnDiscoveryComplete_B, %d %x\n", nRecs, lResultCode);
		nRecs; lResultCode; // Unused.
		//pHandleDiscoveryComplete ();
	}

	//----------
	virtual void OnStackStatusChange(CBtIf::STACK_STATUS new_status)
	{
		//printf("BtIf::OnStackStatusChange\n");
		FnStackStatusChange pFn = pHandleStackStatusChange;
		if (pFn){
			pFn(new_status);
		}
	}

};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void BtIf_Create(BtIf** ppObj, FnDeviceResponded deviceResponded, 
		FnInquiryComplete inquiryCompleted, FnDiscoveryComplete discoveryComplete)
	{
		//printf("BtIf_Create\n");
		BtIf* p = new BtIf();
		p->pHandleDeviceResponded = deviceResponded;
		p->pHandleInquiryComplete = inquiryCompleted;
		p->pHandleDiscoveryComplete = discoveryComplete;
		p->pHandleStackStatusChange = 0;
		*ppObj = p;
		//
		BT_CHAR sdkVers[2000], stackVers[2000];
		wchar_t* isWideStringCheck = NULL;
		char* isNarrowStringCheck = NULL;
		BOOL ret;
#ifdef WINCE
#define FormatBT_CHAR L"%ls"
		isWideStringCheck = sdkVers;
#else
#define FormatBT_CHAR L"%hs"
		isNarrowStringCheck = sdkVers;
#endif
#define Prefix L"32feet.NET's Widcomm "
#ifdef WINCE
		ret = p->GetDKCEVersionInfo(sdkVers, sizeof(sdkVers));
#else
		ret = p->GetDKVersionInfo(sdkVers, sizeof(sdkVers));
#endif
#define Prefix L"32feet.NET's Widcomm "
		if (ret)
			fwprintf(stderr, Prefix L"SDK Version  : " FormatBT_CHAR L"\n", sdkVers);
		else
			fputws(Prefix L"GetDK[CE]VersionInfo error.\n", stderr);
		//
#ifdef WINCE
		ret = p->GetBTWCEVersionInfo(stackVers, sizeof(stackVers));
#else
		ret = p->GetBTWVersionInfo(stackVers, sizeof(stackVers));
#endif
		if (ret)
			fwprintf(stderr, Prefix L"Stack Version: " FormatBT_CHAR L"\n", stackVers);
		else
			fputws(Prefix L"GetBTW[CE]VersionInfo error.\n", stderr);
		isWideStringCheck = NULL;
		isNarrowStringCheck = NULL;
	}

	MY32FEETWIDCOMM_API int BtIf_SetCallback2(BtIf* pObj, int num, FnStackStatusChange stackStatusChange)
	{
		// (At the current version we don't use this on Win32 because of wierd
		// behaviour; we don't call it from managed code).
		BtIf* p = (BtIf* )pObj;
		if (num >= 1) {
			p->pHandleStackStatusChange = stackStatusChange;
		}
		const int SetCallback2_Num_Callbacks_Supported = 1;
		return SetCallback2_Num_Callbacks_Supported;
	}

	MY32FEETWIDCOMM_API void BtIf_Destroy(BtIf* pObj)
	{
		printf("BtIf_Destroy\n");
		BtIf* p = (BtIf* )pObj;
		delete p;
	}

	//----------
	MY32FEETWIDCOMM_API BOOL BtIf_StartInquiry(BtIf* pObj)
	{
		//printf("BtIf_StartInquiry\n");
		BtIf* p = (BtIf* )pObj;
		return p->StartInquiry();
	}

	MY32FEETWIDCOMM_API void BtIf_StopInquiry(BtIf* pObj)
	{
		//printf("BtIf_StopInquiry\n");
		BtIf* p = (BtIf* )pObj;
		p->StopInquiry();
	}

	//----------
	MY32FEETWIDCOMM_API BOOL BtIf_StartDiscovery(BtIf* pObj, BYTE* p_bda ,GUID *p_service_guid,
		/*OUT*/int *pSizeofCSdpDiscoveryRec)
	{
		//printf("BtIf_StartDiscovery\n");
		BtIf* p = (BtIf* )pObj;
		*pSizeofCSdpDiscoveryRec = sizeof(CSdpDiscoveryRec);
		return p->StartDiscovery(p_bda, p_service_guid);
	}

	MY32FEETWIDCOMM_API CBtIf::DISCOVERY_RESULT BtIf_GetLastDiscoveryResult(BtIf* pObj, BYTE* p_bda, UINT16 *p_num_recs)
	{
		//printf("BtIf_GetLastDiscoveryResult\n");
		BtIf* p = (BtIf* )pObj;
		return p->GetLastDiscoveryResult(p_bda, p_num_recs);
	}

	MY32FEETWIDCOMM_API int BtIf_ReadDiscoveryRecords (BtIf* pObj, 
		BYTE* p_bda, int max_size, CSdpDiscoveryRec **pp_list)
	{
		//printf("BtIf_ReadDiscoveryRecords p_bda: %p, pp_list: %p\n", p_bda, pp_list);
		BtIf* p = (BtIf*)pObj;
		// The delete[]... is in SdpDiscoveryRec_DeleteArray
		CSdpDiscoveryRec* pList = new CSdpDiscoveryRec[max_size];
		//printf("    new pList: %p\n", pList);
		*pp_list = pList;
		int count = p->ReadDiscoveryRecords (p_bda, max_size, pList /*, GUID *p_guid_filter = NULL*/);
		//ForceGetPdl(pList, count);
		return count;
	}

	MY32FEETWIDCOMM_API int BtIf_ReadDiscoveryRecordsServiceClassOnly (BtIf* pObj, 
		BYTE* p_bda, int max_size, CSdpDiscoveryRec **pp_list,  GUID *p_guid_filter)
	{
		//printf("BtIf_ReadDiscoveryRecordsServiceClassOnly p_bda: %p, pp_list: %p, p_guid: %p\n", p_bda, pp_list, p_guid_filter);
		//if (!p_guid_filter)
			//printf("    *p_guid_filter: (null)!\n");
		//else
			//printf("    *p_guid_filter: 0x%08x\n", *(int*)p_guid_filter);
		BtIf* p = (BtIf*)pObj;
		// The delete[]... is in SdpDiscoveryRec_DeleteArray
		CSdpDiscoveryRec* pList = new CSdpDiscoveryRec[max_size];
		//printf("    new pList: %p\n", pList);
		*pp_list = pList;
		int count = p->ReadDiscoveryRecords (p_bda, max_size, pList, p_guid_filter);
		//ForceGetPdl(pList, count);
		return count;
	}

	//----------
////#define OLDER_WIDCOMM_EG_1_7_1_1424
//#ifndef OLDER_WIDCOMM_EG_1_7_1_1424
//	MY32FEETWIDCOMM_API void BtIf_IsStackUpAndRadioReady(BtIf* pObj, 
//		/*OUT*/ BOOL* pStackServerUp, /*OUT*/ BOOL* pDeviceReady)
//	{
//		//printf("BtIf_IsStackUpAndRadioReady\n");
//		*pStackServerUp = pObj->IsStackServerUp();
//		*pDeviceReady = pObj->IsDeviceReady();
//	}
//#endif

#ifdef WINCE
	typedef BOOL (*PfnIsStackServerUp)(BtIf *this__);
	typedef BOOL (*PfnIsDeviceReady)(BtIf *this__);

	static PfnIsStackServerUp g_pfnIsStackServerUp;
	static PfnIsDeviceReady g_pfnIsDeviceReady;
#endif

	MY32FEETWIDCOMM_API void BtIf_IsStackUpAndRadioReady(BtIf* pObj, 
		/*OUT*/ BOOL* pStackServerUp, /*OUT*/ BOOL* pDeviceReady)
	{
#ifndef WINCE
		//printf("BtIf_IsStackUpAndRadioReady\n");
		*pStackServerUp = pObj->IsStackServerUp();
		*pDeviceReady = pObj->IsDeviceReady();
#else
		if (!g_pfnIsStackServerUp) {
			ASSERT(!g_pfnIsDeviceReady);
			HMODULE hWC = GetModuleHandle(L"BTSDKCE50.DLL");
			_ASSERT(hWC);
			if (hWC) {
				g_pfnIsStackServerUp = (PfnIsStackServerUp)
					GetProcAddress(hWC, L"?IsStackServerUp@CBtIf@@QAAHXZ");
				g_pfnIsDeviceReady = (PfnIsDeviceReady)
					GetProcAddress(hWC, L"?IsDeviceReady@CBtIf@@QAAHXZ");
				ASSERT(g_pfnIsStackServerUp);
				ASSERT(g_pfnIsDeviceReady);
				ASSERTMSG(L"No asserts, must be got both APIs.\n", false);
			}
		}
		ASSERT(g_pfnIsDeviceReady);
		//
		if (g_pfnIsStackServerUp)
			*pStackServerUp = g_pfnIsStackServerUp(pObj);
		else
			*pStackServerUp = TRUE;
		if (g_pfnIsDeviceReady)
			*pDeviceReady = g_pfnIsDeviceReady(pObj);
		else
			*pDeviceReady = TRUE;
#endif
	}


	MY32FEETWIDCOMM_API int BtIf_SetAutoReconnect(BtIf* pObj, 
		BOOL autoReconnect)
	{
		enum MY32FEET_AutoReconnectErrors {
			SUCCESS = 0,
			NO_FUNCTION,
			CALL_FAILED
		};
#ifdef WINCE
		BOOL ret = pObj->SetAutoReconnect(autoReconnect);
		if (ret) {
			return SUCCESS;
		} else {
			return CALL_FAILED;
		}
#else
		pObj = pObj;
		autoReconnect = autoReconnect;
		return NO_FUNCTION;
#endif
	}

	//----------
	MY32FEETWIDCOMM_API void BtIf_IsDeviceConnectableDiscoverable(BtIf* pObj, 
		/*OUT*/ BOOL* pConnectable, /*OUT*/ BOOL* pDiscoverable)
	{
		//printf("BtIf_IsDeviceConnectableDiscoverable\n");
#ifdef WINCE // These are not present in Win32 :-(
		*pConnectable = pObj->IsDeviceConnectable();
		*pDiscoverable = pObj->IsDeviceDiscoverable();
#else
		*pConnectable = true;
		*pDiscoverable = true;
		// This field any use??
		//long deviceState = pObj->m_DeviceState;
		//printf("    (m_DeviceState: %lx  %lx)\n", deviceState, pObj->m_DeviceState);
		pObj;
#endif
	}

	MY32FEETWIDCOMM_API void BtIf_SetDeviceConnectableDiscoverable(BtIf* pObj, 
		BOOL connectable, BOOL pairedOnly, BOOL discoverable)
	{
		//printf("BtIf_SetDeviceConnectableDiscoverable\n");
		// enum { CONNECT_ALLOW_NONE, CONNECT_ALLOW_ALL, 
		//    CONNECT_ALLOW_PAIRED, CONNECT_ALLOW_FILTERED
		// } CONNECT_ALLOW_TYPE;
#ifdef WINCE // These methods are not present in Win32 :-(
		if (!connectable) {
			pObj->AllowToConnect(CBtIf::CONNECT_ALLOW_NONE);
			// Don't change Discoverable.
		} else {
			if (pairedOnly)
				pObj->AllowToConnect(CBtIf::CONNECT_ALLOW_PAIRED);
			else
				pObj->AllowToConnect(CBtIf::CONNECT_ALLOW_ALL);
			pObj->SetDiscoverable(discoverable);
		}
#else
		pObj, connectable, pairedOnly, discoverable; // unused. :-(
#endif
	}

	MY32FEETWIDCOMM_API BOOL BtIf_GetLocalDeviceInfoBdAddr(BtIf* pObj, 
		void* pBdAddr, int cb)
	{
		//printf("BtIf_GetLocalDeviceInfoBdAddr\n");
		_ASSERT(cb == sizeof(BD_ADDR));
		// Had some problems with alignment etc, so use a native buffer and then 
		// copy it to the passed managed buffer.
		BD_ADDR buf;
		BOOL success = pObj->GetLocalDeviceInfo();
		if (success){
			memcpy_s(pBdAddr, cb, pObj->m_BdAddr, sizeof(buf));
		}
		return success;
	}

	MY32FEETWIDCOMM_API BOOL BtIf_GetLocalDeviceVersionInfo(BtIf* pObj, 
		void* pDevVerInfo, int cb)
	{
		//printf("BtIf_GetLocalDeviceVersionInfo\n");
		_ASSERT(cb == sizeof(CBtIf::DEV_VER_INFO));
		// Had some problems with alignment etc, so use a native buffer and then 
		// copy it to the passed managed buffer.
		CBtIf::DEV_VER_INFO buf;
		BOOL success = pObj->GetLocalDeviceVersionInfo(&buf);
		memcpy_s(pDevVerInfo, cb, &buf, sizeof(buf));
		return success;
	}

	MY32FEETWIDCOMM_API BOOL BtIf_GetLocalDeviceName (BtIf* pObj, 
		void* pBdName, int cb)
	{
		//printf("BtIf_GetLocalDeviceName\n");
		_ASSERT(cb == BD_NAME_LEN);
		// Had some problems with alignment etc, so use a native buffer and then 
		// copy it to the passed managed buffer.
		BD_NAME buf;
		BD_NAME *pBuf = &buf;
		// BD_NAME is an array, but is passed by reference!
		// "BOOL GetLocalDeviceName(BD_NAME *BdName);"
		BOOL success = pObj->GetLocalDeviceName(pBuf);
		memcpy_s(pBdName, cb, buf, BD_NAME_LEN);
		return success;
	}

	//----------
#ifndef WINCE     // "BEGIN:  added BTW-CE and SDK 1.7.1.2700"
	//
	// We don't use these currently, because:
	// 1) GetRemoteDeviceInfoEx isn't supported on Win32, so only devices matching 
	// a particular CoD can be looked-up.
	// 2) On CE/WM they are only supported on a version later than on my iPAQ, so 
	// can't test them.  Note the DLL wouldn't load when these calls were present!
	//

	MY32FEETWIDCOMM_API CBtIf::REM_DEV_INFO_RETURN_CODE BtIf_GetRemoteDeviceInfo(BtIf* pObj, 
		CBtIf::REM_DEV_INFO* p_rem_dev_info, int cb)
	{
		printf("BtIf_GetRemoteDeviceInfo, pBuf=%p\n", p_rem_dev_info);
		printf("  sizeof(CBtIf::DEV_VER_INFO)=%d, cb=%d\n", sizeof(CBtIf::REM_DEV_INFO), cb);
		_ASSERT(cb == sizeof(CBtIf::REM_DEV_INFO));
		if(cb != sizeof(CBtIf::REM_DEV_INFO))
			return (CBtIf::REM_DEV_INFO_RETURN_CODE)69;
		printf("gonna...\n");
		//CBtIf::REM_DEV_INFO tmp;
#if _WIN32
		DEV_CLASS devClass;
		devClass[0] = 0x02;
		devClass[1] = 0x01;
		devClass[2] = 0x04;
		CBtIf::REM_DEV_INFO_RETURN_CODE ret = pObj->GetRemoteDeviceInfo(devClass, p_rem_dev_info);
#else
		DEV_CLASS devClass;
		devClass[0] = 0xFF;
		devClass[1] = 0xFF;
		devClass[2] = 0xFF;
		CBtIf::REM_DEV_INFO_RETURN_CODE ret = pObj->GetRemoteDeviceInfoEx(devClass, p_rem_dev_info);
#endif
		printf("exit BtIf_GetRemoteDeviceInfo\n");
		return ret;
	}

	MY32FEETWIDCOMM_API CBtIf::REM_DEV_INFO_RETURN_CODE BtIf_GetNextRemoteDeviceInfo(BtIf* pObj, 
		CBtIf::REM_DEV_INFO* p_rem_dev_info, int /*cb*/)
	{
		printf("BtIf_GetNextRemoteDeviceInfo\n");
		CBtIf::REM_DEV_INFO_RETURN_CODE ret = pObj->GetNextRemoteDeviceInfo(p_rem_dev_info);
		printf("exit BtIf_GetNextRemoteDeviceInfo\n");
		return ret;
	}

#endif

		//----------
	MY32FEETWIDCOMM_API BOOL BtIf_GetConnectionStats (BtIf* pObj, UINT8* bdAddr, 
		void* pStats, int cb)
	{
		//printf("BtIf_GetConnectionStats\n");
		BtIf* p = (BtIf* )pObj;
		_ASSERT(p);
		_ASSERT(pStats);
		_ASSERT(sizeof(tBT_CONN_STATS) == cb);
		if (sizeof(tBT_CONN_STATS) != cb)
			return false;
		// BOOL GetConnectionStats (BD_ADDR bd_Addr, tBT_CONN_STATS *p_conn_stats);
		//                               // added BTW 3.0.0.1500, SDK 3.0.1.901
		tBT_CONN_STATS *p_conn_stats = (tBT_CONN_STATS *)pStats;
		BOOL ret = p->GetConnectionStats(bdAddr, p_conn_stats);
		return ret;
	}

	//----------
	MY32FEETWIDCOMM_API BOOL BtIf_BondQuery (BtIf* pObj, UINT8* bdAddr)
	{
		BOOL arePaired = pObj->BondQuery(bdAddr);
		return arePaired;
	}

	MY32FEETWIDCOMM_API int BtIf_Bond (BtIf* pObj, UINT8* bdAddr, char*pin_code)
	{
		//printf("BtIf_Bond: addr: %x-%x-%x-%x-%x-%x"
			//", pin: %x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x-%x, \n", 
			//bdAddr[0], bdAddr[1], bdAddr[2], bdAddr[3], bdAddr[4], bdAddr[5],
			//pin_code[0], pin_code[1], pin_code[2], pin_code[3], 
			//pin_code[4], pin_code[5], pin_code[6], pin_code[7], 
			//pin_code[8], pin_code[9], pin_code[10], pin_code[11], 
			//pin_code[12], pin_code[13], pin_code[14], pin_code[15],
			//pin_code[16]
			//);
#ifndef WINCE
		int ret = pObj->Bond(bdAddr, pin_code);
#else
		int ret = pObj->Bond(bdAddr, (wchar_t*)pin_code);
#endif
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL BtIf_UnBond (BtIf* pObj, UINT8* bdAddr)
	{
		BOOL ret = pObj->UnBond(bdAddr);
		return ret;
	}

#ifndef WINCE
#ifdef WIDCOMM_SDK6
	MY32FEETWIDCOMM_API int BtIf_BondEx (BtIf* pObj,
		UINT8* bdAddr, CBtIf::tBond_CB *bondCb, void * user_data)
	{
		CBtIf::BOND_RETURN_CODE ret = pObj->BondEx(bdAddr, bondCb, user_data) ;
		return ret;
	}

    MY32FEETWIDCOMM_API void BtIf_BondReply(BtIf* pObj,
		int reply0, UINT32 nPinLength=0, UCHAR * szPin=NULL)
	{
		CBtIf::eBOND_REPLY reply = (CBtIf::eBOND_REPLY)reply0;
		pObj->BondReply(reply, nPinLength, szPin);
	}
#endif
#endif

	//----------
	MY32FEETWIDCOMM_API unsigned int BtIf_GetExtendedError (BtIf* pObj)
	{
		int err;
#if !WINCE
		err = pObj->GetExtendedError();
#else
		pObj; // Fake use for compiler
		err = -1;
#endif
		printf("BtIf_GetExtendedError ret: 0x%08x\n", err);
		return err;
	}

	//----------
	// XXXXXXXXXXXXX 2) On CE/WM they are only supported on a version later than on my iPAQ, so 
	// can't test them.  Note the DLL wouldn't load when these calls were present!

#ifdef WINCE // GetProcAddress for IsRemoteDeviceConnected/Present etc on WM/CE

	typedef BOOL (*PfnIsRemoteDeviceConnected)(BtIf *this__, BD_ADDR bd_addr_remote);
	typedef SDK_RETURN_CODE (*PfnIsRemoteDevicePresent)(BtIf *this__, BD_ADDR bd_addr_remote);

	static PfnIsRemoteDeviceConnected g_pfnIsRemoteDeviceConnected;
	static PfnIsRemoteDevicePresent g_pfnIsRemoteDevicePresent;

	static BOOL IsRemoteDeviceConnected_NotSupported(BtIf * /*this__*/, BD_ADDR /*bd_addr_remote*/)
	{
		return false;
	}

	static SDK_RETURN_CODE IsRemoteDevicePresent_NotSupported(BtIf * /*this__*/, BD_ADDR /*bd_addr_remote*/)
	{
		return SDK_NOT_SUPPORTED;
	}

	static void IsRemoteDevicePresentConnectedFunctionsLookup()
	{
		if (g_pfnIsRemoteDeviceConnected) { // Already done?
			ASSERT(g_pfnIsRemoteDevicePresent);
			return;
		}
		ASSERT(!g_pfnIsRemoteDevicePresent);
		//
		HMODULE hWC = GetModuleHandle(L"BTSDKCE50.DLL");
		_ASSERT(hWC);
		PfnIsRemoteDeviceConnected pfnC = NULL;
		PfnIsRemoteDevicePresent pfnP = NULL;
		if (hWC) {
			pfnC = (PfnIsRemoteDeviceConnected)
				GetProcAddress(hWC, L"?GetRemoteDeviceInfoEx@CBtIf@@QAA?AW4REM_DEV_INFO_RETURN_CODE@1@QAEPAUREM_DEV_INFO@1@@Z");
			pfnP = (PfnIsRemoteDevicePresent)
				GetProcAddress(hWC, L"?IsRemoteDeviceConnected@CBtIf@@QAAHQAE@Z");
		}
		if (!pfnC) {
			pfnC = IsRemoteDeviceConnected_NotSupported;
		}
		if (!pfnP) {
			pfnP = IsRemoteDevicePresent_NotSupported;
		}
		g_pfnIsRemoteDeviceConnected = pfnC;
		g_pfnIsRemoteDevicePresent = pfnP;
	}
#endif

	MY32FEETWIDCOMM_API int BtIf_IsRemoteDevicePresent (BtIf* pObj, UINT8* bdAddr)
	{
		SDK_RETURN_CODE p;
#ifdef WINCE // GetProcAddress for IsRemoteDeviceConnected/Present etc on WM/CE
		IsRemoteDevicePresentConnectedFunctionsLookup();
		p = g_pfnIsRemoteDevicePresent (pObj, bdAddr);
#else
		p = pObj->IsRemoteDevicePresent(bdAddr);
#endif
		return p;
	}

	MY32FEETWIDCOMM_API BOOL BtIf_IsRemoteDeviceConnected (BtIf* pObj, UINT8* bdAddr)
	{
		BOOL c;
#ifdef WINCE // GetProcAddress for IsRemoteDeviceConnected/Present etc on WM/CE
		IsRemoteDevicePresentConnectedFunctionsLookup();
		c = g_pfnIsRemoteDeviceConnected(pObj, bdAddr);
#else
		c = pObj->IsRemoteDeviceConnected(bdAddr);
#endif
		return c;
	}

	MY32FEETWIDCOMM_API void BtIf_IsRemoteDevicePresentConnected (BtIf* pObj, UINT8* bdAddr,
		int *pPresent, BOOL *pConnected)
	{
		*pPresent = BtIf_IsRemoteDevicePresent(pObj, bdAddr);
		*pConnected = BtIf_IsRemoteDeviceConnected(pObj, bdAddr);
	}

	//----------
#ifndef WINCE
    MY32FEETWIDCOMM_API BOOL BtIf_CreateCOMPortAssociation(BtIf* pObj,
		BD_ADDR bda, GUID *p_guid, LPCSTR szServiceName, USHORT mtu, BYTE SecurityID,
		BYTE SecurityLevel, USHORT uuid, USHORT *p_com_port)
	{
		BOOL ret = pObj->CreateCOMPortAssociation(
			bda, p_guid, szServiceName, mtu, SecurityID,
			SecurityLevel, uuid, p_com_port);
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL RemoveCOMPortAssociation(BtIf* pObj,
		USHORT com_port)
	{
		BOOL ret = pObj->RemoveCOMPortAssociation(com_port);
		return ret;
	}

    MY32FEETWIDCOMM_API BOOL ReadCOMPortAssociation(BtIf* pObj,
		tBT_REM_ASSOC_REC *pBuffList, DWORD dwBuffSize, DWORD *pdwRequiredSize)
	{
		BOOL ret = pObj->ReadCOMPortAssociation(
			pBuffList, dwBuffSize, pdwRequiredSize);
		return ret;
	}
#endif

	//----------
	MY32FEETWIDCOMM_API BOOL BtIf_SetLinkSupervisionTimeOut (
		BD_ADDR BdAddr, UINT16 timeout)
	{
		BOOL ret = CBtIf::SetLinkSupervisionTimeOut(BdAddr, timeout);
		return ret;
	}
	
	MY32FEETWIDCOMM_API BOOL BtIf_SwitchRole (
		BD_ADDR BdAddr, int new_role0)
	{
		MASTER_SLAVE_ROLE new_role = (MASTER_SLAVE_ROLE)new_role0;
		BOOL ret = CBtIf::SwitchRole(BdAddr, new_role);
		return ret;
	}


	//----------
//	MY32FEETWIDCOMM_API void BtIf_FooBar (BtIf* pObj)
//	{
//#ifndef WINCE
//		WBtRc rc = pObj->GetExtendedError();
//		BD_NAME* x = &pObj->m_BdName;
//#else
//		int i;
//#endif
//	}

}//extern
