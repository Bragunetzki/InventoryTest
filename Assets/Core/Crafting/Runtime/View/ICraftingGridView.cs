using Core.Inventory.Runtime.View;
using Core.Utils;
using UnityEngine.InputSystem;

namespace Core.Crafting.Runtime.View
{
    public interface ICraftingGridView
    {
        void Init(InputAction splitAction, string containerKey, int totalSlots);
        Result<IItemSlotView> GetSlot(int slotIndex);
    }
}