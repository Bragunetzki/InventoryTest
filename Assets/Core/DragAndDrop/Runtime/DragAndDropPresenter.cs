using System;
using System.Collections.Generic;
using System.Linq;
using Core.ContainerResolver;
using Core.DragAndDrop.Runtime.View;
using Core.Inventory.Runtime;
using Core.Items.Runtime;
using Core.Utils;
using UnityEngine;

namespace Core.DragAndDrop.Runtime
{
    public class DragAndDropPresenter : IDisposable
    {
        private readonly SlotContainerResolver _containerResolver;
        private readonly IReadOnlyList<IDragSlotContainerView> _slotContainerViews;
        private readonly ItemDragState _itemDragState;
        private readonly IDraggedItemView _draggedItemView;

        public bool IsDragging => _itemDragState.IsHoldingItem;

        public DragAndDropPresenter(
            SlotContainerResolver containerResolver,
            IReadOnlyList<IDragSlotContainerView> slotContainerViews,
            IDraggedItemView draggedItemView)
        {
            _containerResolver = containerResolver;
            _slotContainerViews = slotContainerViews;
            _draggedItemView = draggedItemView;
            _itemDragState = new ItemDragState();
        }

        public void Init()
        {
            _draggedItemView.Init();
            Subscribe();
        }

        private void OnSlotDragStarted(string containerKey, int slotIndex)
        {
            if (!_containerResolver.TryResolve(containerKey, out ISlotContainer container))
            {
                return;
            }

            Result<IItemSlot> sourceSlot = container.GetSlot(slotIndex);
            _itemDragState.TakenFromNonAcceptingSlot = sourceSlot.Exists && !sourceSlot.Object.CanAcceptItems;

            if (!container.TryTakeAllItems(slotIndex, out Item takenItem))
            {
                return;
            }

            _itemDragState.PickUp(takenItem, containerKey, slotIndex);
        }

        private void OnSplitSlotDragStarted(string containerKey, int slotIndex)
        {
            if (!_containerResolver.TryResolve(containerKey, out ISlotContainer container))
            {
                return;
            }

            Result<IItemSlot> sourceSlot = container.GetSlot(slotIndex);
            _itemDragState.TakenFromNonAcceptingSlot = sourceSlot.Exists && !sourceSlot.Object.CanAcceptItems;
            int quantity = Mathf.Max(sourceSlot.Object.OccupyingItem.Object.Quantity / 2, 1);

            if (!container.TryTakeItem(slotIndex, quantity, out Item takenItem))
            {
                return;
            }

            _itemDragState.PickUp(takenItem, containerKey, slotIndex);
        }

        private void OnDroppedItem(string containerKey, int slotIndex)
        {
            if (!_itemDragState.IsHoldingItem)
            {
                return;
            }

            if (!_containerResolver.TryResolve(containerKey, out ISlotContainer container))
            {
                return;
            }

            if (!_containerResolver.TryResolve(_itemDragState.SourceContainerKey, out ISlotContainer sourceContainer))
            {
                return;
            }
 
            if (_itemDragState.TakenFromNonAcceptingSlot)
            {
                Result<IItemSlot> targetSlot = container.GetSlot(slotIndex);
                if (targetSlot.Exists)
                {
                    if (targetSlot.Object.IsOccupied)
                    {
                        OnDroppedItemNonSwappable(container, sourceContainer, slotIndex);
                        return;
                    }

                    if (!targetSlot.Object.CanAcceptItems)
                    {
                        OnDroppedItemSwappable(container, sourceContainer, slotIndex, true);
                        return;
                    }
                }
            }

            OnDroppedItemSwappable(container, sourceContainer, slotIndex, false);
        }

        private void OnDroppedItemSwappable(
            ISlotContainer container, 
            ISlotContainer sourceContainer, 
            int slotIndex, 
            bool forcePutBack)
        {
            Item heldItem = _itemDragState.HeldItem.Object;
            IItemSlot sourceSlot = sourceContainer.GetSlot(_itemDragState.SourceSlotIndex).Object;
            IItemSlot targetSlot = container.GetSlot(slotIndex).Object;
            bool bothOccupied = targetSlot.IsOccupied && sourceSlot.IsOccupied;
            
            if (bothOccupied && 
                !targetSlot.OccupyingItem.Object.Definition.Key.Equals(sourceSlot.OccupyingItem.Object.Definition.Key))
            {
                PutItemBackToDragSource(heldItem, sourceContainer, forcePutBack);
                return;
            }
            
            bool noSwap = container.TryAddItem(slotIndex, heldItem, out Item swappedItem);

            if (!noSwap)
            {
                PutItemBackToDragSource(swappedItem, sourceContainer, forcePutBack);
            }
            _itemDragState.Clear();
        }

        private void OnDroppedItemNonSwappable(ISlotContainer container, ISlotContainer sourceContainer, int slotIndex)
        {
            IItemSlot targetSlot = container.GetSlot(slotIndex).Object;
            string itemKey = targetSlot.OccupyingItem.Object.Definition.Key;

            if (itemKey.Equals(_itemDragState.HeldItem.Object.Definition.Key))
            {
                OnDroppedItemSwappable(container, sourceContainer, slotIndex, true);
                return;
            }

            PutItemBackToDragSource(_itemDragState.HeldItem.Object, sourceContainer, true);
        }

        private void PutItemBackToDragSource(Item item, ISlotContainer sourceContainer, bool forcePutBack = false)
        {
            int sourceSlotIndex = _itemDragState.SourceSlotIndex;

            bool placedBackFully = sourceContainer.TryAddItem(sourceSlotIndex, item, out Item _, forcePutBack);
            if (!placedBackFully)
            {
                Debug.LogError($"Source slot at index {sourceSlotIndex} somehow became occupied while dragging.");
            }

            _itemDragState.Clear();
        }

        private void OnSlotDragEnded()
        {
            _itemDragState.Clear();
        }

        private void RefreshDraggedItem()
        {
            if (_itemDragState.IsHoldingItem)
            {
                Item item = _itemDragState.HeldItem.Object;
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
            foreach (IDragSlotContainerView slotsView in _slotContainerViews)
            {
                slotsView.SlotDragStarted += OnSlotDragStarted;
                slotsView.SplitSlotDragStarted += OnSplitSlotDragStarted;
                slotsView.SlotDragUpdated += OnSlotDragUpdated;
                slotsView.DroppedOnSlot += OnDroppedItem;
                slotsView.SlotDragEnded += OnSlotDragEnded;
            }

            _itemDragState.StateChanged += RefreshDraggedItem;
        }

        private void Unsubscribe()
        {
            foreach (IDragSlotContainerView slotsView in _slotContainerViews)
            {
                slotsView.SlotDragStarted -= OnSlotDragStarted;
                slotsView.SplitSlotDragStarted -= OnSplitSlotDragStarted;
                slotsView.SlotDragUpdated -= OnSlotDragUpdated;
                slotsView.DroppedOnSlot -= OnDroppedItem;
                slotsView.SlotDragEnded -= OnSlotDragEnded;
            }

            _itemDragState.StateChanged -= RefreshDraggedItem;
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}