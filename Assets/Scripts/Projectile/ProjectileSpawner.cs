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

    public void LoadAndSpawn(AssetReference assetReference, Vector3 objectPosition)
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
            SpawnParticleFromLoadedReference(assetReference, objectPosition);
        };
    }

    private void SpawnParticleFromLoadedReference(AssetReference assetReference, Vector3 position)
    {
        assetReference.InstantiateAsync().Completed += (asyncOperationHandle) =>
        {
            if (_spawnedParticleSystems.ContainsKey(assetReference) == false)
            {
                _spawnedParticleSystems[assetReference] = new List<GameObject>();
            }

            GameObject spawnedObject = asyncOperationHandle.Result;
            spawnedObject.transform.position = position;
            _spawnedParticleSystems[assetReference].Add(spawnedObject);
            var notify = spawnedObject.GetComponent<BaseProjectile>();
            notify.Destroyed += Remove;
            notify.AssetReference = assetReference;
            
            spawnedObject.GetComponent<Rigidbody>().AddForce(spawnedObject.transform.rotation * Vector3.back * 10f, ForceMode.Impulse);
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
