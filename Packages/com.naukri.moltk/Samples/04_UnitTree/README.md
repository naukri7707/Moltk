# UnitTree 單元樹

單元樹可以幫助你快速製作章節類型的教學架構。

- 對於所有的單元樹，你都需要一個 `UnitTreeController` 來驅動，這個控制器同時也會是這棵單元樹的根節點。
- 建議在根節點上掛載 `UnitTreeDebugger`，它提供了許多實用功能，如葉節點選擇器、批量重新命名等，方便你進行偵錯和設定單元樹。另外當你掛載 `UnitTreeDebugger` 時，它會自動為你掛載 `UnitTreeController`。
- 當節點移動時，會從當前頁節點逐一調用 `OnExit()` 方法，直到達到與目標葉節點的最近共同祖先 (LCA, Lowest Common Ancestor) 後，再從該節點逐一調用 `OnEnter()` 方法，直到達到目標節點。
- 如果你希望在單元樹的任一節點上產生行為，需要建立一個繼承自 `UnitTreeBehaviour` 的類別，並覆寫 `OnEnter()`、`OnExit()` 或 `HandleEvent()` 方法，以在目標情境下產生行為。

> 單元樹沒有複雜的背景技術，只是一個簡單的節點遍歷工具，並在特定時間段調用指定行為，因此框架設計較為空泛。建議參考範例場景 `UnitTree` 來更清楚地了解如何具體實作。


