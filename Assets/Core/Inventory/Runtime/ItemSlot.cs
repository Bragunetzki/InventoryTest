using Core.Items.Runtime;
using Core.Utils;

namespace Core.Inventory.Runtime
{
    public class ItemSlot : IItemSlot
    {
        private Result<Item> _occupyingItem;

        public bool IsOccupied => _occupyingItem.Exists;
        public Result<Item> OccupyingItem => _occupyingItem;
        public bool CanAcceptItems { get; }

        public ItemSlot(bool canAcceptItems = true)
        {
            CanAcceptItems = canAcceptItems;
        }

        public bool TryAddItem(Item item, out Item swappedItem, bool forceAccept = false)
        {
            swappedItem = null;
            
            if (!CanAcceptItems && !forceAccept)
            {
                swappedItem = item;
                return false;
            }
            
            if (!IsOccupied)
            {
                _occupyingItem = new Result<Item>(item, true);
                return true;
            }

            if (!_occupyingItem.Object.Definition.Key.Equals(item.Definition.Key))
            {
                swappedItem = _occupyingItem.Object;
                _occupyingItem = new Result<Item>(item, true);
                return false;
            }

            _occupyingItem.Object.AddQuantity(item.Quantity, out var addedQuantity);
            item.Quantity -= addedQuantity;
            swappedItem = item;
            return item.Quantity == 0;
        }

        public bool TryTakeAllItems(out Item item)
        {
            if (!IsOccupied)
            {
                item = null;
                return false;
            }
            
            return TryTakeItem(_occupyingItem.Object.Quantity, out item);
        }

        public bool TryTakeItem(int quantityToRemove, out Item takenItem)
        {
            if (!IsOccupied)
            {
                takenItem = null;
                return false;
            }

            _occupyingItem.Object.RemoveQuantity(quantityToRemove, out var removedQuantity);
            takenItem = _occupyingItem.Object.Clone();
            takenItem.Quantity = removedQuantity;
            
            if (_occupyingItem.Object.Quantity <= 0)
            {
                _occupyingItem = default;
            }

            return true;
        }

        public void ForceSetItem(Item item)
        {
            _occupyingItem = new Result<Item>(item, item != null);
        }
    }
}