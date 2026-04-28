using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, ITakeDamage
{
    [Header("Config")]
    [SerializeField] private float health;

    private SpriteRenderer sp;
    private float enemyHealth;
    private Color initailColor;
    private Coroutine colorCoroutine;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        enemyHealth = health;
        initailColor = sp.color;
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
        ShowDamadeColor();
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ShowDamadeColor()
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        colorCoroutine = StartCoroutine(IETakeDamage());
    }

    private IEnumerator IETakeDamage()
    {
        sp.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sp.color = initailColor;

    }
   
  
}
