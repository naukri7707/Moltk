using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

namespace Naukri.Moltk.XRInteraction
{
    [RequireComponent(typeof(XRDirectInteractor))]
    public partial class DebugInteractor : MonoBehaviour
    {
        private XRDirectInteractor interactor;

        [SerializeField, Template]
        private XRBaseInteractable interactable;

        [SerializeField, Template]
        private bool selectOnStart;

        private XRInteractionManager InteractionManager => interactor.interactionManager;

        private bool IsRuntime => Application.isPlaying;

        protected virtual void Awake()
        {
            interactor = GetComponent<XRDirectInteractor>();
            interactor.selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Toggle;
        }

        protected virtual void Start()
        {
            if (selectOnStart)
            {
                Select();
            }
        }

        [Target]
        protected void Select()
        {
            InteractionManager.SelectEnter(interactor as IXRSelectInteractor, interactable);
        }

        [Target]
        protected void Deselect()
        {
            InteractionManager.SelectExit(interactor as IXRSelectInteractor, interactable);
        }

        [Target]
        protected void Hover()
        {
            InteractionManager.HoverEnter(interactor as IXRHoverInteractor, interactable);
        }

        [Target]
        protected void Dehover()
        {
            InteractionManager.HoverExit(interactor as IXRHoverInteractor, interactable);
        }

        private void OnDrawGizmos()
        {
            if (interactor == null || interactable == null)
            {
                return;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                interactor.transform.position,
                interactable.transform.position
                );
        }
    }

    [
        ScriptField,
        Slot(nameof(interactable), nameof(selectOnStart)),
        HelpBox("InteractorTester work on Runtime", HelpBoxMessageType.Warning), HideIf(nameof(IsRuntime)),
        ColumnScope, EnableIf(nameof(IsRuntime)),
            Base,
            Members,
        EndScope,
    ]
    partial class DebugInteractor
    {
    }
}
