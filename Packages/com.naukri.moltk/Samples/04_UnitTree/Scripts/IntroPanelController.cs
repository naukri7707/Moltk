using Naukri.Moltk.MVU;
using Naukri.Moltk.UnitTree;
using Naukri.Moltk.UnitTree.Providers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public record IntroPanelState() : State
{
}

public class IntroPanelController : MVUController<IntroPanelState>
{
    public UnitTreeController unitTreeController;

    private Label titleLabel;

    private Label contentLabel;

    private Label pageNumberLabel;

    private Button prevButton;

    private Button nextButton;

    public void NextPage()
    {
        unitTreeController.MoveNext();
    }

    public void PrevPage()
    {
        unitTreeController.RollBack();
    }

    protected override void Build(IProvider provider)
    {
        if (provider is IntroPanelController introPanelController)
        {
            // Nothing to do.
        }
        else if (provider is IntroTextProvider textProvider)
        {
            var state = textProvider.State;
            titleLabel.text = state.Title;
            contentLabel.text = state.Content;
        }
        else if (provider is PageNumberProvider pageNumberProvider)
        {
            var state = pageNumberProvider.State;
            pageNumberLabel.text = state.PageNumberText;
            prevButton.SetEnabled(state.Current > 0);
            nextButton.SetEnabled(state.Current < state.Total - 1);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // 獲取 UI 元件參考
        var root = GetComponent<UIDocument>().rootVisualElement;
        titleLabel = root.Q<Label>("title-label");
        contentLabel = root.Q<Label>("content-label");
        pageNumberLabel = root.Q<Label>("page-number-label");
        prevButton = root.Q<Button>("prev-button");
        nextButton = root.Q<Button>("next-button");
        // 設定按鈕點擊事件
        prevButton.clicked += PrevPage;
        nextButton.clicked += NextPage;
    }

    protected override void Start()
    {
        base.Start();
        //State = State with
        //{
        //    PageCount = LeafFinder
        //}
    }
}
