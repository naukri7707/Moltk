using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Naukri.Moltk.Fusion
{
    public interface IConsumer
    {
        internal void OnRefresh();
    }

    [AddComponentMenu("")] // Hide ProviderScope in AddComponent's Menu
    public abstract class ConsumerBase : MonoBehaviour, IConsumer
    {
        private Node _node;

        private bool isInitialized;

        [SuppressMessage("Style", "IDE1006")]
        internal Node node
        {
            get
            {
                _node ??= new(OnRefesh, HandleEvent);
                return _node;
            }
        }

        void IConsumer.OnRefresh()
        {
            OnRefesh();
        }

        internal virtual bool Initialize()
        {
            if (isInitialized)
                return false;
            isInitialized = true;

            OnInitialize(node);

            return true;
        }

        internal abstract void OnRefesh();

        protected IContext GetContext() => node;

        protected void Awake()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            node.Dispose();
        }

        protected virtual void OnInitialize(IContext ctx) { }

        protected virtual void HandleEvent(Provider sender, ProviderEvent evt) { }
    }

    public class Consumer : ConsumerBase
    {
        internal override void OnRefesh() => Build();

        protected virtual void Build() { }
    }
}
