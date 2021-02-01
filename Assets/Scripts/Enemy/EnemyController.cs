
using System;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour, IPooledObject
{
    [Inject] private GameManager _gameManager;
    [Inject] private GameSettings _gameSettings;

    private EnemyData _enemyData;
    private WaveController _waveController;
    private float _shootTimer;
    private string _poolTag;

    private bool _isInvulnerable;
    
    public string PoolTag
    {
        get => _poolTag;
        private set {}
    }

    public void Fire()
    {
        GameObject proj = Instantiate(_gameSettings.enemyShootPrefab, transform.position, _gameSettings.enemyShootPrefab.transform.rotation);
        proj.GetComponent<Rigidbody>().AddForce(proj.transform.rotation * Vector3.back * 10f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerShot")
        {
            Destroy(other.gameObject);
            if (!_isInvulnerable)
            {
                EnemyHit();   
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RightEnemyEdge" || other.gameObject.tag == "LeftEnemyEdge" )
        {
            _waveController.ChangeMovingDirection();
        }
    }

    public void SetupEnemyData(EnemyData enemyData, WaveController waveController, string poolTag)
    {
        _enemyData = enemyData;
        _waveController = waveController;
        _poolTag = poolTag;
    }
    
    private void EnemyHit()
    {
        _gameManager.OnEnemyHit(_enemyData.pointsOnKill, this);
    }

    public void OnObjectSpawn()
    {
        _isInvulnerable = true;
    }

    public void EnableEnemyDeath()
    {
        _isInvulnerable = false;
    }
}
