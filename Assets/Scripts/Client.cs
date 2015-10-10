using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class Client 
{
    public TcpClient ClientSocket { get; private set; }
    private NetworkStream stream;
    private byte[] intBuffer = new byte[4];

    public readonly List<ServerPacket> IncomingData = new List<ServerPacket>();

    public delegate void ConnectedCallback();

    public ConnectedCallback OnConnectedCallback;

    public enum ConnectedState
    {
        NotConnected,
        Connecting,
        PreConnected,
        Error,
        Connected
    }

    public ConnectedState State { get; private set; }

    private List<ClientPacket> packetsToSend = new List<ClientPacket>();

    public Client()
    {
        State = ConnectedState.NotConnected;
    }

    public void Connect(string server, int port)
    {
        State = ConnectedState.Connecting;

        ClientSocket = new TcpClient();
        ClientSocket.Connect(server, port);
        State = ConnectedState.Connecting;
    }

    public void CheckNetwork()
    {
        if (ClientSocket.Connected && State == ConnectedState.Connecting)
        {
            State = ConnectedState.PreConnected;
        }

        if (State == ConnectedState.NotConnected || State == ConnectedState.Connecting)
        {
            return;
        }

        if (State == ConnectedState.PreConnected)
        {
            stream = ClientSocket.GetStream();
            State = ConnectedState.Connected;
            if (OnConnectedCallback != null)
            {
                OnConnectedCallback();
            }
        }

        if (State != ConnectedState.Connected || stream == null || !stream.DataAvailable)
        {
            return;
        }

        ProcessIncoming();
        ProcessOutgoing();
    }

    private void ProcessIncoming()
    {
        while (stream.DataAvailable)
        {
            stream.Read(intBuffer, 0, 4);
            var length = intBuffer[0] << 24 |
                intBuffer[1] << 16 |
                intBuffer[2] << 8 |
                intBuffer[3];

            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            var packet = new ServerPacket(buffer);
        
            IncomingData.Add(packet);
        }
    }
    private void ProcessOutgoing()
    {
        foreach (var packet in packetsToSend)
        {
            var l = packet.Data.Count;
            intBuffer[0] = (byte)(l >> 24);
            intBuffer[1] = (byte)(l >> 16);
            intBuffer[2] = (byte)(l >> 8);
            intBuffer[3] = (byte)l;

            stream.Write(intBuffer, 0, 4);
            stream.Write(packet.Data.ToArray(), 0, l);
        }
        packetsToSend.Clear();
    }

    public ClientPacket Send(Commands.Type command)
    {
        var result = new ClientPacket();
        result.WriteByte((byte)command);
        packetsToSend.Add(result);
        return result;
    }
}
