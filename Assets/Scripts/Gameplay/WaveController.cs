using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameManager _gameManager;

    private float _enemyShootTimer;

    private bool _isMovingRight;
    private bool _isMovingLeft;
    private bool _isChangingDirection;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (_isMovingRight || _isMovingLeft)
        {
            if (_enemyShootTimer > _gameSettings.enemiesShootingRate)
            {
                Fire();
                _enemyShootTimer = 0;
            }
            _enemyShootTimer += Time.deltaTime;

            if (_isMovingRight)
            {
                MoveInDirection(Vector3.right);
            }
            
            if (_isMovingLeft)
            {
                MoveInDirection(Vector3.left);
            }
        }
    }

    public void MoveInDirection(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * (_gameManager.horizontalWaveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
    
    private void Fire()
    {
        if (_gameManager.ShootingEnemies.Count == 0)
        {
            return;
        }
        int shootingEnemyId = Random.Range(0, _gameManager.ShootingEnemies.Count - 1);
        _gameManager.ShootingEnemies[shootingEnemyId].Fire();
    }
    
    public void StartMoving()
    {
        Stop();
        _isMovingRight = true;
        foreach (var enemy in _gameManager.Enemies)
        {
            enemy.EnableEnemyDeath();
        }
    }


    public void ChangeMovingDirection()
    {
        if (_isChangingDirection)
        {
            return;
        }

        bool wasMovingRight = _isMovingRight;
        _isMovingLeft = false;
        _isMovingRight = false;
        _isChangingDirection = true;
        LeanTween.moveLocalZ(gameObject, gameObject.transform.position.z - _gameSettings.enemiesYChangeAmount,_gameManager.verticalWaveSpeed).setOnComplete(() =>
        {
            _isChangingDirection = false;
            if (wasMovingRight)
            {
                _isMovingRight = false;
                _isMovingLeft = true;
                return;
            }
            else
            {
                _isMovingRight = true;
                _isMovingLeft = false;
            }
        });
    }

    public void Stop()
    {
        _isMovingRight = false;
        _isMovingLeft = false;
        _isChangingDirection = false;
        _enemyShootTimer = 0;
    }
}
