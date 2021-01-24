using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/Player", order = 0)]
public class PlayerData : ScriptableObject
{
    public int healthAmount;
    public float movementSpeed;
    public float shootRate;
}
