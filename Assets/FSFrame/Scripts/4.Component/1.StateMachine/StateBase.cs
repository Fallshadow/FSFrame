using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 状态基类
public abstract class StateBase
{
    protected StateMachine stateMachine;

    // 初始化状态
    // 只在状态第一次创建时执行
    public virtual void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }


    // 反初始化
    // 不再使用时候，放回对象池时调用
    // 把一些引用置空，防止不能被GC
    public virtual void UnInit()
    {
        // 放回对象池
        this.FSObjectPushPool();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }

}