using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro.EditorUtilities;
using System;

public class TweakCube : MonoBehaviour
{
    /// <summary>
    /// 魔方被还原
    /// </summary>
    public Action OnRestore;
    GameObject[,,] _cubes = new GameObject[3, 3, 3];
    List<GameObject> _list = new List<GameObject>();
    [SerializeField]
    Material _material;
    Tween _tween;
    /// <summary>
    /// 是否正在旋转中
    /// </summary>
    public bool IsRotating
    {
        get
        {
            if (_tween == null) return false;
            return _tween.IsPlaying();
        }
    }

    float _doTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        CreateCube();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(0, null, null, true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Rotate(null, 2, null, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(null, null, 0, true);
        }

    }



    /// <summary>
    /// 创建魔方方块
    /// </summary>
    public void CreateCube()
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var script = cube.AddComponent<Cube>();
                    cube.transform.parent = transform;
                    cube.transform.localPosition = new Vector3(i - 1, j - 1, k - 1);
                    cube.name = string.Format("{0}_{1}_{2}", i, j, k);
                    cube.GetComponent<MeshRenderer>().material = _material;
                    _cubes[i, j, k] = cube;
                    _list.Add(cube);
                }
            }
        }
    }

    /// <summary>
    /// 重新计算魔方坐标值
    /// </summary>
    public void SetCubeIndex()
    {
        foreach (var item in _list)
        {
            Transform t = item.transform;
            t.SetParent(transform);
            int x = (int)(t.position.x + 1.5f);// + 1;
            int y = (int)(t.position.y + 1.5f);// + 1;
            int z = (int)(t.position.z + 1.5f);// + 1;
            _cubes[x, y, z] = t.gameObject;
            t.name = string.Format("{0}_{1}_{2}", x, y, z);
        }
        Jude();
    }

    /// <summary>
    /// 判断魔方是否还原
    /// 所有魔方朝向相同即为还原
    /// </summary>
    void Jude()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (_cubes[i, j, k].transform.eulerAngles != _cubes[0, 0, 0].transform.eulerAngles)
                    {
                        return;
                    }
                }
            }
        }
        //Debug.Log("完成！");
        OnRestore?.Invoke();
    }

    /// <summary>
    /// 旋转魔方
    /// </summary>
    /// <param name="x">非空时，表示已当前轴作为旋转中心，数字表示旋转第几排</param>
    /// <param name="y">非空时，表示已当前轴作为旋转中心，数字表示旋转第几排</param>
    /// <param name="z">非空时，表示已当前轴作为旋转中心，数字表示旋转第几排</param>
    /// <param name="dir">旋转方向，正向还是负向</param>
    public void Rotate(int? x, int? y, int? z, bool dir)
    {
        if (_tween != null && _tween.IsPlaying()) return;
        GameObject center = new GameObject();
        center.transform.parent = transform;
        Vector3 rotateDir = Vector3.zero;
        if (x != null)
        {
            int v = (int)x;
            rotateDir = Vector3.left;
            center.transform.localPosition = new Vector3(v, 0, 0);
            center.name = "X:" + v;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _cubes[v, i, j].transform.parent = center.transform;
                }
            }
        }
        else if (y != null)
        {
            int v = (int)y;
            rotateDir = Vector3.up;
            center.transform.localPosition = new Vector3(0, v, 0);
            center.name = "Y:" + v;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _cubes[i, v, j].transform.parent = center.transform;
                }
            }
        }
        else if (z != null)
        {
            int v = (int)z;
            rotateDir = Vector3.back;
            center.transform.localPosition = new Vector3(0, 0, v);
            center.name = "Z:" + v;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _cubes[i, j, v].transform.parent = center.transform;
                }
            }
        }


        _tween = center.transform.DOLocalRotate(Vector3.Normalize(rotateDir) * 90 * (dir ? 1 : -1), _doTime)
            .OnComplete(() =>
            {
                SetCubeIndex();
                Destroy(center);
            });
    }

}
