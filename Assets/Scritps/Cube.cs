using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{


    bool _isSelect = false;
    //public Action<GameObject, int, int, int> OnSetCubeIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }





    //void ComputeIndex()
    //{
    //    OnSetCubeIndex?.Invoke(gameObject, (int)transform.localPosition.x, (int)transform.localPosition.y, (int)transform.localPosition.z);
    //}



    private void OnMouseDown()
    {
        _isSelect = true;
        Debug.Log(name);
    }
    private void OnMouseUp()
    {
        if (!_isSelect) return;
        _isSelect = false;
        Debug.Log("Up");
    }


    private void OnMouseExit()
    {
        if (!_isSelect) return;
        _isSelect = false;
        Debug.Log("Exit");
    }
}
