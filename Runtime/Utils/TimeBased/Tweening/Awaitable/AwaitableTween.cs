using System;
using System.Threading;
using UnityEngine;
using UtilsToolbox.Utils.TimeBased.Tweening.Core;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Awaitable
{
    public sealed class AwaitableTween : Tween
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly AwaitableCompletionSource _completionSource;

        internal CancellationToken CancellationToken => _cancellationTokenSource.Token;

        internal AwaitableTween(Action onSkip) : base(onSkip)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _completionSource = new AwaitableCompletionSource();
        }

        public UnityEngine.Awaitable.Awaiter GetAwaiter()
        {
            return _completionSource.Awaitable.GetAwaiter();
        }

        protected override void StopExecution()
        {
            if (_cancellationTokenSource.IsCancellationRequested == false)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        protected override void OnFinished(TweenState state)
        {
            _cancellationTokenSource.Dispose();

            if (state == TweenState.Aborted)
            {
                _completionSource.SetCanceled();
                return;
            }

            _completionSource.SetResult();
        }
    }
}