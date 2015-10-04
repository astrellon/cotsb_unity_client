using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public class ClientPacket 
{
    public readonly List<byte> Data = new List<byte>();

    [StructLayout(LayoutKind.Explicit)]
    private struct Number32Union
    {
        [FieldOffset(0)] public int Int32;
        [FieldOffset(0)] public uint Uint32;
        [FieldOffset(0)] public float Float;
    }
    private static Number32Union NumberConverter = new Number32Union();

    public ClientPacket()
    {

    }

    public void WriteByte(byte value)
    {
        Data.Add(value);
    }
    public void WriteInt32(int value)
    {
        NumberConverter.Int32 = value;
        WriteConverter();
    }
    public void WriteUint32(uint value)
    {
        NumberConverter.Uint32 = value;
        WriteConverter();
    }
    public void WriteFloat(float value)
    {
        NumberConverter.Float = value;
        WriteConverter();
    }
    public void WriteString(string message)
    {
        NumberConverter.Int32 = message.Length;
        WriteConverter();

        Data.AddRange(System.Text.Encoding.UTF8.GetBytes(message));
    }

    private void WriteConverter()
    {
        var v = NumberConverter.Int32;
        Data.Add((byte)(v >> 24));
        Data.Add((byte)(v >> 16));
        Data.Add((byte)(v >> 8));
        Data.Add((byte)v);
    }
}
