using UnityEngine;

namespace Naukri.Moltk.Utility
{
    public interface IComponentCreatedHandler
    {
        public void OnComponentCreated();
    }

    public static class ComponentLocator<T> where T : Component
    {
        private static T componentCache;

        public static T FindComponent()
        {
            TryFindComponent(out var component);
            return component;
        }

        public static bool TryFindComponent(out T component)
        {
            if (componentCache == null)
            {
                componentCache = FindImpl();
            }

            component = componentCache;
            return componentCache != null;
        }

        public static T FindOrCreateComponent(string name = null, bool dontDestroyOnLoad = false)
        {
            if (componentCache == null)
            {
                componentCache = FindImpl();

                if (componentCache == null)
                {
                    var type = typeof(T);
                    var goName = name ?? type.Name;
                    var go = new GameObject(goName, type);

                    if (dontDestroyOnLoad)
                    {
                        Object.DontDestroyOnLoad(go);
                    }

                    componentCache = go.GetComponent<T>();
                    if (componentCache is IComponentCreatedHandler createdHandler)
                    {
                        createdHandler.OnComponentCreated();
                    }
                }
            }

            return componentCache;
        }

        public static void Invalidate()
        {
            componentCache = null;
        }

        static T FindImpl()
        {
            return Object.FindObjectOfType<T>();
        }
    }
}
