using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Temp")]
    [SerializeField] private GameObject player;

    [Header("Config")]
    [SerializeField] private RoomTemplate roomTemplate;
    [SerializeField] private DungeonLibrary dungeonLibrary;

    public GameObject Player => player;
    public RoomTemplate RoomTemplate => roomTemplate;
    public DungeonLibrary DungeonLibrary => dungeonLibrary;

    private Room currentRoom;
    private int currentLevelIndex;
    private int currentDungeonIndex;
    private GameObject currentDungeonGO;

    private List<GameObject> currentLevelChestItems = new List<GameObject>();

    private void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        currentDungeonGO = Instantiate(dungeonLibrary.Levels[currentLevelIndex]
            .Dungeons[currentDungeonIndex], transform);
        currentLevelChestItems = new List<GameObject>
            (dungeonLibrary.Levels[currentLevelIndex].ChestItems.AvilableItems);
    }

    private void ContinueDungeon()
    {
        currentDungeonIndex++;
        if (currentDungeonIndex > dungeonLibrary.Levels[currentLevelIndex].Dungeons.Length - 1)
        {
            currentDungeonIndex = 0;
            currentLevelIndex++;
        }

        Destroy(currentDungeonGO);
        CreateDungeon();
        PositionPlayer();
    }

    private void PositionPlayer()
    {
        Room[] dungeonRooms = currentDungeonGO.GetComponentsInChildren<Room>();
        Room entranceRoom = null;
        for (int i = 0; i < dungeonRooms.Length; i++)
        {
            if (dungeonRooms[i].RoomType == RoomType.RoomEntrance)
            {
                entranceRoom = dungeonRooms[i];
            }
        }

        if (entranceRoom != null)
        {
            if (player != null)
            {
                player.transform.position = entranceRoom.transform.position;
            }
        }
    }

    public GameObject GetRandomItemForChest()
    {
        int randomIndex  = Random.Range(0, currentLevelChestItems.Count);
        GameObject item = currentLevelChestItems[randomIndex];
        currentLevelChestItems.Remove(item);
        return item;
    }

    private IEnumerator IEContinueDungeon()
    {
        UIManager.Instance.FadeNewDungeon(1f);
        yield return new WaitForSeconds(2f);
        ContinueDungeon();
        UIManager.Instance.FadeNewDungeon(0f);
    }

    private void PlayerEnterEventCallback(Room room)
    {
        currentRoom = room;
        if (currentRoom.RoomCompleted == false)
        {
            currentRoom.CloseDoors();
        }
    }

    private void PortalEventCallback()
    {
        StartCoroutine(IEContinueDungeon());
    }

    private void OnEnable()
    {
        Room.OnPlayeEnterEvent += PlayerEnterEventCallback;
        Portal.OnPortalEvent += PortalEventCallback;
    }

    private void OnDisable()
    {
        Room.OnPlayeEnterEvent -= PlayerEnterEventCallback;
        Portal.OnPortalEvent -= PortalEventCallback;
    }
}
