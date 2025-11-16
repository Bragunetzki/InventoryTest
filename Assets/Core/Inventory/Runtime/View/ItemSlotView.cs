using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Core.Inventory.Runtime.View
{
    public class ItemSlotView :
        MonoBehaviour,
        IItemSlotView,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private int _index;
        private InputAction _splitAction;
        private RectTransform _rectTransform;

        private event Action<int> _onDragStart;
        private event Action<int> _onSplitDragStart;
        private event Action<Vector2> _onDragUpdate;
        private event Action<int> _onDropped;
        private event Action _onDragEnd;
        private event Action<int, RectTransform> _onHoverStart;
        private event Action _onHoverEnd;

        public void Init(int index, InputAction splitAction)
        {
            _index = index;
            _splitAction = splitAction;
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetDragCallbacks(
            Action<int> onDragStart,
            Action<int> onSplitDragStart,
            Action<Vector2> onDragUpdate,
            Action<int> onDropped,
            Action onDragEnd)
        {
            _onDragStart = onDragStart;
            _onSplitDragStart = onSplitDragStart;
            _onDragUpdate = onDragUpdate;
            _onDropped = onDropped;
            _onDragEnd = onDragEnd;
        }

        public void SetHoverCallbacks(Action<int, RectTransform> onHoverStart, Action onHoverEnd)
        {
            _onHoverStart = onHoverStart;
            _onHoverEnd = onHoverEnd;
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
            bool isSplit = _splitAction.IsPressed();
            if (isSplit)
            {
                _onSplitDragStart?.Invoke(_index);
            }
            else
            {
                _onDragStart?.Invoke(_index);
            }
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            _onHoverStart?.Invoke(_index, _rectTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _onHoverEnd?.Invoke();
        }
    }
}