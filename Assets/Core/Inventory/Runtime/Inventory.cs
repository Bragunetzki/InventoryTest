using System;
using System.Collections.Generic;
using Core.Inventory.Runtime.Config;
using Core.Items.Runtime;
using Core.Utils;

namespace Core.Inventory.Runtime
{
    public class Inventory
    {
        public event Action<int> SlotChanged;
        
        private readonly List<ItemSlot> _slots = new();

        public int Capacity => _slots.Count;

        public Inventory(InventoryConfig config)
        {
            for (int i = 0; i < config.Capacity; i++)
            {
                _slots.Add(new ItemSlot());
            }
        }

        public bool TryAddItem(int slotIndex, Item item, out Item swappedItem)
        {
            var result = _slots[slotIndex].TryAddItem(item, out swappedItem);
            SlotChanged?.Invoke(slotIndex);
            return result;
        }

        public bool TryTakeItem(int slotIndex, int quantity, out Item item)
        {
            bool result = _slots[slotIndex].TryTakeItem(quantity, out item);
            if (result)
            {
                SlotChanged?.Invoke(slotIndex);
            }

            return result;
        }

        public bool TryTakeAllItems(int slotIndex, out Item item)
        {
            bool result = _slots[slotIndex].TryTakeAllItems(out item);
            if (result)
            {
                SlotChanged?.Invoke(slotIndex);
            }

            return result;
        }
        
        public Result<IItemSlot> GetSlot(int index)
        {
            if (index < 0 || index >= _slots.Count)
            {
                return default;
            }
            
            return new Result<IItemSlot>(_slots[index], true);
        }
    }
}