using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;       // เลือดสูงสุดของกล่อง
    private int currentHealth;

    [Header("Effects & Drops (Optional)")]
    public GameObject destroyEffect; // เอฟเฟกต์ตอนแตก (ถ้ามี)
    public GameObject dropItem;    // ไอเทมที่จะดรอป (ถ้ามี)
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ฟังก์ชันรับดาเมจ (เรียกใช้จากสคริปต์กระสุน)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " โดนโจมตี! เลือดเหลือ: " + currentHealth);

        // เอฟเฟกต์กระพริบสีขาว (เพื่อให้รู้ว่าโดนยิง)
        StopAllCoroutines();
        StartCoroutine(FlashColor());

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    System.Collections.IEnumerator FlashColor()
    {
        spriteRenderer.color = Color.white; // หรือสีแดง
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.gray;  // สีปกติ (หรือสีเดิมของกล่อง)
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }

    void DestroyObject()
    {
        // 1. สร้างเอฟเฟกต์แตก (ถ้ามี)
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // 2. สุ่มดรอปไอเทม (ถ้ามี)
        if (dropItem != null)
        {
            // ตัวอย่าง: ดรอปไอเทมแค่ 50%
            if (Random.value < 0.5f)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }

        Debug.Log(gameObject.name + " พังแล้ว!");
        
        // 3. ทำลายกล่องทิ้ง
        Destroy(gameObject);
    }
}