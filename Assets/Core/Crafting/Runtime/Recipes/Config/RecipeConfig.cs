using System;
using UnityEngine;

namespace Core.Crafting.Runtime.Recipes.Config
{
    [Serializable]
    public class RecipeConfig
    {
        [SerializeField] private string _resultItemKey;
        [SerializeField] private int _resultQuantity;
        
        [Header("Recipe Pattern")]
        [SerializeField] private string[] _row1 = new string[3];
        [SerializeField] private string[] _row2 = new string[3];
        [SerializeField] private string[] _row3 = new string[3];

        public string ResultItemKey => _resultItemKey;
        public int ResultQuantity => _resultQuantity;

        public string[,] GetPattern()
        {
            var pattern = new string[3, 3];
            
            CopyRowToPattern(pattern, _row1, 0);
            CopyRowToPattern(pattern, _row2, 1);
            CopyRowToPattern(pattern, _row3, 2);
            
            pattern = TrimPattern(pattern);
            return pattern;
        }
        
        private void CopyRowToPattern(string[,] pattern, string[] row, int rowIndex)
        {
            if (row == null)
            {
                return;
            }
        
            int length = Mathf.Min(row.Length, 3);
            for (int col = 0; col < length; col++)
            {
                string value = row[col];
                pattern[rowIndex, col] = string.IsNullOrEmpty(value) ? null : value;
            }
        }
        
        private string[,] TrimPattern(string[,] pattern)
        {
            int minRow = 3, maxRow = -1;
            int minCol = 3, maxCol = -1;
        
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (pattern[row, col] != null)
                    {
                        minRow = Mathf.Min(minRow, row);
                        maxRow = Mathf.Max(maxRow, row);
                        minCol = Mathf.Min(minCol, col);
                        maxCol = Mathf.Max(maxCol, col);
                    }
                }
            }
        
            if (maxRow < 0)
            {
                return new string[0, 0];
            }
        
            int height = maxRow - minRow + 1;
            int width = maxCol - minCol + 1;
            string[,] trimmed = new string[height, width];
        
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    trimmed[row, col] = pattern[minRow + row, minCol + col];
                }
            }
        
            return trimmed;
        }
    }
}