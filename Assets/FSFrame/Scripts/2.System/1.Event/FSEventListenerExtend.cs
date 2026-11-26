using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class FSEventListenerExtend
{
    #region 工具函数

    private static FSEventListener GetOrAddFSEventListener(Component com)
    {
        FSEventListener listener = com.GetComponent<FSEventListener>();
        if (listener == null) return com.gameObject.AddComponent<FSEventListener>();
        return listener;
    }
    
    public static void AddEventListener<T>(this Component com, 
        FSEventType eventType, Action<T, object[]> action, params object[] args)
    {
        FSEventListener listener = GetOrAddFSEventListener(com);
        listener.AddListener(eventType, action, args);
    }

    public static void RemoveEventListener<T>(this Component com,
        FSEventType eventType, Action<T, object[]> action, bool checkArgs = false, params object[] args)
    {
        FSEventListener listener = GetOrAddFSEventListener(com);
        listener.RemoveListener(eventType, action, checkArgs, args);
    }

    public static void RemoveAllListener<T>(this Component com, FSEventType eventType)
    {
        FSEventListener listener = GetOrAddFSEventListener(com);
        listener.RemoveAllListener(eventType);
    }
    
    public static void RemoveAllListener<T>(this Component com)
    {
        FSEventListener listener = GetOrAddFSEventListener(com);
        listener.RemoveAllListener();
    }
    
    #endregion
    
        #region 鼠标相关事件
    public static void OnMouseEnter(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnMouseEnter, action, args);
    }
    public static void OnMouseExit(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnMouseExit, action, args);
    }
    public static void OnClick(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnClick, action, args);
    }
    public static void OnClickDown(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnClickDown, action, args);
    }
    public static void OnClickUp(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnClickUp, action, args);
    }
    public static void OnDrag(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnDrag, action, args);
    }
    public static void OnBeginDrag(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnBeginDrag, action, args);
    }
    public static void OnEndDrag(this Component com, Action<PointerEventData, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnEndDrag, action, args);
    }
    public static void RemoveClick(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnClick, action, checkArgs, args);
    }
    public static void RemoveClickDown(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnClickDown, action, checkArgs, args);
    }
    public static void RemoveClickUp(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnClickUp, action, checkArgs, args);
    }
    public static void RemoveDrag(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnDrag, action, checkArgs, args);
    }
    public static void RemoveBeginDrag(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnBeginDrag, action, checkArgs, args);
    }
    public static void RemoveEndDrag(this Component com, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnEndDrag, action, checkArgs, args);
    }


    #endregion

    #region 碰撞相关事件

    public static void OnCollisionEnter(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        com.AddEventListener(FSEventType.OnCollisionEnter, action, args);
    }


    public static void OnCollisionStay(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnCollisionStay, action, args);
    }
    public static void OnCollisionExit(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnCollisionExit, action, args);
    }
    public static void OnCollisionEnter2D(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnCollisionEnter2D, action, args);
    }
    public static void OnCollisionStay2D(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnCollisionStay2D, action, args);
    }
    public static void OnCollisionExit2D(this Component com, Action<Collision, object[]> action, params object[] args)
    {
        AddEventListener(com, FSEventType.OnCollisionExit2D, action, args);
    }
    public static void RemoveCollisionEnter(this Component com, Action<Collision, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionEnter, action, checkArgs, args);
    }
    public static void RemoveCollisionStay(this Component com, Action<Collision, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionStay, action, checkArgs, args);
    }
    public static void RemoveCollisionExit(this Component com, Action<Collision, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionExit, action, checkArgs, args);
    }
    public static void RemoveCollisionEnter2D(this Component com, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionEnter2D, action, checkArgs, args);
    }
    public static void RemoveCollisionStay2D(this Component com, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionStay2D, action, checkArgs, args);
    }
    public static void RemoveCollisionExit2D(this Component com, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnCollisionExit2D, action, checkArgs, args);
    }
    #endregion

    #region 触发相关事件
    public static void OnTriggerEnter(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerEnter, action, checkArgs, args);
    }
    public static void OnTriggerStay(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerStay, action, checkArgs, args);
    }
    public static void OnTriggerExit(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerExit, action, checkArgs, args);
    }
    public static void OnTriggerEnter2D(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerEnter2D, action, checkArgs, args);
    }
    public static void OnTriggerStay2D(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerStay2D, action, checkArgs, args);
    }
    public static void OnTriggerExit2D(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        AddEventListener(com, FSEventType.OnTriggerExit2D, action, checkArgs, args);
    }
    public static void RemoveTriggerEnter(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerEnter, action, checkArgs, args);
    }
    public static void RemoveTriggerStay(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerStay, action, checkArgs, args);
    }
    public static void RemoveTriggerExit(this Component com, Action<Collider, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerExit, action, checkArgs, args);
    }
    public static void RemoveTriggerEnter2D(this Component com, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerEnter2D, action, checkArgs, args);
    }
    public static void RemoveTriggerStay2D(this Component com, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerStay2D, action, checkArgs, args);
    }
    public static void RemoveTriggerExit2D(this Component com, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args)
    {
        RemoveEventListener(com, FSEventType.OnTriggerExit2D, action, checkArgs, args);
    }
    #endregion


}