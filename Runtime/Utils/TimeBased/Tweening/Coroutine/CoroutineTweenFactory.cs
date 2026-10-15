using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UtilsToolbox.Utils.TimeBased.Tweening.Core;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Coroutine
{
    public static class CoroutineTweenFactory
    {
        #region PUBLIC METHODS

        public static CoroutineTween TweenScale(MonoBehaviour caller, TweenTimeMode mode, GameObject target, 
            Vector3 startScale, Vector3 finishScale, float duration, Action onFinish = null)
        {
            return Tween(caller, mode, duration, TweenInterpolation.Scale(target, startScale, finishScale), onFinish);
        }

        public static CoroutineTween TweenAlpha(MonoBehaviour caller, TweenTimeMode mode, CanvasGroup target, 
            float startAlpha, 
            float finishAlpha, float duration, Action onFinish = null)
        {
            return Tween(caller, mode, duration, TweenInterpolation.Alpha(target, startAlpha, finishAlpha), onFinish);
        }

        public static CoroutineTween TweenColor(MonoBehaviour caller, TweenTimeMode mode, Graphic target, 
            Color startColor, Color finishColor, float duration, Action onFinish = null)
        {
            return Tween(caller, mode, duration, TweenInterpolation.Color(target, startColor, finishColor), onFinish);
        }

        public static CoroutineTween TweenAnchoredPosition(MonoBehaviour caller, TweenTimeMode mode, RectTransform target,
            Vector2 startPosition, Vector2 finishPosition, float duration, Action onFinish = null)
        {
            return Tween(caller, mode, duration, TweenInterpolation.AnchoredPosition(target, startPosition, finishPosition), 
                onFinish);
        }

        public static CoroutineTween TweenSprite(MonoBehaviour caller, TweenTimeMode mode, Image image, 
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

            return Tween(caller, mode, duration, TweenInterpolation.Sprite(image, sprites), onFinish);
        }

        /// <summary>
        /// Creates a custom tween.
        /// onTick receives normalized progress from 0 to 1.
        /// </summary>
        public static CoroutineTween Tween(MonoBehaviour caller, TweenTimeMode mode, float duration, 
            Action<float> onTick, Action onFinish = null)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            Validate(duration, onTick);

            Action onComplete = () =>
            {
                onTick(1f);
                onFinish?.Invoke();
            };

            CoroutineTween tween = new CoroutineTween(caller, onComplete);

            UnityEngine.Coroutine coroutine = caller.StartCoroutine(TweenCoroutine(tween, mode, duration, onTick, onFinish));

            tween.Bind(coroutine);

            return tween;
        }

        public static void AbortTween(CoroutineTween tween, Action onAbort = null)
        {
            if (tween is not { IsActive: true })
            {
                return;
            }

            tween.Abort();
            onAbort?.Invoke();
        }

        public static void SkipTween(CoroutineTween tween, Action onSkip = null)
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

        private static IEnumerator TweenCoroutine(CoroutineTween tween, TweenTimeMode mode, float duration, 
            Action<float> onTick, Action onFinish)
        {
            float elapsedTime = 0f;

            while (tween.IsActive && elapsedTime < duration)
            {
                float progress = Mathf.Clamp01(elapsedTime / duration);

                onTick(progress);

                elapsedTime += TweenUtility.GetDeltaTime(mode);

                yield return null;
            }

            if (tween.IsActive == false)
            {
                yield break;
            }

            // Final value.
            onTick(1f);

            // Update state before notifying user code.
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