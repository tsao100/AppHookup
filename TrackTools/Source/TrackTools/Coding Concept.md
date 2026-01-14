# Revit 2020 Adaptive FamilyInstance 讀取與修改說明（C# Macro）

## 一、核心觀念

```csharp
FamilyInstance instance =
    AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, symbol);
```

- `FamilyInstance` 本身不是幾何容器  
- **不能直接修改 Loft / Face / Solid**
- 可修改的只有「控制條件」

| 項目 | 可否修改 |
|---|---|
| Adaptive Points | ✅ |
| Instance / Type Parameters | ✅ |
| Solid / Face / Edge | ❌（只能讀） |

---

## 二、讀取 Adaptive Placement Points（點位）

```csharp
IList<ElementId> pointIds =
    AdaptiveComponentInstanceUtils
        .GetInstancePlacementPointElementRefIds(instance);

foreach (ElementId pid in pointIds)
{
    ReferencePoint rp = doc.GetElement(pid) as ReferencePoint;
    XYZ pos = rp.Position;
}
```

---

## 三、修改 Adaptive Point（沿曲線）

```csharp
var loc = new PointLocationOnCurve(
    PointOnCurveMeasurementType.NonNormalizedCurveParameter,
    param,
    PointOnCurveMeasureFrom.Beginning);

var edgeRef = uiapp.Application.Create
    .NewPointOnEdge(mc.GeometryCurve.Reference, loc);

rp.SetPointElementReference(edgeRef);
```

- Loft / Sweep 會自動重算

---

## 四、讀取幾何（只讀）

```csharp
Options opt = new Options
{
    DetailLevel = ViewDetailLevel.Fine,
    ComputeReferences = true
};

GeometryElement geomElem = instance.get_Geometry(opt);
```

### Solid / Face / Edge

```csharp
foreach (Solid solid in geomElem.OfType<Solid>())
{
    foreach (Face face in solid.Faces)
    {
        double area = face.Area;
    }
}
```

---

## 五、修改幾何的正確方式

| 想改的結果 | 實際要改 |
|---|---|
| Loft 形狀 | Adaptive Points |
| 斷面大小 | Family Parameters |
| 軌道寬度 | Type Parameters |
| 曲率 | 控制線（ModelCurve） |

---

## 六、讀取與修改參數

```csharp
Parameter p = instance.LookupParameter("Width");
if (p != null && !p.IsReadOnly)
{
    p.Set(UnitUtils.ConvertToInternalUnits(
        2500, UnitTypeId.Millimeters));
}
```

---

## 七、總結一句話

> **Revit Adaptive Family 的 API 操作 = 改控制條件，而不是改幾何本身**

---

## 八、判斷準則

> 我現在要改的是「結果幾何」還是「控制條件」？

- 結果幾何 ❌  
- 控制條件 ✅  

---

（適用於鐵路 BIM / 線性工程 / Adaptive Component 建模）
