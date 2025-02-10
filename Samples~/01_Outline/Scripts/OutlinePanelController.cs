using Naukri.Moltk.Outline;
using Naukri.Physarum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public record OutlinePanelState(
    bool IsHighlighted = false,
    bool IsHovered = false,
    bool IsSelected = false
) { }

public class OutlinePanelController : ViewController<OutlinePanelState>.Behaviour
{
    public GameObject target;

    public Button highlightButton;

    public Button hoverButton;

    public Button selectButton;

    public Text highlightButtonText;

    public Text hoverButtonText;

    public Text selectButtonText;

    public void Highlight()
    {
        var outlineService = ctx.Read<OutlineService>();
        outlineService.ToggleHighlight(target);
        SetState(s => s with { IsHighlighted = outlineService.IsHighlighted(target), });
    }

    public void Hover()
    {
        var outlineService = ctx.Read<OutlineService>();
        outlineService.ToggleHover(target);
        SetState(s => s with { IsHovered = outlineService.IsHovered(target), });
    }

    public void Select()
    {
        var outlineService = ctx.Read<OutlineService>();
        outlineService.ToggleSelect(target);

        SetState(s => s with { IsSelected = outlineService.IsSelected(target), });
    }

    protected override OutlinePanelState Build()
    {
        var state = State ?? new();
        // Highlight button
        highlightButtonText.text = $"Highlight ({OnOffText(state.IsHighlighted)})";

        // Hover button
        hoverButtonText.text = $"Hover ({OnOffText(state.IsHovered)})";

        // Select button
        selectButtonText.text = $"Select ({OnOffText(state.IsSelected)})";
        return state;
    }

    protected override void Awake()
    {
        base.Awake();
        highlightButton.onClick.AddListener(Highlight);
        hoverButton.onClick.AddListener(Hover);
        selectButton.onClick.AddListener(Select);
    }

    private string OnOffText(bool on)
    {
        return on ? "ON" : "OFF";
    }
}
