using Naukri.Moltk.Core;
using System;
using System.Collections.Generic;

namespace Naukri.Moltk.MVU
{
    public interface IConsumer
    {
        internal void Subscribe(params IProvider[] providers);

        internal void Unsubscribe();

        internal void Build(IProvider notifier);

        internal void HandleEvent(IProvider provider, IProviderEvent evt);
    }

    public abstract class ConsumerBehaviour : MoltkBehaviour, IConsumer
    {
        private readonly List<Unsubscriber> unsubscribers = new();

        void IConsumer.Unsubscribe()
        {
            UnsubscribeAll();
        }

        void IConsumer.Build(IProvider provider)
        {
            Build(provider);
        }

        void IConsumer.HandleEvent(IProvider provider, IProviderEvent evt)
        {
            HandleEvent(provider, evt);
        }

        void IConsumer.Subscribe(params IProvider[] providers)
        {
            Subscribe(providers);
        }

        protected void Subscribe(params IProvider[] providers)
        {
            foreach (var provider in providers)
            {
                var unsubscriber = provider.AddConsumer(this);
                if (unsubscriber != null)
                {
                    unsubscribers.Add(unsubscriber);
                    Build(provider);
                }
            }
        }

        protected void UnsubscribeAll()
        {
            foreach (var unsubscriber in unsubscribers)
            {
                unsubscriber.Dispose();
            }
            unsubscribers.Clear();
        }

        protected void Unsubscribe(params IProvider[] providers)
        {
            foreach (var provider in providers)
            {
                var targetUnsubscribers = unsubscribers.FindAll(it => it.Provider == provider);
                foreach (var unsubscriber in targetUnsubscribers)
                {
                    unsubscribers.Remove(unsubscriber);
                }
            }
        }

        protected abstract void Build(IProvider provider);

        protected virtual void HandleEvent(IProvider provider, IProviderEvent evt) { }
    }

    internal class Unsubscriber : IDisposable
    {
        public Unsubscriber(IProvider provider, IConsumer consumer)
        {
            this.provider = provider;
            this.consumer = consumer;
        }

        private readonly IProvider provider;

        private readonly IConsumer consumer;

        public IProvider Provider => provider;

        public void Dispose()
        {
            Provider.RemoveConsumer(consumer);
        }
    }
}
