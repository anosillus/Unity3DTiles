using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity3DTiles;

public class B3DMLoadtest : MonoBehaviour
{
    public string url = "";
    // Start is called before the first frame update
    void Start()
    {
        var b3dmcom = gameObject.AddComponent<B3DMComponent>();
        b3dmcom.Url = url;
        b3dmcom.Multithreaded = false;
        b3dmcom.MaximumLod = 1;
        b3dmcom.ShaderOverride = Shader.Find("Standard");
        b3dmcom.AddColliders = false;
        b3dmcom.DownloadOnStart = false;
        StartCoroutine(b3dmcom.Download(null));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
