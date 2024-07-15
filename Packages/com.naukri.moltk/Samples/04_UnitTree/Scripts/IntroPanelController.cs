using Naukri.Moltk.UnitTree;
using Naukri.Moltk.UnitTree.Providers;
using Naukri.Physarum;
using UnityEngine.UI;

public record IntroPanelState(string Title = "", string Content = "");

public class IntroPanelController : ViewController<IntroPanelState>.Behaviour
{
    public UnitTreeController unitTreeController;

    public Text titleText;

    public Text contentText;

    public Text pageNumberText;

    public Button prevButton;

    public Button nextButton;

    public void NextPage()
    {
        unitTreeController.MoveNext();
    }

    public void PrevPage()
    {
        unitTreeController.RollBack();
    }

    protected override IntroPanelState Build()
    {
        var state = State ?? new();
        var pageNumberState = ctx.Watch<PageNumberProvider>().State;
        //
        titleText.text = state.Title;
        contentText.text = state.Content;
        //
        pageNumberText.text = pageNumberState.PageNumberText;
        prevButton.gameObject.SetActive(pageNumberState.CurrentPage > 0);
        nextButton.gameObject.SetActive(pageNumberState.CurrentPage < pageNumberState.TotalPage - 1);

        return state;
    }

    protected override void Awake()
    {
        base.Awake();
        // 設定按鈕點擊事件
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }
}
