using Naukri.InspectorMaid;
using Naukri.Moltk.MVU;
using UnityEngine;

public class MyDataConsumer : ConsumerBehaviour
{
    public ProviderBehaviour[] subscribers;

    // 訂閱提供者，我們要訂閱 MyDataProvider，所以在 Inspector 中
    // 把 MyDataProvider 物件拖曳至 subscribers 欄位中。
    [
        Button("Subscribe", nameof(Subscribe)),
        Target, Hide
    ]
    public void Subscribe()
    {
        Subscribe(subscribers);
        foreach (var provider in subscribers)
        {
            Debug.Log($"{provider.name} subscribed");
        }
    }

    // 在這裡消費註冊的提供者在狀態變換後提供的資料
    protected override void Build(IProvider provider)
    {
        // 檢查傳入的 provider 哪個類型的提供者
        if (provider is MyDataProvider myDataProvider)
        {
            // 根據提供的資料進行消費，這裡簡單的 Log 出來。
            var value = myDataProvider.State.Value;
            Debug.Log($"Your value: {value}");
        }
    }
}
