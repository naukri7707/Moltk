using Naukri.InspectorMaid;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.Utility;
using System;
using UnityEngine;

public class IntroTextUpdaterNode : UnitTreeBehaviour
{
    [SerializeField]
    private Property targetProperty;

    [SerializeField, ShowIf(nameof(targetProperty), Property.Title, ConditionLogic.Flag)]
    private string title;

    [SerializeField, TextArea(3, 5), ShowIf(nameof(targetProperty), Property.Content, ConditionLogic.Flag)]
    private string content;

    protected override void OnEnter()
    {
        base.OnEnter();
        var pageDataProvider = IntroTextProvider.Of(this);
        var state = pageDataProvider.State;

        var newState = state with
        {
            Title = targetProperty.HasFlag(Property.Title) ? title : pageDataProvider.State.Title,
            Content = targetProperty.HasFlag(Property.Content) ? content : pageDataProvider.State.Content,
        };

        pageDataProvider.SetText(newState);
    }

    [Flags]
    public enum Property
    {
        Title = Flag._00,

        Content = Flag._01,
    }
}
