using System.Collections.Generic;

namespace Core.Crafting.Runtime.Recipes.Config
{
    public class RecipesHolder
    {
        private readonly List<Recipe> _recipes;
        
        public RecipesHolder(List<RecipeConfig> recipes)
        {
            _recipes = new List<Recipe>();
            foreach (RecipeConfig config in recipes)
            {
                _recipes.Add(new Recipe(config.ResultItemKey, config.ResultQuantity, config.GetPattern()));
            }
        }

        public List<Recipe> GetRecipes()
        {
            return _recipes;
        }
    }
}