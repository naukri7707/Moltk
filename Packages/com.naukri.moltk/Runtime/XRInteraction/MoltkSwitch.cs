using System;
using System.Linq;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Naukri.Moltk.XRInteraction
{
    public partial class MoltkSwitch : MoltkXRBehaviour, IState<string>
    {
        [SerializeField]
        private string currentState = "off";

        [SerializeField]
        private string[] states = new[] { "off", "on" };

        [SerializeField]
        private XRISwitchType switchType = XRISwitchType.OnSelectExited;

        [SerializeField]
        private UnityEvent<string> onStateChanged;

        public string CurrentStateName => currentState;

        public string[] States => states;

        public UnityEvent<string> OnStateChanged => onStateChanged;

        public XRISwitchType SwitchType => switchType;

        [Target]
        public void SetState(string stateName)
        {
            if (!states.Contains(stateName))
            {
                throw new ArgumentException("Invalid state value", nameof(stateName));
            }
            currentState = stateName;
            onStateChanged?.Invoke(stateName);
        }

        [Target]
        public void NextState()
        {
            var index = Array.IndexOf(states, currentState);
            var nextIndex = (index + 1) % states.Length;
            SetState(states[nextIndex]);
        }

        protected override void OnEnable()
        {
            if (switchType == XRISwitchType.OnSelectEntered)
            {
                interactable.selectEntered.AddListener(OnSelectEntered);
            }
            else if (switchType == XRISwitchType.OnSelectExited)
            {
                interactable.selectExited.AddListener(OnSelectExited);
            }
            else if (switchType == XRISwitchType.OnHoverEntered)
            {
                interactable.hoverEntered.AddListener(OnHoverEntered);
            }
            else if (switchType == XRISwitchType.OnHoverExited)
            {
                interactable.hoverExited.AddListener(OnHoverExited);
            }
        }

        protected override void OnDisable()
        {
            if (switchType == XRISwitchType.OnSelectEntered)
            {
                interactable.selectEntered.RemoveListener(OnSelectEntered);
            }
            else if (switchType == XRISwitchType.OnSelectExited)
            {
                interactable.selectExited.RemoveListener(OnSelectExited);
            }
            else if (switchType == XRISwitchType.OnHoverEntered)
            {
                interactable.hoverEntered.RemoveListener(OnHoverEntered);
            }
            else if (switchType == XRISwitchType.OnHoverExited)
            {
                interactable.hoverExited.RemoveListener(OnHoverExited);
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            NextState();
        }

        private void OnSelectExited(SelectExitEventArgs args)
        {
            NextState();
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            NextState();
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            NextState();
        }
    }

    [
        ScriptField,
        Base,
        Slot(nameof(currentState)),
        Slot(nameof(states)),
        Slot(nameof(switchType)),
        GroupScope("Events"),
            Slot(nameof(onStateChanged)),
        EndScope,
        GroupScope("Debug"),
            Slot(nameof(SetState)),
            Slot(nameof(NextState)),
        EndScope,
    ]
    partial class MoltkSwitch : MoltkXRBehaviour
    {
        public enum XRISwitchType
        {
            None,

            OnSelectEntered,

            OnSelectExited,

            OnHoverEntered,

            OnHoverExited,
        }
    }
}
