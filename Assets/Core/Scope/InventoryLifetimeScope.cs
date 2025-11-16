using Core.AssetLoader;
using Core.ContainerResolver;
using Core.Crafting.Runtime;
using Core.Crafting.Runtime.Recipes.Config;
using Core.Crafting.Runtime.View;
using Core.DragAndDrop.Runtime;
using Core.DragAndDrop.Runtime.View;
using Core.Inventory.Runtime;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Items.Runtime.Config;
using Core.Items.Runtime.Definition;
using Core.Tooltips.Runtime;
using Core.Tooltips.Runtime.View;
using VContainer;
using VContainer.Unity;

namespace Core.Scope
{
    public class InventoryLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SimpleAssetLoader>(Lifetime.Singleton);

            RegisterInput(builder);
            RegisterItemSystem(builder);
            RegisterViews(builder);
            RegisterInventorySystem(builder);
            RegisterCraftingSystem(builder);
            RegisterDragAndDropSystem(builder);
            RegisterTooltipSystem(builder);
        }

        private void RegisterInput(IContainerBuilder builder)
        {
            var input = new InputSystem_Actions();
            input.Enable();
            builder.RegisterInstance(input);
        }

        private void RegisterItemSystem(IContainerBuilder builder)
        {
            builder.Register<ScriptableObjectItemConfigLoader>(Lifetime.Singleton)
                .As<IItemConfigLoader>();
            builder.Register<ItemDefinitionManager>(Lifetime.Singleton);
            builder.Register<ItemFactory>(Lifetime.Singleton);
        }

        private void RegisterInventorySystem(IContainerBuilder builder)
        {
            builder.Register<SlotContainerResolver>(Lifetime.Singleton);
            builder.Register<InventorySystem>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        private void RegisterCraftingSystem(IContainerBuilder builder)
        {
            builder.Register<ScriptableObjectRecipeConfigLoader>(Lifetime.Singleton)
                .As<IRecipeConfigLoader>();
            
            builder.Register<CraftingSystem>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        private void RegisterDragAndDropSystem(IContainerBuilder builder)
        {
            builder.Register<DragAndDropSystem>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InventoryView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<CraftingGridView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<DraggedItemView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<TooltipView>().AsImplementedInterfaces();
        }

        private void RegisterTooltipSystem(IContainerBuilder builder)
        {
            builder.Register<TooltipSystem>(Lifetime.Singleton).AsImplementedInterfaces();
        }

    }
}