using System.Collections.Generic;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;

namespace Core.Inventory.Runtime
{
    public class InventoryGenerator
    {
        private const float EMPTY_SLOT_RATIO = 0.5f;
        
        private readonly List<string> _itemsKeysToGenerate;
        private readonly ItemFactory _itemFactory;

        public InventoryGenerator(List<string> itemsKeysToGenerate, ItemFactory itemFactory)
        {
            _itemsKeysToGenerate = itemsKeysToGenerate;
            _itemFactory = itemFactory;
        }

        public void ClearSlots(Inventory inventory)
        {
            for (int index = 0; index < inventory.Capacity; index++)
            {
                inventory.ForceSetItem(index, default);
            }
        }
        
        public void FillSlots(Inventory inventory)
        {
            for (int index = 0; index < inventory.Capacity; index++)
            {
                float emptyRoll = Random.value;
                if (emptyRoll < EMPTY_SLOT_RATIO)
                {
                    continue;
                }

                Result<Item> itemResult = CreateRandomItem();

                inventory.ForceSetItem(index, itemResult.Object);
            }
        }

        private Result<Item> CreateRandomItem()
        {
            var itemRoll = Random.Range(0, _itemsKeysToGenerate.Count);
            Result<Item> item = _itemFactory.CreateItem(_itemsKeysToGenerate[itemRoll]);
            if (item.Exists)
            {
                item.Object.Quantity = Random.Range(1, item.Object.Definition.StackSize + 1);
            }
            
            return item;
        }
    }
}