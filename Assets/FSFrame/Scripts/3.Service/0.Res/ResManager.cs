using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResManager : ManagerBase<ResManager>
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

    // 加载资源 比如 AudioClip 
    public T LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }


    // 一定是有回调使用的，不然帮你加载干嘛嘞
    // 异步加载资源 比如 AudioClip (也可以是预制体，但是这种方式不配套实例化)
    public void LoadAssetAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        StartCoroutine(DoLoadAssetAsync<T>(path, callback));
    }

    IEnumerator DoLoadAssetAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        callback?.Invoke(request.asset as T);
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

}
