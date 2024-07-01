using Naukri.Moltk;
using Naukri.Moltk.MVU;
using Naukri.Moltk.Outline;
using UnityEngine;
using UnityEngine.UIElements;

public record OutlinePanelState(
    bool IsHighlighted,
    bool IsHovered,
    bool IsSelected
    )
{
    public OutlinePanelState() : this(
        IsHighlighted: false,
        IsHovered: false,
        IsSelected: false
        )
    {
    }
}

public class OutlinePanelController : MVUController<OutlinePanelState>
{
    public GameObject targetGameObject;

    private Button highlightButton;

    private Button hoverButton;

    private OutlineService outlineService;

    private Button selectButton;

    public void Highlight()
    {
        outlineService.ToggleHighlight(targetGameObject);
        State = State with
        {
            IsHighlighted = outlineService.IsHighlighted(targetGameObject),
        };
    }

    public void Hover()
    {
        outlineService.ToggleHover(targetGameObject);
        State = State with
        {
            IsHovered = outlineService.IsHovered(targetGameObject),
        };
    }

    public void Select()
    {
        outlineService.ToggleSelect(targetGameObject);

        State = State with
        {
            IsSelected = outlineService.IsSelected(targetGameObject),
        };
    }

    protected override void Build(IProvider provider)
    {
        if (provider is OutlinePanelController outlinePanelController)
        {
            var state = outlinePanelController.State;

            // Highlight button
            highlightButton.text = $"Highlight ({OnOffText(state.IsHighlighted)})";

            // Hover button
            hoverButton.text = $"Hover ({OnOffText(state.IsHovered)})";

            // Select button
            selectButton.text = $"Select ({OnOffText(state.IsSelected)})";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        outlineService = MoltkManager.GetService<OutlineService>();

        var root = GetComponent<UIDocument>().rootVisualElement;
        highlightButton = root.Q<Button>("HighlightButton");
        hoverButton = root.Q<Button>("HoverButton");
        selectButton = root.Q<Button>("SelectButton");

        highlightButton.clicked += Highlight;
        hoverButton.clicked += Hover;
        selectButton.clicked += Select;
    }

    private string OnOffText(bool on)
    {
        return on ? "ON" : "OFF";
    }
}
