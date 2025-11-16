using Core.AssetLoader;
using Core.Utils;
using VContainer;

namespace Core.Items.Runtime.Config
{
    public class ScriptableObjectItemConfigLoader : IItemConfigLoader
    {
        private const string ADDRESS_FORMAT = "ItemConfig/{0}";

        private readonly SimpleAssetLoader _assetLoader;

        [Inject]
        public ScriptableObjectItemConfigLoader(SimpleAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public Result<IItemConfig> LoadConfig(string itemKey)
        {
            var address = string.Format(ADDRESS_FORMAT, itemKey);
            Result<ItemConfig> configResult = _assetLoader.LoadAssetSync<ItemConfig>(address);
            var result = new Result<IItemConfig>(configResult.Object, configResult.Exists);
            return result;
        }
    }
}