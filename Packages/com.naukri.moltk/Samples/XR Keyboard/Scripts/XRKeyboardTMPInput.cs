using Naukri.Physarum;
using TMPro;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardTMPInput : XRKeyboardBinding
    {
        [SerializeField]
        protected TMP_InputField inputField;

        [SerializeField]
        protected bool updateImmediately;

        protected string originalText;

        protected virtual void Reset()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        protected override void Awake()
        {
            base.Awake();
            inputField.onSelect.AddListener(OnInputFieldSelect);
        }

        protected override void Build()
        {
            var keyboardState = ctx.Read<XRKeyboardController>().State;

            if (updateImmediately)
            {
                inputField.text = keyboardState.Text;
                inputField.MoveTextEnd(false);
                originalText = keyboardState.Text;
            }
        }

        protected override void HandleEvent(object sender, IElementEvent evt)
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
