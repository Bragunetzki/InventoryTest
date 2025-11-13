using System;
using Core.Inventory.Runtime.Config;
using Core.Inventory.Runtime.View;
using Core.Items.Runtime;
using Core.Utils;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Inventory.Runtime.Presenter
{
    public class InventoryPresenter : IDisposable
    {
        private readonly Inventory _inventory;
        private readonly InventoryDragState _inventoryDragState;
        private readonly IInventoryView _view;
        private readonly IDraggedItemView _draggedItemView;

        public InventoryPresenter(
            Inventory inventory,
            IInventoryView view,
            IDraggedItemView draggedItemView)
        {
            _inventory = inventory;
            _view = view;
            _draggedItemView = draggedItemView;
            _inventoryDragState = new InventoryDragState();
        }
        
        public void Init()
        {
            RefreshAllSlots();
            _draggedItemView.Init();
            Subscribe();
        }

        private void OnSlotDragStarted(int slotIndex)
        {
            if (!_inventory.TryTakeAllItems(slotIndex, out Item takenItem))
            {
                return;
            }

            _inventoryDragState.PickUp(takenItem, slotIndex);
        }

        private void OnDroppedItem(int slotIndex)
        {
            if (!_inventoryDragState.IsHoldingItem)
            {
                return;
            }
            
            Item heldItem = _inventoryDragState.HeldItem.Object;
            bool noSwap = _inventory.TryAddItem(slotIndex, heldItem, out Item swappedItem);

            if (!noSwap)
            {
                int sourceSlotIndex = _inventoryDragState.SourceSlotIndex;
                bool placedBackFully = _inventory.TryAddItem(sourceSlotIndex, swappedItem, out Item _);
                if (!placedBackFully)
                {
                    Debug.LogError($"Source slot at index {sourceSlotIndex} somehow became occupied while dragging.");
                }
            }
            _inventoryDragState.Clear();
        }

        private void OnSlotDragEnded()
        {
            _inventoryDragState.Clear();
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
        
        private void RefreshDraggedItem()
        {
            if (_inventoryDragState.IsHoldingItem)
            {
                Item item = _inventoryDragState.HeldItem.Object;
                _draggedItemView.UpdateInterface(item.Definition.Icon, item.Quantity);
            }
            else
            {
                _draggedItemView.Hide();
            }
        }

        private void OnSlotDragUpdated(Vector2 position)
        {
            _draggedItemView.UpdatePosition(position);
        }

        private void Subscribe()
        {
            _view.SlotDragStarted += OnSlotDragStarted;
            _view.SlotDragUpdated += OnSlotDragUpdated;
            _view.DroppedOnSlot += OnDroppedItem;
            _view.SlotDragEnded += OnSlotDragEnded;

            _inventory.SlotChanged += RefreshSlot;
            _inventoryDragState.StateChanged += RefreshDraggedItem;
        }

        private void Unsubscribe()
        {
            _view.SlotDragStarted -= OnSlotDragStarted;
            _view.SlotDragUpdated -= OnSlotDragUpdated;
            _view.DroppedOnSlot -= OnDroppedItem;
            _view.SlotDragEnded -= OnSlotDragEnded;
            
            _inventory.SlotChanged -= RefreshSlot;
            _inventoryDragState.StateChanged -= RefreshDraggedItem;
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}