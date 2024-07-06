using Naukri.Moltk.Fusion;

public class CounterConsumer : Consumer
{
    private CounterProvider counterProvider;

    // 在 OnInitialize 處理初始化
    // 不要使用 Awake 初始化，因為他無法保證所有 Provider 的 Awake 在 Consumer 之前被調用
    protected override void OnInitialize(IContext ctx)
    {
        base.OnInitialize(ctx);
        // 透過 Watch 訂閱並取得目標 CounterProvider 實例
        counterProvider = ctx.Watch<CounterProvider>();
    }

    // 在 OnBuild 消費註冊的提供者在狀態變化後提供的資料
    protected override void OnBuild()
    {
        // 消費 counterProvider 提供的資料，這裡簡單的將 Value 打印出來。
        print($"Your value: {counterProvider.State.Value}");
    }
}
