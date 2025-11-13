using UnityEngine;

namespace Core.Inventory.Runtime.View
{
    public interface IDraggedItemView
    {
        void Init();
        void UpdateInterface(Sprite icon, int quantity);
        void Hide();
        void UpdatePosition(Vector2 screenPosition);
    }
}