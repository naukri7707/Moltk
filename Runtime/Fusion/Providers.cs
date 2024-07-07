using System;
using UnityEngine.Assertions;

namespace Naukri.Moltk.Fusion
{
    public static class Providers
    {
        public static bool KeepAlive(Provider provider)
        {
            var scope = ProviderScope.LocateOrCreate();
            UnityEngine.Object.DontDestroyOnLoad(provider);
            return scope.Register(provider);
        }

        public static T Get<T>() where T : Provider
        {
            var key = new ProviderKey<Type>(typeof(T));

            return Get<T>(key);
        }

        public static T Get<T>(ProviderKey key) where T : Provider
        {
            var scope = ProviderScope.LocateOrCreate();
            var provider = scope.GetProvider(key);

            Assert.IsNotNull(provider, $"Can not found provider {typeof(T).Name} with key {key}.");

            // 保證 Provider 已初始化
            provider.Initialize();

            return provider as T;
        }
    }
}
