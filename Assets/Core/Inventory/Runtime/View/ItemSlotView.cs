using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Inventory.Runtime.View
{
    public class ItemSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IItemSlotView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private int _index;
        private event Action<int> _onDragStart;
        private event Action<Vector2> _onDragUpdate;
        private event Action<int> _onDropped;
        private event Action _onDragEnd;

        public void Init(
            int index, 
            Action<int> onDragStart,
            Action<Vector2> onDragUpdate,
            Action<int> onDropped,
            Action onDragEnd)
        {
            _index = index;
            _onDragStart = onDragStart;
            _onDragUpdate = onDragUpdate;
            _onDropped = onDropped;
            _onDragEnd = onDragEnd;
        }

        public void UpdateInterface(Sprite icon, int quantity)
        {
            _icon.sprite = icon;
            _icon.enabled = true;

            _countText.text = quantity.ToString();
            _countText.gameObject.SetActive(quantity > 1);
        }

        public void Clear()
        {
            _icon.enabled = false;
            _countText.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _onDragStart?.Invoke(_index);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _onDragUpdate?.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _onDragEnd?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            _onDropped?.Invoke(_index);
        }

    }
}