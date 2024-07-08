using System;
using Naukri.Moltk.Fusion;

namespace Naukri.Moltk.XRKeyboard
{
    public enum Capslock
    {
        Off,
        Once,
        On,
    }

    public class XRKeyboardController : ViewController<XRKeyboardState>
    {
        public void SendKey(string text)
        {
            SetState(s => s with
            {
                Text = s.Text + text,
                Capslock = s.Capslock switch
                {
                    Capslock.Once => Capslock.Off,
                    _ => s.Capslock,
                }
            });
        }

        public void Clear()
        {
            SetState(s => s with
            {
                Text = ""
            });
        }

        public void ToggleCapslock()
        {
            SetState(s => s with
            {
                Capslock = s.Capslock switch
                {
                    Capslock.Off => Capslock.Once,
                    Capslock.Once => Capslock.On,
                    Capslock.On => Capslock.Off,
                    _ => throw new NotImplementedException(),
                }
            });
        }

        public void Backspace()
        {
            if (State.Text.Length > 0)
            {
                SetState(s => s with
                {
                    Text = s.Text[..^1]
                });
            }
        }

        public void Confirm()
        {
            SendEvent(new OnConfirm(State.Text));

            SetState(s => s with
            {
                IsOpen = false
            });
        }

        public void Cancel()
        {
            SendEvent(new OnCancel());

            SetState(s => s with
            {
                IsOpen = false
            });
        }

        public void Open(XRKeyboardBinding binding, string text)
        {
            if (State.Binding != null)
            {
                State.Binding.Unbind();
            }
            binding.Bind();

            SetState(s => new XRKeyboardState(text, true, Binding: binding));
        }

        protected override XRKeyboardState Build(XRKeyboardState state)
        {
            return state ?? new XRKeyboardState(
                IsOpen: gameObject.activeSelf
                );
        }

        protected override void Render()
        {
            var state = State;
            gameObject.SetActive(state.IsOpen);
        }
        public record OnConfirm(string Text) : ProviderEvent;

        public record OnCancel : ProviderEvent;
    }

    public record XRKeyboardState(
        string Text = "",
        bool IsOpen = false,
        Capslock Capslock = Capslock.Off,
        XRKeyboardBinding Binding = null
        )
    {
    }
}
