using System;
using UnityEngine;

namespace Core.Tooltips.Runtime
{
    public interface IItemHoverSource
    {
        event Action<string, int, RectTransform> HoverStarted;
        event Action HoverEnded;
    }
}