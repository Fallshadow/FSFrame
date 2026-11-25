using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase<PoolManager>
{
    public override void Init()
    {
        base.Init();
        Debug.Log("Pool Manager Init.");
    }
}
