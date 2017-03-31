// 32feetWidcommWin32.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "32feetWidcomm.h"


#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE /*hModule*/,
                       DWORD  ul_reason_for_call,
                       LPVOID /*lpReserved*/
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		CloseAnyLivePorts();
		break;
	}
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

//// This is an example of an exported variable
//MY32FEETWIDCOMM_API int nMy32feetWidcommWin32=0;
//
//// This is an example of an exported function.
//MY32FEETWIDCOMM_API int fnMy32feetWidcommWin32(void)
//{
//	return 42;
//}
//
//// This is the constructor of a class that has been exported.
//// see 32feetWidcommWin32.h for the class definition
//CMy32feetWidcommWin32::CMy32feetWidcommWin32()
//{
//	return;
//}
