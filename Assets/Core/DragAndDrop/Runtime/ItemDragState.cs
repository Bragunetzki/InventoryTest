using System;
using Core.Items.Runtime;
using Core.Utils;

namespace Core.DragAndDrop.Runtime
{
    public class ItemDragState
    {
        public event Action StateChanged;
        
        public Result<Item> HeldItem { get; private set; }
        public string SourceContainerKey { get; private set; }
        public int SourceSlotIndex { get; private set; }
        public bool TakenFromNonAcceptingSlot { get; set; }
        public bool IsHoldingItem => HeldItem.Exists;

        public void PickUp(Item item, string sourceContainerKey, int sourceSlotIndex)
        {
            HeldItem = new Result<Item>(item, true);
            SourceContainerKey = sourceContainerKey;
            SourceSlotIndex = sourceSlotIndex;
            StateChanged?.Invoke();
        }
    
        public void Clear()
        {
            HeldItem = default;
            StateChanged?.Invoke();
        }
    }
}