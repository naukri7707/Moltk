namespace Naukri.Moltk.MVU
{
    public abstract class MVUController<TState> : ProviderBehaviour<TState>
        where TState : class, new()
    {
        public ProviderBehaviour[] subscribes;

        protected override void Awake()
        {
            Subscribe(this);
            Subscribe(subscribes);
        }

        protected override void OnEnable()
        {
            Refresh();
        }

        protected override void OnDestroy()
        {
            UnsubscribeAll();
        }
    }
}
