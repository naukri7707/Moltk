using Naukri.Moltk.Fusion;
using Naukri.Moltk.Outline;
using UnityEngine;
using UnityEngine.UI;

public record OutlinePanelState(
    bool IsHighlighted = false,
    bool IsHovered = false,
    bool IsSelected = false
    )
{
}

public class OutlinePanelController : ViewController<OutlinePanelState>
{
    public GameObject targetGameObject;

    public Button highlightButton;

    public Button hoverButton;

    public Button selectButton;

    public Text highlightButtonText;

    public Text hoverButtonText;

    public Text selectButtonText;

    private OutlineService outlineService;

    public void Highlight()
    {
        outlineService.ToggleHighlight(targetGameObject);
        SetState(s => s with
        {
            IsHighlighted = outlineService.IsHighlighted(targetGameObject),
        });
    }

    public void Hover()
    {
        outlineService.ToggleHover(targetGameObject);
        SetState(s => s with
        {
            IsHovered = outlineService.IsHovered(targetGameObject),
        });
    }

    public void Select()
    {
        outlineService.ToggleSelect(targetGameObject);

        SetState(s => s with
        {
            IsSelected = outlineService.IsSelected(targetGameObject),
        });
    }

    protected override OutlinePanelState Build(OutlinePanelState state)
    {
        return state ?? new OutlinePanelState();
    }

    protected override void Render()
    {
        // Highlight button
        highlightButtonText.text = $"Highlight ({OnOffText(State.IsHighlighted)})";

        // Hover button
        hoverButtonText.text = $"Hover ({OnOffText(State.IsHovered)})";

        // Select button
        selectButtonText.text = $"Select ({OnOffText(State.IsSelected)})";
    }

    protected override void OnInitialize(IContext ctx)
    {
        base.OnInitialize(ctx);
        outlineService = ctx.Read<OutlineService>();

        highlightButton.onClick.AddListener(Highlight);
        hoverButton.onClick.AddListener(Hover);
        selectButton.onClick.AddListener(Select);
    }

    private string OnOffText(bool on)
    {
        return on ? "ON" : "OFF";
    }
}
