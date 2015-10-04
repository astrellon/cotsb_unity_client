using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class Client 
{
    public TcpClient ClientSocket { get; private set; }
    private NetworkStream stream;
    private byte[] intBuffer = new byte[4];

    private List<ClientPacket> packetsToSend = new List<ClientPacket>();

    public Client()
    {

    }

    public void Connect(string server, int port)
    {
        ClientSocket = new TcpClient(server, port);
        stream = ClientSocket.GetStream();

        var packet = Send(Commands.Type.Message);
        packet.WriteByte(0x02);
        packet.WriteString("Hi from Unity!");
    }

    public void CheckNetwork()
    {
        if (stream == null || !stream.DataAvailable)
        {
            return;
        }

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
        
            var command = packet.ReadByte();
            switch (command)
            {
                case 0x01:
                    {
                        var messageType = packet.ReadByte();
                        var message = packet.ReadString();
                        Debug.Log("Message: " + messageType + ": " + message);
                    }
                    break;
            }
        }

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
