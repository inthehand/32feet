#include "stdafx.h"
#include "32feetWidcomm.h"

extern "C" {
	BOOL MyFindRFCommScn (CSdpDiscoveryRec*pRec, UINT8 *pScn)
	{
		UINT8 val;
		BOOL got = pRec->FindRFCommScn(&val);
		//printf("Record #%d, got: %d, val: %d\n", i, got, val);
		if (got)
			*pScn = val;
		UINT16 const rfcomm = 3;
		// FindRFCommScn seems not to work on some platforms, try using
		// FindProtocolListElem instead...
		tSDP_PROTOCOL_ELEM elem;
		BOOL got2 = pRec->FindProtocolListElem(rfcomm, &elem);
		int const INDEX_OF_SCN = 0;
		if (got2) {
			_ASSERT(elem.protocol_uuid == rfcomm);
			_ASSERT(elem.num_params >= 1); // one *parameter*
			UINT16 val_16 = elem.params[INDEX_OF_SCN];
			UINT8 val2 = (UINT8)val_16;
			if (!got) {
				*pScn = val2;
			} else {
				_ASSERT(val2 == val);
			}
		} else {
			*pScn = 0xFF;
		}
		// exit
		if(got || got2) {
			_ASSERT(*pScn != 0xFF);
			return true;
		} else{
			return false;
		}
	}


	MY32FEETWIDCOMM_API void SdpDiscoveryRec_GetRfcommPorts(void *p, int count, 
		/*OUT*/int ports[])
	{
		//printf("SdpDiscoveryRec_GetRfcommPorts\n");
		CSdpDiscoveryRec* pRec = (CSdpDiscoveryRec*)p;
		for(int i = 0; i < count; ++i) {
			UINT8 val;
			BOOL got = MyFindRFCommScn(&pRec[i], &val);
			//printf("Record #%d, got: %d, val: %d\n", i, got, val);
			if (got) {
				ports[i] = val;
			} else {
				ports[i] = -1;
			}
		}
	}

	MY32FEETWIDCOMM_API void SdpDiscoveryRec_GetL2CapPsms(void *p, int count, 
		/*OUT*/int ports[])
	{
		//printf("SdpDiscoveryRec_GetL2CapPsms\n");
		CSdpDiscoveryRec* pRec = (CSdpDiscoveryRec*)p;
		for(int i = 0; i < count; ++i) {
			UINT16 val;
			BOOL got = pRec[i].FindL2CapPsm(&val);
			//printf("Record #%d, got: %d, val: %d\n", i, got, val);
			if (got)
				ports[i] = val;
			else
				ports[i] = -1;
		}
	}

	void WINAPI ForceGetPdl(CSdpDiscoveryRec *pList, int count)
	{
		const int MAX = 100;
		int forceGet[MAX];
		SdpDiscoveryRec_GetRfcommPorts(pList, min(count, MAX), forceGet);
		wprintf(L"BtIf.cpp forceGet: %d, %d, %d, %d, %d, ...\n", 
			forceGet[0], forceGet[1], forceGet[2], forceGet[3], forceGet[4]);
		//----
		//UINT16 const sdp = 1;
		UINT16 const rfcomm = 3;
		UINT16 const obex = 8;
		UINT16 const l2cap = 0x100;
		tSDP_PROTOCOL_ELEM elem;
		for (int i = 0; i< count; ++i) {
			BOOL got1L = pList[i].FindProtocolListElem(l2cap, &elem);
			BOOL got2R = pList[i].FindProtocolListElem(rfcomm, &elem);
			BOOL got3O = pList[i].FindProtocolListElem(obex, &elem);
			wprintf(L"cpp ForceGetPdl %d/%d: %d, %d, %d, ...\n", 
				i, count,
				got1L, got2R, got3O
				);
		}
		//----
		//const int MAX = 100;
		//int forceGet[MAX];
		SdpDiscoveryRec_GetRfcommPorts(pList, min(count, MAX), forceGet);
		wprintf(L"BtIf.cpp forceGet: %d, %d, %d, %d, %d, ...\n", 
			forceGet[0], forceGet[1], forceGet[2], forceGet[3], forceGet[4]);
	}


	struct SimpleInfo
	{
		//int fake;
		GUID serviceUuid;
		wchar_t *serviceNameWchar;
		char *serviceNameChar;
		int scn; // -1 for not present, byte otherwise
	};

	MY32FEETWIDCOMM_API void SdpDiscoveryRec_GetSimpleInfo(void *p, int count, 
		/*[Out]*/ struct SimpleInfo *info, int cbItem)
	{
		//printf("SdpDiscoveryRec_GetSimpleInfo\n");
#if defined(WINCE) || !defined(_DEBUG)
		cbItem;
#endif
#if WIN32
		_ASSERT(cbItem == sizeof(SimpleInfo));
#endif
		CSdpDiscoveryRec* pRec = (CSdpDiscoveryRec*)p;
		for(int i = 0; i < count; ++i) {
			//info[i].fake = i;
			info[i].serviceUuid = pRec[i].m_service_guid; //copy
			//
#ifdef _WIN32_WINDOWS
			info[i].serviceNameWchar = NULL;
			info[i].serviceNameChar = pRec[i].m_service_name; //pointer
#else
			info[i].serviceNameChar = NULL;
			info[i].serviceNameWchar = pRec[i].m_service_name; //pointer
#endif
			//
			UINT8 val;
			BOOL got = MyFindRFCommScn(&pRec[i], &val);
			if (got)
				info[i].scn = val;
			else
				info[i].scn = -1;
		}
	}


	MY32FEETWIDCOMM_API void SdpDiscoveryRec_DeleteArray(void *p)
	{
		//printf("SdpDiscoveryRec_DeleteArray\n");
		// The new CSdpDiscoveryRec[...] is in BtIf_ReadDiscoveryRecords
		CSdpDiscoveryRec* pList = (CSdpDiscoveryRec*)p;
		delete[] pList;
	}

}