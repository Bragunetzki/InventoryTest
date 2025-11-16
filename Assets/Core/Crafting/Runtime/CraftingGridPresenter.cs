using System;
using Core.Crafting.Runtime.View;
using Core.Inventory.Runtime;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Crafting.Runtime
{
    public class CraftingGridPresenter : IDisposable
    {
        private readonly CraftingGrid _grid;
        private readonly Crafter _crafter;
        private readonly ICraftingGridView _view;
        
        private Action _queueCraftingCheckCallback;

        public CraftingGridPresenter(
            string containerKey,
            InputAction splitAction,
            Crafter crafter, 
            CraftingGrid grid, 
            ICraftingGridView view)
        {
            _grid = grid;
            _view = view;
            _crafter = crafter;
            _view.Init(splitAction, containerKey, _grid.TotalSlots);
        }
        
        public void Init(Action queueCraftingCheckCallback)
        {
            Subscribe();
            _queueCraftingCheckCallback = queueCraftingCheckCallback;
            RefreshAllSlots();
        }

        private void RefreshAllSlots()
        {
            for (int i = 0; i < _grid.TotalSlots; i++)
            {
                RefreshSlot(i, i < _grid.TotalSlots - 1);
            }
        }

        private void RefreshSlot(int slotIndex, bool isResultSlot)
        {
            Result<IItemSlot> slotResult = _grid.GetSlot(slotIndex);
            Result<IItemSlotView> slotViewResult = _view.GetSlot(slotIndex);
            if (!slotResult.Exists || !slotViewResult.Exists)
            {
                return;
            }
            
            IItemSlot slot = slotResult.Object;
            IItemSlotView slotView = slotViewResult.Object;
        
            if (slot.IsOccupied)
            {
                Item item = slot.OccupyingItem.Object;
                int quantity = item.Quantity;
                Sprite icon = item.Definition.Icon;
                slotView.UpdateInterface(icon, quantity);
            }
            else
            {
                slotView.Clear();
            }

            if (!isResultSlot)
            {
                _queueCraftingCheckCallback?.Invoke(); 
            }
        }

        public void CheckCrafting()
        {
            Result<Item> result = _crafter.CheckCrafting();
            if (result.Exists)
            {
                _grid.SetResultSlot(result.Object);
            }
            else
            {
                _grid.SetResultSlot(default);
            }
        }

        private void Subscribe()
        {
            _grid.SlotChanged += RefreshSlot;
        }

        private void Unsubscribe()
        {
            _grid.SlotChanged -= RefreshSlot;
        }

        public void Dispose()
        {
            Unsubscribe();
        }

    }
}