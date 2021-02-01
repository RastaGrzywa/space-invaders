using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseProjectile : MonoBehaviour
{
    public event Action<AssetReference, BaseProjectile> Destroyed;
    public AssetReference AssetReference { get; set; }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BulletCatcher")
        {
            Destroy(gameObject);
        }
    }
    

    public void OnDestroy()
    {
        Destroyed?.Invoke(AssetReference, this);
    }
}
