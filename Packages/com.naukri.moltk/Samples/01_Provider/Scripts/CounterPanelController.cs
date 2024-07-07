using Naukri.Moltk.Fusion;
using UnityEngine;
using UnityEngine.UI;

public record CounterPanelState(bool IsOdd = false)
{
}

// 繼承一個 ViewController<TState> 用來實作控制 CounterPanel 的 Controller
// 如果你的 Controller 不需要狀態也可以繼承無狀態版本的 ViewController
public class CounterPanelController : ViewController<CounterPanelState>
{
    public int startNumber = 0;

    public Text numberText;

    public Button addButton;

    public Button resetButton;

    private CounterProvider counterProvider;

    protected override void OnInitialize(IContext ctx)
    {
        base.OnInitialize(ctx);
        counterProvider = ctx.Watch<CounterProvider>();
        // 註冊事件
        addButton.onClick.AddListener(AddOne);
        resetButton.onClick.AddListener(ResetCounter);
    }

    protected override void Start()
    {
        base.Start();
        ResetCounter();
    }

    protected override CounterPanelState Build(CounterPanelState state)
    {
        return (state ?? new CounterPanelState()) with
        {
            IsOdd = counterProvider.State.Value % 2 == 1
        };
    }

    // 覆寫抽象方法 Render 來更新你的 UI 。他與 Build 類似，會在及訂閱的 Provider 和自己的狀態更新時被呼叫
    // 但同時他也會在 Start 時被調用一次
    protected override void Render()
    {
        numberText.text = counterProvider.State.Value.ToString();
        numberText.color = State.IsOdd ? Color.red : Color.blue;
    }

    private void AddOne()
    {
        counterProvider.SetState(s => s with
        {
            Value = s.Value + 1
        });
    }

    private void ResetCounter()
    {
        counterProvider.SetState(s => s with
        {
            Value = startNumber
        });
    }
}
