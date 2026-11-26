using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    
    // 一类事件的数据包装类，包含多个 FSEventListenerEventInfo
    private class FSEventListenerEventInfos<T>
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
        public void RemoveListener(Action<T, object> action, bool checkArgs = false, params object[] args)
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
    
    #region 鼠标事件

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    
    #endregion
    #region 碰撞事件

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision other)
    {
        
    }

    private void OnCollisionStay(Collision other)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        
    }

    #endregion
    #region 触发事件

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }

    #endregion
}
