using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // --- 1. เช็คว่าชนศัตรูไหม ---
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return; // หยุดทำงาน เพื่อไม่ให้กระสุนไปเช็คอย่างอื่นต่อ
        }

        // --- 2. เช็คว่าชนกล่องพังได้ไหม ---
        DestructibleObject destructible = hitInfo.GetComponent<DestructibleObject>();
        if (destructible != null)
        {
            destructible.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        
        // --- 3. ถ้าชนกำแพง ---
        if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}