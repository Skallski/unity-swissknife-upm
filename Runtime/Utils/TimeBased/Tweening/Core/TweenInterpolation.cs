using System;
using UnityEngine;
using UnityEngine.UI;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Core
{
    internal static class TweenInterpolation
    {
        internal static Action<float> Scale(GameObject target, Vector3 startScale, Vector3 finishScale)
        {
            return target == null
                ? throw new ArgumentNullException(nameof(target))
                : progress =>
                {
                    target.transform.localScale = Vector3.Lerp(startScale, finishScale, progress);
                };
        }

        internal static Action<float> Alpha(CanvasGroup target, float startAlpha, float finishAlpha)
        {
            return target == null
                ? throw new ArgumentNullException(nameof(target))
                : progress =>
                {
                    target.alpha = Mathf.Lerp(startAlpha, finishAlpha, progress);
                };
        }

        internal static Action<float> Color(Graphic target, Color startColor, Color finishColor)
        {
            return target == null
                ? throw new ArgumentNullException(nameof(target))
                : progress =>
                {
                    target.color = UnityEngine.Color.Lerp(startColor, finishColor, progress);
                };
        }

        internal static Action<float> AnchoredPosition(RectTransform target, Vector2 startPosition, Vector2 finishPosition)
        {
            return target == null
                ? throw new ArgumentNullException(nameof(target))
                : progress =>
                {
                    target.anchoredPosition = Vector2.Lerp(startPosition, finishPosition, progress);
                };
        }

        internal static Action<float> Sprite(Image target, Sprite[] sprites)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (sprites == null || sprites.Length == 0)
            {
                throw new ArgumentException("Sprites cannot be null or empty.", nameof(sprites));
            }

            int lastFrameIndex = sprites.Length - 1;

            return progress =>
            {
                int frameIndex = Mathf.Clamp(Mathf.FloorToInt(progress * sprites.Length), 0, lastFrameIndex);
                target.sprite = sprites[frameIndex];
            };
        }
    }
}