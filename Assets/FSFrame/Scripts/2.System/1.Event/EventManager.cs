using System;
using System.Collections.Generic;

public static class EventManager
{
        #region 内部接口、内部类
        
        private interface IEventInfo
        {
                void Destory();
        }

        private class EventInfo : IEventInfo
        {
                public Action action;

                public void Init(Action action)
                {
                        this.action = action;
                }

                public void Destory()
                {
                        action = null;
                        this.FSObjectPushPool();
                }
        }
        
        private class EventInfo<T>  : IEventInfo
        {
                public Action<T> action;

                public void Init(Action<T> action)
                {
                        this.action = action;
                }
                
                public void Destory()
                {
                        action = null;
                        this.FSObjectPushPool();
                }
        }
        
        private class EventInfo<T1, T2>  : IEventInfo
        {
                public Action<T1, T2> action;

                public void Init(Action<T1, T2> action)
                {
                        this.action = action;
                }
                
                public void Destory()
                {
                        action = null;
                        this.FSObjectPushPool();
                }
        }
        
        private class EventInfo<T1, T2, T3>   : IEventInfo
        {
                public Action<T1, T2, T3> action;

                public void Init(Action<T1, T2, T3> action)
                {
                        this.action = action;
                }
                
                public void Destory()
                {
                        action = null;
                        this.FSObjectPushPool();
                }
        }

        #endregion
        
        private static Dictionary<string, IEventInfo> eventInfoDic = new Dictionary<string, IEventInfo>();

        #region 添加事件监听
        
        public static void AddEventListener(string eventName, Action action)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo).action += action;
                }
                else
                {
                        EventInfo eventInfo = PoolManager.Instance.GetObject<EventInfo>();
                        eventInfo.Init(action);
                        eventInfoDic.Add(eventName, eventInfo);
                }
        }
        
        public static void AddEventListener<T>(string eventName, Action<T> action)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T>).action += action;
                }
                else
                {
                        EventInfo<T> eventInfo = PoolManager.Instance.GetObject<EventInfo<T>>();
                        eventInfo.Init(action);
                        eventInfoDic.Add(eventName, eventInfo);
                }
        }
        
        public static void AddEventListener<T1,T2>(string eventName, Action<T1,T2> action)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T1,T2>).action += action;
                }
                else
                {
                        EventInfo<T1,T2> eventInfo = PoolManager.Instance.GetObject<EventInfo<T1,T2>>();
                        eventInfo.Init(action);
                        eventInfoDic.Add(eventName, eventInfo);
                }
        }
        
        public static void AddEventListener<T1,T2,T3>(string eventName, Action<T1,T2,T3> action)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T1,T2,T3>).action += action;
                }
                else
                {
                        EventInfo<T1,T2,T3> eventInfo = PoolManager.Instance.GetObject<EventInfo<T1,T2,T3>>();
                        eventInfo.Init(action);
                        eventInfoDic.Add(eventName, eventInfo);
                }
        }
        
        #endregion

        #region 触发事件

        public static void EventTrigger(string eventName)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo).action?.Invoke();
                }
        }
        
        public static void EventTrigger<T>(string eventName, T arg)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T>).action?.Invoke(arg);
                }
        }
        
        public static void EventTrigger<T1,T2>(string eventName, T1 arg1, T2 arg2)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T1, T2>).action?.Invoke(arg1, arg2);
                }
        }
        
        public static void EventTrigger<T1,T2,T3>(string eventName, T1 arg1, T2 arg2,T3 arg3)
        {
                if (eventInfoDic.TryGetValue(eventName, out var value))
                {
                        (value as EventInfo<T1, T2, T3>).action?.Invoke(arg1, arg2,  arg3);
                }
        }

        #endregion
        
        
        #region 取消事件的监听
        public static void RemoveEventListener(string eventName,Action action)
        {
                if (eventInfoDic.ContainsKey(eventName))
                {
                        (eventInfoDic[eventName] as EventInfo).action -= action;
                }
        }

        public static void RemoveEventListener<T>(string eventName, Action<T> action)
        {
                if (eventInfoDic.ContainsKey(eventName))
                {
                        (eventInfoDic[eventName] as EventInfo<T>).action -= action;
                }
        }

        public static void RemoveEventListener<T1,T2>(string eventName, Action<T1,T2> action)
        {
                if (eventInfoDic.ContainsKey(eventName))
                {
                        (eventInfoDic[eventName] as EventInfo<T1,T2>).action -= action;
                }
        }

        public static void RemoveEventListener<T1,T2,T3>(string eventName, Action<T1,T2,T3> action)
        {
                if (eventInfoDic.ContainsKey(eventName))
                {
                        (eventInfoDic[eventName] as EventInfo<T1,T2,T3>).action -= action;
                }
        }
        #endregion

        #region 移除事件
        
        public static void RemoveEventListener(string eventName)
        {
                if (eventInfoDic.ContainsKey(eventName))
                {
                        eventInfoDic[eventName].Destory();
                        eventInfoDic.Remove(eventName);
                }
        }


        public static void Clear()
        {
                foreach (string eventName in eventInfoDic.Keys)
                {
                        eventInfoDic[eventName].Destory();
                }
                eventInfoDic.Clear();
        }

        #endregion
}