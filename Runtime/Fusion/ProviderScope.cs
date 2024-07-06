﻿using Naukri.Moltk.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Naukri.Moltk.Fusion
{
    [AddComponentMenu("")] // Hide ProviderScope in AddComponent's Menu
    internal sealed partial class ProviderScope : MonoBehaviour
    {
        private readonly Dictionary<ProviderKey, Provider> cachedProviders = new();

        public T GetProvider<T>() where T : Provider
        {
            var key = new ProviderKey<Type>(typeof(T));

            return GetProvider<T>(key);
        }

        public T GetProvider<T>(ProviderKey key)
          where T : Provider
        {
            if (!cachedProviders.ContainsKey(key))
            {
                // 如果找不到 Provider 就再遍例一次場景中的 Provider 並嘗試註冊
                Resolve();
            }
            var akey = cachedProviders.Keys.FirstOrDefault();

            bool a = key.Equals(akey);

            return cachedProviders[key] as T;
        }

        internal bool Register(Provider provider)
        {
            if (provider == null)
                return false;

            var key = provider.Key;

            if (cachedProviders.TryGetValue(key, out var cachedProvider))
            {
                // 如果 provider 與已快取的 provider (衝突) 報錯
                if (provider != cachedProvider)
                {
                    throw new ArgumentException($"Provider conflict: {provider} and {cachedProvider}");
                }
                // 相同就略過
                else
                {
                    return false;
                }
            }
            else
            {
                // 不存在則新增之
                cachedProviders[key] = provider;
                return true;
            }
        }

        internal bool Unregister(Provider provider)
        {
            var key = provider.Key;

            return cachedProviders.Remove(key);
        }

        private void Resolve()
        {
            var providers = FindObjectsOfType<Provider>();
            foreach (var provider in providers)
            {
                Register(provider);
            }
        }

        private void OnDestroy()
        {
            ComponentLocator<ProviderScope>.Invalidate();
        }
    }

    partial class ProviderScope
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