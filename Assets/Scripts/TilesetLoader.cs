using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using Unity3DTiles;

public class TilesetLoader : MonoBehaviour
{
    public string baseurl = "";
    public string tilesetFileName = "";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Load");
    }

    private IEnumerator Load(){
        UnityWebRequest req = UnityWebRequest.Get(baseurl+tilesetFileName);
        yield return req.SendWebRequest();

        if(req.isHttpError || req.isNetworkError){
            Debug.Log(req.error);
            yield break;
        }
        var json = req.downloadHandler.text;

        var data = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;

        var obj = parseTileset(data["root"] as Dictionary<string, object>, 0,0,0, true);
    }

    private GameObject parseTileset(Dictionary<string,object> data, double x, double y, double z, bool root){
        var content = data["content"] as Dictionary<string,object>;
        var bV = content["boundingVolume"] as Dictionary<string, object>;
        var box = bV["box"] as List<object>;
        double bx = (double)box[0];
        double by = (double)box[1];
        double bz = (double)box[2];
        string url = (string)content["url"];

        GameObject obj = new GameObject(url);
        if(root){
            obj.transform.localPosition = new Vector3((float)x,(float)z,(float)y);
        (double b, double l, double h) = CoordConv.XYZ2BLH(bx,by,bz);
        Quaternion q = Quaternion.Euler((float)(-b),0f,0f) * Quaternion.Euler(0.0f,(float)-l,0.0f);
        obj.transform.localRotation = q;
        } else {
            obj.transform.localPosition = new Vector3((float)(bx - x),(float)(bz - z),(float)(by - y));
        }

        GameObject model = new GameObject("Model");
        model.transform.localEulerAngles = new Vector3(0f,180f,0f);
        model.transform.localScale = new Vector3(1f,1f,1f);
        model.transform.localPosition = Vector3.zero;
        model.transform.SetParent(obj.transform, false);
        var b3dmcom = model.AddComponent<B3DMComponent>();
        b3dmcom.Url = baseurl+url;
        b3dmcom.Multithreaded = false;
        b3dmcom.MaximumLod = 1;
        b3dmcom.ShaderOverride = Shader.Find("Standard");
        b3dmcom.AddColliders = false;
        b3dmcom.DownloadOnStart = false;
        StartCoroutine(b3dmcom.Download(null));

        if(data.ContainsKey("children")){
            var children = data["children"] as List<object>;
            foreach(Dictionary<string,object> child in children){
                var childObj = parseTileset(child, bx,by,bz,false);
                childObj.transform.SetParent(obj.transform, false);
            }
        }

        return obj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
