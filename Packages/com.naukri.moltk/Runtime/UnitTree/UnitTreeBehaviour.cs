using Naukri.Moltk.UnitTree.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Moltk.UnitTree
{
    public abstract class UnitTreeBehaviour : MonoBehaviour
    {
        private UnitTreeController _controller;

        public UnitTreeController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = GetComponentInParent<UnitTreeController>();
                }
                return _controller;
            }
        }

        public string GetRelativePath()
        {
            var names = new List<string>();
            var current = transform;
            while (current != null && current != Controller.transform)
            {
                names.Add(current.name);
                current = current.parent;
            }

            if (current == null)
            {
                throw new System.Exception("Can't find controller in parent");
            }

            names.Add(Controller.name);
            names.Reverse();

            var path = string.Join('.', names.ToArray());

            return path;
        }

        public void SendEvent<T>(EventType eventType = EventType.SelfOnly) where T : UnitTreeEvent, new()
        {
            switch (eventType)
            {
                case EventType.SelfOnly:
                    SendMessage(nameof(HandleTreeEvent), new T(), SendMessageOptions.DontRequireReceiver);
                    break;

                case EventType.Upwards:
                    SendMessageUpwards(nameof(HandleTreeEvent), new T(), SendMessageOptions.DontRequireReceiver);
                    break;

                case EventType.Broadcast:
                    BroadcastMessage(nameof(HandleTreeEvent), new T(), SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        public void SendEvent<T>(T evt, EventType eventType = EventType.SelfOnly) where T : UnitTreeEvent
        {
            switch (eventType)
            {
                case EventType.SelfOnly:
                    SendMessage(nameof(HandleTreeEvent), evt, SendMessageOptions.DontRequireReceiver);
                    break;

                case EventType.Upwards:
                    SendMessageUpwards(nameof(HandleTreeEvent), evt, SendMessageOptions.DontRequireReceiver);
                    break;

                case EventType.Broadcast:
                    BroadcastMessage(nameof(HandleTreeEvent), evt, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        internal void InvokeEnter()
        {
            OnEnter();
        }

        internal void InvokeExit()
        {
            OnExit();
        }

        protected virtual void HandleTreeEvent(UnitTreeEvent evt)
        {
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnExit()
        {
        }
    }
}
