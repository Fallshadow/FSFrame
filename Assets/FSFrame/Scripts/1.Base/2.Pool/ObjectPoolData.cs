using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolData
{
    public readonly Queue<object> PoolQueue = new Queue<object>();

    public void PushObj(object obj)
    {
        PoolQueue.Enqueue(obj);
    }

    public object GetObj()
    {
        return PoolQueue.Dequeue();
    }
}
