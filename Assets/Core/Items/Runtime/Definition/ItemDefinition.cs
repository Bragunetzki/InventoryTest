using System;
using UnityEngine;

namespace Core.Items.Runtime.Definition
{
    public class ItemDefinition
    {
        public Sprite Icon { get; }
        public string Key { get; }
        public string DisplayName { get; }
        public int StackSize { get; }

        public ItemDefinition(Sprite icon, string key, int stackSize)
        {
            Icon = icon;
            Key = key;
            DisplayName = Key;
            StackSize = stackSize;
        }
    }
}