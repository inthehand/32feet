// 32feet.NET - Personal Area Networking for .NET
//
// Widcomm SppClient
// 
// Copyright (c) 2009-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2009-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#include "stdafx.h"
#include "32feetWidcomm.h"


typedef void (CALLBACK* FnClientStateChange)(BD_ADDR bda, DEV_CLASS dev_class,
	BD_NAME name, short com_port, SPP_STATE_CODE state);


class SppClient : public CSppClient
{
public:
	FnClientStateChange  pHandleClientStateChange;

private:

	virtual void OnClientStateChange(BD_ADDR bda, DEV_CLASS dev_class,
		BD_NAME name, short com_port, SPP_STATE_CODE state)
	{
		//printf("SppClient::OnClientStateChange\n");
		pHandleClientStateChange(bda, dev_class, name, com_port, state);
	}

};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void SppClient_Create(SppClient** ppObj,
		FnClientStateChange clientStateChange)
	{
		//printf("SppClient_Create\n");
		SppClient* p = new SppClient();
		p->pHandleClientStateChange = clientStateChange;
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void SppClient_Destroy(SppClient* pObj)
	{
		printf("SppClient_Destroy\n");
		SppClient* p = (SppClient* )pObj;
		delete p;
	}

	//----------
	MY32FEETWIDCOMM_API int SppClient_CreateConnection(SppClient* pObj,
		UINT8 bda[], BT_CHAR* arrServiceName) // NB Platform dependent!!!
	{
		//printf("SppClient_CreateConnection\n");
		SppClient* p = (SppClient*)pObj;
		int ret = p->CreateConnection(bda, arrServiceName);
		return ret;
	}

	MY32FEETWIDCOMM_API int SppClient_RemoveConnection(SppClient* pObj)
	{
		SppClient* p = (SppClient*)pObj;
		int ret = p->RemoveConnection();
		return ret;
	}

	MY32FEETWIDCOMM_API int SppClient_GetConnectionStats(SppClient* pObj,
		void* pStats, int cb)
	{
		//printf("SppClient_GetConnectionStats\n");
		_ASSERT(pObj);
		_ASSERT(pStats);
		_ASSERT(sizeof(tBT_CONN_STATS) == cb);
		if (sizeof(tBT_CONN_STATS) != cb)
			return false;
		tBT_CONN_STATS *p_conn_stats = (tBT_CONN_STATS *)pStats;
		CSppClient::SPP_CLIENT_RETURN_CODE ret
			= pObj->GetConnectionStats(p_conn_stats);
		return ret;
	}

}