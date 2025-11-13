using System;
using System.Collections.Generic;
using Core.AssetLoader;
using Core.Utils;
using UnityEngine;
using VContainer;

namespace Core.Items.Runtime
{
    public class ItemDefinitionManager
    {
        private const string ADDRESS_FORMAT = "ItemConfig/{0}";
        
        private readonly Dictionary<string, ItemDefinition> _cachedDefinitions = new();
        private readonly HashSet<Guid> _ids = new();
        private readonly SimpleAssetLoader _assetLoader;

        [Inject]
        public ItemDefinitionManager(SimpleAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }
        
        public Result<ItemDefinition> GetDefinition(string itemName)
        {
            if (_cachedDefinitions.TryGetValue(itemName, out ItemDefinition definition))
            {
                return new Result<ItemDefinition>(definition, true);
            }

            var address = string.Format(ADDRESS_FORMAT, itemName);
            Result<ItemConfig> configResult = _assetLoader.LoadAssetSync<ItemConfig>(address);
            if (configResult.Exists)
            {
                ItemConfig config = configResult.Object;
                Guid id = CreateDefinitionId();
                definition = new ItemDefinition(id, config.Icon, config.ItemName, config.StackSize);
                _cachedDefinitions[itemName] = definition;
                return new Result<ItemDefinition>(definition, true);
            }

            return default;
        }

        private Guid CreateDefinitionId()
        {
            var id = Guid.NewGuid();
            
            // astronomically unlikely
            while (_ids.Contains(id))
            {
                id = Guid.NewGuid();
            }

            _ids.Add(id);
            return id;
        }
    }
}