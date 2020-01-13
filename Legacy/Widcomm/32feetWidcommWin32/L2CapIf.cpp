#include "stdafx.h"
#include "32feetWidcomm.h"


class L2CapIf : public CL2CapIf
{
};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void L2CapIf_Create(L2CapIf** ppObj)
	{
		//printf("L2CapIf_Create\n");
		L2CapIf* p = new L2CapIf();
		_ASSERT(p != NULL);
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void L2CapIf_Destroy(L2CapIf* pObj)
	{
		//printf("L2CapIf_Destroy\n");
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		delete p;
	}

	MY32FEETWIDCOMM_API BOOL L2CapIf_AssignPsmValue(L2CapIf* pObj, 
		GUID *p_service_guid, UINT16 psm)
	{
		//printf("L2CapIf_Client_AssignScnValue\n");
		//char* pG = (char*)p_service_guid;
		//printf("RCI_C_ASV params: %p %p={%x-%x-%x-%x-%x-%x-%x-%x} %x\n",
		//	pObj,
		//	p_service_guid,
		//	pG[0], pG[1], pG[2], pG[3],
		//	pG[4], pG[5], pG[6], pG[7],
		//	scn
		//	);
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		//
		//
		return p->AssignPsmValue (p_service_guid, psm);
	}

	MY32FEETWIDCOMM_API UINT16 L2CapIf_GetPsm(L2CapIf* pObj)
	{
		//printf("L2CapIf_GetScn\n");
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		return p->GetPsm();
	}

	MY32FEETWIDCOMM_API BOOL L2CapIf_SetSecurityLevel(L2CapIf* pObj,
		BT_CHAR *p_service_name, UINT8 security_level, BOOL is_server)
	{
		//printf("L2CapIf_SetSecurityLevel\n");
		//printf("L2CapIf_SetSecurityLevel %p {%x-%x-%x-%x-%x-%x-%x-%x} %x %d\n", pObj, 
		//	p_service_name[0], p_service_name[1], p_service_name[2], p_service_name[3], 
		//	p_service_name[4], p_service_name[5], p_service_name[6], p_service_name[7], 
		//	security_level, is_server);
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		BOOL ret= p->SetSecurityLevel (p_service_name, security_level, is_server);
		//printf("exit L2CapIf_SetSecurityLevel: %d\n", ret);
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL L2CapIf_Register(L2CapIf* pObj)
	{
		//printf("L2CapIf_Register\n");
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		return p->Register();
	}

	MY32FEETWIDCOMM_API void L2CapIf_Deregister(L2CapIf* pObj)
	{
		//printf("L2CapIf_Deregister\n");
		_ASSERT(pObj != NULL);
		L2CapIf* p = (L2CapIf*)pObj;
		p->Deregister();
	}

}//extern
