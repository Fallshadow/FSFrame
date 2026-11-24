using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 扩展方法
/// </summary>
public static class FSExtension
{
    #region 通用

    /// <summary>
    /// 获取自身的某个特性
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T">指定特性</typeparam>
    /// <returns></returns>
    public static T GetAttribute<T>(this object obj) where T : Attribute
    {
        return obj.GetType().GetCustomAttribute<T>();
    }
    
    /// <summary>
    /// 获取某个类型的特性
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type">特性所在的类型</param>
    /// <typeparam name="T">指定特性</typeparam>
    /// <returns></returns>
    public static T GetAttribute<T>(this object obj, Type type) where T : Attribute
    {
        return type.GetCustomAttribute<T>();
    }
    
    #endregion
}
