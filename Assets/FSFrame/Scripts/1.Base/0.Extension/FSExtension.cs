using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 扩展方法
/// </summary>
public static class FSExtension
{
    #region 通用

    // 获取自身的某个特性
    public static T GetAttribute<T>(this object obj) where T : Attribute
    {
        return obj.GetType().GetCustomAttribute<T>();
    }
    
    // 获取某个类型的特性
    public static T GetAttribute<T>(this object obj, Type type) where T : Attribute
    {
        return type.GetCustomAttribute<T>();
    }

    public static bool ArrayEquals(this object[] objs, object[] other)
    {
        if (other == null || objs.Length != other.Length || objs.GetType() != other.GetType())
        {
            return false;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            if (!objs[i].Equals(other[i]))
            {
                return false;
            }
        }
        
        return true;
    }
    
    #endregion

    #region 资源管理

    public static void FSGameObjectPushPool(this GameObject go)
    {
        PoolManager.Instance.PushGameObject(go);   
    }

    public static void FSGameObjectPushPool(this Component com)
    {
        FSGameObjectPushPool(com.gameObject);   
    }
    
    public static void FSObjectPushPool(this object obj)
    {
        PoolManager.Instance.PushObject(obj);   
    }
    
    #endregion

    #region 本地化

    public static void Localization(this Text com, string workSpace, string key)
    {
        com.text = LocalizationManager.Instance.GetLocalizationData<L_Text>(workSpace, key).content;
    }
    
    public static void Localization(this Image com, string workSpace, string key)
    {
        com.sprite = LocalizationManager.Instance.GetLocalizationData<L_Image>(workSpace, key).content;
    }
    
    public static void Localization(this AudioSource com, string workSpace, string key)
    {
        com.clip = LocalizationManager.Instance.GetLocalizationData<L_Audio>(workSpace, key).content;
    }
    
    public static void Localization(this VideoPlayer com, string workSpace, string key)
    {
        com.clip = LocalizationManager.Instance.GetLocalizationData<L_Video>(workSpace, key).content;
    }

    #endregion
}
