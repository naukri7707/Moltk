using Naukri.Moltk.Fusion;

namespace Naukri.Moltk.XRKeyboard
{
    public abstract class XRKeyboardBinding : ViewController
    {
        protected XRKeyboardController keyboardController;

        private Subscription subscription;

        public void Bind()
        {
            subscription.Start();
        }

        public void Unbind()
        {
            subscription.Cancel();
        }

        protected override void OnInitialize(IContext ctx)
        {
            base.OnInitialize(ctx);
            keyboardController = ctx.Read<XRKeyboardController>();
            subscription = ctx.Listen<XRKeyboardController>();
        }

        protected void OpenKeyboard(string text)
        {
            keyboardController.Open(this, text);
        }
    }
}
