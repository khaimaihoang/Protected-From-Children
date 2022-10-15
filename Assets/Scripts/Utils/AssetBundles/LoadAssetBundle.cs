using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    private string savePath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAssetBundleFromFile()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(savePath);

        Debug.Log("Load asset bundle from file: " + bundle.name);

        var assets = bundle.LoadAllAssets();
        Debug.Log(assets);
    }
}
