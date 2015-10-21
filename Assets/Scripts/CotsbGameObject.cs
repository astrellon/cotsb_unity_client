using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CotsbGameObject
{
    public uint Id;    
    public CotsbMap CurrentMap;
    public Color Colour;
    public float Size;
}

public class CotsbGameObjectManager
{
    private static readonly Dictionary<uint, CotsbGameObject> GameObjects = new Dictionary<uint, CotsbGameObject>();

    public static CotsbGameObject GameObject(uint id)
    {
        CotsbGameObject obj;
        if (GameObjects.TryGetValue(id, out obj))
        {
            return obj;
        }
        return null;
    }

    public static T Create<T>(uint id) where T : CotsbGameObject, new()
    {
        var obj = new T();
        obj.Id = id;

        GameObjects[id] = obj;

        return obj;
    }
}
