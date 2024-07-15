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
            var keyboardState = ctx.Watch<XRKeyboardController>().State;
            inputField.text = keyboardState.Text;
        }
    }
}
