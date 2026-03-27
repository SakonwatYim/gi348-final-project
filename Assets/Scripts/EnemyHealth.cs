using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;  // เลือดสูงสุด
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("ศัตรูโดนโจมตี! เลือดเหลือ: " + currentHealth);

        // เอฟเฟกต์กระพริบสีแดง (Optional)
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
    {
        // ใส่เอฟเฟกต์ระเบิดตรงนี้ได้
        Debug.Log("ศัตรูตายแล้ว!");
        Destroy(gameObject);
    }
}