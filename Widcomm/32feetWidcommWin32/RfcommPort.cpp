#include "stdafx.h"
#include "32feetWidcomm.h"

typedef void (CALLBACK* FnDataReceived) (void *p_data, UINT16 len);
typedef void (CALLBACK* FnEventReceived) (UINT32 event_code);


class RfcommPort : public CRfCommPort
{
public:
	FnDataReceived  pHandleDataReceived;
	FnEventReceived pHandleEventReceived;

private:
	virtual void OnDataReceived (void *p_data, UINT16 len)
	{
		//printf("OnDataReceived\n");
		pHandleDataReceived(p_data, len);
	}
	virtual void OnEventReceived (UINT32 event_code)
	{
		//printf("OnEventReceived: %x\n", event_code);  // e.g. PORT_EV_FC;
		pHandleEventReceived(event_code);
	}

	//--
	virtual void OnAudioConnected(UINT16 audioHandle)
	{
		printf("OnAudioConnected: 0x%x\n", audioHandle);
	}

	virtual void OnAudioDisconnect(UINT16 audioHandle)
	{
		printf("OnAudioDisconnect: 0x%x\n", audioHandle);
	}
};



//----------------------------------------------------------------------

extern "C" {
	MY32FEETWIDCOMM_API void RfcommPort_Create(RfcommPort** ppObj,
		FnDataReceived handleDataReceived, FnEventReceived handleEvent)
	{
		RfcommPort* p = new RfcommPort();
		//printf("RfcommPort_Create: %p\n", p);
		p->pHandleDataReceived = handleDataReceived;
		p->pHandleEventReceived = handleEvent;
		*ppObj = p;
	}

	MY32FEETWIDCOMM_API void RfcommPort_Destroy(void* pObj)
	{
		//printf("RfcommPort_Destroy: %p\n", pObj);
		RfcommPort* p = (RfcommPort* )pObj;
		delete p;
	}

	MY32FEETWIDCOMM_API /*PORT_RETURN_CODE*/int RfcommPort_OpenClient(void* pObj, BYTE scn, BYTE* pAddr)
	{
		//printf("RfcommPort_OpenClient: scn: %d, addr: %x-%x-%x-%x-%x-%x\n", scn,  
		//	pAddr[0], pAddr[1], pAddr[2], pAddr[3], pAddr[4], pAddr[5]);
		RfcommPort* p = (RfcommPort* )pObj;
		int ret = p->OpenClient(scn, pAddr);
		if (CRfCommPort::SUCCESS == (CRfCommPort::PORT_RETURN_CODE)ret) {
			AddLivePort(p);
		}
		return ret;
	}

	MY32FEETWIDCOMM_API /*PORT_RETURN_CODE*/int RfcommPort_OpenServer(void* pObj, BYTE scn)
	{
		//printf("RfcommPort_OpenServer: scn: %d\n", scn);
		RfcommPort* p = (RfcommPort* )pObj;
		int ret = p->OpenServer(scn);
		if (CRfCommPort::SUCCESS == (CRfCommPort::PORT_RETURN_CODE)ret) {
			AddLivePort(p);
		}
		return ret;
	}

	MY32FEETWIDCOMM_API /*PORT_RETURN_CODE*/int RfcommPort_Write(void* pObj, void *p_data, UINT16 len_to_write,UINT16 *p_len_written)
	{
		RfcommPort* p = (RfcommPort* )pObj;
		return p->Write(p_data, len_to_write, p_len_written);
	}

	MY32FEETWIDCOMM_API /*PORT_RETURN_CODE*/int RfcommPort_Close(void* pObj)
	{
		//printf("RfcommPort_Close ENTER\n");
		RfcommPort* p = (RfcommPort* )pObj;
		RemoveLivePort(p); // Assume if Close fails here, it'll fail at shutdown too.
		CRfCommPort::PORT_RETURN_CODE ret = p->Close();
		//printf("RfcommPort_Close EXIT\n");
		return ret;
	}

	MY32FEETWIDCOMM_API BOOL RfcommPort_IsConnected(void* pObj, UINT8* p_remote_bdaddr,
		int bufLen)
	{
		RfcommPort* p = (RfcommPort* )pObj;
		_ASSERT(p);
		_ASSERT(sizeof(BD_ADDR) == bufLen);
#ifndef _DEBUG
		bufLen;
#endif
		return p->IsConnected((BD_ADDR*)p_remote_bdaddr);
	}

	//---------------
	MY32FEETWIDCOMM_API /*AUDIO_RETURN_CODE*/int RfcommPort_CreateAudioConnection(
		void* pObj, BOOL bIsClient, UINT16 *audioHandle)
	{
		RfcommPort* p = (RfcommPort* )pObj;
		_ASSERT(p);
		AUDIO_RETURN_CODE ret = p->CreateAudioConnection(bIsClient, audioHandle);
		return ret;
	}

#ifndef WINCE
	MY32FEETWIDCOMM_API /*AUDIO_RETURN_CODE*/int RfcommPort_CreateAudioConnectionA(
		void* pObj, BOOL bIsClient, UINT16 *audioHandle, BD_ADDR bda)
	{
		RfcommPort* p = (RfcommPort* )pObj;
		_ASSERT(p);
		// added BTW 4.0.1.1400, SDK 4.0
		AUDIO_RETURN_CODE ret = p->CreateAudioConnection(bIsClient, audioHandle, bda);
		return ret;
	}
#endif

	//---------------
	static int const MaxLivePorts = 10;
	static RfcommPort * _livePorts[MaxLivePorts];

	void AddLivePort(RfcommPort *pPort)
	{
		for (int i = 0 ; i < MaxLivePorts; ++i) {
			if (_livePorts[i] == NULL) {
				_livePorts[i] = pPort;
				return;
			}
		}//for
		// Overflow, discard the new one or an (arbitrary) old one?
	}

	void RemoveLivePort(RfcommPort *pPort)
	{
		for (int i = 0 ; i < MaxLivePorts; ++i) {
			if (_livePorts[i] == pPort) {
				_livePorts[i] = NULL;
				return;
			}
		}//for
		// Not found!
	}

	void CloseAnyLivePorts()
	{
		int numDone = 0;
		for (int i = 0; i < MaxLivePorts; ++i) {
			if (_livePorts[i] != NULL) {
				RfcommPort** ppCur = &_livePorts[i];
#if defined( _M_X64)
				void **ppCurV = (void **)ppCur;
				void *pPortV = InterlockedExchangePointer(ppCurV, NULL);
#else
				void *pPortV = InterlockedExchangePointer(ppCur, NULL);
#endif
				RfcommPort* pPort = (RfcommPort*)pPortV;
				if (pPort != NULL) {
					CRfCommPort::PORT_RETURN_CODE ret = pPort->Close();
					if (ret != CRfCommPort::SUCCESS)
						wprintf(L"CloseAnyLivePorts: #%d, ret: %d=0x%x.\n", i, ret, ret);
					++numDone;
				}
				_ASSERT(_livePorts[i] == NULL);
			}
		}//for
		wprintf(L"CloseAnyLivePorts: was %d ports.\n", numDone);
	}
}
