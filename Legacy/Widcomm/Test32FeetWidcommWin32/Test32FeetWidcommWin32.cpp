// Test32FeetWidcommWin32.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


//----------------------------------------------------------------------
//typedef void (CALLBACK* FnDeviceResponded)(BD_ADDR bdAddr, DEV_CLASS devClass, BD_NAME deviceName, BOOL connected);
//typedef void (CALLBACK* FnInquiryComplete)(BOOL success, UINT16 numResponses);
//typedef void (CALLBACK* FnDiscoveryComplete)();
//
//MY32FEETWIDCOMM_API void BtIf_Create(BtIf** ppObj, FnDeviceResponded deviceResponded, 
//		FnInquiryComplete inquiryCompleted, FnDiscoveryComplete discoveryComplete);
//
typedef void (*FAKE_BtIf_Create)(void** ppObj, void* deviceResponded, 
	void* inquiryCompleted, void* discoveryComplete);

//
//internal static extern bool BtIf_GetLocalDeviceInfoBdAddr(IntPtr m_pBtIf, 
//	[Out]byte[] bdAddr, int cb);
typedef int (*FAKE_BtIf_GetLocalDeviceInfoBdAddr)(void* pBtIf, BYTE* bdAddr, int cb);

//----------------------------------------------------------------------

void PrintModuleFilename(HMODULE hModule)
{
	wchar_t buf[MAX_PATH];
	DWORD ret = GetModuleFileName(hModule, buf, sizeof(buf));
	if (ret == 0) {
		_putws(L"(Don't know module filename.)");
	} else {
		buf[MAX_PATH-1] = 0;
		wprintf(L"DLL found at >>%s<<\n", buf);
	}
}

int use(HINSTANCE hDll)
{
#define ROOT_FUNCTION_NAME "BtIf_Create"
	void* pfn0 = GetProcAddress(hDll, ROOT_FUNCTION_NAME);
	if (!pfn0){
		puts("Didn't find init function! (" ROOT_FUNCTION_NAME ")");
		return 11;
	}
	_putws(L"Calling init function...");
	FAKE_BtIf_Create pfnInit = (FAKE_BtIf_Create)pfn0;
	void *pObj;
	pfnInit(&pObj, NULL, NULL, NULL);
	if (pObj){
		_putws(L"   Success");
	} else {
		puts("   Didn't initialise 32feetWidcomm interface!");
		return 12;
	}
	//
#define RADIO_ADDRESS_FUNCTION_NAME "BtIf_GetLocalDeviceInfoBdAddr"
	pfn0 = GetProcAddress(hDll, RADIO_ADDRESS_FUNCTION_NAME);
	if (!pfn0){
		puts("Didn't find radio address function! (" RADIO_ADDRESS_FUNCTION_NAME ")");
		return 13;
	}
	_putws(L"Calling radio address function...");
	FAKE_BtIf_GetLocalDeviceInfoBdAddr pfnLocalAddr = (FAKE_BtIf_GetLocalDeviceInfoBdAddr)pfn0;
	BYTE addr[6];
	int ret = pfnLocalAddr(pObj, addr, sizeof(addr));
	if (ret){
		wprintf(L"   Success, local address is %02x-%02x-%02x-%02x-%02x-%02x\n",
			addr[0], addr[1], addr[2], addr[3], addr[4], addr[5]);
	} else {
		puts("   Didn't get radio address!");
		return 14;
	}
	//
	return 0;
}

int check(void)
{
#define ROOT_32FEET_DLL L"32feetWidcomm.dll"
	//
	// Is DLL file present (ignoring dependencies)?
	HINSTANCE hDll0 = LoadLibraryEx(ROOT_32FEET_DLL, NULL, LOAD_LIBRARY_AS_DATAFILE);
	if (!hDll0){
		_putws(L"Didn't find " ROOT_32FEET_DLL L" file!");
		//return 1;
	} else {
		_putws(L"Did find " ROOT_32FEET_DLL L" file. :-)");
	}
	//
	// Is DLL file and its dependencies present?  Though since we use delayload for
	// the Widcomm dependency now on Win32 it won't fail until we call a function.
	HINSTANCE hDll = LoadLibrary(ROOT_32FEET_DLL);
	if (!hDll){
		_putws(L"Didn't find " ROOT_32FEET_DLL L" DLL!"
			L"  A dialog box should have reported which dependency is missing, "
			L"otherwise see the event log.");
		return 2;
	} else {
		_putws(L"Did find " ROOT_32FEET_DLL L" DLL. :-)");
	}
	//
	PrintModuleFilename(hDll);
	//
	return use(hDll);
}

int _tmain(int argc, _TCHAR* argv[])
{
	argc, argv; // unused
	_putws(L"32feet.NET using Widcomm stack dependency checker tool.");
	_putws(L"We will attempt to load and use the 32feetWidcomm support.");
	_putws(L"");
	int ret = check();
	//
	wprintf(L"Hit Return to continue>");
	wchar_t tmp[100];
	_getws_s(tmp);
	return ret;
}

