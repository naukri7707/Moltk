using Naukri.InspectorMaid;
using Naukri.Moltk.MVU;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Naukri.Moltk.XRKeyboard
{
    public abstract class XRKeyboardButton : ConsumerBehaviour
    {
        protected XRKeyboardController controller;

        [
                    SerializeField,
            GroupScope("Dispaly"),
            HelpBox("The character shown if dispalyIcon is null, otherwise the icon will be shown.", UnityEngine.UIElements.HelpBoxMessageType.Info),
            Target,
            Slot(
            nameof(shiftDisplayCharacter),
            nameof(displayIcon),
            nameof(shiftDisplayIcon)
            ),
            Button("Update View", nameof(UpdateView))
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
            Slot(
            nameof(characterText),
            nameof(iconImage),
            nameof(highlightImage)
            )
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

        [Template]
        public void UpdateView()
        {
            var enable = displayIcon != null;
            iconImage.enabled = enable;
            characterText.enabled = !enable;
            iconImage.sprite = displayIcon;
            characterText.SetText(displayCharacter);
        }

        protected override void Reset()
        {
            base.Reset();
            var images = GetComponentsInChildren<Image>();
            button = GetComponentInChildren<Button>();
            iconImage = Array.Find(images, it => it.name == "Icon");
            characterText = GetComponentInChildren<TMP_Text>();
            highlightImage = Array.Find(images, it => it.name == "Highlight");
        }

        protected override void Awake()
        {
            base.Awake();
            controller = XRKeyboardController.Of(this);
            Subscribe(controller);
            // 更新 View 到當前狀態
            UpdateView();
            button.onClick.AddListener(OnClicked);
        }

        protected abstract void OnClicked();

        protected override void Build(IProvider provider)
        {
            if (provider is XRKeyboardController controller)
            {
                var isShift = controller.State.Capslock != Capslock.Off;

                characterText.text = isShift ? shiftDisplayCharacter : displayCharacter;
                iconImage.sprite = isShift ? shiftDisplayIcon : displayIcon;
            }
        }
    }
}
