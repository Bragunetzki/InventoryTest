using UnityEngine;

namespace Core.Inventory.Runtime.View
{
    public interface IItemSlotView
    {
        void UpdateInterface(Sprite icon, int quantity);
        void Clear();
    }
}