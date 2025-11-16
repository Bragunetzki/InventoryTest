using Core.Inventory.Runtime;

namespace Core.ContainerResolver
{
    public interface ISlotContainerProvider
    {
        ISlotContainer GetSlotContainer();
    }
}