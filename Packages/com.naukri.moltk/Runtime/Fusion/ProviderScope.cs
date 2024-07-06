using Naukri.Moltk.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Naukri.Moltk.Fusion
{
    [AddComponentMenu("")] // Hide ProviderScope in AddComponent's Menu
    internal sealed partial class ProviderScope : MonoBehaviour
    {
        private readonly HashSet<Scene> loadedScenes = new();

        private readonly Dictionary<
                ProviderKey,
                HashSet<Provider>
                > cachedProviderMap = new();

        private IEnumerable<Provider> CachedProviders => cachedProviderMap.Values.SelectMany(set => set);

        public T GetProvider<T>() where T : Provider
        {
            return CachedProviders.OfType<T>().FirstOrDefault();
        }

        public T GetProvider<T>(ProviderKey key)
          where T : Provider
        {
            return cachedProviderMap[key].OfType<T>().FirstOrDefault();
        }

        public T[] GetProviders<T>() where T : Provider
        {
            return CachedProviders.OfType<T>().ToArray();
        }

        public T[] GetProviders<T>(ProviderKey key) where T : Provider
        {
            return cachedProviderMap[key].OfType<T>().ToArray();
        }

        public void OnComponentCreated()
        {
            var sceneCount = SceneManager.sceneCount;

            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                EnsureSceneProviderCached(scene);
            }
        }

        internal bool Register(Provider provider)
        {
            if (provider == null)
                return false;

            var key = provider.Key;

            if (!cachedProviderMap.TryGetValue(key, out var keyCached))
            {
                keyCached = new();
                cachedProviderMap[key] = keyCached;
            }
            return keyCached.Add(provider);
        }

        internal bool Unregister(Provider provider)
        {
            var key = provider.Key;

            if (!cachedProviderMap.TryGetValue(key, out var keyCached))
            {
                return false;
            }

            var isRemoved = keyCached.Remove(provider);

            if (keyCached.Count == 0)
            {
                cachedProviderMap.Remove(key);
            }

            return isRemoved;
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ComponentLocator<ProviderScope>.Invalidate();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureSceneProviderCached(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            loadedScenes.Remove(scene);
        }

        private void EnsureSceneProviderCached(Scene scene)
        {
            if (loadedScenes.Contains(scene))
            {
                return;
            }
            loadedScenes.Add(scene);

            var roots = scene.GetRootGameObjects();

            foreach (var provider in roots.SelectMany(it => it.GetComponentsInChildren<Provider>(true)))
            {
                Register(provider);
            }
        }
    }

    partial class ProviderScope : IComponentCreatedHandler
    {
        internal const string kInstanceName = "[Provider Scope]";

        internal static ProviderScope LocateOrCreate()
        {
            var scope = ComponentLocator<ProviderScope>.FindOrCreateComponent(
               name: kInstanceName,
               dontDestroyOnLoad: true
               );

            return scope;
        }

        internal static bool TryLocate(out ProviderScope scope)
        {
            return ComponentLocator<ProviderScope>.TryFindComponent(out scope);
        }
    }
}
