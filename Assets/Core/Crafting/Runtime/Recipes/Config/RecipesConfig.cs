using System.Collections.Generic;
using UnityEngine;

namespace Core.Crafting.Runtime.Recipes.Config
{
    [CreateAssetMenu(fileName = "New RecipesConfig", menuName = "Crafting/RecipesConfig")]
    public class RecipesConfig : ScriptableObject
    {
        [SerializeField] private List<RecipeConfig> _recipes;
        
        public List<RecipeConfig> Recipes => _recipes;
    }
}