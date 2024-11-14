using System;
using Naukri.InspectorMaid;
using Naukri.Physarum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Naukri.Moltk.XRKeyboard
{
    public abstract partial class XRKeyboardButton : Consumer.Behaviour
    {
        [
            SerializeField,
            GroupScope("Display"),
            HelpBox(
                "The character shown if displayIcon is null, otherwise the icon will be shown.",
                UnityEngine.UIElements.HelpBoxMessageType.Info
            ),
            Target,
            Slot(nameof(shiftDisplayCharacter), nameof(displayIcon), nameof(shiftDisplayIcon)),
            Button("Update View", nameof(UpdateViewInitialState))
        ]
        protected string displayCharacter;

        [SerializeField, Template]
        protected string shiftDisplayCharacter;

        [SerializeField, Template]
        protected Sprite displayIcon;

        [SerializeField, Template]
        protected Sprite shiftDisplayIcon;

        [
            SerializeField,
            GroupScope("References"),
            Target,
            Slot(nameof(characterText), nameof(iconImage), nameof(highlightImage))
        ]
        protected Button button;

        [SerializeField, Template]
        protected TMP_Text characterText;

        [SerializeField, Template]
        protected Image iconImage;

        [SerializeField, Template]
        protected Image highlightImage;

        public void Highlight(bool highlight)
        {
            highlightImage.enabled = highlight;
        }

        protected override void Build()
        {
            var keyboardController = ctx.Watch<XRKeyboardController>();
            var keyboardState = keyboardController.State;
            var isShift = keyboardState.Capslock != Capslock.Off;

            characterText.text = isShift ? shiftDisplayCharacter : displayCharacter;
            iconImage.sprite = isShift ? shiftDisplayIcon : displayIcon;
        }

        protected override void Awake()
        {
            base.Awake();

            // 根據欄位設定更新 View 初始狀態
            UpdateViewInitialState();
            button.onClick.AddListener(OnClicked);
        }

        protected abstract void OnClicked();
    }

    // Editor
    partial class XRKeyboardButton
    {
        protected virtual void Reset()
        {
            var images = GetComponentsInChildren<Image>();
            button = GetComponentInChildren<Button>();
            iconImage = Array.Find(images, it => it.name == "Icon");
            characterText = GetComponentInChildren<TMP_Text>();
            highlightImage = Array.Find(images, it => it.name == "Highlight");
        }

        // 根據欄位設定更新 View 初始狀態
        [Template]
        private void UpdateViewInitialState()
        {
            var enable = displayIcon != null;
            iconImage.enabled = enable;
            characterText.enabled = !enable;
            iconImage.sprite = displayIcon;
            characterText.SetText(displayCharacter);
        }
    }
}
