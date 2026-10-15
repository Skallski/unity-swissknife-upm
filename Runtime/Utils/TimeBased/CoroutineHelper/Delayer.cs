using System;
using System.Collections;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper
{
    internal static class Delayer
    {
        /// <summary>
        /// Delays callback invocation by single frame
        /// </summary>
        /// <param name="callback"> action that will be performed after the delay </param>
        /// <returns></returns>
        public static IEnumerator DelaySingleFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }

        /// <summary>
        /// Delays callback invocation by amount of time
        /// </summary>
        /// <param name="callback"> action that will be performed after the delay </param>
        /// <param name="time"> amount of time that delays the callback </param>
        /// <returns></returns>
        public static IEnumerator Delay(Action callback, float time)
        {
            return Delay(callback, new WaitForSeconds(time));
        }

        /// <summary>
        /// Delays callback invocation by a custom yield instruction
        /// </summary>
        /// <param name="callback"> action that will be performed after the delay </param>
        /// <param name="customYieldInstruction"> yield instruction to wait for </param>
        /// <returns></returns>
        public static IEnumerator Delay(Action callback, CustomYieldInstruction customYieldInstruction)
        {
            yield return customYieldInstruction;
            callback?.Invoke();
        }
        
        /// <summary>
        /// Delays callback invocation by a yield instruction
        /// </summary>
        /// <param name="callback"> action that will be performed after the delay </param>
        /// <param name="yieldInstruction"> yield instruction to wait for </param>
        /// <returns></returns>
        public static IEnumerator Delay(Action callback, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            callback?.Invoke();
        }
    }
}