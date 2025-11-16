using TMPro;
using UnityEngine;

namespace Core.Tooltips.Runtime.View
{
    public class TooltipView : MonoBehaviour, ITooltipView
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _quantityText;

        private RectTransform _rectTransform;
        
        public void Init()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void Show(string itemName, int quantity)
        {
            _nameText.text = itemName;
            _quantityText.text = quantity.ToString();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetPosition(Vector2 screenPos)
        {
            _rectTransform.position = screenPos;
        }
    }
}