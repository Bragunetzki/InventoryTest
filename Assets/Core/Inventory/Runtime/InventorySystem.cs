using Core.AssetLoader;
using Core.Inventory.Runtime.Config;
using Core.Inventory.Runtime.Presenter;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Inventory.Runtime
{
    public class InventorySystem : IInitializable
    {
        private const string CONFIG_ADDRESS = "InventoryConfig";
        
        private readonly IInventoryView _view;
        private readonly SimpleAssetLoader _assetLoader;
        private readonly IDraggedItemView _draggedItemView;
        private readonly ItemFactory _itemFactory;

        private InventoryPresenter _presenter;
        private Inventory _inventory;
        private InventoryGenerator _inventoryGenerator;

        [Inject]
        public InventorySystem(
            SimpleAssetLoader assetLoader,
            IInventoryView view,
            IDraggedItemView draggedItemView,
            ItemFactory itemFactory)
        {
            _assetLoader = assetLoader;
            _view = view;
            _draggedItemView = draggedItemView;
            _itemFactory = itemFactory;
        }


        public void Initialize()
        {
            Result<InventoryConfig> configResult = _assetLoader.LoadAssetSync<InventoryConfig>(CONFIG_ADDRESS);
            if (!configResult.Exists)
            {
                Debug.LogError($"Can't find inventory config at address: {CONFIG_ADDRESS}");
                return;
            }
            
            _inventory = new Inventory(configResult.Object);
            _presenter = new InventoryPresenter(_inventory, _view, _draggedItemView);
            _inventoryGenerator = new InventoryGenerator(configResult.Object.ItemsKeysToGenerate, _itemFactory);
            
            _view.Init(configResult.Object);
            _presenter.Init();
            _inventoryGenerator.FillSlots(_inventory);
        }
    }
}