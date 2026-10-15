using System;
using System.Collections;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper
{
    public static class CoroutineRunner
    {
        #region BASIC START/STOP
        /// <summary>
        /// Runs coroutine by passing IEnumerator
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="iEnumerator"> coroutine instruction </param>
        public static void Run(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, IEnumerator iEnumerator)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine);
            }

            coroutine = caller.StartCoroutine(iEnumerator);
        }
        
        /// <summary>
        /// Stops coroutine with invoking a callback afterwards
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine stop will be called </param>
        /// <param name="coroutine"> coroutine to stop </param>
        /// <param name="onStop"> callback that will be invoked after stopping the coroutine </param>
        public static void Stop(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Action onStop = null)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine);
                onStop?.Invoke();

                coroutine = null;
            }
        }
        #endregion

        #region DELAY
        /// <summary>
        /// Runs action after certain time period as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="callback"> action that will be performed </param>
        /// <param name="delay"> amount of time (in seconds) after which the action will be called </param>
        public static void RunAfterTime(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Action callback, float delay)
        {
            Run(caller, ref coroutine, Delayer.Delay(callback, delay));
        }

        /// <summary>
        /// Runs action in next frame as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="callback"> action that will be performed </param>
        public static void RunNextFrame(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Action callback)
        {
            Run(caller, ref coroutine, Delayer.DelaySingleFrame(callback));
        }
        
        /// <summary>
        /// Runs action after a yield instruction as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="callback"> action that will be performed </param>
        /// <param name="yieldInstruction"> yield instruction after which the action will be called </param>
        public static void RunAfterYieldInstruction(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Action callback, 
            YieldInstruction yieldInstruction)
        {
            Run(caller, ref coroutine, Delayer.Delay(callback, yieldInstruction));
        }
        
        /// <summary>
        /// Runs action after a yield instruction as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="callback"> action that will be performed </param>
        /// <param name="customYieldInstruction"> yield instruction after which the action will be called </param>
        public static void RunAfterYieldInstruction(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Action callback, 
            CustomYieldInstruction customYieldInstruction)
        {
            Run(caller, ref coroutine, Delayer.Delay(callback, customYieldInstruction));
        }
        #endregion

        #region REPEAT
        /// <summary>
        /// Runs action in loop as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="duration"> duration of the loop </param>
        /// <param name="timestamp"> frequency (in seconds) of the loop (higher, the slower) </param>
        /// <param name="onEachTimestamp"> action that will be performed every timestamp (direct access to current timestamp) </param>
        /// <param name="onFinish"> action that will be performed when loop is finished </param>
        public static void RunRepeating(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, float duration, 
            float timestamp, Action<float> onEachTimestamp, Action onFinish = null)
        {
            Run(caller, ref coroutine, Repeater.EachTimestamp(duration, timestamp, onEachTimestamp, onFinish));
        }
        
        /// <summary>
        /// Runs action in loop as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="stopPredicate"> predicate that indicate stop of the loop </param>
        /// <param name="timestamp"> frequency (in seconds) of the loop (higher, the slower) </param>
        /// <param name="onEachTimestamp"> action that will be performed every timestamp (direct access to current timestamp) </param>
        /// <param name="onFinish"> action that will be performed when loop is finished </param>
        public static void RunRepeating(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Func<bool> stopPredicate, 
            float timestamp, Action<float> onEachTimestamp, Action onFinish = null)
        {
            Run(caller, ref coroutine, Repeater.EachTimestamp(stopPredicate, timestamp, onEachTimestamp, onFinish));
        }
        
        /// <summary>
        /// Runs action in a loop (every second) as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="duration"> duration of the loop </param>
        /// <param name="onEachTimestamp"> action that will be performed every timestamp (direct access to current timestamp) </param>
        /// <param name="onFinish"> action that will be performed when loop is finished </param>
        public static void RunRepeatingEverySecond(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, float duration, 
            Action<float> onEachTimestamp, Action onFinish = null)
        {
            Run(caller, ref coroutine, Repeater.EachTimestamp(duration, 1, onEachTimestamp, onFinish));
        }
        
        /// <summary>
        /// Runs action in a loop (every second) as a coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="stopPredicate"> predicate that indicate stop of the loop </param>
        /// <param name="onEachTimestamp"> action that will be performed every timestamp (direct access to current timestamp) </param>
        /// <param name="onFinish"> action that will be performed when loop is finished </param>
        public static void RunRepeatingEverySecond(MonoBehaviour caller, ref UnityEngine.Coroutine coroutine, Func<bool> stopPredicate, 
           Action<float> onEachTimestamp, Action onFinish = null)
        {
            Run(caller, ref coroutine, Repeater.EachTimestamp(stopPredicate, 1, onEachTimestamp, onFinish));
        }
        #endregion
    }
}