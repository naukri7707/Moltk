using System;

namespace Naukri.Moltk.Fusion
{
    public interface IViewController
    {
        internal void Render();
    }

    public abstract class ViewController : ConsumerBase, IViewController
    {
        void IViewController.Render() => Render();

        internal override void OnRefresh() => Render();

        protected virtual void Start()
        {
            OnRefresh();
        }

        protected abstract void Render();
    }

    public abstract class ViewController<TState> : Provider<TState>, IViewController
        where TState : IEquatable<TState>
    {
        void IViewController.Render() => Render();

        internal override void OnRefresh()
        {
            // Restore base.OnRefresh() logic
            var stateChanged = SetState(Build);

            // If stateChanged, the UI was re-render from SetState already,
            // So we don't need to call Render in this case.
            // But we still need to call Render if the state wasn't changed,
            // Because sometime the UI only depend on it's provider but not self state.
            if (!stateChanged)
            {
                Render();
            }
        }

        internal override bool SetStateImpl(TState newState)
        {
            var stateChanged = base.SetStateImpl(newState);

            // Rerender UI if state changed
            if (stateChanged)
            {
                Render();
            }

            return stateChanged;
        }

        protected abstract void Render();

        protected virtual void Start()
        {
            node.Refresh();
        }
    }
}
