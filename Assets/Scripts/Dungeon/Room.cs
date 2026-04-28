using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public enum RoomType
{
    RoomFree,
    RoomEntrance,
    RoomEnemy,
    RoomBoss
}
public class Room : MonoBehaviour
{
    public static event Action<Room> OnPlayeEnterEvent;

    [Header("Config")]
    [SerializeField] private bool useDebug;
    [SerializeField] private RoomType roomType ;

    [Header("Grid")]
    [SerializeField] private Tilemap extraTilemap;

    [Header("Doors")]
    [SerializeField] private Transform[] posDoorNS;
    [SerializeField] private Transform[] posDoorWE;
    public RoomType RoomType => roomType;

    public bool RoomCompleted { get; set; }

    // Positionn (Key) - Free/Not Free
    private Dictionary<Vector3, bool> tiles = new Dictionary<Vector3, bool>();
    private List<Door> doorList = new List<Door>(); 

    private void Start()
    {
        GeTiles();
        CreateDoors();
        GenerateRoomUsingTemplate();
    }

    private void GeTiles()
    {
        if (NormalRoom())
        {
            return;
        }

        foreach (Vector3Int tilePos in extraTilemap.cellBounds.allPositionsWithin)
        {
            // 1. ตรวจสอบว่าตำแหน่งนั้นมีการวาด Tile ลงไปจริงๆ หรือไม่
            if (extraTilemap.HasTile(tilePos))
            {
                // 2. ใช้ GetCellCenterWorld เพื่อให้ตำแหน่ง X,Y อยู่กึ่งกลางช่องพอดี
                Vector3 position = extraTilemap.GetCellCenterWorld(tilePos);

                // ป้องกัน Error หากมี key ซ้ำ
                if (!tiles.ContainsKey(position))
                {
                    tiles.Add(position, true);
                }
            }
        }
    }

    public void CloseDoors()
    {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].ShowCloseAnimtion();
        }
    } 

    public void OpenDoors()
    {
        for (int i = 0; i < doorList.Count; i++)
        {
            doorList[i].ShowOpenAnimtion();
        }
    }

    private void CreateDoors()
    {
        if (posDoorNS.Length > 0)
        { 
            for (int i = 0; i < posDoorNS.Length; i++)
            {
                RegisterDoor(LevelManager.Instance.DungeonLibrary.DoorNS, posDoorNS[i]);
            }

        }

        if (posDoorWE.Length > 0)
        {
            for (int i = 0; i < posDoorWE.Length; i++)
            {
                RegisterDoor(LevelManager.Instance.DungeonLibrary.DoorWE, posDoorWE[i]);
            }
        }
    }

    private void RegisterDoor(GameObject doorPrefab,Transform objTransform)
    {
       GameObject doorGO = Instantiate(doorPrefab, objTransform.position,Quaternion.identity , objTransform);

        Door door = doorGO.GetComponent<Door>();
        doorList.Add(door);
    }

    private void GenerateRoomUsingTemplate()
    {
        if (NormalRoom())
        {
            return;
        }

        int randomIndex =
            Random.Range(0, LevelManager.Instance.RoomTemplate.Template.Length);
        Texture2D texture = LevelManager.Instance.RoomTemplate.Template[randomIndex];
        List<Vector3> positions = new List<Vector3>(tiles.Keys);
        for (int y = 0, a = 0 ; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++, a++) 
            { 
                Color pixelColor = texture.GetPixel(x, y);
                foreach (RoomRrop prop in LevelManager.Instance.RoomTemplate.PropsData)
                {
                    if (pixelColor == prop.PropColor)
                    {
                        GameObject propInstance =
                            Instantiate(prop.PropPrefab, extraTilemap.transform);
                        propInstance.transform.position = new Vector3(positions[a].x, positions[a].y, 0f);
                        if (tiles.ContainsKey(positions[a]))
                        {
                            tiles[positions[a]] = false;
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetAvailableTilePos()
    {
      List<Vector3> availableTiles = (from tile in tiles 
                                      where tile.Value select tile.Key).ToList();
       int randomIndex = Random.Range(0, availableTiles.Count);
        Vector3 pos = availableTiles[randomIndex];
        return pos;
    }


    private bool NormalRoom()
    {
        return roomType == RoomType.RoomEntrance || roomType == RoomType.RoomFree;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (NormalRoom())
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if(OnPlayeEnterEvent != null)
            {
                OnPlayeEnterEvent.Invoke(this);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (useDebug == false)
        {
            return;
        }

        if (tiles.Count > 0)
        {
            foreach (KeyValuePair<Vector3, bool> tile in tiles)
            {
                if (tile.Value) // True
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(tile.Key, Vector3.one * 0.8f);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(tile.Key, 0.3f);
                }
            }
        }
    }
}





