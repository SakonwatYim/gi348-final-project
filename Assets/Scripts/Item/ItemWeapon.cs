using UnityEngine;

public  enum WeaponType
{
    Melee,
    Gun

}
public enum WeaponRarity
{
    Common,
    Rare,
    Epic,
    Legendary

}

[CreateAssetMenu(menuName = "Items/Weapon")]
public class ItemWeapon : ItemData
{
    [Header("Data")]
    public WeaponType WeaponType;
    public WeaponRarity Rarity;

    [Header("Setting")]
    public float Damage;
    public float RequiredEnergy;
    public float TimeBetweenshots;
    public float MinSpread;
    public float MaxSpread;

    [Header("Weapon")]
    public Weapon WeaponPrefab;

    public override void Pickup()
    {
        LevelManager.Instance.Player
            .GetComponent<PlayerWeapon>().EquipWeapon(WeaponPrefab);
    }
}
