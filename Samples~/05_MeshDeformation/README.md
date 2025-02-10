# Mesh Deformation 網格變形

## 概觀

網格變形模組提供了一套即時的工件變形解決方案，可以用於模擬工業加工中的切削過程。本模組採用頂點變形的策略，透過預先在模型的邊緣部分插入額外的頂點，並在碰撞發生時移動這些頂點來完成變形效果。

### 快速開始

1. 建立工件模型：
   - 使用內建的 `CubeBuilder` 工具產生具有均勻網格分布的立方體工件
   - 或匯入使用 Blender、Maya 等 3D 建模軟體製作的工件模型

2. 設定變形組件：
   - 在工件上掛載 `DeformableObject` 組件
   - 在工具（如銑刀）上掛載 `BoxDeformer` 及 `DeformOnTriggerStay` 組件

3. 調整變形參數：
   - 設定 `BoxDeformer` 的變形方向
   - 調整 `DeformOnTriggerStay` 的觸發條件
   - 根據需求添加 `VertexModifier` 或 `ShaderPassLayer`

## API 參考

### DeformableObject

負責管理工件模型的頂點和變形行為的核心組件。

#### 屬性
- `parameters`：工件的參數設定
- `vertexModifiers`：頂點修飾器陣列
- `changeShaderPassDynamicly`：是否動態改變材質
- `materials`：材質陣列
- `shaderPassLayers`：材質層級設定

#### 方法
- `ModifyVertex(args)`：修改指定頂點的位置
- `UpdateShaderPass()`：更新材質狀態

### BoxDeformer

負責執行具體變形計算的組件。

#### 屬性
- `deformDirection`：變形方向，可選擇 Right、Left、Up、Down、Forward、Back

#### 方法
- `Deform(deformable)`：對指定工件執行變形運算

### DeformOnTriggerStay

自動化變形觸發流程的輔助組件。

#### 屬性
- `deformer`：指定的變形器
- `filter`：過濾條件
- `distanceTolerance`：位移容許值
- `angleTolerance`：角度容許值

### VertexModifier

用於微調頂點變形後的位置。系統內建兩種修飾器：
- SpacingModifier：在變形後添加微小間距
- KeepInBoxModifier：限制變形範圍在指定區域內

### ShaderPassLayerCondition

用於根據變形狀態動態調整渲染材質。系統內建兩種條件：
- DeformedCondition：根據頂點位移量判定變形程度
- CliffCondition：根據三角形內角判定表面落差

## 擴展功能

### 自定義 VertexModifier

透過繼承 VertexModifier 類別並實作 OnVertexModify 方法來建立自定義的頂點修飾邏輯：

```csharp
public class CustomModifier : VertexModifier
{
    protected override void OnVertexModify(ref Vector3 current, VertexModifierArgs args)
    {
        // 實作自定義的修飾邏輯
    }
}
```

### 自定義 ShaderPassLayerCondition

透過繼承 ShaderPassLayerCondition 類別並實作 OnEvaluation 方法來定義自己的材質切換條件：

```csharp
public class CustomCondition : ShaderPassLayerCondition
{
    protected override bool OnEvaluation(Args args)
    {
        // 實作自定義的判定邏輯
        return true;
    }
}
```