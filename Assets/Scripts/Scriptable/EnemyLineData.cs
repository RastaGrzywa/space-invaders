using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLineData", menuName = "GameData/EnemyLineData", order = 2)]
public class EnemyLineData : ScriptableObject
{
    public EnemyData enemy;
    public int amountOfEnemies;
}
