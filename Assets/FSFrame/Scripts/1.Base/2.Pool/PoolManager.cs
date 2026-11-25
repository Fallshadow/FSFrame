using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase<PoolManager>
{
    [SerializeField]
    private GameObject rootObj;
    private readonly Dictionary<string, GameObjectPoolData> gameObjPoolDic = new Dictionary<string, GameObjectPoolData>();
    private readonly Dictionary<string, ObjectPoolData> objPoolDic = new Dictionary<string, ObjectPoolData>();
    
    public override void Init()
    {
        base.Init();
        Debug.Log("Pool Manager Init.");
    }

    #region GameObjectPool

    public T GetGameObject<T>(GameObject prefab, Transform parent = null) where T : UnityEngine.Object
    {
        GameObject gameObj = GetGameObject(prefab, parent);
        if (gameObj != null)
        {
            return gameObj.GetComponent<T>();
        }
        return null;
    }
    
    /// <summary>
    /// 外部从对象池获取游戏物体
    /// </summary>
    public GameObject GetGameObject(GameObject prefab, Transform parent = null)
    {
        string prefabName = prefab.name;
        GameObject gameObj = null;
        if (IsPoolHaveGameObject(prefabName))
        {
            gameObj = gameObjPoolDic[prefabName].GetObj(parent);
        }
        else
        {
            gameObj = GameObject.Instantiate(prefab);
            gameObj.name = prefabName;
        }
        return null;
    }

    /// <summary>
    /// 游戏物体回收进对象池
    /// </summary>
    public void PushGameObject(GameObject gameObj)
    {
        string gameObjName = gameObj.name;
        if (!gameObjPoolDic.TryGetValue(gameObjName, out GameObjectPoolData poolData))
        {
            poolData = new GameObjectPoolData(rootObj, gameObj);
            gameObjPoolDic.Add(gameObjName, poolData);
        }
        poolData.PushObj(gameObj);
    }

    public bool IsPoolHaveGameObject(GameObject prefab)
    {
        string prefabName = prefab.name;
        return IsPoolHaveGameObject(prefabName);
    }
    
    public bool IsPoolHaveGameObject(string prefabName)
    {
        return gameObjPoolDic.ContainsKey(prefabName) && gameObjPoolDic[prefabName].PoolQueue.Count > 0;
    }

    #endregion

    #region ObjectPool
    
    public T GetObject<T>() where T : class, new()
    {
        T obj;
        foreach (var item in objPoolDic.Values)
        {
            Debug.Log("获取-队列数量" + item.PoolQueue.Count);
        }
        
        if (IsPoolHaveObject<T>(out string objName))
        {
            objName = typeof(T).FullName;
            obj = (T)objPoolDic[objName].GetObj();
            return obj;
        }
        else
        {
            return new T();
        }
    }
    
    public void PushObject(object obj)
    {
        foreach (var item in objPoolDic.Values)
        {
            Debug.Log("放进-队列数量" + item.PoolQueue.Count);
        }
        string objName = obj.GetType().FullName;
        if (!objPoolDic.TryGetValue(objName, out ObjectPoolData poolData))
        {
            poolData = new ObjectPoolData();
            objPoolDic.Add(objName, poolData);
        }
        poolData.PushObj(obj);
    }

    public bool IsPoolHaveObject<T>(out string objName)
    {
        // 考虑到命名空间
        objName = typeof(T).FullName;
        return objPoolDic.ContainsKey(objName) && objPoolDic[objName].PoolQueue.Count > 0;
    }
    
    #endregion

    public void Clear(bool isClearGameObjPool = true, bool isClearObjPool = true)
    {
        if(isClearGameObjPool) gameObjPoolDic.Clear();
        if(isClearObjPool) objPoolDic.Clear();
    }
}
