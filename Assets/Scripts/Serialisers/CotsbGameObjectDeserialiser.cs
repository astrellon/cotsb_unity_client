using UnityEngine;
using System.Collections;

public class CotsbGameObjectDeserialiser
{
    public static CotsbGameObject Deserialise()
    {
        var result = CotsbGameObjectManager.Create<CotsbGameObject>();
        return result;
    }
}
