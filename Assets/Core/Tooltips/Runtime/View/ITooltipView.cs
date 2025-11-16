using UnityEngine;

namespace Core.Tooltips.Runtime.View
{
    public interface ITooltipView
    {
        void Init();
        void Show(string itemName, int quantity);
        void Hide();
        void SetPosition(Vector2 screenPos);
    }
}