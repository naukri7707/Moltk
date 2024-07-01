using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naukri.Moltk.MVU
{
    public interface IProvider : IConsumer
    {
        internal HashSet<IConsumer> Consumers { get; }

        internal void NotifyConsumer()
        {
            var consumers = Consumers.ToArray();
            foreach (var consumer in consumers)
            {
                consumer.Build(this);
            }
        }

        internal Unsubscriber AddConsumer(IConsumer consumer)
        {
            if (Consumers.Add(consumer))
            {
                var unsubscriber = new Unsubscriber(this, consumer);
                return unsubscriber;
            }
            return null;
        }

        internal bool RemoveConsumer(IConsumer consumer)
        {
            return Consumers.Remove(consumer);
        }

        internal void SendEvent(IProviderEvent e)
        {
            var consumers = Consumers.ToArray();
            foreach (var consumer in consumers)
            {
                consumer.HandleEvent(this, e);
            }
        }
    }

    internal interface IProvider<TState> : IProvider where TState : class, new()
    {
        public TState State { get; }

        internal Task SetStateAsync(Func<Task<TState>> stateUpdater);
    }

    public abstract class ProviderBehaviour : ConsumerBehaviour, IProvider
    {
        HashSet<IConsumer> IProvider.Consumers { get; } = new();

        protected void Refresh()
        {
            ((IProvider)this).NotifyConsumer();
        }

        protected void SendEvent(IProviderEvent e)
        {
            ((IProvider)this).SendEvent(e);
        }
    }

    public abstract class ProviderBehaviour<TState> : ProviderBehaviour, IProvider<TState>
         where TState : class, new()
    {
        private TState _state = new();

        public TState State
        {
            get => _state;
            protected set
            {
                if (!_state.Equals(value))
                {
                    _state = value;
                    Refresh();
                }
            }
        }

        TState IProvider<TState>.State => State;

        Task IProvider<TState>.SetStateAsync(Func<Task<TState>> stateUpdater)
        {
            return SetStateAsync(stateUpdater);
        }

        protected async Task SetStateAsync(Func<Task<TState>> stateUpdater)
        {
            var newState = await stateUpdater();
            State = newState;
        }
    }
}
