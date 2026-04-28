using Unity.Cinemachine;
using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    private CinemachineCamera cmVC;

    private void Awake()
    {
        cmVC = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        cmVC.Follow = LevelManager.Instance.SelectedPlayer.transform;
    }

    
}
