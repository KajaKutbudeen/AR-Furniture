using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem;
using System.IO;
public class AddresablesBundleLoader : MonoBehaviour
{
    public string prefabKey = "Cube";
    private InputActionSettings _InputAction;
    private InputAction _TouchPress;
    public List<string> FurnitureList;

    private void OnEnable()
    {
        _InputAction.Enable();
       _TouchPress = _InputAction.Touch.TouchPressed;

        _TouchPress.Enable();
        _TouchPress.performed += TouchPress;
    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        Debug.Log("Touched");
    }
    void Start()
    {      
      //  Addressables.InstantiateAsync(prefabKey).Completed += OnPrefabLoaded;
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

    private void OnDisable()
    {
        _InputAction.Disable();
        _TouchPress.Disable();
        _TouchPress.performed -= TouchPress;
    }
}
