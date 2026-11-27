using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameRoot : SingletonMono<GameRoot>
{
    /// <summary>
    /// 框架设置
    /// </summary>
    [SerializeField]
    private GameSetting setting;
    public GameSetting Setting { get { return setting; } }
    protected override void Awake()
    {
        // 多场景测试方便，但是需要做这个检测防止重复
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
        DontDestroyOnLoad(gameObject);
        
        InitManager();
    }

    private void InitManager()
    {
        ManagerBase[] managers = GetComponents<ManagerBase>();
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Init();
        }
    }
    
    #if UNITY_EDITOR

    [InitializeOnLoadMethod]
    public static void InitForEditor()
    {
        var gameRootGo = GameObject.Find("GameRoot");
        if (Instance == null &&  gameRootGo != null)
        {
            EventManager.Clear();
            Instance = gameRootGo.GetComponent<GameRoot>();
            Instance.InitManager();
            Instance.Setting.InitForEditor();
        }
    }
    
    #endif
}
