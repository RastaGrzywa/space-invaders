using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WavesSpawner : MonoBehaviour
{
    [Inject] private GameSettings _gameSettings;
    [Inject] private GameManager _gameManager;
    [Inject] private UIController _uiController;
    [Inject] private ObjectsPooler _objectsPooler;
    
    public WaveController waveController;
    
    private float _distance = 2;

    private void Start()
    {
        ResetWavePosition();
    }

    public void ResetWavePosition()
    {
        waveController.transform.position = new Vector3(0, 0, 16f);
    }
    
    public void EnableWave()
    {
        LeanTween.moveZ(waveController.gameObject, 2.5f, 2f).setOnComplete(() =>
        {
            waveController.StartMoving();
        });
    }
    
    public void SpawnWave()
    {
        StopWave();
        float enemyYPosition = waveController.transform.position.z 
                               - (_gameSettings.enemyWaveData.enemyLines.Count * 2f) / 2f;
        float enemyXPosition;

        for (var enemyLineIndex = 0; enemyLineIndex < _gameSettings.enemyWaveData.enemyLines.Count; enemyLineIndex++)
        {
            var enemyLine = _gameSettings.enemyWaveData.enemyLines[enemyLineIndex];
            enemyXPosition = waveController.transform.position.x - (enemyLine.amountOfEnemies * 2f) / 2f;

            for (int i = 0; i < enemyLine.amountOfEnemies; i++)
            {
                GameObject enemyObject = _objectsPooler.SpawnFromPool("Line" + enemyLineIndex, Vector3.zero);
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                enemy.transform.position = new Vector3(enemyXPosition, 0, enemyYPosition);
                if (enemyLine.enemy.canShoot)
                {
                    _gameManager.AddShootingEnemy(enemy);
                }

                _gameManager.AddEnemy(enemy);
                enemyXPosition += 2;
            }

            enemyYPosition += 2;
        }
    }

    public void StopWave()
    {
        waveController.Stop();
    }

}
