using UnityEngine;

public class GunWeapon : Weapon
{
    [SerializeField] private Projectile projectilePrefab;

    public override void UseWeapon()
    {
        PlayShootAnimation();

        // play shoot SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShoot();
        }

        // Create porjectile
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootPos.position;
        projectile.Direction = shootPos.right;
        if (CharacterParent is PlayerWeapon player)
        {
            projectile.Damage = player.GetDamgeUsingCriticalChance();
        }
        else
        {
            projectile.Damage = itemWeapon.Damage;
        }
        float randomSpread = Random.Range(itemWeapon.MinSpread, itemWeapon.MaxSpread);
        projectile.transform.rotation = Quaternion.Euler(randomSpread * Vector3.forward);
    }

    public override void DestoryWeapon()
    {
        Destroy(gameObject);
    }
}
