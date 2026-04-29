using UnityEngine;

[CreateAssetMenu(menuName = "Items/Energy Potion")]
public class ItemEnrgyPotion : ItemData
{
    [Header("Config")]
    [SerializeField] private float energy;
    public override void Pickup()
    {
        LevelManager.Instance.SelectedPlayer.
            GetComponent<PlayerEnergy>().RecoverEnergy(energy);
    }
}
