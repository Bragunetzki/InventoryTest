using System;
using Core.Items.Runtime;
using Core.Utils;

namespace Core.Inventory.Runtime
{
    public class InventoryDragState
    {
        public event Action StateChanged;
        
        public Result<Item> HeldItem { get; private set; }
        public int SourceSlotIndex { get; private set; }
    
        public bool IsHoldingItem => HeldItem.Exists;
    
        public void PickUp(Item item, int sourceIndex)
        {
            HeldItem = new Result<Item>(item, true);
            SourceSlotIndex = sourceIndex;
            StateChanged?.Invoke();
        }
    
        public void Clear()
        {
            HeldItem = default;
            StateChanged?.Invoke();
        }
    }
}