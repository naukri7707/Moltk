using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.Extensions;
using Naukri.Moltk.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Naukri.Moltk.XRInteraction
{
    public partial class MoltkHandwheel : MoltkXRBehaviour, IRotatable
    {
        [SerializeField]
        private MoltkPlane plane;

        [SerializeField]
        private Axis rotateAxis = Axis.Forward;

        [SerializeField]
        [Tooltip("Rotate the game object of this component. You can disable it if you want to rotate the game object by yourself.")]
        private bool rotateGameObject = true;

        private float currentAngle;

        [SerializeField]
        private UnityEvent<float> onAngleChanged;

        [SerializeField]
        [Tooltip("Limit the upper limit of the angle of rotation per frame.")]
        private bool softRotation;

        [SerializeField]
        [Tooltip("The maximum angle of rotation per frame.")]
        private float softRotationAngle = 2;

        [SerializeField]
        [Tooltip("Limit the rotate angle of the handwheel.")]
        private bool limitRotation;

        private float deltaAngle;

        [SerializeField]
        [Tooltip("The limit of clockwise round.")]
        private float clockwiseRoundLimit = 1;

        [SerializeField]
        [Tooltip("The limit of counter clockwise round.")]
        private float counterClockwiseLimit = 1;

        [SerializeField]
        private UnityEvent onClockwiseLimitReached;

        [SerializeField]
        private UnityEvent onCounterClockwiseLimitReached;

        public MoltkPlane Plane => plane;

        public Axis RotateAxis => rotateAxis;

        public bool RotateGameObject => rotateGameObject;

        public float CurrentAngle => currentAngle;

        public UnityEvent<float> OnAngleChanged => onAngleChanged;

        public UnityEvent OnClockwiseLimitReached => onClockwiseLimitReached;

        public UnityEvent OnCounterClockwiseLimitReached => onCounterClockwiseLimitReached;

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
                if (softRotation)
                {
                    var x = Mathf.DeltaAngle(0, currentAngle - softRotationAngle);
                    var y = Mathf.DeltaAngle(0, currentAngle + softRotationAngle);
                    angle = MathUtility.ClampAngle(angle, x, y);
                }
                if (limitRotation)
                {
                    var clockwiseAngleLimit = clockwiseRoundLimit * 360;
                    var counterClockwiseAngleLimit = -counterClockwiseLimit * 360;

                    var unlimitDelta = deltaAngle + Mathf.DeltaAngle(currentAngle, angle);

                    if (unlimitDelta < counterClockwiseAngleLimit)
                    {
                        deltaAngle = counterClockwiseAngleLimit;
                        var tureDelta = Mathf.DeltaAngle(currentAngle, counterClockwiseAngleLimit);
                        angle = currentAngle + tureDelta;
                        onCounterClockwiseLimitReached?.Invoke();
                    }
                    else if (unlimitDelta > clockwiseAngleLimit)
                    {
                        deltaAngle = clockwiseAngleLimit;
                        var tureDelta = Mathf.DeltaAngle(currentAngle, clockwiseAngleLimit);
                        angle = currentAngle + tureDelta;
                        onClockwiseLimitReached?.Invoke();
                    }
                    else
                    {
                        deltaAngle = unlimitDelta;
                    }
                }
            }

            // Update angle
            if (angle != currentAngle)
            {
                currentAngle = angle;
                onAngleChanged?.Invoke(currentAngle);

                if (rotateGameObject)
                {
                    transform.SetRotationWithAxisAngle(angle, rotateAxis);
                }
            }
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
            HelpBox("You may need a plane to calculate the angle of rotation.", HelpBoxMessageType.Warning), Style(flexGrow: "1"),
            Button("Add Plane", binding: nameof(AddPlane)),
        EndScope,
        Slot(nameof(rotateAxis)),
        Slot(nameof(rotateGameObject)),
        GroupScope("Optionals"),
            Slot(nameof(limitRotation)),
            ColumnScope, ShowIf(nameof(limitRotation)), Style(paddingLeft: "20"),
                Slot(nameof(clockwiseRoundLimit)),
                Slot(nameof(counterClockwiseLimit)),
            EndScope,
            Slot(nameof(softRotation)),
            ColumnScope, ShowIf(nameof(softRotation)), Style(paddingLeft: "20"),
                Slot(nameof(softRotationAngle)),
            EndScope,
        EndScope,
        GroupScope("Events"),
            Slot(nameof(onAngleChanged)),
            ColumnScope, ShowIf(nameof(limitRotation)),
                Slot(nameof(onClockwiseLimitReached)),
                Slot(nameof(onCounterClockwiseLimitReached)),
            EndScope,
        EndScope,
        GroupScope("Debug"),
            Slot(nameof(SetAngle)),
        EndScope,
    ]
    partial class MoltkHandwheel
    {
    }
}
