# Moltk

Moltk 是一款專為機械操作訓練開發的 VR 開發工具包，旨在協助開發者輕鬆建構類似專案。它採用模組化的設計理念，提供了完整的開發框架，並透過直覺的開發者介面來降低開發門檻。

## 功能特色

- `單元樹`：提供節點遍歷工具，協助開發者建立結構化的教學系統，可於特定時機執行指定行為。透過單元樹，您可以輕鬆管理教學流程，打造更有組織的教學內容。

- `存檔系統`：採用鍵值結構的資料庫設計，支援無限巢狀結構，使用 Newtonsoft Json 進行序列化。系統會自動追蹤狀態變化，確保學習歷程能被完整記錄。

- `XR 互動`：基於 XR Interaction Toolkit 開發，提供手輪、開關、旋鈕等常見的工業操作控制元件。每個元件都經過優化設計，能為使用者帶來更直覺的操作體驗。

- `外輪廓系統`：基於 UnityFx.Outline 開發的視覺回饋系統，整合了物件外輪廓及工具提示功能。系統能夠有效引導使用者注意重點，並提供即時的操作提示。

- `變形系統`：提供網格變形功能，用於模擬加工過程中的工件變化。系統採用優化演算法，在保持良好效能的同時，也能呈現細緻的加工效果。

## 相依套件

- [Unity XRITK-2.5.2](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/index.html)
- [Inspector Maid-2.12.2](https://github.com/naukri7707/InspectorMaid)
- [Physarum-2.12.0](https://github.com/naukri7707/Physarum)
- [UnityFx.Outline-3.2.1](https://github.com/Arvtesh/UnityFx.Outline)
- [NewtonsoftJson](https://www.newtonsoft.com/json) 

## 快速安裝

1. 建立一個 Unity 版本 2022.3.11f1 或以上的專案

2. 安裝所需的 Git 相依套件：
   - Inspector Maid
   - Physarum
   - UnityFx.Outline (建議透過 [Npm package](https://github.com/Arvtesh/UnityFx.Outline?tab=readme-ov-file#npm-packages) 進行安裝)

3. 在 PackageManager 中點擊左上角的 + 號並選擇 `Add package form git URL...` 然後貼以下網址以安裝 Moltk：

    ```bash
    https://github.com/naukri7707/Moltk.git
    ```

4. 完成後即可使用 Moltk。