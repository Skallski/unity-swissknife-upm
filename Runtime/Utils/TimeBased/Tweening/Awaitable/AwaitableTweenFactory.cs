using System;
using UnityEngine;
using UnityEngine.UI;
using UtilsToolbox.Utils.TimeBased.Tweening.Core;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Awaitable
{
    public static class AwaitableTweenFactory
    {
        #region PUBLIC METHODS
        public static AwaitableTween TweenScale(TweenTimeMode mode, GameObject target, 
            Vector3 startScale, Vector3 finishScale, float duration, Action onFinish = null)
        {
            return Tween(mode, duration, TweenInterpolation.Scale(target, startScale, finishScale), onFinish);
        }

        public static AwaitableTween TweenAlpha(TweenTimeMode mode, CanvasGroup target, 
            float startAlpha, float finishAlpha, float duration, Action onFinish = null)
        {
            return Tween(mode, duration, TweenInterpolation.Alpha(target, startAlpha, finishAlpha), onFinish);
        }

        public static AwaitableTween TweenColor(TweenTimeMode mode, Graphic target, 
            Color startColor, Color finishColor, float duration, Action onFinish = null)
        {
            return Tween(mode, duration, TweenInterpolation.Color(target, startColor, finishColor), onFinish);
        }

        public static AwaitableTween TweenAnchoredPosition(TweenTimeMode mode, RectTransform target, 
            Vector2 startPosition, Vector2 finishPosition, float duration, Action onFinish = null)
        {
            return Tween(mode, duration, TweenInterpolation.AnchoredPosition(target, startPosition, finishPosition),
                onFinish);
        }

        public static AwaitableTween TweenSprite(TweenTimeMode mode, Image image, 
            Sprite[] sprites, float frameDuration, Action onFinish = null)
        {
            if (frameDuration is <= 0f or Single.NaN || float.IsInfinity(frameDuration))
            {
                throw new ArgumentOutOfRangeException(nameof(frameDuration));
            }

            if (sprites == null || sprites.Length == 0)
            {
                throw new ArgumentException("Sprites cannot be null or empty.", nameof(sprites));
            }

            float duration = sprites.Length * frameDuration;

            return Tween(mode, duration, TweenInterpolation.Sprite(image, sprites), onFinish);
        }

        /// <summary>
        /// Creates a custom tween.
        /// onTick receives normalized progress from 0 to 1.
        /// </summary>
        public static AwaitableTween Tween(TweenTimeMode mode, float duration, 
            Action<float> onTick, Action onFinish = null)
        {
            Validate(duration, onTick);

            Action onComplete = () =>
            {
                onTick(1f);
                onFinish?.Invoke();
            };

            AwaitableTween tween = new AwaitableTween(onComplete);

            RunTweenAsync(tween, mode, duration, onTick, onFinish);

            return tween;
        }

        public static void AbortTween(AwaitableTween tween, Action onAbort = null)
        {
            if (tween is not { IsActive: true })
            {
                return;
            }

            tween.Abort();
            onAbort?.Invoke();
        }

        public static void SkipTween(AwaitableTween tween, Action onSkip = null)
        {
            if (tween is not { IsActive: true })
            {
                return;
            }

            tween.Skip();
            onSkip?.Invoke();
        }
        #endregion

        #region PRIVATE METHODS
        private static async UnityEngine.Awaitable RunTweenAsync(AwaitableTween tween, TweenTimeMode mode, float duration,
            Action<float> onTick, Action onFinish)
        {
            float elapsedTime = 0f;

            while (tween.IsActive && elapsedTime < duration)
            {
                float progress = Mathf.Clamp01(elapsedTime / duration);

                onTick(progress);

                elapsedTime += TweenUtility.GetDeltaTime(mode);

                await UnityEngine.Awaitable.NextFrameAsync();
            }

            if (tween.IsActive == false)
            {
                return;
            }

            onTick(1f);

            tween.Complete();

            onFinish?.Invoke();
        }

        private static void Validate(float duration, Action<float> onTick)
        {
            if (duration is < 0f or Single.NaN || float.IsInfinity(duration))
            { 
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            if (onTick == null)
            {
                throw new ArgumentNullException(nameof(onTick));
            }
        }
        #endregion
    }
}