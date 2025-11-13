using Core.AssetLoader;
using Core.Inventory.Runtime;
using Core.Inventory.Runtime.Config;
using Core.Inventory.Runtime.Presenter;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using UnityEditor.Experimental.GraphView;
using VContainer;
using VContainer.Unity;

namespace Core.Scope
{
    public class InventoryLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SimpleAssetLoader>(Lifetime.Singleton);
            
            RegisterItemSystem(builder);
            RegisterViews(builder);
            RegisterInventorySystem(builder);
        }

        private void RegisterItemSystem(IContainerBuilder builder)
        {
            builder.Register<ItemDefinitionManager>(Lifetime.Singleton);
            builder.Register<ItemFactory>(Lifetime.Singleton);
        }

        private void RegisterInventorySystem(IContainerBuilder builder)
        {
            builder.Register<InventorySystem>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<IInventoryView>();
            builder.RegisterComponentInHierarchy<IDraggedItemView>();
        }
    }
}