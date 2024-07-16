using UnityEngine;

namespace Naukri.Moltk.UnitTree.Events
{
    public class NodeChangingEvent : UnitTreeEvent
    {
        public readonly GameObject from;

        public readonly GameObject to;

        public NodeChangingEvent(GameObject from, GameObject to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
