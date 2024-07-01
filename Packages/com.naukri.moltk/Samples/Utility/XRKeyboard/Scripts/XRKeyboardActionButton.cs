using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.MVU;

[
    ScriptField,
    Members,
    Base,
]
public class XRKeyboardActionButton : XRKeyboardButton
{
    public Action action;

    [ShowIf(nameof(action), Action.SendKey)]
    public string key;

    [ShowIf(nameof(action), Action.SendKey)]
    public string shiftKey;

    protected override void Build(IProvider provider)
    {
        base.Build(provider);

        if (provider is XRKeyboardController controller)
        {
            if (action == Action.ToggleCapslock)
            {
                var isCapslock = controller.State.Capslock == Capslock.On;

                characterText.text = isCapslock ? shiftDisplayCharacter : displayCharacter;
                iconImage.sprite = isCapslock ? shiftDisplayIcon : displayIcon;
            }
        }
    }

    protected override void OnClicked()
    {
        switch (action)
        {
            case Action.SendKey:
                var isShift = controller.State.Capslock != Capslock.Off;
                var text = isShift ? shiftKey : key;
                controller.SendKey(text);
                break;

            case Action.Backspace:
                controller.Backspace();
                break;

            case Action.Clear:
                controller.Clear();
                break;

            case Action.ToggleCapslock:
                controller.ToggleCapslock();
                break;

            case Action.Confirm:
                controller.Confirm();
                break;

            case Action.Cancel:
                controller.Cancel();
                break;
        }
    }

    public enum Action
    {
        SendKey,

        Backspace,

        Clear,

        ToggleCapslock,

        Confirm,

        Cancel,
    }
}
