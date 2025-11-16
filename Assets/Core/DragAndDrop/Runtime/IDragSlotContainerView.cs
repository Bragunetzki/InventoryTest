using System;
using UnityEngine;

namespace Core.DragAndDrop.Runtime
{
    public interface IDragSlotContainerView
    {
        event Action<string, int> SlotDragStarted;
        event Action<string, int> SplitSlotDragStarted; 
        event Action<string, int> DroppedOnSlot;
        event Action<Vector2> SlotDragUpdated;
        event Action SlotDragEnded;
    }
}