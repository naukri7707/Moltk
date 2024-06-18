using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using System.Linq;
using UnityEngine.UIElements;

namespace Naukri.Moltk.Core
{
    public abstract partial class MoltkService : MoltkBehaviour
    {
        private bool isInit;

        internal bool IsRegistered => MoltkManager.Instance.Services.Contains(this);

        internal void RegisterService()
        {
            MoltkManager.Instance.RegisterService(this);
        }

        protected virtual void OnInit() { }
    }

    [
        Base,
        RowScope, HideIf(nameof(IsRegistered)),
            HelpBox("Service wasn't register to the manager.", HelpBoxMessageType.Warning), Style(flexGrow: "1"),
            Button("Register", binding: nameof(RegisterService)),
        EndScope,
    ]
    partial class MoltkService
    { }
}
