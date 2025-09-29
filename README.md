## VariableRowGrid

Custom LayoutGroup for Unity UI that lays out children into rows with a variable number of columns per row. Supports fixed and dynamic spacing (in percentages), row alignment (left/center/right), and preserves a consistent cell aspect ratio.

### Features
- **Variable columns per row**: configured via the `rowCounts` list.
- **Row alignment**: `Left`, `Center`, `Right`.
- **Spacing**: fixed (`spacing`) or dynamic in percentages (`dynamicSpacing` + `spacingPercentage`).
- **Stable aspect ratio**: derived from `cellReferenceSize`.
- **Force update**: call `ForceUpdate()`.

### Installation
1. Copy `VariableRowGrid.cs` into your Unity project (e.g., `Assets/Scripts/`).
2. Add the `VariableRowGrid` component to a `RectTransform` object.
3. Add the children you want to lay out under this container.

### Configuration
- **rowCounts (List<int>)**: number of columns per row, e.g. `[3,4,3]`.
- **rowAlignment (RowAlignment)**: `Left`, `Center`, `Right`.
- **spacing (Vector2)**: spacing between cells; `x` is horizontal, `y` is vertical.
- **dynamicSpacing (bool)**: if enabled, spacing is computed as a percentage of the container size.
- **spacingPercentage (Vector2)**: percentages used when `dynamicSpacing=true`. `x` from width, `y` from height.
- **cellReferenceSize (Vector2)**: base size used to compute the cell aspect ratio (`height/width`).
- **onStartOnly (bool)**: when off, layout updates every frame (in `Update`).
- **ForceUpdate()**: triggers recalculation and layout immediately.

### How it works (brief)
- Cell width: `(availableWidth - spacingX * (maxColumns - 1)) / maxColumns`.
- Cell height: `cellWidth * (cellReferenceSize.y / cellReferenceSize.x)`.
- For each row, `startX` is computed based on `rowAlignment` and the row width.

### Tips
- For responsive spacing across resolutions, enable `dynamicSpacing` and set `spacingPercentage`.
- Avoid zero width in `cellReferenceSize.x` (guard exists in code, but use valid values).
- Can be combined with `ContentSizeFitter`/`LayoutElement` if needed.
