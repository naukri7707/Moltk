using System;
using Naukri.InspectorMaid;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.Utility;
using UnityEngine;

public class IntroTextUpdaterNode : UnitTreeBehaviour
{
    [SerializeField]
    private Property targetProperty;

    [SerializeField, ShowIf(nameof(targetProperty), Property.Title, ConditionLogic.Flag)]
    private string title;

    [SerializeField, TextArea(3, 5), ShowIf(nameof(targetProperty), Property.Content, ConditionLogic.Flag)]
    private string content;

    [Flags]
    public enum Property
    {
        Title = Flag._00,

        Content = Flag._01,
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        var controller = ctx.Read<IntroPanelController>();
        controller.SetState(s => s with
        {
            Title = targetProperty.HasFlag(Property.Title) ? title : s.Title,
            Content = targetProperty.HasFlag(Property.Content) ? content : s.Content,
        });
    }
}
