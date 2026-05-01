using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public static event Action OnRoomCompletedEvent;

    [Header("Config")]
    [SerializeField] private RoomTemplate roomTemplate;
    [SerializeField] private DungeonLibrary dungeonLibrary;

    
    public RoomTemplate RoomTemplate => roomTemplate;
    public DungeonLibrary DungeonLibrary => dungeonLibrary;
    public GameObject SelectedPlayer { get; set; }

    private Room currentRoom;
    private int currentLevelIndex;
    private int currentDungeonIndex;
    private int enemyCounter;
    private GameObject currentDungeonGO;

    private List<GameObject> currentLevelChestItems = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        CreatePlayer();

    }

    private void Start()
    {
        CreateDungeon();
    }

    private void CreatePlayer()
    {
        if (GameManager.Instance.Player != null)
        {
            SelectedPlayer = Instantiate(GameManager.Instance.Player.PlayerPrefab);
        }
    }

    private void CreateEnemies()
    {
        int enemyAmount = GetEnemyAmount();
        enemyCounter = enemyAmount;
        for (int i = 0; i < enemyAmount; i++)
        {
            Vector3 tilePos = currentRoom.GetAvailableTilePos();
            EnemyBrain enemy = Instantiate(GetEnemy(), tilePos, 
                Quaternion.identity, currentRoom.transform);
            enemy.RoomParent = currentRoom;
        }
    }

    private void CreateBoss()
    {
        Vector3 tilePos = currentRoom.GetAvailableTilePos();
        EnemyBrain boss = Instantiate(dungeonLibrary.Levels[currentLevelIndex].Boss, tilePos,
            Quaternion.identity, currentRoom.transform);
        boss.RoomParent = currentRoom;
    }
    private EnemyBrain GetEnemy()
    {
        EnemyBrain[] enemies = dungeonLibrary.Levels[currentLevelIndex].Enemies;
        int randomIndex = UnityEngine.Random.Range(0, enemies.Length);
        EnemyBrain randomEnemy = enemies[randomIndex];
        return randomEnemy;
    }

    private int GetEnemyAmount()
    {
        int amount = 
            Random.Range(dungeonLibrary.Levels[currentLevelIndex].MinEnemyPerRoom,
            dungeonLibrary.Levels[currentLevelIndex].MaxEnemyPerRoom);
        return amount;
    }

    private void CreateDungeon()
    {
        currentDungeonGO = Instantiate(dungeonLibrary.Levels[currentLevelIndex]
            .Dungeons[currentDungeonIndex], transform);
        currentLevelChestItems = new List<GameObject>
            (dungeonLibrary.Levels[currentLevelIndex].ChestItems.AvilableItems);
    }

    private void CreateTombstoneInEnemyPos(Transform enemyTransform)
    {
        Instantiate(dungeonLibrary.Tombstones, enemyTransform.position, 
            Quaternion.identity , currentRoom.transform);
    }

    private void CreateChestInsideRoom()
    {
        Vector3 chestPoes = currentRoom.GetAvailableTilePos();
        Instantiate(dungeonLibrary.Chest, chestPoes, 
            Quaternion.identity, currentRoom.transform);
    }

    private void CreateBonusInEnemyPos(Transform enemyPos)
    {
        int bonusAmount = 
            Random.Range(dungeonLibrary.Levels[currentLevelIndex].
            MinBonusPerEnemy,dungeonLibrary.Levels[currentLevelIndex].MaxBonusPerEnemy);
        for (int i = 0; i < bonusAmount; i++)
        {
            int randomBonusIndex = Random.Range(0, dungeonLibrary.EnemyBonus.Length);
            Vector3 bonusExtraPos = Random.insideUnitCircle.normalized * dungeonLibrary.BonusCreationRadius;
            Instantiate(dungeonLibrary.EnemyBonus[randomBonusIndex], enemyPos.position + bonusExtraPos, 
                Quaternion.identity, currentRoom.transform);
        }
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
            if (SelectedPlayer != null)
            {
                SelectedPlayer.transform.position = entranceRoom.transform.position;
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
        UIManager.Instance.UpdateLevelText(GetCurrentLevelText());
        UIManager.Instance.FadeNewDungeon(0f);
    }

    private string GetCurrentLevelText()
    {
        return 
            $"{dungeonLibrary. Levels[currentLevelIndex].Name} - {currentDungeonIndex + 1}";
    }

    private void PlayerEnterEventCallback(Room room)
    {
        currentRoom = room;
        if (currentRoom.RoomCompleted == false)
        {
            currentRoom.CloseDoors();
            switch(currentRoom.RoomType)
            {
                case RoomType.RoomEnemy:
                    CreateEnemies();
                    break;
                case RoomType.RoomBoss:
                    CreateBoss();
                    // switch to boss BGM
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayBossBGM();
                    }
                    break;
                
            }
        }
    }

    private void PortalEventCallback()
    {
        StartCoroutine(IEContinueDungeon());
    }

    private void EnemyKilledCallback(Transform enemyTransform)
    {
        enemyCounter--;
        CreateTombstoneInEnemyPos(enemyTransform);
        CreateBonusInEnemyPos(enemyTransform);
        if (enemyCounter <= 0)
        {
            if(currentRoom.RoomCompleted == false)
            {
                enemyCounter = 0;
                currentRoom.SetRoomCompleted();
                CreateChestInsideRoom();
                OnRoomCompletedEvent?.Invoke();
                if (currentRoom.RoomType == RoomType.RoomBoss)
                {
                    Vector3 tilePos = currentRoom.GetAvailableTilePos();
                    Instantiate(dungeonLibrary.Portal, tilePos, Quaternion.identity, currentRoom.transform);

                    // switch back to normal BGM when boss dies
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayMainBGM();
                    }
                }
            }
            
        }
    }

    private void OnEnable()
    {
        Room.OnPlayeEnterEvent += PlayerEnterEventCallback;
        EnemyHealth.OnEnemyKilledEvent += EnemyKilledCallback;
        Portal.OnPortalEvent += PortalEventCallback;
    }

    

    private void OnDisable()
    {
        Room.OnPlayeEnterEvent -= PlayerEnterEventCallback;
        Portal.OnPortalEvent -= PortalEventCallback;
        EnemyHealth.OnEnemyKilledEvent -= EnemyKilledCallback;
    }
}
