using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : SingletonMono<GameRoot>
{
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
}
