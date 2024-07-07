using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

namespace Naukri.Moltk.XRInteraction
{
    public abstract partial class MoltkXRBehaviour : MonoBehaviour
    {
        protected XRBaseInteractable interactable;

        private bool IsGameObjectHasInteractable => GetComponent<XRBaseInteractable>() != null;

        protected virtual void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        protected virtual void Start() { }

        protected virtual void OnDestroy() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void Update() { }

        private void AddXRSimpleInteractable()
        {
            gameObject.AddComponent<XRSimpleInteractable>();
        }

        private void AddXRGrabInteractable()
        {
            gameObject.AddComponent<XRGrabInteractable>();
        }
    }

    [
        ColumnScope, HideIf(nameof(IsGameObjectHasInteractable)),
            HelpBox("You must add 'XRSimpleInteractable' or 'XRGrabInteractable' on this gameobject otherwise the script may not work correctly.", HelpBoxMessageType.Error),
            RowScope,
            Button("Add Simple", binding: nameof(AddXRSimpleInteractable)), Style(flexGrow: "1"),
            Button("Add Grab", binding: nameof(AddXRGrabInteractable)), Style(flexGrow: "1"),
            EndScope,
        EndScope,
        Members,
    ]
    partial class MoltkXRBehaviour
    {
    }
}
