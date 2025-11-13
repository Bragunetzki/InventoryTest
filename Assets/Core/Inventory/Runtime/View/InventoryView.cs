using System;
using System.Collections.Generic;
using Core.Inventory.Runtime.Config;
using Core.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Inventory.Runtime.View
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        public event Action<int> SlotDragStarted;
        public event Action<Vector2> SlotDragUpdated;
        public event Action<int> DroppedOnSlot;
        public event Action SlotDragEnded;

        [SerializeField] private List<ItemSlotView> _slots;

        private int _capacity;

        public void Init(InventoryConfig config)
        {
            _capacity = config.Capacity;
            
            // todo: instantiate prefabs
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].gameObject.SetActive(i < _capacity);
                _slots[i].Init(i, OnSlotDragStart, OnSlotDragUpdate, OnSlotDrop, OnSlotDragEnd);
            }
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
            SlotDragStarted?.Invoke(index);
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
            DroppedOnSlot?.Invoke(index);
        }
    }
}