using System.Collections.Generic;
using Core.Crafting.Runtime.Recipes;
using Core.Inventory.Runtime;
using Core.Items.Runtime;
using Core.Utils;

namespace Core.Crafting.Runtime
{
    public class Crafter
    {
        private readonly List<Recipe> _recipes = new();
        private readonly CraftingGrid _craftingGrid;
        private readonly ItemFactory _itemFactory;

        public Crafter(CraftingGrid grid, ItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
            _craftingGrid = grid;
        }

        public void RegisterRecipe(Recipe recipe)
        {
            _recipes.Add(recipe);
        }

        public Result<Item> CheckCrafting()
        {
            IItemSlot[,] grid = _craftingGrid.GetCraftSlots();

            foreach (Recipe recipe in _recipes)
            {
                if (MatchesRecipe(grid, recipe))
                {
                    return CreateResult(recipe);
                }
            }

            return default;
        }
        
        private bool MatchesRecipe(IItemSlot[,] grid, Recipe recipe)
        {
            int gridHeight = grid.GetLength(0);
            int gridWidth = grid.GetLength(1);
            int patternHeight = recipe.Pattern.GetLength(0);
            int patternWidth = recipe.Pattern.GetLength(1);
            
            for (int startRow = 0; startRow <= gridHeight - patternHeight; startRow++)
            {
                for (int startCol = 0; startCol <= gridWidth - patternWidth; startCol++)
                {
                    if (PatternMatchesAtPosition(grid, recipe.Pattern, startRow, startCol))
                    {
                        if (NoExtraItems(grid, startRow, startCol, patternHeight, patternWidth))
                        {
                            return true;
                        }
                    }
                }
            }
        
            return false;
        }

        private bool PatternMatchesAtPosition(IItemSlot[,] grid, string[,] pattern, int startRow, int startCol)
        {
            int patternHeight = pattern.GetLength(0);
            int patternWidth = pattern.GetLength(1);
        
            for (int row = 0; row < patternHeight; row++)
            {
                for (int col = 0; col < patternWidth; col++)
                {
                    IItemSlot itemSlot = grid[startRow + row, startCol + col];
                    var itemKey = pattern[row, col];
                
                    if (string.IsNullOrEmpty(itemKey))
                    {
                        if (itemSlot.IsOccupied)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!itemSlot.IsOccupied)
                        {
                            return false;
                        }

                        if (itemSlot.OccupyingItem.Object.Definition.Key != itemKey)
                        {
                            return false;
                        }
                    }
                }
            }
        
            return true;
        }
        
        private bool NoExtraItems(
            IItemSlot[,] grid, 
            int patternStartRow, 
            int patternStartCol, 
            int patternHeight, 
            int patternWidth)
        {
            int gridHeight = grid.GetLength(0);
            int gridWidth = grid.GetLength(1);
        
            for (int row = 0; row < gridHeight; row++)
            {
                for (int col = 0; col < gridWidth; col++)
                {
                    if (row >= patternStartRow && row < patternStartRow + patternHeight &&
                        col >= patternStartCol && col < patternStartCol + patternWidth)
                    {
                        continue;
                    }
                
                    if (grid[row, col].IsOccupied)
                    {
                        return false;
                    }
                }
            }
        
            return true;
        }
        
        private Result<Item> CreateResult(Recipe recipe)
        {
            Result<Item> item = _itemFactory.CreateItem(recipe.ResultItemKey);
            if (!item.Exists)
            {
                return default;
            }
            
            item.Object.Quantity = recipe.ResultQuantity;
            return item;
        }
    }
}