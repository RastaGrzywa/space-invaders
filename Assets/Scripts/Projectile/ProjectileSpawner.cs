using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ProjectileSpawner : MonoBehaviour
{
    
    private readonly Dictionary<AssetReference, List<GameObject>> _spawnedParticleSystems = 
        new Dictionary<AssetReference, List<GameObject>>();
    
    private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles = 
        new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
    
    
    public void LoadAndSpawn(AssetReference assetReference, Vector3 objectPosition, Quaternion rotation)
    {
        if (!assetReference.RuntimeKeyIsValid())
        {
            Debug.LogError("No asset for: " + assetReference.RuntimeKey);
            return;
        }
        
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
        _asyncOperationHandles[assetReference] = op;
        op.Completed += (operation) =>
        {
            SpawnParticleFromLoadedReference(assetReference, objectPosition, rotation);
        };
    }

    private void SpawnParticleFromLoadedReference(AssetReference assetReference, Vector3 position, Quaternion rotation)
    {
        assetReference.InstantiateAsync(position, rotation).Completed += (asyncOperationHandle) =>
        {
            if (_spawnedParticleSystems.ContainsKey(assetReference) == false)
            {
                _spawnedParticleSystems[assetReference] = new List<GameObject>();
            }
            
            _spawnedParticleSystems[assetReference].Add(asyncOperationHandle.Result);
            var notify = asyncOperationHandle.Result.GetComponent<BaseProjectile>();
            notify.Destroyed += Remove;
            notify.AssetReference = assetReference;
            
            asyncOperationHandle.Result.GetComponent<Rigidbody>().AddForce(rotation * Vector3.back * 10f, ForceMode.Impulse);
        };
    }
    
    private void Remove(AssetReference assetReference, BaseProjectile obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);
        
        _spawnedParticleSystems[assetReference].Remove(obj.gameObject);
        if (_spawnedParticleSystems[assetReference].Count == 0)
        {
            _asyncOperationHandles.Remove(assetReference);
        }
    }
}
