using System;
using Naukri.Physarum;
using Naukri.Physarum.Core;
using UnityEngine;
using UnityEngine.Assertions;

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
    );

    partial class XRKeyboardController
    {
        public static XRKeyboardController Of(Component component)
        {
            Assert.IsNotNull(component);

            return component.GetComponentInParent<XRKeyboardController>();
        }
    }

    public partial class XRKeyboardController : ViewController<XRKeyboardState>.Behaviour
    {
        [SerializeField]
        private GameObject uiRoot;

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
            SetState(s => s with { IsOpen = false });
            ctx.DispatchListeners(new OnConfirm(State.Text));
        }

        public void Cancel()
        {
            SetState(s => s with { IsOpen = false });
            ctx.DispatchListeners(new OnCancel());
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
            var state = State ?? new XRKeyboardState(IsOpen: uiRoot.activeSelf);
            uiRoot.SetActive(state.IsOpen);
            return state;
        }

        public record OnConfirm(string Text) : IElementEvent;

        public record OnCancel : IElementEvent;
    }
}
