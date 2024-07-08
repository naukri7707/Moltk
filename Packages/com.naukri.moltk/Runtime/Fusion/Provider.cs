using System;
using System.Threading.Tasks;

namespace Naukri.Moltk.Fusion
{
    public interface IProvider : IConsumer { }

    public interface IProvider<T> : IProvider
    {
        public T State { get; }

        internal T Build(T state);
    }

    public abstract partial class SingletonProvider<TState> : Provider<TState>
            where TState : IEquatable<TState>
    {
        protected override ProviderKey BuildKey()
        {
            return base.BuildKey();
        }
    }

    public abstract partial class Provider : ConsumerBase
    {
        private ProviderKey _key;

        public ProviderKey Key => _key ??= BuildKey();

        public void SendEvent(ProviderEvent evt)
        {
            node.SendEvent(this, evt);
        }

        internal override void OnRefresh()
        {
            // Do nothing
        }

        protected virtual ProviderKey BuildKey()
        {
            var type = GetType();
            return new ProviderKey<Type>(type);
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

        public TState State => _state;

        TState IProvider<TState>.State => State;

        public bool SetState(Func<TState, TState> update)
        {
            var oldState = State;
            var newState = update(oldState);
            return SetStateImpl(newState);
        }

        public async Task<bool> SetStateAsync(Func<TState, Task<TState>> update)
        {
            var oldState = State;
            var newState = await update(oldState);
            return SetStateImpl(newState);
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

        internal override void OnRefresh()
        {
            SetState(Build);
        }

        internal virtual bool SetStateImpl(TState newState)
        {
            if (!Equals(_state, newState))
            {
                _state = newState;
                node.NotifyListeners();

                return true;
            }

            return false;
        }

        protected abstract TState Build(TState state);
    }
}
