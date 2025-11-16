using System;
using System.Collections.Generic;
using Core.ContainerResolver;
using Core.Crafting.Runtime.Recipes;
using Core.Crafting.Runtime.Recipes.Config;
using Core.Crafting.Runtime.View;
using Core.Inventory.Runtime;
using Core.Items.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Core.Crafting.Runtime
{
    public class CraftingSystem : IInitializable, IDisposable, ITickable, ISlotContainerProvider
    {
        private const string CONTAINER_KEY = "CraftingContainer";
        private const int GRID_WIDTH = 3;
        private const int GRID_HEIGHT = 3;

        private readonly ICraftingGridView _gridView;
        private readonly CraftingGrid _craftingGrid;
        private readonly CraftingGridPresenter _gridPresenter;
        private readonly Crafter _crafter;
        private readonly RecipesHolder _recipesHolder;

        private bool _checkCraftingNextTick;

        [Inject]
        public CraftingSystem(
            ICraftingGridView gridView,
            InputSystem_Actions input,
            ItemFactory itemFactory,
            IRecipeConfigLoader configLoader)
        {
            _gridView = gridView;
            _craftingGrid = new CraftingGrid(CONTAINER_KEY, GRID_WIDTH, GRID_HEIGHT);
            _crafter = new Crafter(_craftingGrid, itemFactory);
            _gridPresenter = new CraftingGridPresenter(CONTAINER_KEY, input.Player.Split, _crafter, _craftingGrid, _gridView);
            _recipesHolder = new RecipesHolder(configLoader.LoadRecipes());
        }

        public void Initialize()
        {
            _gridPresenter.Init(QueueCraftingCheck);
            List<Recipe> recipes = _recipesHolder.GetRecipes();
            foreach (Recipe recipe in recipes)
            {
                _crafter.RegisterRecipe(recipe);
            }
        }

        public ISlotContainer GetSlotContainer()
        {
            return _craftingGrid;
        }

        public void Tick()
        {
            if (_checkCraftingNextTick)
            {
                _gridPresenter.CheckCrafting();
                _checkCraftingNextTick = false;
            }
        }

        private void QueueCraftingCheck()
        {
            _checkCraftingNextTick = true;
        }

        public void Dispose()
        {
            _gridPresenter.Dispose();
        }

    }
}