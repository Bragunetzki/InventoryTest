using System;

namespace Core.Crafting.Runtime.Recipes
{
    public class Recipe
    {
        public string ResultItemKey { get; }
        public int ResultQuantity { get; }
        public string[,] Pattern { get; }
    
        public Recipe(string resultItemKey, int resultQuantity, string[,] pattern)
        {
            ResultItemKey = resultItemKey;
            ResultQuantity = resultQuantity;
            Pattern = pattern;
        }
    }
}