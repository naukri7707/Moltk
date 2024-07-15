using System;
using Naukri.Physarum;

namespace Naukri.Moltk.XRKeyboard
{
    public enum Capslock
    {
        Off,
        Once,
        On,
    }

    public record XRKeyboardState(
        string Text = "",
        bool IsOpen = false,
        Capslock Capslock = Capslock.Off,
        XRKeyboardBinding Binding = null
    ) { }

    public class XRKeyboardController : ViewController<XRKeyboardState>.Behaviour
    {
        public void SendKey(string text)
        {
            SetState(s =>
                s with
                {
                    Text = s.Text + text,
                    Capslock = s.Capslock switch
                    {
                        Capslock.Once => Capslock.Off,
                        _ => s.Capslock,
                    }
                }
            );
        }

        public void Clear()
        {
            SetState(s => s with { Text = "" });
        }

        public void ToggleCapslock()
        {
            SetState(s =>
                s with
                {
                    Capslock = s.Capslock switch
                    {
                        Capslock.Off => Capslock.Once,
                        Capslock.Once => Capslock.On,
                        Capslock.On => Capslock.Off,
                        _ => throw new NotSupportedException(),
                    }
                }
            );
        }

        public void Backspace()
        {
            if (State.Text.Length > 0)
            {
                SetState(s => s with { Text = s.Text[..^1] });
            }
        }

        public void Confirm()
        {
            ctx.DispatchListeners(new OnConfirm(State.Text));
            SetState(s => s with { IsOpen = false });
        }

        public void Cancel()
        {
            ctx.DispatchListeners(new OnCancel());
            SetState(s => s with { IsOpen = false });
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

        protected override XRKeyboardState Build()
        {
            gameObject.SetActive(State.IsOpen);
            return State ?? new XRKeyboardState(IsOpen: gameObject.activeSelf);
        }

        public record OnConfirm(string Text) : IElementEvent;

        public record OnCancel : IElementEvent;
    }
}
