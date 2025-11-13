using System.Collections.Generic;
using Core.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.AssetLoader
{
    public class SimpleAssetLoader
    {
        private readonly Dictionary<string, AsyncOperationHandle> _cachedHandles = new();
        
        public Result<T> LoadAssetSync<T>(string address) where T : Object
        {
            if (_cachedHandles.TryGetValue(address, out AsyncOperationHandle cachedHandle))
            {
                return new Result<T>(cachedHandle.Result as T, true);
            }
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.WaitForCompletion();

            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                Debug.LogError($"Failed to load addressable asset: {address}");
                return default;
            }
            
            _cachedHandles[address] = handle;
            return new Result<T>(handle.Result, true);
        }
    }
}