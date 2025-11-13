using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventory.Runtime.View
{
    public class DraggedItemView : MonoBehaviour, IDraggedItemView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private bool _showQuantityWhenOne;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Camera _camera;

        public void Init()
        {
            _rectTransform = transform as RectTransform;
            _canvas = GetComponentInParent<Canvas>();
            _camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            Hide();
        }

        public void UpdateInterface(Sprite icon, int quantity)
        {
            _icon.sprite = icon;

            var showQuantity = quantity > 1 || _showQuantityWhenOne && quantity == 1;
            _quantityText.gameObject.SetActive(showQuantity);
            if (showQuantity)
            {
                _quantityText.text = quantity.ToString();
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _quantityText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void UpdatePosition(Vector2 screenPosition)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPosition,
            _camera,
            out Vector2 localPoint);

            _rectTransform.localPosition = localPoint;
        }
    }
}