# Synergia

## 概述

這個狀態管理框架旨在為Unity專案提供一個強大而靈活的應用程式狀態管理解決方案。它建立在Provider和Consumer的概念之上,允許高效的數據流動和UI更新。

## 核心組件

### Provider

`Provider`負責管理和存儲狀態。它們可以被 `Consumer` 訂閱。

主要特點:

- 持有和管理狀態
- 當狀態改變時通知訂閱的 `Consumer`
- 可以透過覆寫 `Provider.Key` 來設定他的識別項
- 每個 `Provider` 都同時具有 `Consumer` 的特性

### Consumer

`Consumer` 訂閱 `Provider` 並對狀態變化做出反應。

主要特點:

- 透過 `Watch()` 、 `Listen()` 監聽多個任意型態的 `Provider`
- 當訂閱的 `Provider` 狀態改變時自動更新

### ViewController

`ViewController` 是專門處理基於所訂閱的 `Provider` 狀態變化而更新 UI 的特殊 Consumer。
另外有一個帶狀態 `ViewController<TState>` 可以讓 `ViewController` 訂閱自己本身的狀態變化。

主要特點:

- 會在 `Start` 階段強制 `Refresh()` 一次
- 每當訂閱的狀態改變時自動重新渲染 UI

## 使用方法

這裡建立一個 Counter App 來簡單示範 `Provider` 、 `Consumer` 和 `ViewController` 是如何協同工作的。
請參考 `Counter Sampele` 範例場景獲得完整實作場景及註解。

### 如何創建 Provider

```cs
// 定義 CounterData 的狀態
public record CounterData(int Value = 0)
{
}

// 定義 CounterProvider，它使用 CounterData 作為狀態
public class CounterProvider : Provider<CounterData>
{
    // 簡單定義一個 +1 方法
    // 當這個方法被調用後由於狀態被重新賦值且發生變化，因此框架內部會自動通知所有訂閱它的消費者進行更新。
    public void AddOne()
    {
        // 更新狀態
        SetState(s => s with
        {
            Value = State.Value + 1
        });
    }

    // 建立狀態實例，在這裡簡單生成一個新的 CounterData
    protected override CounterData Build(CounterData state)
    {
        // 我們用 ?? 運算子判斷 state 是否為 null
        // 如果是就生成一個新的實例，否不做變更返回舊的狀態
        return state ?? new CounterData();
    }
}
```

### 如何創建 Consumer

```cs
public class CounterConsumer : Consumer
{
    private CounterProvider counterProvider;

    // 在 OnInitialize 處理初始化
    // 不要使用 Awake 初始化，因為他無法保證所有 Provider 的 Awake 在 Consumer 之前被調用
    protected override void OnInitialize()
    {
        // 透過 Watch 訂閱並取得目標 CounterProvider 實例
        counterProvider = Watch<CounterProvider>();
        base.OnInitialize();
    }

    // 在 OnBuild 消費註冊的提供者在狀態變化後提供的資料
    protected override void OnBuild()
    {
        // 消費 counterProvider 提供的資料，這裡簡單的將 Value 打印出來。
        print($"Your value: {counterProvider.State.Value}");
    }
}
```

### 如何創建 ViewController

```cs
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

    protected override void OnInitialize()
    {
        base.OnInitialize();
        counterProvider = Watch<CounterProvider>();
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

    // 覆寫抽象方法 Render 來更新你的 UI，他類似於 Consumer 中的 OnBuild 會在及訂閱的 Provider 和自己的狀態更新時被呼叫
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
```

## 進階功能

### 事件系統

`Provider` 可以發送事件，而 `Consumer` 可以接收並處理這些事件。
這可以很好的處理不會保留狀態的一次性事件 (例如: 確認、取消)

```cs
public record MyState;

public class MyProvider : Provider<MyState>
{
    public void Confirm(string msg) => SendEvent(new OnConfirm(msg));

    public void Cancel() => SendEvent(new OnCancel());

    protected override MyState Build(MyState state)
    {
        return state ?? new MyState();
    }

    public record OnConfirm(string msg) : ProviderEvent;

    public record OnCancel() : ProviderEvent;
}

public class MyConsumer : Consumer
{
    protected override void OnBuild()
    {
        // do nothing
    }

    protected override void HandleEvent(Provider sender, ProviderEvent evt)
    {
        switch (evt)
        {
            case MyProvider.OnConfirm onConfirm:
                print($"confirm: {onConfirm.msg}");
                break;

            case MyProvider.OnCancel:
                print("cancel");
                break;
        }
    }
}
```

## 最佳實踐

1. 使用 `record` 來定義不可變的狀態。
2. 只在 `Provider` 中使用 `SetState` 方法來更新狀態。如果要從外部更新 `Provider` 狀態，應該在 `Provider` 中建立一個對應的方法透過該方法更新。
3. 只在 `OnIn` 中使用 `Watch` 或 `Listen` 來訂閱 `Provider`。
4. 在ViewController中將UI邏輯與狀態管理分離。
5. 使用Provider Scope來管理不同場景或層次結構中的Provider。
