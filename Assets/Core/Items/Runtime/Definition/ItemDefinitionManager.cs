using System.Collections.Generic;
using Core.Items.Runtime.Config;
using Core.Utils;
using VContainer;

namespace Core.Items.Runtime.Definition
{
    public class ItemDefinitionManager
    {
        private readonly Dictionary<string, ItemDefinition> _namesToDefinitions = new();
        private readonly IItemConfigLoader _configLoader;

        [Inject]
        public ItemDefinitionManager(IItemConfigLoader configLoader)
        {
            _configLoader = configLoader;
        }
        
        public Result<ItemDefinition> GetDefinition(string itemKey)
        {
            if (_namesToDefinitions.TryGetValue(itemKey, out ItemDefinition definition))
            {
                return new Result<ItemDefinition>(definition, true);
            }

            Result<IItemConfig> configResult = _configLoader.LoadConfig(itemKey);
            
            if (configResult.Exists)
            {
                IItemConfig config = configResult.Object;
                definition = new ItemDefinition(config.Icon, config.ItemKey, config.StackSize);
                _namesToDefinitions[itemKey] = definition;
                return new Result<ItemDefinition>(definition, true);
            }

            return default;
        }
    }
}