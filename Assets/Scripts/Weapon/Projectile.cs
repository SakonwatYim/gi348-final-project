using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed;

    public Vector3 Direction { get; set; }
    public float Damage { get; set; }

    public float Speed { get; set; }

    private void Start()
    {
        Speed = speed;
    }

    void Update()
    {
        transform.Translate(Direction * (Speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ITakeDamage>() != null)
        {
            other.GetComponent<ITakeDamage>().TakeDamage(Damage);   

            // play hit SFX when hitting an enemy
            if (other.CompareTag("Enemy"))
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayEnemyHit();
                }
            }
        }

        Debug.Log(Damage);
        Destroy(gameObject);
    }
}
