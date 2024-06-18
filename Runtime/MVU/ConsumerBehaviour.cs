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
    }

    public abstract class ConsumerBehaviour : MoltkBehaviour, IConsumer
    {
        private readonly List<IDisposable> unsubscribers = new();

        void IConsumer.Unsubscribe()
        {
            Unsubscribe();
        }

        void IConsumer.Build(IProvider provider)
        {
            Build(provider);
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
                }
            }
        }

        protected void Unsubscribe()
        {
            foreach (var unsubscriber in unsubscribers)
            {
                unsubscriber.Dispose();
            }
        }

        protected abstract void Build(IProvider provider);
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

        public void Dispose()
        {
            provider.RemoveConsumer(consumer);
        }
    }
}
