using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PoolManager
{
    private readonly Dictionary<string, ObjectPoolData> objPoolDic = new Dictionary<string, ObjectPoolData>();


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

    #region 删除

    public void ClearObject<T>()
    {
        objPoolDic.Remove(typeof(T).FullName);
    }

    public void ClearObject(Type type)
    {
        objPoolDic.Remove(type.FullName);
    }

    #endregion
}