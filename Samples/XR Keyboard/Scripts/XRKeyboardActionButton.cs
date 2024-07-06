using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    [
    ScriptField,
    Members,
    Base,
]
    public class XRKeyboardActionButton : XRKeyboardButton
    {
        [SerializeField]
        protected Action action;

        [SerializeField, ShowIf(nameof(action), Action.SendKey)]
        protected string key;

        [SerializeField, ShowIf(nameof(action), Action.SendKey)]
        protected string shiftKey;

        protected override void Render()
        {
            base.Render();

            var keyboardState = keyboardController.State;

            if (action == Action.ToggleCapslock)
            {
                var isCapslock = keyboardState.Capslock == Capslock.On;

                characterText.text = isCapslock ? shiftDisplayCharacter : displayCharacter;
                iconImage.sprite = isCapslock ? shiftDisplayIcon : displayIcon;
            }
        }

        protected override void OnClicked()
        {
            var keyboardState = keyboardController.State;

            switch (action)
            {
                case Action.SendKey:
                    var isShift = keyboardState.Capslock != Capslock.Off;
                    var text = isShift ? shiftKey : key;
                    keyboardController.SendKey(text);
                    break;

                case Action.Backspace:
                    keyboardController.Backspace();
                    break;

                case Action.Clear:
                    keyboardController.Clear();
                    break;

                case Action.ToggleCapslock:
                    keyboardController.ToggleCapslock();
                    break;

                case Action.Confirm:
                    keyboardController.Confirm();
                    break;

                case Action.Cancel:
                    keyboardController.Cancel();
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
}
