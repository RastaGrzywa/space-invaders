using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/Player", order = 0)]
public class PlayerData : ScriptableObject
{
    public byte healthAmount;
    public float movementSpeed;
    public float shootRate;
    public float invulnerableTime;
}
