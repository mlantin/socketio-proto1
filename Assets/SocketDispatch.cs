using UnityEngine;
using System.Collections;
using Quobject.SocketIoClientDotNet.Client;

public class SocketDispatch : MonoBehaviour {

	public string url = "http://127.0.0.1:4567";
	public GameObject testobj = null;
	Vector3 newpos = new Vector3();
	Quaternion newrot = new Quaternion();
	private Socket socket;

	// Use this for initialization
	void Start () {
		socket = IO.Socket (url);
		socket.On(Socket.EVENT_CONNECT, () =>
			{
				Debug.Log("Connected!");

			});
		socket.On (Socket.EVENT_ERROR, () => {
			Debug.Log ("Event Error!");
		});

		socket.On (Socket.EVENT_DISCONNECT, () => {
			Debug.Log ("Disconnecting!");
		});
		socket.On (Socket.EVENT_RECONNECTING, () => {
			Debug.Log ("Reconnecting!");
		});
		socket.On ("hydra", (data) => {
			Google.Protobuf.VRCom.Hydra msg = Google.Protobuf.VRCom.Hydra.Parser.ParseFrom((System.Byte[])data);
			if (msg.CtrlNum == 0)
				newrot.Set(msg.Rot.X, msg.Rot.Y, msg.Rot.Z, msg.Rot.Z);

		});
		socket.On ("mocap", handleMocap);
	}

	// Update is called once per frame
	void Update () {
		testobj.transform.position = newpos;
	}

	void handleMocap(System.Object data) {
		Google.Protobuf.VRCom.Mocap msg = Google.Protobuf.VRCom.Mocap.Parser.ParseFrom((System.Byte[])data);
		newpos.Set(msg.Pos.X, msg.Pos.Y, msg.Pos.Z);
	}

	void OnApplicationQuit() {
		Debug.Log ("quitting");
		socket.Close ();
	}
}
