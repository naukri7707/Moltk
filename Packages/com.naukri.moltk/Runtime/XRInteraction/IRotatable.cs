using Naukri.Moltk.Extensions;
using UnityEngine.Events;

namespace Naukri.Moltk.XRInteraction
{
    internal interface IRotatable
    {
        public MoltkPlane Plane { get; }

        public Axis RotateAxis { get; }

        public bool RotateGameObject { get; }

        public float CurrentAngle { get; }

        public UnityEvent<float> OnAngleChanged { get; }
    }
}
