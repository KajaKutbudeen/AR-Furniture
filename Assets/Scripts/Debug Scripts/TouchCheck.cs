using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchCheck : MonoBehaviour
{
    [Header("References")]
    private TouchInputAction _touchaction;
    public ARPlaneManager planeManager;
    private Camera _cam;
    public GameObject _PrefabSpawn;

    [Header("Addresables")]
    public AssetReferenceGameObject _prefab;


    [SerializeField]
    private string assetAddressToLoad = "Cube.prefab";


    private void Awake()
    {
        _touchaction = new TouchInputAction();
        _cam = Camera.main;
    }
    void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Prefab loaded and instantiated!");
        }
        else
        {
            Debug.LogError("Failed to load prefab!");
        }
    }

    private void OnAddressableLoaded(AsyncOperationHandle <GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded) 
        {
            Instantiate(handle.Result);
        }
        else
        {
            Debug.Log("failed to load");
        }
    }
    private void Start()
    {
        if (planeManager != null)
        {
            planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
        }
        Addressables.InstantiateAsync(assetAddressToLoad).Completed += OnPrefabLoaded;
    //    _prefab.LoadAssetAsync().Completed += OnAddressableLoaded;
    }

    private void OnEnable()
    {
        _touchaction.Enable();

        _touchaction.Player.Move.performed += OnTapPerform;
        _touchaction.Player.Move.canceled += OnTapPerformCancelled;
    }

    public void OnTapPerform(InputAction.CallbackContext Context)
    {

        Vector3 mousepos = Mouse.current.position.ReadValue();
        ARRaycastManager arRaycastManager = GetComponent<ARRaycastManager>(); // Or get it from somewhere else
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (arRaycastManager != null && arRaycastManager.Raycast(mousepos, hits, TrackableType.PlaneWithinPolygon))
        {         
            Pose hitPose = hits[0].pose;
            StartCoroutine(bundleloader("https://cdn.jsdelivr.net/gh/Robrert3490/Sample@main/3SM.fbx"
                  , "3SM.fbx", hitPose.position));
            Debug.Log("AR Plane Hit World Pos: " + hitPose.position);        
        }
        else
        {
            Debug.Log("No AR plane hit at mouse position.");
        }

    }
    public void OnTapPerformCancelled(InputAction.CallbackContext Context)
    {
        
    }

    private void OnDisable()
    {
        _touchaction.Disable();

        _touchaction.Player.Move.performed -= OnTapPerform;
        _touchaction.Player.Move.canceled -= OnTapPerformCancelled;
    }

    IEnumerator bundleloader(string url, string assetname, Vector3 pos)
    {

        UnityWebRequest web = UnityWebRequestAssetBundle.GetAssetBundle(url);
        //  StartCoroutine(WaitForResponse(web));
        yield return web.SendWebRequest();

        if (web.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Network Error");
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(web);
            if (bundle != null)
            {
                _PrefabSpawn = bundle.LoadAsset(assetname) as GameObject;
             //   _PrefabSpawn.transform.localScale = pos;
                GameObject obj = Instantiate(_PrefabSpawn, pos,Quaternion.identity);
                bundle.Unload(false);
           //     spawnedPrefab.AddComponent<LeanSelect>();
                //    spawnedPrefab.AddComponent<LeanDragTranslate>();
                //     spawnedPrefab.AddComponent<LeanPinchScale>();
                //      spawnedPrefab.AddComponent<LeanTwistRotateAxis>();
                //   string type = AssignTag();
                //    Instantiate(spawnedPrefab,transform.position,transform.rotation);
                //       Instantiate(_canvas,spawnedPrefab.transform);
                //    DisablepreFunctions(type);
               // Manager.GetComponent<Manager>().EnableSpawnManger();
              //  _PlaceAR.GetObject(spawnedPrefab);


            }
            else
            {
                Debug.LogError("Invalid Asset Bundle");
            }
        }

    }


}
