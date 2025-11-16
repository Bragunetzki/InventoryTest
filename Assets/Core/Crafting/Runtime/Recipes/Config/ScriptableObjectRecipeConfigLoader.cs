using System.Collections.Generic;
using Core.AssetLoader;
using Core.Utils;
using VContainer;

namespace Core.Crafting.Runtime.Recipes.Config
{
    public class ScriptableObjectRecipeConfigLoader : IRecipeConfigLoader
    {
        private const string RECIPES_ADDRESS = "RecipesConfig";
        
        private readonly SimpleAssetLoader _assetLoader;

        [Inject]
        public ScriptableObjectRecipeConfigLoader(SimpleAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public List<RecipeConfig> LoadRecipes()
        {
            Result<RecipesConfig> configResult = _assetLoader.LoadAssetSync<RecipesConfig>(RECIPES_ADDRESS);
            if (configResult.Exists)
            {
                return configResult.Object.Recipes;
            }
            
            return new List<RecipeConfig>();
        }
    }
}