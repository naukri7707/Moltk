using Naukri.Moltk.UnitTree.Behaviours;

public class HelloSavedataNode : SavedataNode
{
    protected override void OnEnter()
    {
        base.OnEnter();
        DataCell.SetValue("name", gameObject.name);
        DataCell.SetValue("path", DataCell.Path);
        DataCell.SetValue("msg", $"Hello, I am '{gameObject.name}', my parent is '{transform.parent.name}'.");
    }
}
