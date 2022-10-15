using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using System;

public class LoadAssetBundleFromURL : MonoBehaviour
{

    public string url = "";
    public string assetName = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onLoadAssetBundleFromURL(){
        // WWW www = new WWW(url);

        StartCoroutine(WebReq(url));
    }

    IEnumerator WebReq(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url)){
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success){
                Debug.Log(uwr.error);
            }
            else{
                // AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                DownloadHandler handler = uwr.downloadHandler;
                string savePath = Path.Combine(Application.persistentDataPath, "AssetData");
                savePath = Path.Combine(savePath, "qs1");
                saveAssetBundle(handler.data, savePath);
                //AssetBundle bundle = AssetBundle.LoadFromFile(savePath);

                //Debug.Log("Load asset bundle from file: " + bundle.name);

                //var assets = bundle.LoadAllAssets();
                //Debug.Log(assets);
            }
        }
    }

    void saveAssetBundle(byte[] data, string path){
        if (!Directory.Exists(Path.GetDirectoryName(path))){
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try{
            File.WriteAllBytes(path, data);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e){
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
