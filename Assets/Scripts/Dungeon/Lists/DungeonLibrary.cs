using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLibrary", menuName = "Dungeon/Library")]

public class DungeonLibrary : ScriptableObject
{
    [Header("Levels")]
    public level[] Levels;

    [Header("Room")]
    public GameObject DoorNS;
    public GameObject DoorWE;

    [Header("Extra")]
    public GameObject Chest;
    public GameObject Tombstones;
    public GameObject Portal;

    [Header("Bonus")]
    public BonusBase[] EnemyBonus;
    public float BonusCreationRadius;

}
[Serializable]
public class level
{
    public string Name;
    public GameObject[] Dungeons;
    public EnemyBrain[] Enemies;
    public EnemyBrain Boss;
    public int MinEnemyPerRoom;
    public int MaxEnemyPerRoom;
    public int MinBonusPerEnemy;
    public int MaxBonusPerEnemy;

    public ChestItems ChestItems;
    
}
