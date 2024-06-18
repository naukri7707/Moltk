using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Naukri.Moltk.MVU
{
    public abstract class MVUController<TState> : ProviderBehaviour<TState>
        where TState : State, new()
    {
        public ProviderBehaviour[] subscribes;

        protected override void OnEnable()
        {
            Subscribe(this);
            Subscribe(subscribes);
            Refresh();
        }

        protected override void OnDisable()
        {
            Unsubscribe();
        }
    }
}
