using Naukri.Moltk.Fusion;
using UnityEngine;

public record IntroText(
    string Title = "",
    string Content = ""
    )
{
}

public partial class IntroTextProvider : Provider<IntroText>
{
    protected override IntroText Build(IntroText state)
    {
        return state ?? new IntroText();
    }
}

partial class IntroTextProvider
{
    public static IntroTextProvider Of(Component component)
    {
        return component.GetComponentInParent<IntroTextProvider>();
    }
}
