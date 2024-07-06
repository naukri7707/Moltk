using System;
using System.Threading.Tasks;

namespace Naukri.Moltk.Fusion
{
    public interface IProvider : IConsumer
    {
    }

    public interface IProvider<T> : IProvider
    {
        public T State { get; }

        internal T Build(T state);
    }

    public abstract partial class Provider : Consumer
    {
        public virtual ProviderKey Key => new();

        public void SendEvent(ProviderEvent evt)
        {
            node.SendEvent(this, evt);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ProviderScope.TryLocate(out var scope))
            {
                scope.Unregister(this);
            }
        }
    }

    public abstract class Provider<TState> : Provider, IProvider<TState>
        where TState : IEquatable<TState>
    {
        private TState _state;

        public virtual TState State
        {
            get
            {
                return _state;
            }

            private set
            {
                if (!Equals(_state, value))
                {
                    OnStateChanging();
                    _state = value;
                    OnStateChanged();
                    node.NotifyListeners();
                }
            }
        }

        TState IProvider<TState>.State => State;

        public void SetState(Func<TState, TState> update)
        {
            var state = State;
            State = update(state);
        }

        public async Task SetStateAsync(Func<TState, Task<TState>> update)
        {
            var state = State;
            State = await update(state);
        }

        TState IProvider<TState>.Build(TState state) => Build(state);

        internal override bool Initialize()
        {
            var isInitializing = base.Initialize();

            // 如果在初始化中，則調用 SetState 生成一個初始狀態
            if (isInitializing)
            {
                SetState(Build);
            }

            return isInitializing;
        }

        protected virtual void OnStateChanging() { }

        protected virtual void OnStateChanged() { }

        protected virtual void OnBuilding() { }

        protected virtual void OnBuilt() { }

        protected override sealed void OnBuild()
        {
            OnBuilding();
            SetState(Build);
            OnBuilt();
        }

        protected abstract TState Build(TState state);
    }

    partial class Provider
    {
        public static bool KeepAlive(Provider provider)
        {
            var scope = ProviderScope.LocateOrCreate();
            DontDestroyOnLoad(provider);
            return scope.Register(provider);
        }
    }
}
