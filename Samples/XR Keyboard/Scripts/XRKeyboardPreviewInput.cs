using Naukri.Moltk.Fusion;
using TMPro;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardPreviewInput : ViewController
    {
        public TMP_InputField inputField;

        private XRKeyboardController keyboardController;

        protected override void OnInitialize(IContext ctx)
        {
            base.OnInitialize(ctx);
            keyboardController = ctx.Watch<XRKeyboardController>();
        }

        protected override void Render()
        {
            var keyboardState = keyboardController.State;
            inputField.text = keyboardState.Text;
        }
    }
}
