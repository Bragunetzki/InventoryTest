using Core.Utils;

namespace Core.Items.Runtime.Config
{
    public interface IItemConfigLoader
    {
        Result<IItemConfig> LoadConfig(string itemKey);
    }
}