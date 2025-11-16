using Core.Items.Runtime;
using Core.Utils;

namespace Core.Inventory.Runtime
{
    public interface ISlotContainer
    {
        string Key { get; }
        bool TryAddItem(int slotIndex, Item item, out Item swappedItem, bool forceAccept = false);
        bool TryTakeItem(int slotIndex, int quantity, out Item item);
        bool TryTakeAllItems(int slotIndex, out Item item);
        void ForceSetItem(int slotIndex, Item item);
        Result<IItemSlot> GetSlot(int slotIndex);
    }
}