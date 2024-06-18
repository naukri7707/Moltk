# Moltk

Moltk 是一個簡易的機械操作訓練開發工具包，旨在讓開發者更輕鬆地開發類似的專案。

## 特點

- **SupplyChain**：基於觀察者模式的狀態管理框架，主要特色是能夠讓任何的 `Subject` 同時作為 `Observer` 使用，使鏈式更新狀態更簡單快捷。
- **MVU**：一個基於 `SupplyChain` 實作的極簡 MVU 框架，讓你只需撰寫一個腳本就能完成整個 MVU 設計，也能夠訂閱 `SupplyChain` 中的 `Provider`，使得當 `Provider` 更新時，UI 也會自動隨之更新。
- **Outline**：基於 [UnityFx.Outline](https://github.com/Arvtesh/UnityFx.Outline) 的高光工具。
- **DataStorage**：一個極簡的鍵值記憶體資料庫，並其集成於 `UnitTree` 模組中，使你能在單元樹的任一節點預覽該節點的存檔資料。並使用 [Newtonsoft Json Unity Package](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html) 進行序列化。
- **UnitTree**：一個模擬章節類型教學架構的節點遍歷工具，允許你在特定時機 (如: 進入、離開節點) 產生對應行為。
- **XR Interaction**：利用 Unity 的 XRITK 實作了工業操作中常用的控制元件原型,如手輪、旋鈕和開關等。您可以使用這些原型作為基礎,設計更加複雜的交互體驗。
- **MeshDeformance**：模型形變工具，可以用來模擬工件切銷。

## 依賴包

- **Unity XRITK**：用於 XR 開發的工具包，支援擴增實境 (AR) 和虛擬實境 (VR)。
- **Inspector Maid**：一個方便的 Unity 編輯器擴充套件，可以更高效地管理和檢查物件。
- **Newtonsoft Json**：一個流行的 JSON 處理庫，用於序列化和反序列化資料。
- **UnityFx.Outline**：提供對物件進行輪廓描邊效果的工具。

## 快速安裝

要快速安裝 Moltk，請按照以下步驟進行：

1. **下載並導入 Moltk**：從官方網站或 GitHub 儲存庫下載 Moltk 工具包，並將其導入到你的 Unity 專案中。
2. **配置依賴項**：確保你的專案中已安裝並配置了 Unity XRITK、Inspector Maid、Newtonsoft Json 和 UnityFx.Outline 等依賴項。
3. **初始化 Moltk**：在 Unity 中啟動 Moltk 並進行初始化設置，以便開始使用各種工具和功能。

這樣，你就可以開始使用 Moltk 來開發和訓練機械操作專案了。