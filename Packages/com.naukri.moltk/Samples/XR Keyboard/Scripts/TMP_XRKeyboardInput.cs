using Naukri.Moltk.Fusion;
using TMPro;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardTMP_Input : XRKeyboardBinding
    {
        [SerializeField]
        protected TMP_InputField inputField;

        [SerializeField]
        protected bool updateImmediately;

        protected string originalText;

        protected override void Reset()
        {
            base.Reset();
            inputField = GetComponent<TMP_InputField>();
        }

        protected override void OnInitialize(IContext ctx)
        {
            base.OnInitialize(ctx);
            inputField.onSelect.AddListener(OnInputFieldSelect);
        }

        protected override void Render()
        {
            var keyboardState = keyboardController.State;

            if (updateImmediately)
            {
                inputField.text = keyboardState.Text;
                inputField.MoveTextEnd(false);
                originalText = keyboardState.Text;
            }
        }

        protected override void HandleEvent(Provider sender, ProviderEvent evt)
        {
            switch (evt)
            {
                case XRKeyboardController.OnConfirm onConfirm:
                    originalText = inputField.text = onConfirm.Text;
                    inputField.MoveTextEnd(false);
                    break;

                case XRKeyboardController.OnCancel:
                    if (!updateImmediately)
                    {
                        inputField.text = originalText;
                    }
                    inputField.MoveTextEnd(false);
                    break;

                default:
                    break;
            }
        }

        private void OnInputFieldSelect(string text)
        {
            inputField.MoveTextEnd(false);
            originalText = text;
            OpenKeyboard(text);
        }
    }
}
