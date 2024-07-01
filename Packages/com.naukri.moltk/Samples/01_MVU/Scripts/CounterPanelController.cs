using Naukri.Moltk.MVU;
using UnityEngine.UIElements;

// 定義一個 State 的 record 用來管理 CounterPanelController 的狀態
// record 可以簡單理解為 "不可變" 的類別，每次修改都須要產生一個新的實例，這有助於我們追蹤狀態變化
// 如果你不了解 record，可以參考官方文檔 https://learn.microsoft.com/zh-tw/dotnet/csharp/fundamentals/types/records
public record CounterPanelState(
    int Counter
    )
{
    // 我們需要建立公用 (public) 無參數的建構子來讓 MVUController 可以初始化 State
    public CounterPanelState() : this(
        Counter: 0)
    {
    }
}

// 定義一個 Controller 用來控制 CounterPanel
public class CounterPanelController : MVUController<CounterPanelState> // 記得要繼承 MVUController，泛行參數為 CounterPanelState
{
    public int startNumber = 0;

    private Label numberLabel;

    private Button addButton;

    private Button resetButton;

    // 覆寫 Awake 用來取得 UI 控件、處理資料綁定和註冊事件
    protected override void Awake()
    {
        base.Awake();
        // 這裡使用了新的的 UI 框架 `UI Toolkit`
        // 你仍然可以使用舊版的 UI 框架實作並使用 GetComponent 取得目標控件
        var root = GetComponent<UIDocument>().rootVisualElement;
        // 取得目標控件
        numberLabel = root.Q<Label>("number-label");
        addButton = root.Q<Button>("add-button");
        resetButton = root.Q<Button>("reset-button");
        // 註冊事件
        addButton.clicked += AddOne;
        resetButton.clicked += ResetCounter;
    }

    // 覆寫 Start 用來初始化
    protected override void Start()
    {
        ResetCounter();
    }

    // 覆寫抽象方法 Build，他會在每次訂閱的 Provider 狀態被改變時被呼叫
    // MVUController 會在 Awake 的時候訂閱自己本身，所以當 State 被改變時，此方法便會被呼叫
    // 我們要利用這個方法更新我的的 UI
    protected override void Build(IProvider provider)
    {
        // 判斷此次通知是來自於哪個 Provider；在本例中我們只有一個 Provider : CounterPanelController
        // 因此只會有一個 if，如果你訂閱了多個 Provider，你會使用到更多的 if 來判斷
        if (provider is CounterPanelController counterPanelController)
        {
            // 我們確認了此次通知是來自於 CounterPanelController 之後，我們就可以取得他的狀態
            var state = counterPanelController.State;
            // 使用狀態更新 UI
            numberLabel.text = state.Counter.ToString();
        }
    }

    private void AddOne()
    {
        // 更新狀態，請記住每次更新狀態時都應該使用 `State = ....` 而非 `State.Counter = ...` 否則框架將無法追蹤到你的資料更新
        // 在這裡我們另外使用 with 關鍵字來輔助我們賦值這段程式碼等效於
        // var newState = new CounterPanelState()
        // {
        //     Counter = State.Counter + 1
        // };
        // State = newState;
        // 想了解更多請參考官方文檔 : https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/with-expression
        State = State with
        {
            Counter = State.Counter + 1
        };
    }

    private void ResetCounter()
    {
        State = State with
        {
            Counter = startNumber
        };
    }
}
