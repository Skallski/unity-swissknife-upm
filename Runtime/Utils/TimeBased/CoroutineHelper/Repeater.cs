using System;
using System.Collections;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper
{
    internal static class Repeater
    {
        /// <summary>
        /// Invokes callback each timestamp in n seconds duration
        /// </summary>
        /// <param name="duration"> duration of whole task </param>
        /// <param name="timestamp"> time interval between which the callback will be invoked </param>
        /// <param name="onEachTimestamp"> callback that will be invoked each timestamp </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachTimestamp(float duration, float timestamp, Action<float> onEachTimestamp,
            Action onFinish = null)
        {
            float timer = 0;
            
            bool StopPredicate()
            {
                return timer >= duration;
            }
            
            void OnEachTimestamp(float t)
            {
                onEachTimestamp(t);
                timer = t;
            }

            return EachTimestamp(StopPredicate, timestamp, OnEachTimestamp, onFinish);
        }

        /// <summary>
        /// Invokes callback each timestamp until stopPredicate condition is met
        /// </summary>
        /// <param name="stopPredicate"> a condition that must be met in order to stop the callback execution </param>
        /// <param name="timestamp"> time interval between which the callback will be invoked </param>
        /// <param name="onEachTimestamp"> callback that will be invoked each timestamp </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachTimestamp(Func<bool> stopPredicate, float timestamp, Action<float> onEachTimestamp, 
            Action onFinish = null)
        {
            WaitForSeconds delay = new WaitForSeconds(timestamp);
            float elapsedTime = 0f;

            while (stopPredicate() == false)
            {
                onEachTimestamp?.Invoke(elapsedTime);
                elapsedTime += timestamp; 
                yield return delay;
            }
            
            onFinish?.Invoke();
        }
    }
}