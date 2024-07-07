using Naukri.Moltk.Fusion;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.UnitTree.Providers;
using UnityEngine.UI;

public class IntroPanelController : ViewController
{
    public UnitTreeController unitTreeController;

    public Text titleText;

    public Text contentText;

    public Text pageNumberText;

    public Button prevButton;

    public Button nextButton;

    private IntroTextProvider introTextProvider;

    private PageNumberProvider pageNumberProvider;

    public void NextPage()
    {
        unitTreeController.MoveNext();
    }

    public void PrevPage()
    {
        unitTreeController.RollBack();
    }

    protected override void Render()
    {
        var introText = introTextProvider.State;
        var pageNumber = pageNumberProvider.State;
        //
        titleText.text = introText.Title;
        contentText.text = introText.Content;
        //
        pageNumberText.text = pageNumber.PageNumberText;
        prevButton.gameObject.SetActive(pageNumber.Current > 0);
        nextButton.gameObject.SetActive(pageNumber.Current < pageNumber.Total - 1);
    }

    protected override void OnInitialize(IContext ctx)
    {
        base.OnInitialize(ctx);
        //
        introTextProvider = ctx.Watch<IntroTextProvider>();
        pageNumberProvider = ctx.Watch<PageNumberProvider>();

        // 設定按鈕點擊事件
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
    }
}
