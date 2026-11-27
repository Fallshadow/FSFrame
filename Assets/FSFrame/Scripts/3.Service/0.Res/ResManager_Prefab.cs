using System;
using System.Collections;
using UnityEngine;

public partial class ResManager
{
    // 获取实例 组件，如果需要缓存，会从对象池中获取
    public static T Load<T>(string path, Transform parent) where T : Component
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

    public static GameObject GetPrefab(string path)
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

    public static GameObject InstantiateForPrefab(string path, Transform parent)
    {
        return InstantiateForPrefab(GetPrefab(path), parent);
    }

    public static GameObject InstantiateForPrefab(GameObject prefab, Transform parent = null)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        // 默认实例化的名称会带有 clone
        go.name = prefab.name;
        return go;
    }
    
    // 异步加载游戏物体
    // 考虑对象池
    public static void LoadGameObjectAsync<T>(string path, Transform parent = null, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (CheckCacheDic(typeof(T)))
        {
            PoolManager.Instance.IsPoolHaveGameObjectThenOutGameObject(path, out GameObject gameObj, parent);
            if (gameObj != null)
            {
                callback?.Invoke(gameObj.GetComponent<T>());
            }
            else
            {
                MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync<T>(path, parent, callback));
            }
        }
        else
        {
            MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync<T>(path, parent, callback));
        }
    }
    
    static IEnumerator DoLoadGameObjectAsync<T>(string path, Transform parent = null, Action<T> callback = null) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        yield return request;
        GameObject go = InstantiateForPrefab(request.asset as GameObject, parent);
        callback?.Invoke(go.GetComponent<T>());
    }
}