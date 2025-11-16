using System;
using System.Collections.Generic;
using Core.ContainerResolver;
using Core.DragAndDrop.Runtime;
using Core.Inventory.Runtime;
using Core.Items.Runtime;
using Core.Tooltips.Runtime.View;
using Core.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Tooltips.Runtime
{
    public class TooltipSystem : IInitializable, IDisposable
    {
        private readonly List<IItemHoverSource> _hoverSources;
        private readonly ITooltipView _tooltipView;
        private readonly DragAndDropSystem _dragAndDropSystem;
        private readonly SlotContainerResolver _containerResolver;

        [Inject]
        public TooltipSystem(
            IReadOnlyList<IItemHoverSource> hoverSources,
            ITooltipView tooltipView,
            DragAndDropSystem dragAndDropSystem,
            SlotContainerResolver containerResolver)
        {
            _hoverSources = new List<IItemHoverSource>(hoverSources);
            _tooltipView = tooltipView;
            _dragAndDropSystem = dragAndDropSystem;
            _containerResolver = containerResolver;
        }

        public void Initialize()
        {
            _tooltipView.Init();
            
            foreach (IItemHoverSource source in _hoverSources)
            {
                source.HoverStarted += OnHoverStart;
                source.HoverEnded += OnHoverEnd;
            }
        }

        private void OnHoverStart(string containerKey, int slotIndex, RectTransform slotRect)
        {
            if (_dragAndDropSystem.IsDragging)
            {
                return;
            }

            if (!_containerResolver.TryResolve(containerKey, out ISlotContainer container))
            {
                return;
            }
            
            Result<IItemSlot> slot = container.GetSlot(slotIndex);
            if (!slot.Exists || !slot.Object.IsOccupied)
            {
                return;
            }

            Item item = slot.Object.OccupyingItem.Object;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, slotRect.position);
            
            _tooltipView.SetPosition(screenPos);
            _tooltipView.Show(item.Definition.DisplayName, item.Quantity);
        }

        private void OnHoverEnd()
        {
            _tooltipView.Hide();
        }
        
        public void Dispose()
        {
            foreach (IItemHoverSource source in _hoverSources)
            {
                source.HoverStarted -= OnHoverStart;
                source.HoverEnded -= OnHoverEnd;
            }
        }

    }

}