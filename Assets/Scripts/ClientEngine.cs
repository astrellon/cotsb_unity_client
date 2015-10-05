using UnityEngine;
using System.Collections;

public class ClientEngine : MonoBehaviour 
{
    private Client client;

	// Use this for initialization
	void Start () 
    {
        client = new Client();	
        client.Connect("127.0.0.1", 8888);

        client.OnConnectedCallback = () =>
        {
            Debug.Log("Connected!");
        };
	}
	
	// Update is called once per frame
	void Update () 
    {
        client.CheckNetwork();	
	}
}
