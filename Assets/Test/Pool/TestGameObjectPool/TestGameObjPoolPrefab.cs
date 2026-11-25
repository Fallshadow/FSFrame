using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameObjPoolPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("TestPoolPrefab.Start");
        Invoke("Dead",3);
    }

    void Dead()
    {
        PoolManager.Instance.PushGameObject(this.gameObject);
    }
    
}
