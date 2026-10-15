using System;
using UnityEngine;
using UtilsToolbox.Utils.TimeBased.Tweening.Core;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Coroutine
{
    public sealed class CoroutineTween : Tween
    {
        private MonoBehaviour _caller;
        private UnityEngine.Coroutine _coroutine;

        internal CoroutineTween(MonoBehaviour caller, Action onSkip) : base(onSkip)
        {
            if (caller == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            _caller = caller;
        }

        internal void Bind(UnityEngine.Coroutine coroutine)
        {
            _coroutine = coroutine;
            
            if (IsActive == false)
            {
                StopExecution();
            }
        }

        protected override void StopExecution()
        {
            if (_caller != null && _coroutine != null)
            {
                _caller.StopCoroutine(_coroutine);
            }

            _caller = null;
            _coroutine = null;
        }
    }
}