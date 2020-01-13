#include "stdafx.h"
#include "32feetWidcomm.h"

typedef void (CALLBACK* FnDataReceived) (void *p_data, UINT16 len);
typedef void (CALLBACK* FnEventReceived2) (UINT32 event_code, UINT32 data);

        enum MyL2CapEvent
        {
            MyL2CapEvent_IncomingConnection,
            MyL2CapEvent_ConnectPendingReceived,
            MyL2CapEvent_Connected,
            MyL2CapEvent_CongestionStatus,
            MyL2CapEvent_RemoteDisconnected
        };


class L2CapConn : public CL2CapConn
{
public:
	FnDataReceived  pHandleDataReceived;
	FnEventReceived2 pHandleEventReceived;

private:
	virtual void OnDataReceived (void *p_data, UINT16 len)
	{
		//printf("OnDataReceived\n");
		pHandleDataReceived(p_data, len);
	}

	//----
	virtual void OnIncomingConnection ()
	{
		pHandleEventReceived(MyL2CapEvent_IncomingConnection, 0);
	}

	virtual void OnConnectPendingReceived (void)
	{
		pHandleEventReceived(MyL2CapEvent_ConnectPendingReceived, 0);
	}

	virtual void OnConnected()
	{
		pHandleEventReceived(MyL2CapEvent_Connected, 0);
	}

	virtual void OnCongestionStatus (BOOL is_congested)
	{
		pHandleEventReceived(MyL2CapEvent_CongestionStatus, is_congested);
	}

	virtual void OnRemoteDisconnected (UINT16 reason)
	{
		pHandleEventReceived(MyL2CapEvent_RemoteDisconnected, reason);
	}

};


//----------------------------------------------------------------------
extern "C" {
	MY32FEETWIDCOMM_API void L2CapConn_Create(L2CapConn** ppObj,
		FnDataReceived handleDataReceived, FnEventReceived2 handleEvent)
	{
		L2CapConn* p = new L2CapConn();
		//printf("L2CapConn_Create: %p\n", p);
		p->pHandleDataReceived = handleDataReceived;
		p->pHandleEventReceived = handleEvent;
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void L2CapConn_Destroy(void* pObj)
	{
		//printf("L2CapConn_Destroy: %p\n", pObj);
		L2CapConn* p = (L2CapConn* )pObj;
		delete p;
	}

	MY32FEETWIDCOMM_API BOOL L2CapConn_Connect(void* pObj, void* pIf, BYTE* pAddr, int mru)
	{
		mru = mru;
		//printf("L2CapConn_OpenClient: scn: %d, addr: %x-%x-%x-%x-%x-%x\n", scn,  
		//	pAddr[0], pAddr[1], pAddr[2], pAddr[3], pAddr[4], pAddr[5]);
		L2CapConn* p = (L2CapConn* )pObj;
		CL2CapIf* pIf2 = (CL2CapIf* )pIf;
		BOOL ret = p->Connect(pIf2, pAddr);
		//TODO if (ret) {
		//	AddLivePort(p);
		//}
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL L2CapConn_Listen(void* pObj, void* pIf)
	{
		//printf("L2CapConn_OpenServer: scn: %d\n", scn);
		L2CapConn* p = (L2CapConn* )pObj;
		CL2CapIf* pIf2 = (CL2CapIf* )pIf;
		BOOL ret = p->Listen(pIf2);
		//TODO if (ret) {
		//	AddLivePort(p);
		//}
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL L2CapConn_Accept(void* pObj, int mru)
	{
		mru = mru;
		L2CapConn* p = (L2CapConn* )pObj;
		BOOL ret = p->Accept();
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL L2CapConn_Write(void* pObj, void *p_data, UINT16 len_to_write,UINT16 *p_len_written)
	{
		L2CapConn* p = (L2CapConn* )pObj;
		BOOL success  = p->Write(p_data, len_to_write, p_len_written);
		return success;
	}

	MY32FEETWIDCOMM_API void L2CapConn_Disconnect(void* pObj)
	{
		//printf("L2CapConn_Close ENTER\n");
		L2CapConn* p = (L2CapConn* )pObj;
		//TODO RemoveLivePort(p); // Assume if Close fails here, it'll fail at shutdown too.
		p->Disconnect();
		//printf("L2CapConn_Close EXIT\n");
	}

	MY32FEETWIDCOMM_API void L2CapConn_GetRemoteBdAddr(void* pObj,
		UINT8* p_remote_bdaddr, int bufLen)
	{
		L2CapConn* p = (L2CapConn* )pObj;
		_ASSERT(p);
		_ASSERT(sizeof(BD_ADDR) == bufLen);
#if defined(WINCE)
		p->GetRemoteBdAddr(p_remote_bdaddr);
#else
		memcpy_s(p_remote_bdaddr, bufLen, p->m_RemoteBdAddr, sizeof(BD_ADDR));
#endif
	}

	MY32FEETWIDCOMM_API void L2CapConn_GetProperties(void* pObj,
		OUT BOOL *pIsCongested, OUT UINT16 *pMtu
		//, OUT UINT16 *pCid
		)
	{
		L2CapConn* p = (L2CapConn* )pObj;
#if defined(WINCE)
		*pIsCongested = p->GetCongested();
		*pMtu = p->GetRemoteMtu();
#else
		*pIsCongested = p->m_isCongested;
		*pMtu = p->m_RemoteMtu;
		int cid = p->GetCid();
		cid = cid;
#endif
	}

	//---------------



}