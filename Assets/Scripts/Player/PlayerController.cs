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
    [Inject] private ProjectileSpawner _projectileSpawner;
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
    
    private void Fire()
    {
        _projectileSpawner.LoadAndSpawn(_gameSettings.playerShot, transform.position);
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
