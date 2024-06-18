using UnityEngine;

namespace Naukri.Moltk.Extensions
{
    public static class UnityExtensions
    {
        public static void SetRotationWithAxisAngle(this Transform transform, float angle, Axis axis)
        {
            var newEulerAngle = transform.localEulerAngles;

            if (axis == Axis.Forward)
            {
                newEulerAngle.z = angle;
            }
            else if (axis == Axis.Back)
            {
                newEulerAngle.z = -angle;
            }
            else if (axis == Axis.Left)
            {
                newEulerAngle.x = angle;
            }
            else if (axis == Axis.Right)
            {
                newEulerAngle.x = -angle;
            }
            else if (axis == Axis.Up)
            {
                newEulerAngle.y = angle;
            }
            else if (axis == Axis.Down)
            {
                newEulerAngle.y = -angle;
            }

            transform.localEulerAngles = newEulerAngle;
        }
    }
}
