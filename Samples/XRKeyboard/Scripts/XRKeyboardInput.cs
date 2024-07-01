using Naukri.Moltk.MVU;
using TMPro;
using UnityEngine;

namespace Naukri.Moltk.XRKeyboard
{
    public class XRKeyboardInput : XRKeyboardBinding
    {
        [SerializeField]
        protected TMP_InputField inputField;

        [SerializeField]
        protected bool updateImmediately = true;

        [SerializeField]
        protected string originalText;

        public void TrackKeyboard()
        {
            Subscribe(Keyboard);
        }

        protected override void Reset()
        {
            base.Reset();
            inputField = GetComponent<TMP_InputField>();
        }

        protected override void Awake()
        {
            base.Awake();
            inputField.onSelect.AddListener(OpenKeyboard);
        }

        protected override void Build(IProvider provider)
        {
            if (provider is XRKeyboardController keyboardController)
            {
                var state = keyboardController.State;

                if (updateImmediately)
                {
                    inputField.text = state.Text;
                    inputField.MoveTextEnd(false);
                    originalText = state.Text;
                }
            }
        }

        protected override void HandleEvent(IProvider provider, IProviderEvent evt)
        {
            if (provider is XRKeyboardController xRKeyboardController)
            {
                switch (evt)
                {
                    case XRKeyboardController.OnConfirm:
                        inputField.text = xRKeyboardController.State.Text;
                        inputField.MoveTextEnd(false);
                        Unsubscribe(Keyboard);
                        break;

                    case XRKeyboardController.OnCancel:
                        inputField.text = originalText;
                        inputField.MoveTextEnd(false);
                        Unsubscribe(Keyboard);
                        break;
                }
            }
        }

        private void OpenKeyboard(string text)
        {
            originalText = text;
            Subscribe(Keyboard);
            Keyboard.Open(text);
        }
    }
}
