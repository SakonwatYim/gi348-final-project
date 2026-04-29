using UnityEngine;

public class DamageManager : Singleton<DamageManager>
{
    [Header("Config")]
    [SerializeField] private DamageText damageTextPrefab;

    public void ShowDamage(float damage, Transform entityPos)
    {
        Vector3 extraPos = Vector3.right * Random.Range(-0.5f, 0.5f) + Vector3.up * Random.Range(0.5f, 1f);
        DamageText instance = Instantiate(damageTextPrefab, entityPos.position + extraPos , Quaternion.identity);
        instance.SetDamage(damage);
    }
}
