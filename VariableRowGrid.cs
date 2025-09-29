using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class VariableRowGrid : LayoutGroup
{
    [Header("Row Structure")]
    public List<int> rowCounts = new List<int> { 3, 4, 3 };
    [Header("Row Alignment")]
    public RowAlignment rowAlignment = RowAlignment.Center;
    public Vector2 spacing = Vector2.zero;
    [Header("Dynamic Spacing")]
    public bool dynamicSpacing = false;
    public Vector2 spacingPercentage = new Vector2(5, 5); // in %
    private int maxColumnsInRow => (rowCounts != null && rowCounts.Count > 0) ? Math.Max(1, rowCounts.Max()) : 1;
    public Vector2 cellReferenceSize = new Vector2(100, 100);
    public bool onStartOnly = true;

    private float _aspectRatio => cellReferenceSize.x != 0 ? cellReferenceSize.y / cellReferenceSize.x : 1f;

    public override void CalculateLayoutInputVertical()
    {
        // Keep calculations consistent with UpdateLayout: respect padding and dynamic spacing
        float parentWidth = rectTransform.rect.width - padding.left - padding.right;

        float spacingX = dynamicSpacing
            ? parentWidth * spacingPercentage.x / 100f
            : spacing.x;

        float spacingY = dynamicSpacing
            ? rectTransform.rect.height * spacingPercentage.y / 100f
            : spacing.y;

        float cellWidth = (parentWidth - spacingX * (maxColumnsInRow - 1)) / maxColumnsInRow;
        float cellHeight = cellWidth * _aspectRatio;

        int rowCount = Mathf.Max(0, rowCounts?.Count ?? 0);
        float totalHeight = padding.top + padding.bottom + rowCount * cellHeight + Mathf.Max(0, rowCount - 1) * spacingY;

        SetLayoutInputForAxis(totalHeight, totalHeight, -1, 1); // axis 1 = vertical
    }

    public override void SetLayoutHorizontal() => UpdateLayout();
    public override void SetLayoutVertical() => UpdateLayout();

    public enum RowAlignment
    {
        Left,
        Center,
        Right
    }

    private void Update()
    {
        if (!onStartOnly)
            UpdateLayout();
    }
    public void ForceUpdate()
    {
        CalculateLayoutInputHorizontal();
        CalculateLayoutInputVertical();
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        if (!enabled) return;

        float spacingX = spacing.x;
        float spacingY = spacing.y;

        float parentWidth = rectTransform.rect.width - padding.left - padding.right;

        if (dynamicSpacing)
        {
            spacingX = parentWidth * spacingPercentage.x / 100f;
            spacingY = rectTransform.rect.height * spacingPercentage.y / 100f;
        }

        float cellWidth = (parentWidth - spacingX * (maxColumnsInRow - 1)) / maxColumnsInRow;
        float cellHeight = cellWidth * _aspectRatio;

        int childIndex = 0;
        float y = padding.top;

        foreach (int rowCount in rowCounts)
        {
            if (childIndex >= rectChildren.Count)
                break;

            float availableWidth = rectTransform.rect.width - padding.left - padding.right;
            float availableHeight = rectTransform.rect.height - padding.top - padding.bottom;

            float rowWidth = rowCount * cellWidth + (rowCount - 1) * spacingX;
            float startX = padding.left;

            switch (rowAlignment)
            {
                case RowAlignment.Left:
                    startX = padding.left;
                    break;
                case RowAlignment.Center:
                    startX = padding.left + (availableWidth - rowWidth) / 2f;
                    break;
                case RowAlignment.Right:
                    startX = padding.left + (availableWidth - rowWidth);
                    break;
            }

            for (int i = 0; i < rowCount && childIndex < rectChildren.Count; i++, childIndex++)
            {
                var child = rectChildren[childIndex];
                float x = startX + i * (cellWidth + spacingX);
                SetChildAlongAxis(child, 0, x, cellWidth);
                SetChildAlongAxis(child, 1, y, cellHeight);
            }

            y += cellHeight + spacingY;
        }
    }
}
