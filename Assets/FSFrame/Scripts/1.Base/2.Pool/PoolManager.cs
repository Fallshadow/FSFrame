using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PoolManager : ManagerBase<PoolManager>
{
    public override void Init()
    {
        base.Init();
        Debug.Log("Pool Manager Init.");
    }
    

    
    public void ClearALL(bool clearGameObjPool = true, bool clearObjPool = true)
    {
        if (clearGameObjPool)
        {
            for (int i = 0; i < rootObj.transform.childCount; i++)
            {
                Destroy(rootObj.transform.GetChild(0).gameObject);
            }
            gameObjPoolDic.Clear();
        }

        if (clearObjPool)
        {
            objPoolDic.Clear();
        }
    }
}
