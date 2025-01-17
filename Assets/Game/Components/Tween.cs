using System;

namespace UnityToolkit
{
    public static class Tween
    {
        // public struct TweenData
        // {
        //     public float start;
        //     public float end;
        //     public float duration;
        //     public float time;
        //     public Func<float, float, float, float> easing;
        //     public Action<float> onUpdate;
        //     public Action onComplete;
        // }
        public delegate float EasingFunction(in float cur, in float target, in float percent);
        
        public static float Linear(in float cur, in float target, in float percent)
        {
            return cur + (target - cur) * percent;
        }

        public static float EaseIn(in float cur, in float target, in float percent)
        {
            return cur + (target - cur) * percent * percent;
        }

        public static float EaseOut(in float cur, in float target, in float percent)
        {
            return cur + (target - cur) * (1 - (1 - percent) * (1 - percent));
        }

        public static float EaseInOut(in float cur, in float target, in float percent)
        {
            return (float)(cur + (target - cur) *
                (percent < 0.5f ? percent * percent * 2 : 1 - Math.Pow(-2 * percent + 2, 2) / 2));
        }

        public static float Bounce(in float cur, in float target, in float percent)
        {
            return (float)(cur + (target - cur) *
                (1 - Math.Pow(1 - percent, 2) * Math.Abs(Math.Cos(percent * 10 * Math.PI))));
        }
    }
}