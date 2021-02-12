using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLog3D : MonoBehaviour
{
    public Texture skinLog;
    public Material logMaterial;

    private void Start()
    {
        if ((skinLog != null) && (logMaterial != null))
        {
            logMaterial.SetTexture("_MainTex", skinLog);
        }
    }
}
