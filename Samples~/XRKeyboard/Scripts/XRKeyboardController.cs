using Naukri.Moltk.MVU;
using System;

namespace Naukri.Moltk.XRKeyboard
{
    public enum Capslock
    {
        Off,

        Once,

        On,
    }

    public record XRKeyboardState(string Text = "", Capslock Capslock = Capslock.Off)
    {
        public XRKeyboardState() : this("", Capslock.Off)
        {
        }
    }

    public partial class XRKeyboardController : MVUController<XRKeyboardState>
    {
        public void SendKey(string text)
        {
            State = State with
            {
                Text = State.Text + text,
                Capslock = State.Capslock switch
                {
                    Capslock.Once => Capslock.Off,
                    _ => State.Capslock,
                }
            };
        }

        public void Clear()
        {
            State = State with
            {
                Text = ""
            };
        }

        public void ToggleCapslock()
        {
            State = State with
            {
                Capslock = State.Capslock switch
                {
                    Capslock.Off => Capslock.Once,
                    Capslock.Once => Capslock.On,
                    Capslock.On => Capslock.Off,
                    _ => throw new NotImplementedException(),
                }
            };
        }

        public void Backspace()
        {
            if (State.Text.Length > 0)
            {
                State = State with
                {
                    Text = State.Text[..^1]
                };
            }
        }

        public void Confirm() => SendEvent(new OnConfirm());

        public void Cancel() => SendEvent(new OnCancel());

        public void Open(string text) => SendEvent(new OnOpen(text));

        protected override void Build(IProvider provider)
        {
            if (provider is XRKeyboardController keyboardController)
            {
                var state = keyboardController.State;
                print(state.Text);
            }
        }

        protected override void HandleEvent(IProvider provider, IProviderEvent evt)
        {
            if (provider is XRKeyboardController)
            {
                switch (evt)
                {
                    case OnOpen onOpen:
                        State = new XRKeyboardState(onOpen.Text);
                        gameObject.SetActive(true);
                        break;

                    case OnConfirm:
                        gameObject.SetActive(false);
                        break;

                    case OnCancel:
                        gameObject.SetActive(false);
                        break;
                }
            }
        }

        public record OnOpen(string Text) : IProviderEvent { }

        public record OnConfirm : IProviderEvent { }

        public record OnCancel : IProviderEvent { }
    }

    partial class XRKeyboardController
    {
        public static XRKeyboardController Of(ConsumerBehaviour consumer)
        {
            return consumer.GetComponentInParent<XRKeyboardController>();
        }
    }
}
