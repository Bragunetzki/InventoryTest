using UnityEngine;

namespace Core.Items.Runtime.Config
{
    public interface IItemConfig
    {
        Sprite Icon { get; }
        string ItemKey { get; }
        int StackSize { get; }
    }
}