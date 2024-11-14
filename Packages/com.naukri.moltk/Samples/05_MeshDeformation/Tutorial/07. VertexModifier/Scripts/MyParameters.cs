using Naukri.Moltk.MeshDeformation;
using Naukri.Moltk.MeshDeformation.Modifier;
using UnityEngine;

public class MyParameters : DeformableParameters, KeepInBoxModifier.IParameter
{
    [field: SerializeField]
    public BoxCollider BoxCollider { get; set; }
}
