using System;
using System.Collections.Generic;
using Core.DragAndDrop.Runtime;
using Core.Inventory.Runtime.Config;
using Core.Tooltips.Runtime;
using Core.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core.Inventory.Runtime.View
{
    public class InventoryView : MonoBehaviour, IInventoryView, IDragSlotContainerView, IItemHoverSource
    {
        public event Action RandomizeInventoryClicked;
        public event Action ClearInventoryClicked;
        public event Action<string, int> SlotDragStarted;
        public event Action<string, int> SplitSlotDragStarted;
        public event Action<Vector2> SlotDragUpdated;
        public event Action<string, int> DroppedOnSlot;
        public event Action SlotDragEnded;
        public event Action<string, int, RectTransform> HoverStarted;
        public event Action HoverEnded;

        [SerializeField] private List<ItemSlotView> _slots;
        [SerializeField] private Button _randomizeButton;
        [SerializeField] private Button _clearButton;

        private int _capacity;
        private string _containerKey;

        public void Init(InputAction splitAction, string containerKey, InventoryConfig config)
        {
            _containerKey = containerKey;
            _capacity = config.Capacity;

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].gameObject.SetActive(i < _capacity);
                _slots[i].Init(i, splitAction);
                _slots[i].SetDragCallbacks(OnSlotDragStart,
                    OnSplitSlotDragStart,
                    OnSlotDragUpdate,
                    OnSlotDrop,
                    OnSlotDragEnd);
                _slots[i].SetHoverCallbacks(OnSlotHover, OnSlotHoverEnd);
            }

            _randomizeButton.onClick.AddListener(OnRandomizeClicked);
            _clearButton.onClick.AddListener(OnClearClicked);
        }

        public Result<IItemSlotView> GetSlot(int index)
        {
            if (index < 0 || index >= _slots.Count)
            {
                Debug.LogError($"Slot with index {index} does not exist.");
                return default;
            }

            return new Result<IItemSlotView>(_slots[index], true);
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

        private void OnRandomizeClicked()
        {
            RandomizeInventoryClicked?.Invoke();
        }

        private void OnClearClicked()
        {
            ClearInventoryClicked?.Invoke();
        }

        private void OnSlotHover(int index, RectTransform rectTransform)
        {
            HoverStarted?.Invoke(_containerKey, index, rectTransform);
        }

        private void OnSlotHoverEnd()
        {
            HoverEnded?.Invoke();
        }

        private void OnDestroy()
        {
            _randomizeButton.onClick.RemoveListener(OnRandomizeClicked);
            _clearButton.onClick.RemoveListener(OnClearClicked);
        }
    }
}