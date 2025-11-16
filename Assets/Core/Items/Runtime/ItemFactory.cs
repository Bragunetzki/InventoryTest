using Core.Items.Runtime.Definition;
using Core.Utils;
using VContainer;

namespace Core.Items.Runtime
{
    public class ItemFactory
    {
        private readonly ItemDefinitionManager _definitionManager;

        [Inject]
        public ItemFactory(ItemDefinitionManager definitionManager)
        {
            _definitionManager = definitionManager;
        }
        
        public Result<Item> CreateItem(string itemKey)
        {
            Result<ItemDefinition> definition = _definitionManager.GetDefinition(itemKey);
            if (!definition.Exists)
            {
                return default;
            }

            return new Result<Item>(new Item(definition.Object), true);
        }
    }
}