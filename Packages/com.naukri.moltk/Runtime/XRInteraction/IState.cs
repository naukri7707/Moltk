using UnityEngine.Events;

namespace Naukri.Moltk.XRInteraction
{
    public interface IState<T>
    {
        public string CurrentStateName { get; }

        public T[] States { get; }

        public UnityEvent<string> OnStateChanged { get; }

        public void SetState(string stateName);

        public void NextState();
    }
}
