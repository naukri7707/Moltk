using Naukri.Physarum;
using TMPro;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardPreviewInput : Consumer.Behaviour
    {
        [SerializeField]
        private TMP_InputField inputField;

        protected override void Build()
        {
            var controller = XRKeyboardController.Of(this);
            ctx.Listen(controller);
            var state = controller.State;
            inputField.text = state.Text;
        }
    }
}
