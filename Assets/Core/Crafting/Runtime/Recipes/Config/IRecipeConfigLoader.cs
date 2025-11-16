using System.Collections.Generic;

namespace Core.Crafting.Runtime.Recipes.Config
{
    public interface IRecipeConfigLoader
    {
        List<RecipeConfig> LoadRecipes();
    }
}