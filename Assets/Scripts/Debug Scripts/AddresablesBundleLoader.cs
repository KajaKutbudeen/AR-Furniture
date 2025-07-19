using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem;
public class AddresablesBundleLoader : MonoBehaviour
{
    public string prefabKey = "Cube";

    


    void Start()
    {
        Addressables.InstantiateAsync(prefabKey).Completed += OnPrefabLoaded;
    }





    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($" Prefab '{prefabKey}' loaded & instantiated!");
        }
        else
        {
            Debug.LogError($" Failed to load prefab '{prefabKey}'!");
        }
    }
}
