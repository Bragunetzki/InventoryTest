using System;
using System.Collections.Generic;
using Core.DragAndDrop.Runtime;
using Core.Inventory.Runtime.View;
using Core.Tooltips;
using Core.Tooltips.Runtime;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Crafting.Runtime.View
{
    public class CraftingGridView : MonoBehaviour, ICraftingGridView, IDragSlotContainerView, IItemHoverSource
    {
        public event Action<string, int> SlotDragStarted;
        public event Action<string, int> SplitSlotDragStarted;
        public event Action<string, int> DroppedOnSlot;
        public event Action<Vector2> SlotDragUpdated;
        public event Action SlotDragEnded;
        public event Action<string, int, RectTransform> HoverStarted;
        public event Action HoverEnded;

        [SerializeField] private List<ItemSlotView> _craftingSlots;
        [SerializeField] private ItemSlotView _resultSlot;
        
        private List<ItemSlotView> _allSlots;
        
        private string _containerKey;

        public void Init(InputAction splitAction, string containerKey, int totalSlots)
        {
            _allSlots = new List<ItemSlotView>(_craftingSlots) { _resultSlot };
            _containerKey = containerKey;
            
            for (int i = 0; i < _allSlots.Count; i++)
            {
                _allSlots[i].gameObject.SetActive(i < totalSlots);
                _allSlots[i].Init(i, splitAction);
                _allSlots[i].SetDragCallbacks(OnSlotDragStart,
                    OnSplitSlotDragStart,
                    OnSlotDragUpdate,
                    OnSlotDrop,
                    OnSlotDragEnd);
                _allSlots[i].SetHoverCallbacks(OnSlotHover, OnSlotHoverEnd);
            }
        }
        
        public Result<IItemSlotView> GetSlot(int index)
        {
            if (index < 0 || index >= _allSlots.Count)
            {
                Debug.LogError($"Slot with index {index} does not exist.");
                return default;
            }

            return new Result<IItemSlotView>(_allSlots[index], true);
        }
        
        private void OnSlotDragStart(int index)
        {
            SlotDragStarted?.Invoke(_containerKey, index);
            OnSlotHoverEnd();
        }
        
        private void OnSplitSlotDragStart(int index)
        {
            SplitSlotDragStarted?.Invoke(_containerKey, index);
        }

        private void OnSlotDragUpdate(Vector2 position)
        {
            SlotDragUpdated?.Invoke(position);
        }

        private void OnSlotDragEnd()
        {
            SlotDragEnded?.Invoke();
        }

        private void OnSlotDrop(int index)
        {
            DroppedOnSlot?.Invoke(_containerKey, index);
        }
        
        private void OnSlotHover(int index, RectTransform rectTransform)
        {
            HoverStarted?.Invoke(_containerKey, index, rectTransform);
        }

        private void OnSlotHoverEnd()
        {
            HoverEnded?.Invoke();
        }
    }
}