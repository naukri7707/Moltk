using Naukri.Physarum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Naukri.Moltk.XRKeyboard
{
    [RequireComponent(typeof(InputField))]
    public class XRKeyboardInput : XRKeyboardBinding, IPointerClickHandler
    {
        #region fields
        protected InputField inputField;

        [SerializeField]
        protected bool updateImmediately;

        protected string originalText;
        #endregion

        #region methods

        public void OnPointerClick(PointerEventData eventData)
        {
            OnInputFieldSelect(inputField.text);
        }

        protected virtual void Reset()
        {
            inputField = GetComponent<InputField>();
        }

        protected override void Awake()
        {
            base.Awake();
            inputField = GetComponent<InputField>();
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
            inputField.MoveTextEnd(false);
            OpenKeyboard(text);
        }

        #endregion
    }
}
