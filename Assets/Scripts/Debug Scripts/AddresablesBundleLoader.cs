using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using CW.Common;
using Lean.Touch;
public class AddresablesBundleLoader : MonoBehaviour
{
    [Header("Debug")]
    public GameObject _prefab;
    Vector2 _objectpos = Vector2.zero;

    [Header("Raycast")]
    public ARRaycastManager _ARraycastManager;
    private List<ARRaycastHit> _rayHits = new List<ARRaycastHit>();
    public LayerMask objectmask;
    private GameObject temp;

    public string prefabKey = "Cube";
    [Header("Inputactions")]
    private InputActionSettings _InputAction;
    private InputAction _TouchPress;
    private InputAction _TouchPosition;

    [Header("Mouse Actions")]
    private InputAction _MousePress;
    private InputAction _MousePosition;

    public List<string> FurnitureList;

    private void Awake()
    {
        _InputAction = new InputActionSettings();
        _TouchPress = _InputAction.Touch.TouchPressed;
        _TouchPosition = _InputAction.Touch.TouchPosition;
        _MousePress = _InputAction.Mouse.Press;
        _MousePosition = _InputAction.Mouse.Position;
    }
    private void OnEnable()
    {     
        _InputAction.Enable();    
        _TouchPress.Enable();
        _TouchPosition.Enable();
        _MousePress.Enable();
        _MousePosition.Enable();
        _TouchPress.performed += TouchPress;
        _TouchPosition.performed += TouchPos;

        _MousePress.performed += MousePress;
        _MousePosition.performed += MousePosition;
    }

    private void MousePress(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse Touched");
    }
    private void MousePosition(InputAction.CallbackContext context)
    {
        Vector2 mousepos = context.ReadValue<Vector2>();
        CreateObject(mousepos);
        SelectObject(mousepos);
    }

    private void CreateObject(Vector2 pos)
    {
        if (_ARraycastManager.Raycast(pos, _rayHits, TrackableType.PlaneWithinPolygon))
        {
            var hitpose = _rayHits[0].pose;
            _objectpos = hitpose.position;
            Addressables.LoadAssetAsync<GameObject>(prefabKey).Completed += OnPrefabLoaded;
        }
    }
    Ray ray;
    RaycastHit hits = new RaycastHit();
    
    private void SelectObject(Vector2 pos)
    {
       ray = Camera.main.ScreenPointToRay(pos);     
        if(Physics.Raycast(ray, out hits,Mathf.Infinity,objectmask))
        {
            if(hits.collider.gameObject != null)
            {
                if(temp == null)
                {
                    temp = hits.collider.gameObject;
                }
                if(temp == hits.collider.gameObject)
                {
                    hits.collider.gameObject.GetComponent<LeanSelectableByFinger>().SelfSelected = true;
                }
                else
                {
                    temp.GetComponent<LeanSelectableByFinger>().SelfSelected = false;
                    temp = hits.collider.gameObject;
                    hits.collider.gameObject.GetComponent<LeanSelectableByFinger>().SelfSelected = true;
                }
             
                
            }
        }
    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        Debug.Log("Touched");
        
       
    }

    private void TouchPos(InputAction.CallbackContext context)
    {
        Vector2 vectorvalue = context.ReadValue<Vector2>();
        Debug.Log("Vector value:" + vectorvalue);
        if (_ARraycastManager.Raycast(vectorvalue,_rayHits,TrackableType.PlaneWithinPolygon))
        {
            var hitpose = _rayHits[0];
            Debug.Log("Hitpose: " +hitpose.ToString());
        }


    }

    
    void Start()
    {      
       // Addressables.InstantiateAsync(prefabKey).Completed += OnPrefabLoaded;
     //   Addressables.LoadAssetAsync<GameObject>(prefabKey).Completed += OnPrefabLoaded;
    }
    void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {           
           _prefab = handle.Result;
            GameObject go = Instantiate(_prefab);
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
        _TouchPosition.Disable();
        _MousePress.Disable();
        _MousePosition.Disable();
        _TouchPress.performed -= TouchPress;
        _TouchPosition.performed -= TouchPos;
        _MousePress.performed -= MousePress;
        _MousePosition.performed -= MousePosition;
    }
}
