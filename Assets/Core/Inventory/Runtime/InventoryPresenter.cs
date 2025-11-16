using System;
using Core.Inventory.Runtime.Config;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Inventory.Runtime
{
    public class InventoryPresenter : IDisposable
    {
        private readonly Inventory _inventory;
        private readonly IInventoryView _view;
        private readonly InventoryGenerator _generator;

        public InventoryPresenter(
            string containerKey, 
            InputAction splitAction,
            Inventory inventory, 
            IInventoryView view,
            InventoryConfig config,
            InventoryGenerator generator)
        {
            _inventory = inventory;
            _view = view;
            _view.Init(splitAction, containerKey, config);
            _generator = generator;
        }
        
        public void Init()
        {
            _generator.FillSlots(_inventory);
            RefreshAllSlots();
            Subscribe();
        }

        private void RefreshAllSlots()
        {
            for (int i = 0; i < _inventory.Capacity; i++)
            {
                RefreshSlot(i);
            }
        }

        private void RefreshSlot(int slotIndex)
        {
            Result<IItemSlot> slotResult = _inventory.GetSlot(slotIndex);
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
        }

        private void RandomizeInventory()
        {
            _generator.ClearSlots(_inventory);
            _generator.FillSlots(_inventory);
        }

        private void ClearInventory()
        {
            _generator.ClearSlots(_inventory);
        }
        
        private void Subscribe()
        {
            _inventory.SlotChanged += RefreshSlot;
            _view.RandomizeInventoryClicked += RandomizeInventory;
            _view.ClearInventoryClicked += ClearInventory;
        }

        private void Unsubscribe()
        {
            _inventory.SlotChanged -= RefreshSlot;
            _view.RandomizeInventoryClicked -= RandomizeInventory;
            _view.ClearInventoryClicked -= ClearInventory;
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}