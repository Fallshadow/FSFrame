using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Pool]
public class TestObjPoolAttr
{
    
}

public class TestObjPool : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 只能一个一个来
    TestObjPoolPrefab testPrefab;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testPrefab = PoolManager.Instance.GetObject<TestObjPoolPrefab>();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            testPrefab.Dead();
        }
    }
}