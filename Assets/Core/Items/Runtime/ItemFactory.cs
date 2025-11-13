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
        
        public Result<Item> CreateItem(string itemName)
        {
            Result<ItemDefinition> definition = _definitionManager.GetDefinition(itemName);
            if (!definition.Exists)
            {
                return default;
            }

            return new Result<Item>(new Item(definition.Object), true);
        }
    }
}