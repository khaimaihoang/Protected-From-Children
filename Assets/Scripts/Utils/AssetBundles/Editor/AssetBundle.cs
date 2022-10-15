using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundle : Editor
{
    [MenuItem("AssetBunles/Create AssetBundles")]
    static void CreateAssetBundle()
    {
        BuildPipeline.BuildAssetBundles("C:\\Users\\gihot\\Desktop\\AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }
}
