using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCController : MonoBehaviour {

	public string serverId = "PureData";
	public string serverIp = "127.0.0.2";
	public int serverPort = 5555;

	public KeyCode startKey = KeyCode.O;
	public string startMessage = "/start";
	public KeyCode writeKey = KeyCode.P;

	// Use this for initialization
	void Start () {
		OSCHandler.Instance.Init(this.serverId, this.serverIp, this.serverPort);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (this.startKey)) {
			RecordMidi ();
		}

		if (Input.GetKeyDown (this.writeKey)) {
			WriteMidi (1);
		}
	}

	public void RecordMidi(){
		OSCHandler.Instance.SendMessageToClient (this.serverId, this.startMessage, "record");
	}

	public void WriteMidi(int i){
		OSCHandler.Instance.SendMessageToClient (this.serverId, "/write", i);
	}
}
