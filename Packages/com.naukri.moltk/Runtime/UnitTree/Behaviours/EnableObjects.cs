using UnityEngine;

namespace Naukri.Moltk.UnitTree.Behaviours
{
    public class EnableObjects : UnitTreeBehaviour
    {
        public GameObject[] targets = new GameObject[0];

        public bool disableOnExit;

        protected override void OnEnter()
        {
            base.OnEnter();
            foreach (var gameObject in targets)
            {
                gameObject.SetActive(true);
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            if (disableOnExit)
            {
                foreach (var gameObject in targets)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
