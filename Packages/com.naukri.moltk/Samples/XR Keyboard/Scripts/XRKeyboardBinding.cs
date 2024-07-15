using Naukri.Physarum;

namespace Naukri.Moltk.XRKeyboard
{
    public abstract class XRKeyboardBinding : Consumer.Behaviour
    {
        private Subscription subscription;

        public void Bind()
        {
            subscription.Start();
        }

        public void Unbind()
        {
            subscription.Cancel();
        }

        protected override void Build()
        {
            subscription = ctx.Listen<XRKeyboardController>();
        }

        protected void OpenKeyboard(string text)
        {
            var keyboardController = ctx.Read<XRKeyboardController>();
            keyboardController.Open(this, text);
        }
    }
}
