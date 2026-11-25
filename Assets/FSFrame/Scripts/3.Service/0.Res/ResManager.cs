using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : ManagerBase<ResManager>
{
    // 需要缓存的类型（和对象池配合）
    private Dictionary<Type, bool> wantCacheDic;

    public override void Init()
    {
        base.Init();
        // TODO: 替换成真正的配置
        wantCacheDic = new Dictionary<Type, bool>();
    }

    // 检查一个类型是否需要缓存
    private bool CheckCacheDic(Type type)
    {
        return wantCacheDic.ContainsKey(type);
    }

    
    // 获取实例 Object，如果需要缓存，会从对象池中获取
    public T Load<T>() where T : class, new()
    {
        if (CheckCacheDic(typeof(T)))
        {
            return PoolManager.Instance.GetObject<T>();
        }
        else
        {
            return new T();
        }
    }

    // 获取实例 组件，如果需要缓存，会从对象池中获取
    public T Load<T>(string path, Transform parent) where T : Component
    {
        if (CheckCacheDic(typeof(T)))
        {
            return PoolManager.Instance.GetGameObject<T>(GetPrefab(path), parent);
        }
        else
        {
            return InstantiateForPrefab(path, parent).GetComponent<T>();
        }
    }
    
    public GameObject GetPrefab(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            throw new Exception("FS:预制体路径有误，没有找到预制体");
        }
    }

    public GameObject InstantiateForPrefab(string path, Transform parent)
    {
        return InstantiateForPrefab(GetPrefab(path), parent);
    }
    
    public GameObject InstantiateForPrefab(GameObject prefab, Transform parent = null)
    {
        GameObject go = Instantiate(prefab, parent);
        // 默认实例化的名称会带有 clone
        go.name = prefab.name;
        return go;
    }
}
