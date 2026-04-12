using System;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomTemplate", menuName = "Dungeon/Room Template")]
public class RoomTemplate : ScriptableObject 
{
    [Header("Template")]
    public Texture2D[] Template;

    [Header("Props")]
    public RoomRrop[] PropsData;


}

[Serializable]
public class RoomRrop
{
    public string Name;
    public Color PropColor;
    public GameObject PropPrefab;
}
