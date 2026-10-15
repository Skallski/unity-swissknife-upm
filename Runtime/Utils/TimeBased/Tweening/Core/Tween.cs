using System;

namespace UtilsToolbox.Utils.TimeBased.Tweening.Core
{
    public abstract class Tween
    {
        private Action _onSkip;

        public TweenState State { get; private set; } = TweenState.Running;

        public bool IsActive => State == TweenState.Running;

        public bool IsCompleted => State is TweenState.Completed or TweenState.Skipped;

        protected Tween(Action onSkip)
        {
            _onSkip = onSkip;
        }

        public void Abort()
        {
            if (IsActive == false)
            {
                return;
            }

            Finish(TweenState.Aborted);
        }

        public void Skip()
        {
            if (IsActive == false)
            {
                return;
            }

            Finish(TweenState.Skipped);
        }

        public void Complete()
        {
            if (IsActive == false)
            {
                return;
            }

            Finish(TweenState.Completed);
        }

        private void Finish(TweenState state)
        {
            State = state;

            StopExecution();

            if (state == TweenState.Skipped)
            {
                _onSkip?.Invoke();
            }

            _onSkip = null;

            OnFinished(state);
        }

        protected abstract void StopExecution();

        protected virtual void OnFinished(TweenState state) {}
    }
}