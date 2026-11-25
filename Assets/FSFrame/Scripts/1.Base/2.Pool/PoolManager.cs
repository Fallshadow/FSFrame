using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase<PoolManager>
{
    [SerializeField]
    private GameObject rootObj;
    private Dictionary<string, GameObjectPoolData> poolDic = new Dictionary<string, GameObjectPoolData>();
    public override void Init()
    {
        base.Init();
        Debug.Log("Pool Manager Init.");
    }

    /// <summary>
    /// 外部从对象池获取游戏物体
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject GetGameObject(GameObject prefab)
    {
        string prefabName = prefab.name;
        GameObject obj = null;
        if (IsPoolHaveObj(prefabName))
        {
            obj = poolDic[prefabName].GetObj();
        }
        else
        {
            obj = GameObject.Instantiate(prefab);
            obj.name = prefabName;
        }
        return null;
    }

    /// <summary>
    /// 游戏物体回收进对象池
    /// </summary>
    /// <param name="obj"></param>
    public void PushGameObject(GameObject obj)
    {
        string objName = obj.name;
        if (!poolDic.TryGetValue(objName, out GameObjectPoolData poolData))
        {
            poolData = new GameObjectPoolData(rootObj, obj);
            poolDic.Add(objName, poolData);
        }
        poolData.PushObj(obj);
    }

    public bool IsPoolHaveObj(GameObject prefab)
    {
        string prefabName = prefab.name;
        return IsPoolHaveObj(prefabName);
    }
    
    public bool IsPoolHaveObj(string prefabName)
    {
        return poolDic.ContainsKey(prefabName) && poolDic[prefabName].poolQueue.Count > 0;
    }
}
