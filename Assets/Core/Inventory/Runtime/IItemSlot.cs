using Core.Items.Runtime;
using Core.Utils;

namespace Core.Inventory.Runtime
{
    public interface IItemSlot
    {
        bool IsOccupied { get; }
        Result<Item> OccupyingItem { get; }
        bool CanAcceptItems { get; }
    }
}