using UnityEngine;
using System.Collections;
using Quobject.SocketIoClientDotNet.Client;

public class SocketDispatch : MonoBehaviour {

	public string url = "http://127.0.0.1:4567";
	public GameObject testobj = null;
	Vector3 newpos = new Vector3();

	private Socket socket;

	// Use this for initialization
	void Start () {
		socket = IO.Socket (url);
		socket.On(Socket.EVENT_CONNECT, () =>
			{
				Debug.Log("Connected!");

			});
		socket.On ("mocap", (data) => {
			Google.Protobuf.VRCom.Mocap msg = Google.Protobuf.VRCom.Mocap.Parser.ParseFrom((System.Byte[])data);
			newpos.Set(msg.Pos.X/100, msg.Pos.Y/100, msg.Pos.Z/100);

		});
	}

	// Update is called once per frame
	void Update () {
		testobj.transform.position = newpos;
	}

	void OnApplicationQuit() {
		socket.Close ();
	}
}
