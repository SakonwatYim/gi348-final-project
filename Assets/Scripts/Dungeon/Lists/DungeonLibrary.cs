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

}
[Serializable]
public class level
{
    public string Name;
    public GameObject[] Dungeons;
}
