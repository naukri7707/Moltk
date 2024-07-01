using Naukri.Moltk.MVU;

public record MyData(int Value)
{
    public MyData() : this(0)
    {
    }
}

public class MyDataProvider : ProviderBehaviour<MyData>
{
    // 簡單定義一個 +1 方法
    // 當這個方法被調用後由於 State 被重新賦值且發生變化，因此框架內部會自動通知所有訂閱它的消費者嘗試進行變更。
    public void AddOne()
    {
        State = State with
        {
            Value = State.Value + 1
        };
    }

    protected override void Build(IProvider provider)
    {
        // 這裡是用來處理上游變更時要如何產生變化的。
        // 如果這個類別是供應商的話，這裡不需要寫任何程式碼。
        // 如果這個類別是製造、分銷或零售商，請參考 Consumer 範例進行實作。
    }
}
