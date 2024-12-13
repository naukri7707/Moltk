# XR Interaction

`XRInteraction` 提供了 VR 工業操作教學專案中常用到物件行為的實作，開發者可以透過掛載以下腳本來快速完成對應功能的開發。也可以透過繼承並覆寫的方式客製化。

為了保持文檔簡潔，不會將所有細節的參數和方法都逐一介紹，如果有客製化功能的需求可以先檢查有沒有相關的設定參數，若沒有再自行實作。

## Switch 開關

> 雖然叫做開關，但其實檔位可以設定不只兩檔

1. 將物件掛上 `MoltkSwitch` 腳本並根據提示完前置設定。
2. 使用 `CurrentState` 和 `States` 指定起始狀態和全部狀態
3. 使用 `SwitchType` 選擇切換時機，你也可以不指定並透過調用`SetState()` 或 `NextState()` 來切換狀態。
4. 透過 `Events` 群組中的 `OnStateChanged` 監聽事件變化並產生對應效果即可。
## Handwheel 手輪

1. 將手輪物件的 Postion 和 Rotation 歸 0 ，如果需要旋轉的話，可以在 Handwheel 物件上新增一個父物件透過父物件進行改變。

	![Handwheel_1](./Docs/Handwheel_1.png)

2. 在手輪物件上掛 `MoltkHandwheel` 腳本，他會指引你完成剩餘的前置步驟

	![Handwheel_2](./Docs/Handwheel_2.png)

3. 完成前置步驟後你會新增一個名為 `[Handwheel] Plane` 的物件並有一個 `MoltkPlane` 腳本在上面，點擊 Show Plane 讓 Editor 顯示當前的平面。

	![Handwheel_3](./Docs/Handwheel_3.png)

4. 旋轉平面，讓平面貼合手輪，並讓藍線對齊手把。

	![Handwheel_4](./Docs/Handwheel_4.png)

5. 回到 `MoltkHandwheel`，指定 `RotateAxis` 改變旋轉軸向，不同的物件要旋轉的軸向會有所不同，在本例中需要將 `RotateAxis` 改為 `Up`。你可以使用 `Debug` 群組裡面的 `SetAngle()` 來輔助設定。但設定完成後需要將角度歸 0。
6. 完成設定後即可上機測試效果。

## Knob 旋鈕

1. 旋鈕可以理解為手輪版的開關 。前置設定的具體步驟請參考[Handwheel](#Handwheel)不進行贅述。
2. 和 `Switch` 不一樣，`Knob` 的狀態是由旋鈕旋轉的角度來決定的。因此你需要在 `KnobState` 中設定 `AngleRange` 來指定觸發範圍，若當前角度同時滿足兩個以上的範圍，會以前面的狀態優先。
3. 旋鈕不會和手輪一樣可以無限旋轉，你可以在 `Optional` 群組中開啟 Limit Angle 設定可旋轉的角度範圍。
4. 你也可以透過調用 `SetState()` 來強制改變 `Knob` 當前的狀態，這時候物件的角度會旋轉到對應 `KnobState` 的 `preferredAngle` 。

## Debug Interactor

`DebugInteractor` 可以讓你在不使用 XR 裝置的情況下進行簡單的偵錯，這在早期測試中相當方便。

1. 建立一個物件並掛上 `DebugInteractor`。
2. 把藥互動的 `XRInteractable` 物件放到 `Interactable` 欄位中。
3.  點擊 Play 在編輯器中開始遊戲，這時候你會發現原本被禁用的事件可以被觸發了。
4. 選擇事件便可以與 `XRInteractable` 做出簡單的互動了。

## Keyboard 虛擬鍵盤

由於版本不相容，我們提供了一個仿造 XRITK 3.0 的虛擬鍵盤

![XRKeyboard](./Docs/XRKeyboard.png)

1. 從 `PackageManager/Moltk/Samples` 匯入 XR Keyboard 範例

2. 將 `XRKeyboard.prefab` 拖曳至場景中

3. 在要進行輸入的 `Input` / `TMPInput` 上掛載 `XRKeyboardInput` 或 `XRKeyboardTMPInput` 組件

4. 選擇是否啟用 `UpdateImmediately` 即時更新輸入框文字

5. 完成設定後每當點擊輸入框，變會開啟虛擬鍵盤以提供輸入功能

## API 參考

### MoltkSwitch

開關控制元件的主要類別。

#### 屬性

- `CurrentState`: 取得或設定目前的狀態
- `States`: 取得或設定可用的狀態列表
- `SwitchType`: 設定狀態切換的觸發方式

#### 方法

- `SetState(string state)`: 設定指定的狀態
- `NextState()`: 切換至下一個狀態
- `ToggleState()`: 在兩個狀態間切換

#### 事件

- `OnStateChanged`: 當狀態改變時觸發

### MoltkHandwheel

手輪控制元件的主要類別。

#### 屬性

- `CurrentAngle`: 取得目前的旋轉角度
- `Plane`: 取得互動平面
- `RotateAxis`: 取得或設定旋轉軸向
- `SoftRotation`: 設定是否啟用平滑旋轉
- `LimitRotation`: 設定是否限制旋轉圈數

#### 方法

- `SetAngle(float angle, bool useCustomizer = false)`: 設定旋轉角度

#### 事件

- `OnAngleChanged`: 當角度改變時觸發
- `OnClockwiseLimitReached`: 當達到順時針旋轉限制時觸發
- `OnCounterClockwiseLimitReached`: 當達到逆時針旋轉限制時觸發

### MoltkKnob

旋鈕控制元件的主要類別。

#### 屬性

- `CurrentAngle`: 取得目前的旋轉角度
- `CurrentStateName`: 取得目前的狀態名稱
- `States`: 取得或設定可用的狀態

#### 方法

- `SetState(string stateName)`: 設定指定的狀態
- `SetAngle(float angle, bool useCustomizer = false)`: 設定旋轉角度

#### 事件

- `OnStateChanged`: 當狀態改變時觸發
- `OnAngleChanged`: 當角度改變時觸發

### XRKeyboardController

虛擬鍵盤的主要控制類別。

#### 屬性

- `CurrentText`: 取得目前輸入的文字
- `IsOpen`: 取得鍵盤是否開啟
- `Capslock`: 取得或設定大小寫狀態

#### 方法

- `SendKey(string key)`: 送出按鍵
- `Backspace()`: 刪除一個字元
- `Clear()`: 清除所有文字
- `Open(XRKeyboardBinding binding, string text)`: 開啟鍵盤
- `Confirm()`: 確認輸入
- `Cancel()`: 取消輸入