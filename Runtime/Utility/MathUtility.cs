using UnityEngine;

namespace Naukri.Moltk.Utility
{
    public static class MathUtility
    {
        public static bool IsSimpleAngle(float angle)
        {
            return angle > -180F && angle <= 180F;
        }

        public static float ClampAngle(float angle, float x, float y)
        {
            if (IsAngleInRange(angle, x, y))
            {
                return angle;
            }
            else
            {
                var deltaToX = Mathf.DeltaAngle(angle, x);
                var deltaToY = Mathf.DeltaAngle(angle, y);
                if (Mathf.Abs(deltaToX) < Mathf.Abs(deltaToY))
                {
                    return x;
                }
                else
                {
                    return y;
                }
            }
        }

        public static bool IsAngleInRange(float angle, float x, float y)
        {
            if (x <= y)
            {
                return x <= angle && angle <= y;
            }
            else
            {
                return x <= angle || angle <= y;
            }
        }

        public static float ToSignedAngle(float angle)
        {
            angle = Mathf.Repeat(angle, 360f);

            if (angle > 180f)
            {
                angle -= 360f;
            }

            return angle;
        }
    }
}
