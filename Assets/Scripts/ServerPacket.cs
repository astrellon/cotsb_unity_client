using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class ServerPacket 
{
    public byte [] Data { get; private set; }
    private int index = 0;

    [StructLayout(LayoutKind.Explicit)]
    private struct Number32Union
    {
        [FieldOffset(0)] public int Int32;
        [FieldOffset(0)] public uint Uint32;
        [FieldOffset(0)] public float Float;
    }
    private static Number32Union NumberConverter = new Number32Union();

    public ServerPacket(byte []data)
    {
        this.Data = data;
    }

    public int Length
    {
        get { return Data.Length; }
    }
    public bool HasData()
    {
        return index < Data.Length;
    }

    public byte ReadByte()
    {
        return Data[index++];
    }
    public int ReadInt32()
    {
        int result = Data[index] << 24 | 
            Data[index + 1] << 16 |
            Data[index + 2] << 8 |
            Data[index + 3];
        index += 4;

        return result;
    }
    public uint ReadUint32()
    {
        NumberConverter.Int32 = ReadInt32();
        return NumberConverter.Uint32;
    }
    public float ReadFloat()
    {
        NumberConverter.Int32 = ReadInt32();
        return NumberConverter.Float;
    }
    public string ReadString()
    {
        var length = ReadInt32();

        var start = index;
        index += length;
        return System.Text.Encoding.UTF8.GetString(Data, start, length);
    }
    public bool ReadBool()
    {
        return Data[index++] > 0;
    }
}
