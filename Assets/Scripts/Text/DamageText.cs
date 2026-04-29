using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI damageTmp;

    public void SetDamage(float value)
    {
        damageTmp.text = value.ToString();
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
