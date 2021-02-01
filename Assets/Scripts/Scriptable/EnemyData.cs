using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public EnemyController prefab;
    public int health;
    public bool canShoot;
    public uint pointsOnKill;
}
