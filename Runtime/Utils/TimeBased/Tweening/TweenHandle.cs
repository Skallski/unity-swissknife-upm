using System;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.Tweening
{
    public class TweenHandle
    {
        private MonoBehaviour _caller;
        private UnityEngine.Coroutine _coroutine;
        private Action _onSkip;
        internal bool IsActive { get; private set; }

        internal TweenHandle(MonoBehaviour caller, UnityEngine.Coroutine coroutine, Action onSkip)
        {
            _caller = caller ?? throw new ArgumentNullException("Coroutine caller cannot be null!");
            _coroutine = coroutine ?? throw new ArgumentNullException("Coroutine cannot be null!");
            _onSkip = onSkip ?? throw new ArgumentNullException("Skip action cannot be null!");

            IsActive = true;
        }

        private void End(Action onFinish = null)
        {
            if (IsActive == false)
            {
                return;
            }

            _caller.StopCoroutine(_coroutine);
            onFinish?.Invoke();
            IsActive = false;
                
            _caller = null;
            _coroutine = null;
            _onSkip = null;
        }

        /// <summary>
        /// Immediately aborts the tween at the moment this method is called.
        /// The tween runs normally until this point and does not complete its final state
        /// (use <see cref="Skip"/> to force completion).
        /// </summary>
        public void Abort()
        {
            End();
        }

        /// <summary>
        /// Immediately completes the tween when called, forcing its final state
        /// (use <see cref="Abort"/> to stop it without completing).
        /// </summary>
        public void Skip()
        {
            End(_onSkip);
        }
    }
}