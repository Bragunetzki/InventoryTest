using System;
using UnityEngine;

namespace Core.Items.Runtime
{
    public class ItemDefinition
    {
        public Guid ID { get; }
        public Sprite Icon { get; }
        public string ItemName { get; }
        public int StackSize { get; }

        public ItemDefinition(Guid id, Sprite icon, string itemName, int stackSize)
        {
            ID = id;
            Icon = icon;
            ItemName = itemName;
            StackSize = stackSize;
        }
    }
}