#include "stdafx.h"
#include "32feetWidcomm.h"


class RfCommIf : public CRfCommIf
{
};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void RfCommIf_Create(RfCommIf** ppObj)
	{
		//printf("RfCommIf_Create\n");
		RfCommIf* p = new RfCommIf();
		_ASSERT(p != NULL);
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void RfCommIf_Destroy(RfCommIf* pObj)
	{
		//printf("RfCommIf_Destroy\n");
		_ASSERT(pObj != NULL);
		RfCommIf* p = (RfCommIf*)pObj;
		delete p;
	}

	MY32FEETWIDCOMM_API BOOL RfCommIf_Client_AssignScnValue(RfCommIf* pObj, 
		GUID *p_service_guid, UINT8 scn)
	{
		//printf("RfCommIf_Client_AssignScnValue\n");
		//char* pG = (char*)p_service_guid;
		//printf("RCI_C_ASV params: %p %p={%x-%x-%x-%x-%x-%x-%x-%x} %x\n",
		//	pObj,
		//	p_service_guid,
		//	pG[0], pG[1], pG[2], pG[3],
		//	pG[4], pG[5], pG[6], pG[7],
		//	scn
		//	);
		_ASSERT(pObj != NULL);
		RfCommIf* p = (RfCommIf*)pObj;
		//
		//
#if 1
		return p->AssignScnValue (p_service_guid, scn);
#else
		char fakeGuid[16];
		memset(fakeGuid, 0, 16);
		return p->AssignScnValue ((GUID*)fakeGuid, scn);
#endif
	}

	MY32FEETWIDCOMM_API int RfCommIf_GetScn(RfCommIf* pObj)
	{
		//printf("RfCommIf_GetScn\n");
		_ASSERT(pObj != NULL);
		RfCommIf* p = (RfCommIf*)pObj;
		return p->GetScn();
	}

	MY32FEETWIDCOMM_API BOOL RfCommIf_SetSecurityLevel(RfCommIf* pObj,
		BT_CHAR *p_service_name, UINT8 security_level, BOOL is_server)
	{
		//printf("RfCommIf_SetSecurityLevel\n");
		//printf("RfCommIf_SetSecurityLevel %p {%x-%x-%x-%x-%x-%x-%x-%x} %x %d\n", pObj, 
		//	p_service_name[0], p_service_name[1], p_service_name[2], p_service_name[3], 
		//	p_service_name[4], p_service_name[5], p_service_name[6], p_service_name[7], 
		//	security_level, is_server);
		_ASSERT(pObj != NULL);
		RfCommIf* p = (RfCommIf*)pObj;
		BOOL ret= p->SetSecurityLevel (p_service_name, security_level, is_server);
		//printf("exit RfCommIf_SetSecurityLevel: %d\n", ret);
		return ret;
	}

}//extern
