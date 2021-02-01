using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class PlayerController : MonoBehaviour
{
    
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameManager _gameManager;
    
    private float _minX;
    private float _maxX;
    private float _projectileTimer;
    private bool _movementAvailable;
    private float _invulnerabilityTimer;
    private bool _isInvulnerable;
    
    private void Start()
    {
        _minX = ViewUtils.MinWorldXPosition(Camera.main);
        _maxX = ViewUtils.MaxWorldXPosition(Camera.main);
    }

    private void Update()
    {
        if (!_movementAvailable)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayer(Vector3.left);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (_projectileTimer > _gameSettings.playerData.shootRate)
            {
                Fire();
                _projectileTimer = 0;
            }
        }

        if (_isInvulnerable)
        {
            if (_invulnerabilityTimer > _gameSettings.playerData.invulnerableTime)
            {
                _isInvulnerable = false;
                _invulnerabilityTimer = 0;
            }
            _invulnerabilityTimer += Time.deltaTime;
        }
        
        _projectileTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "EnemyShot")
        {
            Destroy(other.gameObject);
            if (!_isInvulnerable)
            {
                PlayerHit();
            }
        }
    }

    private void PlayerHit()
    {
        _gameManager.OnPlayerHit();
        _invulnerabilityTimer = 0;
        _isInvulnerable = true;
    }
    

    private void MovePlayer(Vector3 direction)
    {
        Vector3 newPosition =
            transform.position + direction * (_gameSettings.playerData.movementSpeed * Time.deltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
        transform.position = newPosition;
    }

    
    private readonly Dictionary<AssetReference, List<GameObject>> _spawnedParticleSystems = 
        new Dictionary<AssetReference, List<GameObject>>();
    
    private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles = 
        new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
    
    private void Fire()
    {
        // GameObject proj = Instantiate(_gameSettings.playerShootPrefab, transform.position, _gameSettings.playerShootPrefab.transform.rotation);
        // proj.GetComponent<Rigidbody>().AddForce(proj.transform.rotation * Vector3.back * 10f, ForceMode.Impulse);


        if (!_gameSettings.playerShot.RuntimeKeyIsValid())
        {
            Debug.LogError("No asset for: " + _gameSettings.playerShot.RuntimeKey);
            return;
        }
        
        
        LoadAndSpawn(_gameSettings.playerShot, transform.position, _gameSettings.playerShootPrefab.transform.rotation);
    }
    
    private void LoadAndSpawn(AssetReference assetReference, Vector3 objectPosition, Quaternion rotation)
    {
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
    
    public void ResetPosition()
    {
        transform.position = new Vector3(0f, 0f, -13f);
    }

    public void MovePlayerToGamePosition()
    {
        LeanTween.moveZ(gameObject, -9, 0.5f).setOnComplete(() =>
        {
            _movementAvailable = true;
        });
    }

    public void DisablePlayerMovement()
    {
        _movementAvailable = false;
    }
}
