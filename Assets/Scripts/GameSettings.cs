using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class GameSettings
{
    public PlayerData playerData;
    public EnemyWaveData enemyWaveData;
    
    public char playerHealthSymbol;

    public float speedUpWaveAmount;
    public float enemiesShootingRate;
    public float enemiesYMoveSpeed;
    public float enemiesXMoveSpeed;
    public float enemiesYChangeAmount;

    public GameObject enemyShootPrefab;

    public AssetReference playerShot;
}
