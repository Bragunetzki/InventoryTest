using System.Collections.Generic;
using Core.ContainerResolver;
using Core.Crafting.Runtime;
using Core.DragAndDrop.Runtime.View;
using Core.Inventory.Runtime;
using VContainer;
using VContainer.Unity;

namespace Core.DragAndDrop.Runtime
{
    public class DragAndDropSystem : IStartable
    {
        private readonly SlotContainerResolver _containerResolver;
        private readonly IReadOnlyList<IDragSlotContainerView> _containerViews;
        private readonly IDraggedItemView _draggedItemView;
        private readonly CraftingSystem _craftingSystem;
        private readonly InventorySystem _inventorySystem;

        private DragAndDropPresenter _presenter;

        public bool IsDragging => _presenter.IsDragging;

        [Inject]
        public DragAndDropSystem(
            SlotContainerResolver containerResolver,
            IReadOnlyList<IDragSlotContainerView> containerViews,
            InventorySystem inventorySystem,
            CraftingSystem craftingSystem,
            IDraggedItemView draggedItemView)
        {
            _containerViews = containerViews;
            _inventorySystem = inventorySystem;
            _craftingSystem = craftingSystem;
            _draggedItemView = draggedItemView;
            _containerResolver = containerResolver;
        }

        public void Start()
        {
            _presenter = new DragAndDropPresenter(_containerResolver, _containerViews, _draggedItemView);   
            _presenter.Init();
        }
    }
}