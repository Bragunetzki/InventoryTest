using System.Collections.Generic;
using UnityEngine;

namespace Core.Inventory.Runtime.Config
{
    [CreateAssetMenu(fileName = "New Inventory Config", menuName = "Inventory/InventoryConfig")]
    public class InventoryConfig : ScriptableObject
    {
        [SerializeField, Range(1, 256)] private int _capacity;
        [SerializeField] private List<string> _itemsKeysToGenerate;
        
        public int Capacity => _capacity;
        public List<string> ItemsKeysToGenerate => _itemsKeysToGenerate;
    }
}