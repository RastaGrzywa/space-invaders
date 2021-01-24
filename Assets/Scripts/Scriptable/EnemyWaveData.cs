using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveData", menuName = "GameData/EnemyWaveData", order = 3)]
public class EnemyWaveData : ScriptableObject
{
    public List<EnemyLineData> enemyLines;
}
