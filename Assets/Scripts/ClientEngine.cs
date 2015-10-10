using UnityEngine;
using System.Collections;

public class ClientEngine : MonoBehaviour 
{
    private Client client;
    public string Host = "127.0.0.1";
    public int Port = 8888;

	// Use this for initialization
	void Start () 
    {
        client = new Client();	
        client.Connect(Host, Port);

        client.OnConnectedCallback = () =>
        {
            Debug.Log("Connected!");
            
            var packet = client.Send(Commands.Type.JoinGame);
            packet.WriteString("Unity");
        };
	}
	
	// Update is called once per frame
	void Update () 
    {
        client.CheckNetwork();	

        foreach (var data in client.IncomingData)
        {
            var command = (Commands.Type)data.ReadByte();
            if (command == Commands.Type.Message)
            {
                var messageType = data.ReadByte();
                var message = data.ReadString();
                Debug.Log("Message: " + messageType + ": " + message);
            }
            else if (command == Commands.Type.JoinedGame)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    Debug.Log(i + ": " + data.Data[i]);
                }
            }
        }
        client.IncomingData.Clear();
	}
}
