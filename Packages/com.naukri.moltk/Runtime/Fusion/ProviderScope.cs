using Naukri.InspectorMaid;
using Naukri.Moltk.Utility;
using System;
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
                    Type,
            Dictionary<
                ProviderKey,
                HashSet<Provider>
                >
            > providerCache = new();

        public T GetProvider<T>() where T : Provider
        {
            return GetProvider<T>(null);
        }

        public T GetProvider<T>(ProviderKey key)
          where T : Provider
        {
            var type = typeof(T);
            return GetProvider(type, key) as T;
        }

        public Provider GetProvider(Type type)
        {
            return GetProvider(type, null);
        }

        public Provider GetProvider(Type type, ProviderKey key)
        {
            var typedProvidersCache = GetTypedProvidersCache(type, key);
            return typedProvidersCache.FirstOrDefault();
        }

        public T[] GetProviders<T>() where T : Provider
        {
            return GetProviders<T>(null);
        }

        public T[] GetProviders<T>(ProviderKey key) where T : Provider
        {
            var type = typeof(T);
            var typedProvidersCache = GetTypedProvidersCache(type, key);

            return typedProvidersCache.Cast<T>().ToArray();
        }

        public Provider[] GetProviders(Type type)
        {
            return GetProviders(type, null);
        }

        public Provider[] GetProviders(Type type, ProviderKey key)
        {
            var typedProvidersCache = GetTypedProvidersCache(type, key);
            return typedProvidersCache.ToArray();
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

        internal void Register(Provider provider)
        {
            if (provider == null)
                return;
            var type = provider.GetType();
            if (!providerCache.TryGetValue(type, out var typeCache))
            {
                typeCache = new();
                providerCache[type] = typeCache;
            }

            var key = provider.Key;
            if (!typeCache.TryGetValue(key, out var typedProvidersCache))
            {
                typedProvidersCache = new();
                typeCache[key] = typedProvidersCache;
            }

            typedProvidersCache.Add(provider);
        }

        internal void Unregister(Provider provider)
        {
            var type = provider.GetType();
            if (!providerCache.TryGetValue(type, out var typeCache))
            {
                return;
            }

            var key = provider.Key;
            if (!typeCache.TryGetValue(key, out var typedProvidersCache))
            {
                return;
            }

            typedProvidersCache.Remove(provider);
            if (typedProvidersCache.Count == 0)
            {
                typeCache.Remove(provider.Key);
                if (typeCache.Count == 0)
                {
                    providerCache.Remove(type);
                }
            }
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

        private HashSet<Provider> GetTypedProvidersCache(Type type, ProviderKey key)
        {
            var typeCache = providerCache[type];
            var typedProvidersCache = key switch
            {
                null => typeCache.FirstOrDefault().Value,
                _ => typeCache[key],
            };

            return typedProvidersCache;
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
