using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public int health;
    public bool canShoot;
    public float shootRate;
}
