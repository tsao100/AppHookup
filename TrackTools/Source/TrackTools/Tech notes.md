# Revit 2020 → Revit 2026 (.NET Framework → Modern .NET) 轉移指南

## 1. 結論摘要

-   **Revit 2020--2024**：.NET Framework 4.8\
-   **Revit 2025**：.NET 6 (Windows)\
-   **Revit 2026**：.NET 8 (Windows)

> ❌ 無法無痛直接轉\
> ✅ 90% 以上 Revit API 邏輯可完整保留

真正需要調整的只有： 1. Macro → ExternalCommand 2. UI（WinForms / WPF）
3. Framework-only 第三方套件

------------------------------------------------------------------------

## 2. 可 100% 保留的程式碼範圍

-   Transaction / Selection
-   AdaptiveComponentInstanceUtils
-   ReferencePoint / Curve Reference
-   GeometryElement / Solid（讀取）
-   Parameter 操作

這些 API 在 Revit 2026 仍然一致。

------------------------------------------------------------------------

## 3. 必須調整的部分

### 3.1 Macro → ExternalCommand

``` csharp
[Transaction(TransactionMode.Manual)]
public class AddInstanceCmd : IExternalCommand
{
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        UIApplication uiapp = commandData.Application;
        UIDocument uidoc = uiapp.ActiveUIDocument;
        Document doc = uidoc.Document;
        return Result.Succeeded;
    }
}
```

------------------------------------------------------------------------

### 3.2 UI 技術選擇

  技術       建議
  ---------- --------------------
  WinForms   可用但不推薦
  WPF        官方建議，長期穩定

------------------------------------------------------------------------

### 3.3 第三方 DLL 注意事項

避免使用： - AppDomain / Remoting - BinaryFormatter - Excel
Interop（建議改 OpenXML / EPPlus）

------------------------------------------------------------------------

## 4. 正確的遷移架構（強烈建議）

    AICAD.Core (netstandard2.0)
     ├─ Geometry / Algorithm
     ├─ TrackBuilder
     └─ Revit-independent Logic

    AICAD.Revit2020 (net48)
     └─ 呼叫 Core

    AICAD.Revit2026 (net8.0-windows)
     └─ 呼叫 Core

------------------------------------------------------------------------

## 5. 為何選 netstandard2.0

  Target           可行性
  ---------------- ---------------------
  netstandard2.0   ✅ Framework + .NET
  netstandard2.1   ❌ Framework 不支援
  net8.0 only      ⚠️ 僅新 Revit

------------------------------------------------------------------------

## 6. 適用於鐵路 / 線性工程 BIM

-   軌道中心線 → 控制線
-   Adaptive Component → 斷面 / Loft
-   API 控制點 → 幾何唯一入口

這套方法在 Revit 2026 仍然成立。

------------------------------------------------------------------------

## 7. 總結

> **不要再投資 Macro**
>
> **以 ExternalCommand + Core Library 為長期策略**

此文件可直接作為： - 技術轉移說明 - 團隊開發規範 - GitHub 專案 README

------------------------------------------------------------------------
