#include "stdafx.h"
#include "32feetWidcomm.h"


//class RfCommIf : public CRfCommIf
//{
//};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void SdpService_Create(CSdpService** ppObj)
	{
		//printf("SdpService_Create\n");
		CSdpService* p = new CSdpService();
		_ASSERT(p != NULL);
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void SdpService_Destroy(CSdpService* pObj)
	{
		//printf("SdpService_Destroy\n");
		_ASSERT(pObj != NULL);
		CSdpService* p = (CSdpService*)pObj;
		delete p;
	}

	MY32FEETWIDCOMM_API int SdpService_AddServiceClassIdList(CSdpService* pObj,
		int num_guids, GUID *p_service_guids)
	{
		//printf("SdpService_AddServiceClassIdList, num: %d, p_service_guids: %p\n", 
			//num_guids, p_service_guids);
		_ASSERT(pObj != NULL);
		CSdpService* p = (CSdpService*)pObj;
		return p->AddServiceClassIdList(num_guids, p_service_guids);
	}

	MY32FEETWIDCOMM_API int SdpService_AddRFCommProtocolDescriptor(CSdpService* pObj, UINT8 scn)
	{
		//printf("SdpService_AddRFCommProtocolDescriptor\n");
		_ASSERT(pObj != NULL);
		CSdpService* p = (CSdpService*)pObj;
		return p->AddRFCommProtocolDescriptor(scn);
	}

	MY32FEETWIDCOMM_API int SdpService_AddServiceName(CSdpService* pObj, 
		WCHAR* const p_service_nameWchar, char* const p_service_nameChar)
	{
		//printf("SdpService_AddServiceName\n");
		_ASSERT(pObj != NULL);
		CSdpService* p = (CSdpService*)pObj;
#if WINCE
		p_service_nameChar; // fake use
		return p->AddServiceName(p_service_nameWchar);
#else
		p_service_nameWchar; // fake use
		return p->AddServiceName(p_service_nameChar);
#endif
	}

	MY32FEETWIDCOMM_API int SdpService_AddAttribute(CSdpService* pObj, 
		UINT16 attrId, UINT8 attrType, UINT32 attrLen, UINT8* val)
	{
		//printf("SdpService_AddAttribute (0x%04x)\n", (int)attrId);
		_ASSERT(pObj != NULL);
		CSdpService* p = (CSdpService*)pObj;
		SDP_RETURN_CODE ret = p->AddAttribute(attrId, attrType, attrLen, val);
		return ret;
	}


	MY32FEETWIDCOMM_API int SdpService_CommitRecord(CSdpService* pObj)
	{
		//printf("SdpService_Commit\n");
		_ASSERT(pObj != NULL);
#ifndef _DEBUG
		pObj;
#endif
		return SDP_OK;
//#ifdef !WINCE // These are not present in Win32 :-(
//		return SDP_RETURN_CODE_OK;
//#else
//		CSdpService* p = (CSdpService*)pObj;
//		return p->CommitRecord();
//#endif
	}
}
