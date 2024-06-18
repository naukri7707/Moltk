using UnityEngine;

namespace Naukri.Moltk.Core
{
    public abstract partial class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        protected virtual bool DestroyOnLoad => true;

        protected virtual void OnInit() { }
    }

    partial class SingletonBehaviour<T>
    {
        private static readonly object lockObj = new();

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        instance = FindObjectOfType<T>();

                        if (instance == null)
                        {
                            var go = new GameObject(typeof(T).Name);
                            instance = go.AddComponent<T>();
                        }

                        if (!instance.DestroyOnLoad)
                        {
                            DontDestroyOnLoad(instance.gameObject);
                        }

                        instance.OnInit();
                    }
                }
                return instance;
            }
        }
    }
}
