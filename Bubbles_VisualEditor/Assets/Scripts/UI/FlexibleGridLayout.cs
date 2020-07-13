using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIAddons
{
    public class FlexibleGridLayout : LayoutGroup
    {

        //protected override void OnValidate()
        //{
        //    rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
        //}
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        [SerializeField] private FitType fitType = FitType.Uniform;
        [SerializeField] private int rows = 1, columns = 1;
        [SerializeField] private Vector2 cellSize = new Vector2(100, 100);
        [SerializeField] private Vector2 spacing = new Vector2(0, 0);
        [SerializeField] private bool SetCellHeight, SetCellWidth,AutoWidth,AutoHeight;
        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();

            float fcol = (float)columns;
            float frow = (float)rows;


            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                SetCellHeight = false;
                SetCellWidth = false;
                

                float sqrChilden = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrChilden);
                columns = Mathf.CeilToInt(sqrChilden);
            }


            if (fitType == FitType.Width || fitType == FitType.FixedColumns) rows = Mathf.CeilToInt(transform.childCount / fcol);
            else if (fitType == FitType.Height || fitType == FitType.FixedRows) columns = Mathf.CeilToInt(transform.childCount / frow);

         
            rectTransform.sizeDelta = new Vector2(AutoWidth ? cellSize.x*columns+padding.left+padding.right+spacing.x*(columns-1) : rectTransform.sizeDelta.x, AutoHeight ? cellSize.y * rows + padding.top + padding.bottom+spacing.y*(rows-1) : rectTransform.sizeDelta.y);
         
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;



            cellSize.x = SetCellHeight ? cellSize.x : ((parentWidth - padding.left - padding.right) / fcol) - (spacing.x / fcol * (columns - 1));
            cellSize.y = SetCellWidth ? cellSize.y : ((parentHeight - padding.top - padding.bottom) / frow) - (spacing.y / frow * (rows - 1));

            int colIndex = 0;
            int rowIndex = 0;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowIndex = i / columns;
                colIndex = i % columns;

                var item = rectChildren[i];

                float x = (cellSize.x * colIndex) + (spacing.x * colIndex) + padding.left;
                float y = (cellSize.y * rowIndex) + (spacing.y * rowIndex) + padding.top;



                SetChildAlongAxis(item, 0, x, cellSize.x);
                SetChildAlongAxis(item, 1, y, cellSize.y);
            }

        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }

    }
}


