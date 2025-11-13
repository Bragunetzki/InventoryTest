using System;
using Core.Inventory.Runtime.Config;
using Core.Utils;
using UnityEngine;

namespace Core.Inventory.Runtime.View
{
    public interface IInventoryView
    {
        event Action<int> SlotDragStarted;
        event Action<int> DroppedOnSlot;
        event Action<Vector2> SlotDragUpdated;
        event Action SlotDragEnded;

        void Init(InventoryConfig config);
        Result<IItemSlotView> GetSlot(int slotIndex);
    }
}