using Naukri.Moltk.MVU;
using UnityEngine;

public record IntroText(
    string Title,
    string Content
       )
{
    public IntroText() : this(
        Title: "",
        Content: ""
        )
    {
    }
}

public partial class IntroTextProvider : ProviderBehaviour<IntroText>
{
    public void SetText(IntroText text)
    {
        State = text;
    }

    protected override void Build(IProvider provider)
    {
        // 沒有訂閱任何 Provider，所以這裡不需要做任何事情。
    }
}

partial class IntroTextProvider
{
    public static IntroTextProvider Of(Component component)
    {
        return component.GetComponentInParent<IntroTextProvider>();
    }
}
