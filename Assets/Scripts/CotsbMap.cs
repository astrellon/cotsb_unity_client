using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CotsbMap 
{
    public readonly string Name;
    public uint Width { get; private set; }
    public uint Height { get; private set; }

    public string [,]Data { get; private set; }
    public List<CotsbGameObject> GameObjects = new List<CotsbGameObject>();

    public enum Status
    {
        MapNotFound,
        NotLoading,
        Loading,
        Error,
        Loaded
    };
    public Status LoadStatus = Status.NotLoading;

    public CotsbMap(string name)
    {
        Name = name;
    }

    public void SetSize(uint width, uint height)
    {
        if (width == 0u || height == 0u)
        {
            throw new System.Exception("Cannot resize a map to zero width or height.");
        }
        if (Width != 0u && Height != 0u)
        {
            throw new System.Exception("Map already set, cannot resize again.");
        }

        Width = width;
        Height = height;

        Data = new string[Width, Height];
    }

}
