# Moltk

Moltk 是一款簡便的機械操作訓練開發工具包,旨在協助開發者輕鬆構建類似專案。

## 功能特色

- `SupplyChain`: 一種基於觀察者模式的狀態管理框架，其獨特之處在於任何`Subject`實例均可作為 `Observer` 使用，以此大幅簡化狀態鏈式更新的流程。
- `MVU`: 基於 `SupplyChain` 實現的極簡 MVU 框架。開發者只需一個腳本即可完成整個 MVU 設計並訂閱`SupplyChain` 中的 `Provider` 以在 `Provider` 狀態更新時自動更新。
- `Outline`: 基於 [UnityFx.Outline](https://github.com/Arvtesh/UnityFx.Outline) 的高光工具。
- `DataStorage`: 利用 [Newtonsoft Json Unity Package](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html) 實作的鍵值記憶體資料庫。並集成至 `UnitTree` 中，讓開發者能於單元樹中任一節點預覽該節點資料。
- `UnitTree`: 一種模擬教學架構中章節類型的節點遍歷工具，可於特定時機執行指定行為。
- `XR Interaction`: 使用 Unity XRITK 實作的工業操作常用控制元件。依循設定步驟適配後，即可建立常見控件的行為 (例如手輪旋轉)；也可透過繼承並覆寫的方式進一步客製化行為。
- `MeshDeformance`: 模型形變工具，可用於模擬工件切銷。

## 相依套件

- [Unity XRITK-2.5.2](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/index.html)
- [Inspector Maid-2.12.2](https://github.com/naukri7707/InspectorMaid)
- [Newtonsoft Json Unity Package-0.8.5](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html) 
- [UnityFx.Outline-3.2.1](https://github.com/Arvtesh/UnityFx.Outline)

## 快速安裝

要快速安裝 Moltk，請按照以下步驟操作：

1. 建立一個 Unity 版本 2022.3.11f1 或以上的專案
2. 安裝 Inspector Maid 和 UnityFx.Outline
3. 在 PackageManager 中點擊左上角的 + 號並選擇 `Add package form git URL...` 然後貼以下網址。

    ```bash
    https://github.com/naukri7707/Moltk.git
    ```

4. 完成後即可使用 Moltk。
5. 首次使用可以參考專案附帶的範例場景，部分範例場景使用 UIToolkit 建立 UI，你需要使用

> Create > Ul Toolkit > Panel Settings Asset

建立一個 Setting Panel 並掛載給 UIDocument 的 Panel Settings 才能使 UI 正常運作。
