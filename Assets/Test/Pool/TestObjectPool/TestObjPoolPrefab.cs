using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjPoolPrefab 
{
    public TestObjPoolPrefab()
    {
        Debug.Log("TestObjPoolPrefab Create");
    }
    
    public void Dead()
    {
        Debug.Log("TestObjPoolPrefab Dead");
        PoolManager.Instance.PushObject(this);
    }
}
