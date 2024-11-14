using Naukri.Physarum;
using TMPro;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardTMPInput : XRKeyboardBinding
    {
        #region fields
        protected TMP_InputField inputField;

        [SerializeField]
        protected bool updateImmediately;

        protected string originalText;
        #endregion

        #region methods

        protected override void Awake()
        {
            base.Awake();
            inputField = GetComponent<TMP_InputField>();
            inputField.onSelect.AddListener(OnInputFieldSelect);
        }

        protected override void Build()
        {
            var keyboardState = Keyboard.State;

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
                    break;

                case XRKeyboardController.OnCancel:
                    if (!updateImmediately)
                    {
                        inputField.text = originalText;
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnInputFieldSelect(string text)
        {
            originalText = text;
            inputField.caretPosition = inputField.text.Length;
            inputField.ForceLabelUpdate();
            OpenKeyboard(text);
        }

        #endregion
    }
}
