using UnityEngine;

namespace Incode.Utils
{
    public static class EaseUtils
    {
        private const float PI = Mathf.PI;

        #region Sine
        public static float EaseInSine(float value)
        {
            return 1 - Mathf.Cos((value * PI) * 0.5f);
        }

        public static float EaseOutSine(float value)
        {
            return Mathf.Sin((value * PI) * 0.5f);
        }

        public static float EaseInOutSine(float value)
        {
            return -(Mathf.Cos(PI * value) - 1) * 0.5f;
        }
        #endregion

        #region Quad
        public static float EaseInQuad(float value)
        {
            return value * value;
        }

        public static float EaseOutQuad(float value)
        {
            return 1 - (1 - value) * (1 - value);
        }

        public static float EaseInOutQuad(float value)
        {
            return value < 0.5f ? 2 * value * value : 1 - (Mathf.Pow(-2 * value + 2, 2) * 0.5f);
        }
        #endregion

        #region Cubic
        public static float EaseInCubic(float value)
        {
            return value * value * value;
        }

        public static float EaseOutCubic(float value)
        {
            return 1 - (Mathf.Pow(1 - value, 3));
        }

        public static float EaseInOutCubic(float value)
        {
            return value < 0.5f ? 4 * value * value * value : 1 - (Mathf.Pow(-2 * value + 2, 3) * 0.5f);
        }
        #endregion

        #region Quart
        public static float EaseInQuart(float value)
        {
            return value * value * value * value;
        }

        public static float EaseOutQuart(float value)
        {
            return 1 - (Mathf.Pow(1 - value, 4));
        }

        public static float EaseInOutQuart(float value)
        {
            return value < 0.5f ? 8 * value * value * value * value : 1 - (Mathf.Pow(-2 * value + 2, 4) * 0.5f);
        }
        #endregion

        #region Quint
        public static float EaseInQuint(float value)
        {
            return value * value * value * value * value;
        }

        public static float EaseOutQuint(float value)
        {
            return 1 - (Mathf.Pow(1 - value, 5));
        }

        public static float EaseInOutQuint(float value)
        {
            return value < 0.5f ? 16 * value * value * value * value * value : 1 - (Mathf.Pow(-2 * value + 2, 5) * 0.5f);
        }
        #endregion

        #region Expo
        public static float EaseInExpo(float value)
        {
            return value == 0 ? 0 : Mathf.Pow(2, 10 * value - 10);
        }

        public static float EaseOutExpo(float value)
        {
            return value == 1 ? 1 : 1 - (Mathf.Pow(2, -10 * value));
        }

        public static float EaseInOutExpo(float value)
        {
            if (value == 0 || value == 1) return value;

            return value < 0.5f ? Mathf.Pow(2, 20 * value - 10) * 0.5f : (2 - (Mathf.Pow(2, -20 * value + 10))) * 0.5f;
        }
        #endregion

        #region Circ
        public static float EaseInCirc(float value)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(value, 2));
        }

        public static float EaseOutCirc(float value)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(value - 1, 2));
        }

        public static float EaseInOutCirc(float value)
        {
            return value < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * value, 2))) * 0.5f : (Mathf.Sqrt(1 - Mathf.Pow(-2 * value + 2, 2)) + 1) * 0.5f;
        }
        #endregion

        #region Back
        public static float EaseInBack(float value)
        {
            const float dip = 1.70158f;
            const float correction = dip + 1;

            return correction * value * value * value - dip * value * value;
        }

        public static float EaseOutBack(float value)
        {
            const float dip = 1.70158f;
            const float correction = dip + 1;

            return 1 + (correction * Mathf.Pow(value - 1, 3)) + (dip * Mathf.Pow(value - 1, 2));
        }

        public static float EaseInOutBack(float value)
        {
            const float dip = 1.70158f;
            const float correction = dip * 1.525f;

            return value < 0.5f ? (Mathf.Pow(2 * value, 2) * ((correction + 1) * 2 * value - correction)) * 0.5f : (Mathf.Pow(2 * value - 2, 2) * ((correction + 1) * (value * 2 - 2) + correction) + 2) * 0.5f;
        }
        #endregion

        #region Elastic
        public static float EaseInElastic(float value)
        {
            if (value == 0 || value == 1) return value;

            const float dip = (2 * Mathf.PI) / 3f;
            return -Mathf.Pow(2, 10 * value - 10) * Mathf.Sin((value * 10 - 10.75f) * dip);
        }

        public static float EaseOutElastic(float value)
        {
            if (value == 0 || value == 1) return value;

            const float dip = (2 * Mathf.PI) / 3f;
            return Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 10 - 0.75f) * dip) + 1;
        }

        public static float EaseInOutElastic(float value)
        {
            if (value == 0 || value == 1) return value;

            const float dip = (2 * Mathf.PI) / 4.5f;
            return value < 0.5f ? -(Mathf.Pow(2, 20 * value - 10) * Mathf.Sin((20 * value - 11.125f) * dip)) * 0.5f : (Mathf.Pow(2, -20 * value + 10) * Mathf.Sin((20 * value - 11.125f) * dip)) * 0.5f + 1;
        }
        #endregion

        #region Bounce
        public static float EaseInBounce(float value)
        {
            return 1 - EaseOutBounce(1 - value);
        }

        public static float EaseOutBounce(float value)
        {
            const float bounce = 7.5625f;
            const float period = 2.75f;

            if (value < 1f / period)
            {
                return bounce * value * value;
            }
            else if (value < 2 / period)
            {
                return bounce * (value -= 1.5f / period) * value + 0.75f;
            }
            else if (value < 2.5 / period)
            {
                return bounce * (value -= 2.25f / period) * value + 0.9375f;
            }
            else
            {
                return bounce * (value -= 2.625f / period) * value + 0.984375f;
            }
        }

        public static float EaseInOutBounce(float value)
        {
            return value < 0.5 ? (1 - EaseOutBounce(1 - 2 * value)) * 0.5f : (1 + EaseOutBounce(2 * value - 1)) * 0.5f;
        }
        #endregion
    }
}