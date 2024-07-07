using System;
using System.Collections.Generic;
using System.Linq;

namespace Naukri.Moltk.Fusion
{
    public delegate void ProviderEventHandler(Provider sender, ProviderEvent evt);

    public interface IContext
    {
        public Subscription Listen<T>(Provider provider) where T : Provider;

        public Subscription Listen<T>() where T : Provider;

        public Subscription Listen<T>(ProviderKey key) where T : Provider;

        public T Watch<T>() where T : Provider;

        public T Watch<T>(ProviderKey key) where T : Provider;

        public T Read<T>() where T : Provider;

        public T Read<T>(ProviderKey key) where T : Provider;

        public void Refresh();

        public void NotifyListeners();
    }

    public partial class Node : IContext, IDisposable
    {
        public Node(Action onRefresh, ProviderEventHandler providerEventHandler)
        {
            this.onRefresh = onRefresh;
            this.providerEventHandler = providerEventHandler;
        }

        private readonly HashSet<Node> inputs = new();

        private readonly HashSet<Node> outputs = new();

        private readonly Action onRefresh;

        private readonly ProviderEventHandler providerEventHandler;

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
            Link(provider.node, this);
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

        public void NotifyListeners()
        {
            foreach (var output in outputs.ToArray())
            {
                output.Refresh();
            }
        }

        public void SendEvent(Provider sender, ProviderEvent evt)
        {
            foreach (var output in outputs.ToArray())
            {
                output.HandleEvent(sender, evt);
            }
        }

        public void HandleEvent(Provider sender, ProviderEvent evt)
        {
            providerEventHandler.Invoke(sender, evt);
        }

        public void Dispose()
        {
            foreach (var output in outputs.ToArray())
            {
                Unlink(this, output);
            }
            foreach (var input in inputs.ToArray())
            {
                Unlink(input, this);
            }
        }
    }

    partial class Node
    {
        internal static Subscription CreateSubscription(Node input, Node output)
        {
            return new Subscription(input, output);
        }

        internal static void Link(Node input, Node output)
        {
            input.outputs.Add(output);
            output.inputs.Add(input);
        }

        internal static void Unlink(Node input, Node output)
        {
            input.outputs.Remove(output);
            output.inputs.Remove(input);
        }
    }
}
