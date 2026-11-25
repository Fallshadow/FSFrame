using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 框架层面的游戏设置，放在 gameroot
/// 对象池缓存设置、UI元素的设置
/// </summary>
[CreateAssetMenu(fileName = "GameSetting", menuName = "FSFrame/Config/GameSetting")]
public class GameSetting : ConfigBase
{
        [LabelText("对象池设置")]
        [DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "皆可缓存")]
        public Dictionary<Type, bool> CacheDic = new Dictionary<Type, bool>();
        
#if UNITY_EDITOR
        [Button(Name = "初始化游戏配置", ButtonHeight = 50)]
        [GUIColor(0,1,0)]
        private void Init()
        {
                PoolAttributeOnEditor();
        }

        // 编译前执行函数
        [InitializeOnLoadMethod]
        private static void LoadForEditor()
        {
                if (GameObject.Find("GameRoot") != null)
                {
                        GameObject.Find("GameRoot").GetComponent<GameRoot>().GameSetting.Init();
                }
        }

        // 将所有程序集下有 pool 特性的类型加入到 CacheDic 中
        private void PoolAttributeOnEditor()
        {
                CacheDic.Clear();
                // 获取所有程序集
                Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly asm in asms)
                {
                        // 遍历程序集下的每一个类型
                        Type[] types = asm.GetTypes();
                        foreach (Type type in types)
                        {
                                PoolAttribute pool = type.GetCustomAttribute<PoolAttribute>();
                                if (pool != null)
                                {
                                        CacheDic.Add(type, true);
                                }
                        }
                }
        }
        
#endif
        
}