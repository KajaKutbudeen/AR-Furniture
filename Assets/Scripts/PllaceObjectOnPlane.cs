using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject _Prefab;


    

    public void TaptoPlace()
    {
        GameObject obj = Instantiate(_Prefab, Vector3.zero,Quaternion.identity);
        obj.name = "Obj";
    }
}
