using System;
using System.Collections.Generic;
using Core.Inventory.Runtime;
using Core.Items.Runtime;
using Core.Utils;

namespace Core.Crafting.Runtime
{
    public class CraftingGrid : ISlotContainer
    {
        public event Action<int, bool> SlotChanged;

        private readonly int _width;
        private readonly int _height;
        private readonly IItemSlot[,] _craftingSlots;
        private readonly ItemSlot _resultSlot;
        private readonly List<ItemSlot> _allSlots = new();

        public string Key { get; }
        public int TotalSlots => _allSlots.Count;

        public CraftingGrid(string key, int width, int height)
        {
            Key = key;
            
            _resultSlot = new ItemSlot(false);
            _width = width;
            _height = height;
            _craftingSlots = new IItemSlot[_width, _height];

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < _height; row++)
                {
                    var slot = new ItemSlot();
                    _craftingSlots[col, row] = slot;
                    _allSlots.Add(slot);
                }
            }
            
            _resultSlot = new ItemSlot(false);
            
            _allSlots.Add(_resultSlot);
        }

        public void SetResultSlot(Item item)
        {
            _resultSlot.ForceSetItem(item);
            SlotChanged?.Invoke(TotalSlots - 1, true);
        }

        public bool TryAddItem(int slotIndex, Item item, out Item swappedItem, bool forceAccept = false)
        {
            var result = _allSlots[slotIndex].TryAddItem(item, out swappedItem, forceAccept);
            SlotChanged?.Invoke(slotIndex, IsResultSlot(slotIndex));
            return result;
        }

        public bool TryTakeItem(int slotIndex, int quantity, out Item takenItem)
        {
            bool result = _allSlots[slotIndex].TryTakeItem(quantity, out takenItem);
            if (result)
            {
                bool isResultSlot = IsResultSlot(slotIndex);
                SlotChanged?.Invoke(slotIndex, isResultSlot);
                if (isResultSlot)
                {
                    RemoveOneOfEach();
                }
            }

            return result;
        }

        public bool TryTakeAllItems(int slotIndex, out Item item)
        {
            bool result = _allSlots[slotIndex].TryTakeAllItems(out item);
            if (result)
            {
                bool isResultSlot = IsResultSlot(slotIndex);
                SlotChanged?.Invoke(slotIndex, isResultSlot);
                if (isResultSlot)
                {
                    RemoveOneOfEach();
                }
            }

            return result;
        }

        public void ForceSetItem(int slotIndex, Item item)
        {
            _allSlots[slotIndex].ForceSetItem(item);
            SlotChanged?.Invoke(slotIndex, IsResultSlot(slotIndex));
        }

        public void RemoveOneOfEach()
        {
            for (int index = 0; index < _allSlots.Count - 1; index++)
            {
                TryTakeItem(index, 1, out Item _);
            }
        }

        public Result<IItemSlot> GetSlot(int index)
        {
            if (index < 0 || index >= _allSlots.Count)
            {
                return default;
            }

            return new Result<IItemSlot>(_allSlots[index], true);
        }

        public IItemSlot[,] GetCraftSlots()
        {
            return _craftingSlots;
        }

        private bool IsResultSlot(int slotIndex)
        {
            return slotIndex == TotalSlots - 1;
        }

    }
}