using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Core.Items.Runtime
{
    public class Item
    {
        private int _quantity;
        
        public ItemDefinition Definition { get; }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = Mathf.Clamp(value, 0, Definition.StackSize);
        }
        
        public Item(ItemDefinition definition, int quantity = 1)
        {
            Definition = definition;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity, out int addedQuantity)
        {
            addedQuantity = Mathf.Clamp(quantity, 0, Definition.StackSize - _quantity);
            Quantity += addedQuantity;
        }
        public void RemoveQuantity(int quantity, out int removedQuantity)
        {
            removedQuantity = Mathf.Clamp(quantity, 0, _quantity);
            Quantity -= removedQuantity;
        }

        public Item Clone()
        {
            return new Item(Definition, Quantity);
        }
    }
}