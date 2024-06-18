using Naukri.Moltk.Core;
using System;
using System.ComponentModel.Design;
using UnityEngine;

namespace Naukri.Moltk
{
    public partial class MoltkManager : SingletonBehaviour<MoltkManager>
    {
        private readonly ServiceContainer serviceContainer = new();

        [SerializeField]
        private MoltkService[] services = new MoltkService[0];

        public MoltkService[] Services => services;

        internal void RegisterService(MoltkService service)
        {
            Array.Resize(ref services, services.Length + 1);
            services[^1] = service;
        }

        protected override void OnInit()
        {
            base.OnInit();
            foreach (var service in services)
            {
                serviceContainer.AddService(service.GetType(), service);
            }
        }
    }

    partial class MoltkManager
    {
        public static T GetService<T>() where T : MoltkService
        {
            return Instance.serviceContainer.GetService(typeof(T)) as T;
        }
    }
}
