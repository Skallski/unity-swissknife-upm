using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Core
{
    internal static class TweenUtility
    {
        internal static float GetDeltaTime(TweenTimeMode mode)
        {
            return mode switch
            {
                TweenTimeMode.ScaledTime => Time.deltaTime,
                TweenTimeMode.UnscaledTime => Time.unscaledDeltaTime,
                _ => Time.deltaTime
            };
        }
    }
}