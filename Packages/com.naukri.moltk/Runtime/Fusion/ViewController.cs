using System;

namespace Naukri.Moltk.Fusion
{
    public interface IViewController
    {
        internal void Render();
    }

    public abstract class ViewController : Consumer, IViewController
    {
        void IViewController.Render() => Render();

        protected sealed override void OnBuild() => Render();

        protected override void Start()
        {
            base.Start();

            OnBuild();
        }

        protected abstract void Render();
    }

    public abstract class ViewController<TState> : Provider<TState>, IViewController
        where TState : IEquatable<TState>
    {
        void IViewController.Render() => Render();

        protected abstract void Render();

        protected override void Start()
        {
            base.Start();

            node.Refresh();
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();
            Render();
        }

        protected override void OnBuilt()
        {
            base.OnBuilt();
            Render();
        }
    }
}
