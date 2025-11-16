using System;
using Core.Inventory.Runtime.Config;
using Core.Utils;
using UnityEngine.InputSystem;

namespace Core.Inventory.Runtime.View
{
    public interface IInventoryView
    {
        event Action RandomizeInventoryClicked;
        event Action ClearInventoryClicked;
        
        void Init(InputAction splitAction, string containerKey, InventoryConfig config);
        Result<IItemSlotView> GetSlot(int slotIndex);
    }
}