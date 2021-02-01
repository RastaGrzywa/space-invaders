using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private PlayerController _playerController;
    [Inject] private UIController _uiController;
    [Inject] private GameScreen _gameScreen;
    [Inject] private ResultScreen _resultScreen;
    [Inject] private ObjectsPooler _objectsPooler;
    
    [SerializeField] private WavesSpawner wavesSpawner;
    
    private byte _currentPlayerLives;
    private uint _currentPlayerScore;
    private ushort _currentWave;
    public float horizontalWaveSpeed = 5f;
    public float verticalWaveSpeed = 0.5f;

    private List<EnemyController> _shootingEnemies;
    public List<EnemyController> ShootingEnemies
    {
        get => _shootingEnemies;
        private set{}
    }
    
    private List<EnemyController> _enemies;
    public List<EnemyController> Enemies
    {
        get => _enemies;
        private set{}
    }
    private void Start()
    {
        _shootingEnemies = new List<EnemyController>();
        _enemies = new List<EnemyController>();
        verticalWaveSpeed = _gameSettings.enemiesYMoveSpeed;
        horizontalWaveSpeed = _gameSettings.enemiesXMoveSpeed;

        for (var enemyLineId = 0; enemyLineId < _gameSettings.enemyWaveData.enemyLines.Count; enemyLineId++)
        {
            var enemyLine = _gameSettings.enemyWaveData.enemyLines[enemyLineId];
            for (int i = 0; i < enemyLine.amountOfEnemies; i++)
            {
                string poolTag = "Line" + enemyLineId;
                EnemyController enemy = Instantiate(enemyLine.enemy.prefab);
                enemy.transform.parent = wavesSpawner.waveController.transform;
                enemy.SetupEnemyData(enemyLine.enemy, wavesSpawner.waveController, poolTag);
                _objectsPooler.AddObjectToPool(poolTag, enemy.gameObject);
            }
        }

        _uiController.HideUI();
        _uiController.ShowMainMenu();
    }


    public void OnPlayerHit()
    {
        _currentPlayerLives -= 1;
        _gameScreen.UpdatePlayerHealthUI(_currentPlayerLives);
        
        if (_currentPlayerLives <= 0)
        {
            PlayerDie();
            return;
        }
    }

    private void PlayerDie()
    {
        wavesSpawner.StopWave();
        _shootingEnemies.Clear(); 
        _playerController.DisablePlayerMovement();
        _uiController.HideUI();
        _uiController.ShowResult();
        _resultScreen.UpdateResultScreenUI(_currentWave - 1, _currentPlayerScore);
        DataManager.SaveScore(_currentPlayerScore);
    }

    public void WaveCompleted()
    {
        _currentWave += 1;
        _gameScreen.UpdateWaveUI(_currentWave);
        wavesSpawner.ResetWavePosition();
        wavesSpawner.SpawnWave();
        wavesSpawner.EnableWave();
        horizontalWaveSpeed = 5;
    }
    
    public void ResetGameState()
    {
        _playerController.ResetPosition();
        foreach (var enemy in _enemies)
        {
            _objectsPooler.AddObjectToPool(enemy.PoolTag, enemy.gameObject);
        }
        _enemies.Clear();
        _shootingEnemies.Clear();
        verticalWaveSpeed = _gameSettings.enemiesYMoveSpeed;
        horizontalWaveSpeed = _gameSettings.enemiesXMoveSpeed;
    }

    public void SetupPlayMode()
    {
        _currentWave = 1;
        _currentPlayerScore = 0;
        _currentPlayerLives = _gameSettings.playerData.healthAmount;
        _playerController.MovePlayerToGamePosition();
        wavesSpawner.ResetWavePosition();
        wavesSpawner.SpawnWave();
        wavesSpawner.EnableWave();
        _gameScreen.UpdatePlayerHealthUI(_currentPlayerLives);
        _gameScreen.UpdateWaveUI(_currentWave);
        _gameScreen.UpdatePlayerScoreUI(_currentPlayerScore);
    }

    public void OnEnemyHit(uint pointsOnKill, EnemyController enemyController)
    {
        _currentPlayerScore += pointsOnKill;
        horizontalWaveSpeed += _gameSettings.speedUpWaveAmount;
        _gameScreen.UpdatePlayerScoreUI(_currentPlayerScore);
        _shootingEnemies.Remove(enemyController);
        _enemies.Remove(enemyController);
        _objectsPooler.ReturnObjectToPool(enemyController.PoolTag, enemyController.gameObject);
        if (_enemies.Count == 0)
        {
            WaveCompleted();
        }
    }
    
    public void AddShootingEnemy(EnemyController enemy)
    {
        _shootingEnemies.Add(enemy);
    }

    public void AddEnemy(EnemyController enemy)
    {
        _enemies.Add(enemy);
    }
}
