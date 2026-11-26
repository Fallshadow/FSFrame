using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FSEventType
{
    OnMouseEnter,
    OnMouseExit,
    OnClick,
    OnClickDown,
    OnClickUp,
    OnDrag,
    OnBeginDrag,
    OnEndDrag,
    OnCollisionEnter,
    OnCollisionStay,
    OnCollisionExit,
    OnCollisionEnter2D,
    OnCollisionStay2D,
    OnCollisionExit2D,
    OnTriggerEnter,
    OnTriggerStay,
    OnTriggerExit,
    OnTriggerEnter2D,
    OnTriggerStay2D,
    OnTriggerExit2D,
}

public interface IMouseEvent :
    IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
}

// 可以添加鼠标，碰撞，触发等事件
public class FSEventListener : MonoBehaviour, IMouseEvent
{
    #region 内部类、接口等

    // 一次事件的数据包装类
    // 用对象池循环利用
    [Pool]
    private class FSEventListenerEventInfo<T>
    {
        // T：事件本身的参数 （PointerEventData、Collision）
        // object[] 事件的参数
        public Action<T, object[]> action;
        public object[] args;
        
        public void Init(Action<T, object[]> action, object[] args)
        {
            this.action = action;
            this.args = args;
        }

        public void TriggerEvent(T eventData)
        {
            action?.Invoke(eventData, args);
        }
    }

    interface IFSEventListenerEventInfos
    {
        void RemoveAllListener();
    }

    // 一类事件的数据包装类，包含多个 FSEventListenerEventInfo
    [Pool]
    private class FSEventListenerEventInfos<T> : IFSEventListenerEventInfos
    {
        // 这个类型的所有事件
        private List<FSEventListenerEventInfo<T>> eventList = new List<FSEventListenerEventInfo<T>>();

        // 添加事件
        public void AddListener(Action<T, object[]> action, params object[] args)
        {
            FSEventListenerEventInfo<T> info = ResManager.Instance.Load<FSEventListenerEventInfo<T>>();
            info.Init(action, args);
            eventList.Add(info);
        }

        // 传统事件，比如 button 的 onclick 注册 N 次，只要有移除，就全部移除了，如果是没有参数，似乎也是合理的
        // 但是，如果是需要有参数，N 次的参数不同，只希望移除特定参数，是没办法的，所以我们自己这里这样写移除，且一次添加对应一次移除
        public void RemoveListener(Action<T, object[]> action, bool checkArgs = false, params object[] args)
        {
            for (int i = 0; i < eventList.Count; i++)
            {
                // 找到事件
                if (eventList[i].action.Equals(action))
                {
                    // 是否需要检查参数
                    if (checkArgs && args.Length > 0)
                    {
                        // 参数如果不相等，找下一个
                        if (!args.ArrayEquals(eventList[i].args)) continue;
                    }
                    // 参数相同，或者不检查参数，都走移除
                    eventList[i].FSObjectPushPool();
                    eventList.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAllListener()
        {
            for (int i = 0; i < eventList.Count; i++)
            {
                eventList[i].FSObjectPushPool();
            }
            eventList.Clear();
        }

        public void TriggerEvent(T eventData)
        {
            for (int i = 0; i < eventList.Count; i++)
            {
                eventList[i].TriggerEvent(eventData);
            }
        }
    }

    #endregion

    private Dictionary<FSEventType, IFSEventListenerEventInfos> eventInfoDic = new Dictionary<FSEventType, IFSEventListenerEventInfos>();
    
    #region 外部的访问

    public void AddListener<T>(FSEventType eventType, Action<T, object[]> action, 
        bool checkArgs = false, params object[] args)
    {
        if (eventInfoDic.TryGetValue(eventType, out var value))
        {
            (value as FSEventListenerEventInfos<T>)?.AddListener(action, checkArgs, args);
        }
        else
        {
            FSEventListenerEventInfos<T> infos = ResManager.Instance.Load<FSEventListenerEventInfos<T>>();
            infos.AddListener(action, checkArgs, args);
            eventInfoDic.Add(eventType,  infos);
        }
    }

    public void RemoveListener<T>(FSEventType eventType, Action<T, object[]> action, bool checkArgs = false, params object[] args)
    {
        if (eventInfoDic.TryGetValue(eventType, out var value))
        {
            (value as FSEventListenerEventInfos<T>)?.RemoveListener(action, checkArgs, args);
        }
    }

    public void RemoveAllListener<T>(FSEventType eventType)
    {
        if (eventInfoDic.TryGetValue(eventType, out var value))
        {
            (value as FSEventListenerEventInfos<T>)?.RemoveAllListener();
        }
    }
    
    public void RemoveAllListener()
    {
        foreach (var infos in eventInfoDic.Values)
        {
            infos.RemoveAllListener();
        }
        
        eventInfoDic.Clear();
    }
    #endregion

    private void TriggerAction<T>(FSEventType eventType, T eventData)
    {
        if (eventInfoDic.TryGetValue(eventType, out var value))
        {
            (value as FSEventListenerEventInfos<T>)?.TriggerEvent(eventData);
        }
    }
    
    #region 鼠标事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnMouseEnter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnMouseExit, eventData);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnBeginDrag, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnDrag, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnEndDrag, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnClick, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnClickDown, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TriggerAction(FSEventType.OnClickUp, eventData);
    }
    #endregion
    #region 碰撞事件
    private void OnCollisionEnter(Collision collision)
    {
        TriggerAction(FSEventType.OnCollisionEnter, collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        TriggerAction(FSEventType.OnCollisionStay, collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        TriggerAction(FSEventType.OnCollisionExit, collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TriggerAction(FSEventType.OnCollisionEnter2D, collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TriggerAction(FSEventType.OnCollisionStay2D, collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        TriggerAction(FSEventType.OnCollisionExit2D, collision);
    }
    #endregion
    #region 触发事件
    private void OnTriggerEnter(Collider other)
    {
        TriggerAction(FSEventType.OnTriggerEnter, other);
    }
    private void OnTriggerStay(Collider other)
    {
        TriggerAction(FSEventType.OnTriggerStay, other);
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerAction(FSEventType.OnTriggerExit, other);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerAction(FSEventType.OnTriggerEnter2D, collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerAction(FSEventType.OnTriggerStay2D, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerAction(FSEventType.OnTriggerExit2D, collision);
    }


    #endregion
}
