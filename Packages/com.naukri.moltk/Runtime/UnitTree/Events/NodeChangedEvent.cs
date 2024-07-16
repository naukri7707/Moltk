using UnityEngine;

namespace Naukri.Moltk.UnitTree.Events
{
    public class NodeChangedEvent : UnitTreeEvent
    {
        public readonly GameObject from;

        public readonly GameObject to;

        public NodeChangedEvent(GameObject from, GameObject to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
