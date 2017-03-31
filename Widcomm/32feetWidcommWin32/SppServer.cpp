// 32feet.NET - Personal Area Networking for .NET
//
// Widcomm SppServer
// 
// Copyright (c) 2009-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2009-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#include "stdafx.h"
#include "32feetWidcomm.h"


typedef void (CALLBACK* FnServerStateChange)(BD_ADDR bda, DEV_CLASS dev_class,
	BD_NAME name, short com_port, SPP_STATE_CODE state);


class SppServer : public CSppServer
{
public:
	FnServerStateChange  pHandleServerStateChange;

private:

	virtual void OnServerStateChange(BD_ADDR bda, DEV_CLASS dev_class,
		BD_NAME name, short com_port, SPP_STATE_CODE state)
	{
		//printf("SppServer::OnServerStateChange\n");
		pHandleServerStateChange(bda, dev_class, name, com_port, state);
	}

};

//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void SppServer_Create(SppServer** ppObj,
		FnServerStateChange serverStateChange)
	{
		//printf("SppServer_Create\n");
		SppServer* pObj = new SppServer();
		pObj->pHandleServerStateChange = serverStateChange;
		*ppObj = pObj;
	}

	MY32FEETWIDCOMM_API void SppServer_Destroy(SppServer* pObj)
	{
		printf("SppServer_Destroy\n");
		delete pObj;
	}

	//----------
	MY32FEETWIDCOMM_API int SppServer_CreateConnection(SppServer* pObj,
		BT_CHAR* arrServiceName) // NB Platform dependent!!!
	{
		//printf("SppServer_CreateConnection\n");
		int ret = pObj->CreateConnection(arrServiceName);
		return ret;
	}

	MY32FEETWIDCOMM_API int SppServer_RemoveConnection(SppServer* pObj)
	{
		int ret = pObj->RemoveConnection();
		return ret;
	}

	MY32FEETWIDCOMM_API int SppServer_GetConnectionStats(SppServer* pObj,
		void* pStats, int cb)
	{
		//printf("SppServer_GetConnectionStats\n");
		_ASSERT(pObj);
		_ASSERT(pStats);
		_ASSERT(sizeof(tBT_CONN_STATS) == cb);
		if (sizeof(tBT_CONN_STATS) != cb)
			return false;
		tBT_CONN_STATS *p_conn_stats = (tBT_CONN_STATS *)pStats;
		CSppServer::SPP_SERVER_RETURN_CODE ret
			= pObj->GetConnectionStats(p_conn_stats);
		return ret;
	}

}