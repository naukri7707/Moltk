using System;
using System.Linq;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.Extensions;
using Naukri.Moltk.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Naukri.Moltk.XRInteraction
{
    public partial class MoltkKnob : MoltkXRBehaviour, IState<KnobState>, IRotatable
    {
        [SerializeField]
        private MoltkPlane plane;

        [SerializeField]
        private Axis rotateAxis = Axis.Forward;

        [SerializeField]
        [Tooltip("Rotate the game object of this component. You can disable it if you want to rotate the game object by yourself.")]
        private bool rotateGameObject = true;

        [SerializeField]
        private string currentStateName = "Gear 0";

        [SerializeField]
        private KnobState[] states = new[]
        {
            new KnobState { name = "Gear 0", preferredAngle = 0, angleRange = new Vector2(-15, 15) },
            new KnobState { name = "Gear 1", preferredAngle = 45, angleRange = new Vector2(15, 45) },
            new KnobState { name = "Gear 2", preferredAngle = -45, angleRange = new Vector2(-45, -15) },
        };

        [SerializeField]
        private UnityEvent<string> onStateChanged;

        private float currentAngle;

        [SerializeField]
        private UnityEvent<float> onAngleChanged;

        [SerializeField]
        [Tooltip("Limit the rotate angle of the knob.")]
        private bool limitAngle;

        [SerializeField]
        [Tooltip("The rotate angle range of limit, clockwise from X to Y.")]
        private Vector2 limitAngleRange;

        [SerializeField]
        [Tooltip("Limit the upper limit of the angle of rotation per frame.")]
        private bool softRotation;

        [SerializeField]
        [Tooltip("The maximum angle of rotation per frame.")]
        private float softRotationAngle = 2;

        public string CurrentStateName => currentStateName;

        public KnobState[] States => states;

        public UnityEvent<string> OnStateChanged => onStateChanged;

        public MoltkPlane Plane => plane;

        public Axis RotateAxis => rotateAxis;

        public bool RotateGameObject => rotateGameObject;

        public float CurrentAngle => currentAngle;

        public UnityEvent<float> OnAngleChanged => onAngleChanged;

        [Target]
        public void NextState()
        {
            if (states.Length == 0)
            {
                return;
            }
            var index = Array.FindIndex(states, it => it.name == currentStateName);
            var nextIndex = (index + 1) % states.Length;
            var nextStateName = states[nextIndex].name;
            SetState(nextStateName);
        }

        [Target]
        public void SetState(string stateName)
        {
            var state = states.FirstOrDefault(it => it.name == stateName) ?? throw new ArgumentException("Invalid state value", nameof(stateName));
            currentStateName = stateName;
            SetAngle(state.preferredAngle);
            onStateChanged?.Invoke(currentStateName);
        }

        protected override void Update()
        {
            var interactor = interactable.firstInteractorSelecting;
            if (interactor != null)
            {
                var angle = plane.CalcSignedAngle(interactor.transform.position);
                SetAngle(angle, true);
            }
        }

        [Template]
        protected virtual void SetAngle(float angle, bool useCustomizer = false)
        {
            if (useCustomizer)
            {
                if (limitAngle)
                {
                    angle = MathUtility.ClampAngle(angle, limitAngleRange.x, limitAngleRange.y);
                }

                if (softRotation)
                {
                    var x = Mathf.DeltaAngle(0, currentAngle - softRotationAngle);
                    var y = Mathf.DeltaAngle(0, currentAngle + softRotationAngle);
                    angle = MathUtility.ClampAngle(angle, x, y);
                }
            }

            // Update angle and state (If changed)
            if (angle != currentAngle)
            {
                var newStateName = GetAngleStateName(angle) ?? throw new Exception("No state found");

                if (newStateName != currentStateName)
                {
                    SetState(newStateName);
                }

                currentAngle = angle;
                onAngleChanged?.Invoke(currentAngle);

                if (rotateGameObject)
                {
                    transform.SetRotationWithAxisAngle(angle, rotateAxis);
                }
            }
        }

        private string GetAngleStateName(float angle)
        {
            return states.FirstOrDefault(it => MathUtility.IsAngleInRange(angle, it.angleRange.x, it.angleRange.y))?.name;
        }

        private void AddPlane()
        {
            plane = MoltkPlane.Create(transform);
        }
    }

    [
        ScriptField,
        Base,
        Slot(nameof(plane)),
        RowScope, ShowIf(nameof(plane), null),
            HelpBox("You may need a plane to calculate the angle of rotation.", HelpBoxMessageType.Warning),
            Button("Add Plane", binding: nameof(AddPlane)),
        EndScope,
        Slot(nameof(rotateAxis)),
        Slot(nameof(rotateGameObject)),
        Slot(nameof(currentStateName)),
        Slot(nameof(states)),
        GroupScope("Optionals"),
            Slot(nameof(limitAngle)),
            ColumnScope, ShowIf(nameof(limitAngle)), Style(paddingLeft: "20"),
                Slot(nameof(limitAngleRange)),
            EndScope,
            Slot(nameof(softRotation)),
            ColumnScope, ShowIf(nameof(softRotation)), Style(paddingLeft: "20"),
                Slot(nameof(softRotationAngle)),
            EndScope,
        EndScope,
        GroupScope("Events"),
            Slot(nameof(onStateChanged)),
            Slot(nameof(onAngleChanged)),
        EndScope,
        GroupScope("Debug"),
            Slot(nameof(SetState)),
            Slot(nameof(NextState)),
            Slot(nameof(SetAngle)),
        EndScope,
    ]
    partial class MoltkKnob
    {
    }
}
