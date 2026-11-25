using System.Collections.Generic;
using UnityEngine;

public partial class PoolManager
{
    [SerializeField] private GameObject rootObj;

    private readonly Dictionary<string, GameObjectPoolData> gameObjPoolDic =
        new Dictionary<string, GameObjectPoolData>();


    public T GetGameObject<T>(GameObject prefab, Transform parent = null) where T : UnityEngine.Object
    {
        GameObject gameObj = GetGameObject(prefab, parent);
        if (gameObj != null)
        {
            return gameObj.GetComponent<T>();
        }

        return null;
    }
    
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
    
    #region 删除

    public void ClearGameObject(string prefabName)
    {
        GameObject gameObj = rootObj.transform.Find("prefabName").gameObject;
        if (gameObj != null)
        {
            Destroy(gameObj);
            gameObjPoolDic.Remove(prefabName);
        }
    }

    public void ClearGameObject(GameObject gameObj)
    {
        ClearGameObject(gameObj.name);
    }

    #endregion
}