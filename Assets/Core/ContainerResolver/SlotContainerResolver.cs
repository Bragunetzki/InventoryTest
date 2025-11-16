using System.Collections.Generic;
using System.Linq;
using Core.Inventory.Runtime;
using VContainer;

namespace Core.ContainerResolver
{
    public class SlotContainerResolver
    {
        private readonly IReadOnlyDictionary<string, ISlotContainer> _containers;

        [Inject]
        public SlotContainerResolver(IReadOnlyList<ISlotContainerProvider> containers)
        {
            _containers = containers.ToDictionary(
                c => c.GetSlotContainer().Key,
                c => c.GetSlotContainer());
        }

        public bool TryResolve(string key, out ISlotContainer container)
        {
            return _containers.TryGetValue(key, out container);
        }
    }
}