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
    public float RequiredRnergy;
    public float TimeBetweenshots;
    public float MinSpread;
    public float MaxSpread;

}
