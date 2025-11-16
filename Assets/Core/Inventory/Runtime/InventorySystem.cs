using System;
using Core.AssetLoader;
using Core.ContainerResolver;
using Core.Inventory.Runtime.Config;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Core.Inventory.Runtime
{
    public class InventorySystem : IInitializable, IDisposable, ISlotContainerProvider
    {
        private const string CONFIG_ADDRESS = "InventoryConfig";
        private const string INVENTORY_CONTAINER_KEY = "InventoryContainer";

        private readonly IInventoryView _view;
        private readonly SimpleAssetLoader _assetLoader;
        private readonly ItemFactory _itemFactory;
        private readonly InputAction _splitAction;
        private readonly InventoryPresenter _presenter;
        private readonly Inventory _inventory;

        [Inject]
        public InventorySystem(
            SimpleAssetLoader assetLoader,
            InputSystem_Actions input,
            IInventoryView view,
            ItemFactory itemFactory)
        {
            _assetLoader = assetLoader;
            _splitAction = input.Player.Split;
            _view = view;
            _itemFactory = itemFactory;
            
            Result<InventoryConfig> configResult = _assetLoader.LoadAssetSync<InventoryConfig>(CONFIG_ADDRESS);
            if (!configResult.Exists)
            {
                Debug.LogError($"Can't find inventory config at address: {CONFIG_ADDRESS}");
                return;
            }

            InventoryConfig config = configResult.Object;
            _inventory = new Inventory(INVENTORY_CONTAINER_KEY, config);
            var generator = new InventoryGenerator(config.ItemsKeysToGenerate, _itemFactory);
            _presenter = new InventoryPresenter(
            INVENTORY_CONTAINER_KEY,
            _splitAction,
            _inventory,
            _view,
            config,
            generator);
        }

        public void Initialize()
        {
            _presenter.Init();
        }

        public ISlotContainer GetSlotContainer()
        {
            return _inventory;
        }

        public void Dispose()
        {
            _presenter?.Dispose();
        }
    }
}