using Naukri.InspectorMaid;
using Naukri.Moltk.Fusion;

// 定義 CounterData 的狀態，建議使用 record
// record 可以簡單理解為 "不可變" 的類別，每次修改都須要產生一個新的實例，這有助於我們追蹤狀態變化
// 如果你不了解 record，可以參考官方文檔 https://learn.microsoft.com/zh-tw/dotnet/csharp/fundamentals/types/records
public record CounterData(int Value = 0)
{
}

// 定義 CounterProvider，它使用 CounterData 作為狀態
public class CounterProvider : Provider<CounterData>
{
    [
        HelpBox("Play the scene, and then press Invoke to show how to work with provider and consumer.", UnityEngine.UIElements.HelpBoxMessageType.Info),
        Target
    ]
    // 簡單定義一個 +1 方法
    // 當這個方法被調用後由於狀態被重新賦值且發生變化，因此框架內部會自動通知所有訂閱它的消費者進行更新。
    public void AddOne()
    {
        // 更新狀態，我們可以使用 with 關鍵字來輔助我們賦值給 record
        // 這段程式碼等效於 (假設狀態是 class)
        // SetState(s =>
        // {
        //     s.Value++;
        //     return s;
        // });
        // 想了解更多請參考官方文檔 : https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/with-expression
        SetState(s => s with
        {
            Value = s.Value + 1
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
