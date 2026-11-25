using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectPoolData
{
    // 这个池子的父节点
    public GameObject fatherObj;
    // 池子队列
    public Queue<GameObject> poolQueue;

    public GameObjectPoolData(GameObject rootObj, GameObject obj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(rootObj.transform);
        poolQueue = new Queue<GameObject>();
    }

    public void PushObj(GameObject obj)
    {
        poolQueue.Enqueue(obj);
        obj.transform.SetParent(fatherObj.transform);
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj = poolQueue.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        return obj;
    }
}
