using Naukri.Physarum;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public abstract class XRKeyboardBinding : Consumer.Behaviour
    {
        [SerializeField]
        private XRKeyboardController keyboard;

        protected XRKeyboardController Keyboard
        {
            get
            {
                if (keyboard == null)
                {
                    keyboard = ctx.Read<XRKeyboardController>();
                }
                return keyboard;
            }
        }

        private Subscription subscription;

        #region methods

        public void Bind()
        {
            subscription?.Cancel();
            subscription = ctx.Listen(Keyboard);
        }

        public void Unbind()
        {
            subscription?.Cancel();
            subscription = null;
        }

        protected void OpenKeyboard(string text)
        {
            Keyboard.Open(this, text);
        }

        #endregion
    }
}
