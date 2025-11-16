using Core.Items.Runtime.Config;
using UnityEngine;

namespace Core.Items.Runtime
{
    [CreateAssetMenu(fileName = "New Item Config", menuName = "Items/ItemConfig")]
    public class ItemConfig : ScriptableObject, IItemConfig
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _itemKey;
        [SerializeField, Range(1, 256)] private int _stackSize;
        
        public Sprite Icon => _icon;
        public string ItemKey => _itemKey;
        public int StackSize => _stackSize;
    }
}