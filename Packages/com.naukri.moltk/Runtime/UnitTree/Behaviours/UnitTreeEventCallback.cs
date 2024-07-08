using System;
using Naukri.InspectorMaid;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.Utility;
using UnityEngine.Events;

public class UnitTreeEventCallback : UnitTreeBehaviour
{
    public Event callback;

    [ShowIf(nameof(callback), Event.OnEnter, conditionLogic: ConditionLogic.Flag)]
    public UnityEvent onEnter;

    [ShowIf(nameof(callback), Event.OnExit, conditionLogic: ConditionLogic.Flag)]
    public UnityEvent onExit;

    [Flags]
    public enum Event
    {
        OnEnter = Flag._00,

        OnExit = Flag._01,
    }

    protected override void OnEnter()
    {
        if (callback.HasFlag(Event.OnEnter))
        {
            base.OnEnter();
            onEnter.Invoke();
        }
    }

    protected override void OnExit()
    {
        if (callback.HasFlag(Event.OnExit))
        {
            base.OnExit();
            onExit.Invoke();
        }
    }
}
