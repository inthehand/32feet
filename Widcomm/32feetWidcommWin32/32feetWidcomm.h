#include "..\\banned.h"

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the MY32FEETWIDCOMM_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// MY32FEETWIDCOMM_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef MY32FEETWIDCOMM_EXPORTS
#define MY32FEETWIDCOMM_API __declspec(dllexport)
#else
#define MY32FEETWIDCOMM_API __declspec(dllimport)
#endif

//// This class is exported from the 32feetWidcomm.dll
//class MY32FEETWIDCOMM_API CMy32feetWidcomm {
//public:
//	CMy32feetWidcomm(void);
//	// TODO: add your methods here.
//};
//
//extern MY32FEETWIDCOMM_API int nMy32feetWidcomm;
//
//MY32FEETWIDCOMM_API int fnMy32feetWidcomm(void);

extern "C" {
	void WINAPI ForceGetPdl(CSdpDiscoveryRec *pList, int count);
}