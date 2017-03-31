// Functions shared within the project.

class RfcommPort; //forward reference

extern "C"{
	void AddLivePort(RfcommPort *pPort);
	void RemoveLivePort(RfcommPort *pPort);
	void CloseAnyLivePorts();
};
