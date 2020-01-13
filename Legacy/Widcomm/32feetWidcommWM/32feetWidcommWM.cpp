// 32feetWidcommWM.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "32feetWidcommWM.h"
#include <windows.h>
#include <commctrl.h>

BOOL APIENTRY DllMain( HANDLE /*hModule*/, 
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

//// This is an example of an exported variable
//MY32FEETWIDCOMMWM_API int nMy32feetWidcommWM=0;
//
//// This is an example of an exported function.
//MY32FEETWIDCOMMWM_API int fnMy32feetWidcommWM(void)
//{
//	return 42;
//}
//
//// This is the constructor of a class that has been exported.
//// see 32feetWidcommWM.h for the class definition
//CMy32feetWidcommWM::CMy32feetWidcommWM()
//{ 
//	return; 
//}
