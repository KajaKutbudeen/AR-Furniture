using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteBundleLoader : MonoBehaviour
{
    // ✅ Replace with your actual bundle URL!
    string bundleURL = "https://cdn.jsdelivr.net/gh/Robrert3490/Sample@main/ServerData/Android/packedassets_assets_all_bfb8f241d68945348c75f523233cc57d.bundle";

    void Start()
    {
        StartCoroutine(DownloadAndLoad());
    }

    IEnumerator DownloadAndLoad()
    {
        Debug.Log("Starting download: " + bundleURL);

        using (UnityWebRequest uwr = UnityWebRequest.Get(bundleURL))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Download failed: " + uwr.error);
                yield break;
            }

            Debug.Log("✅ Bundle downloaded, size: " + uwr.downloadedBytes + " bytes");

            // Load from downloaded bytes
            AssetBundle bundle = AssetBundle.LoadFromMemory(uwr.downloadHandler.data);
            if (bundle == null)
            {
                Debug.LogError("❌ Failed to load AssetBundle from memory!");
                yield break;
            }

            Debug.Log("✅ Bundle loaded into memory!");

            // List everything in the bundle
            string[] assetNames = bundle.GetAllAssetNames();
            Debug.Log("Bundle contains:");
            foreach (string assetName in assetNames)
            {
                Debug.Log(" → " + assetName);
            }

            // Use the first asset as an example
            if (assetNames.Length > 0)
            {
                string assetToLoad = assetNames[0];
                Debug.Log("Trying to load: " + assetToLoad);

                GameObject prefab = bundle.LoadAsset<GameObject>(assetToLoad);
                if (prefab != null)
                {
                    Instantiate(prefab);
                    Debug.Log("✅ Prefab instantiated!");
                }
                else
                {
                    Debug.LogError("❌ Could not load prefab from bundle!");
                }
            }

            bundle.Unload(false);
        }
    }
}
