using Naukri.Moltk.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Naukri.Moltk.Fusion
{
    public interface IConsumer
    {
        internal void OnBuild();
    }

    public abstract class Consumer : MoltkBehaviour, IConsumer
    {
        private Node _node;

        private bool isInitialized;

        [SuppressMessage("Style", "IDE1006")]
        internal Node node
        {
            get
            {
                _node ??= new(OnBuild, HandleEvent);
                return _node;
            }
        }

        void IConsumer.OnBuild()
        {
            OnBuild();
        }

        internal virtual bool Initialize()
        {
            if (isInitialized)
                return false;
            isInitialized = true;

            OnInitialize(node);

            return true;
        }

        protected override sealed void Awake()
        {
            base.Awake();
            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            node.Dispose();
        }

        protected virtual void OnInitialize(IContext ctx) { }

        protected abstract void OnBuild();

        protected virtual void HandleEvent(Provider sender, ProviderEvent evt) { }
    }
}
