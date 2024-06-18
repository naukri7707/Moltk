using Naukri.InspectorMaid;
using Naukri.Moltk.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace Naukri.Moltk.XRInteraction
{
    [System.Serializable]
    public class KnobState : IInspectorMaidTarget
    {
        public string name;

        [HelpBox("Invalid value, angle must be within -180 to 180.", HelpBoxMessageType.Error), HideIf(nameof(IsStateAngleValid))]
        [HelpBox("'StateAngle' must be within the 'AngleRange'.", HelpBoxMessageType.Error), HideIf(nameof(IsStateAngleInRange))]
        public float preferredAngle;

        [HelpBox("Invalid value, angle must be within -180 to 180.", HelpBoxMessageType.Error), HideIf(nameof(IsAngleRangeValid))]
        [Tooltip("The angle range of state, clockwise from X to Y.")]
        public Vector2 angleRange;

        private bool IsStateAngleInRange => MathUtility.IsAngleInRange(preferredAngle, angleRange.x, angleRange.y);

        private bool IsStateAngleValid => MathUtility.IsSimpleAngle(preferredAngle);

        private bool IsAngleRangeValid => MathUtility.IsSimpleAngle(angleRange.x) && MathUtility.IsSimpleAngle(angleRange.y);
    }
}
