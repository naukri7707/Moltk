using System;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Moltk.Fusion
{
    public delegate void ProviderEventHandler(Provider sender, ProviderEvent evt);

    public interface IContext
    {
        public Subscription Listen<T>() where T : Provider;

        public Subscription Listen<T>(ProviderKey key) where T : Provider;

        public Subscription Listen<T>(Provider provider) where T : Provider;

        public void NotifyListeners();

        public T Read<T>() where T : Provider;

        public T Read<T>(ProviderKey key) where T : Provider;

        public void Refresh();

        public T Watch<T>() where T : Provider;

        public T Watch<T>(ProviderKey key) where T : Provider;
    }

    public partial class Node : IContext, IDisposable
    {
        private readonly Action onRefresh;

        private readonly ProviderEventHandler providerEventHandler;

        private readonly HashSet<Node> publishers = new();

        private readonly HashSet<Node> subscribers = new();

        public Node(Action onRefresh, ProviderEventHandler providerEventHandler)
        {
            this.onRefresh = onRefresh;
            this.providerEventHandler = providerEventHandler;
        }

        public void Dispose()
        {
            foreach (var subscriber in subscribers.ToArray())
            {
                Unsubscribe(this, subscriber);
            }
            foreach (var publisher in publishers.ToArray())
            {
                Unsubscribe(publisher, this);
            }
        }

        public void HandleEvent(Provider sender, ProviderEvent evt)
        {
            providerEventHandler.Invoke(sender, evt);
        }

        public Subscription Listen<T>() where T : Provider
        {
            var provider = Read<T>();
            return Listen<T>(provider);
        }

        public Subscription Listen<T>(ProviderKey key) where T : Provider
        {
            var provider = Read<T>(key);
            return Listen<T>(provider);
        }

        public Subscription Listen<T>(Provider provider) where T : Provider
        {
            return CreateSubscription(provider.node, this);
        }

        public void NotifyListeners()
        {
            foreach (var subscriber in subscribers.ToArray())
            {
                subscriber.Refresh();
            }
        }

        public T Read<T>() where T : Provider
        {
            return Providers.Get<T>();
        }

        public T Read<T>(ProviderKey key) where T : Provider
        {
            return Providers.Get<T>(key);
        }

        public void Refresh()
        {
            onRefresh?.Invoke();
        }

        public void SendEvent(Provider sender, ProviderEvent evt)
        {
            foreach (var subscriber in subscribers.ToArray())
            {
                subscriber.HandleEvent(sender, evt);
            }
        }

        public T Watch<T>() where T : Provider
        {
            var provider = Read<T>();
            Watch<T>(provider);

            return provider;
        }

        public T Watch<T>(ProviderKey key) where T : Provider
        {
            var provider = Read<T>(key);
            Watch<T>(provider);

            return provider;
        }

        public void Watch<T>(Provider provider) where T : Provider
        {
            Subscribe(provider.node, this);
        }

        internal static Subscription CreateSubscription(Node publisher, Node subscriber)
        {
            return new Subscription(publisher, subscriber);
        }

        internal static void Subscribe(Node publisher, Node subscriber)
        {
            publisher.subscribers.Add(subscriber);
            subscriber.publishers.Add(publisher);
        }

        internal static void Unsubscribe(Node publisher, Node subscriber)
        {
            publisher.subscribers.Remove(subscriber);
            subscriber.publishers.Remove(publisher);
        }
    }
}