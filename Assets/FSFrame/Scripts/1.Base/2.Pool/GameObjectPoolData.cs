using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectPoolData
{
    // 这个池子的父节点
    private readonly GameObject fatherObj;
    // 池子队列
    public readonly Queue<GameObject> PoolQueue;

    public GameObjectPoolData(GameObject rootObj, GameObject obj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(rootObj.transform);
        PoolQueue = new Queue<GameObject>();
    }

    public void PushObj(GameObject obj)
    {
        PoolQueue.Enqueue(obj);
        obj.transform.SetParent(fatherObj.transform);
        obj.SetActive(false);
    }

    public GameObject GetObj(Transform parent)
    {
        GameObject obj = PoolQueue.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(parent);
        // 只有没有指定父物体才需要放到原始场景
        if (parent == null)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        }
        return obj;
    }
}
