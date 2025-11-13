using UnityEngine;

namespace Core.Items.Runtime
{
    [CreateAssetMenu(fileName = "New Item Config", menuName = "Items/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _itemName;
        [SerializeField, Range(1, 256)] private int _stackSize;
        
        public Sprite Icon => _icon;
        public string ItemName => _itemName;
        public int StackSize => _stackSize;
    }
}