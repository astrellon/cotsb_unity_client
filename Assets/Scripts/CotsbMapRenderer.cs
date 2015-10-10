using UnityEngine;
using System.Collections;

public class CotsbMapRenderer : MonoBehaviour 
{
    public CotsbMap Map;
    public bool Rendered = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!Rendered && Map != null && Map.LoadStatus == CotsbMap.Status.Loaded)	
        {
            RenderMap();
        }

        if (Rendered)
        {
            // Do stuff
        }
	}

    void RenderMap()
    {
        Rendered = true;
    }
}
