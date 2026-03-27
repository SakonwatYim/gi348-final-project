using UnityEngine;
using System.Collections;

public class EnemyWander : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 2f;
    public float minWalkTime = 1f;
    public float maxWalkTime = 2.5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isWandering = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // สำคัญ: ป้องกันศัตรูหมุนติ้วเวลาชนกำแพง
        rb.freezeRotation = true; 
        rb.gravityScale = 0;

        // เริ่มต้น Coroutine สำหรับตัวนี้โดยเฉพาะ (อิสระจากตัวอื่น)
        StartCoroutine(WanderRoutine());
    }

    IEnumerator WanderRoutine()
    {
        // สุ่มดีเลย์ตอนเริ่มนิดนึง เพื่อไม่ให้ศัตรูทุกตัวขยับพร้อมกันเป๊ะๆ
        yield return new WaitForSeconds(Random.Range(0f, 1f));

        while (true)
        {
            // 1. สุ่มทิศทางใหม่ (8 ทิศทาง)
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            moveDirection = new Vector2(randomX, randomY).normalized;

            // 2. เริ่มเดิน (สุ่มระยะเวลาเดิน)
            isWandering = true;
            float walkDuration = Random.Range(minWalkTime, maxWalkTime);
            yield return new WaitForSeconds(walkDuration);

            // 3. หยุดเดิน (สุ่มระยะเวลาพัก)
            isWandering = false;
            rb.linearVelocity = Vector2.zero; // หยุดเฉพาะตัวมันเอง
            
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);
        }
    }

    void FixedUpdate()
    {
        // ใช้ FixedUpdate เพื่อให้ฟิสิกส์ลื่นไหลและไม่ทะลุกำแพง
        if (isWandering)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            
            // กลับด้าน Sprite ตามทิศทางเดิน (Flip)
            if (moveDirection.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
            else if (moveDirection.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // ถ้าชนกำแพง ให้หยุดเดินทันทีแล้วรอสุ่มทิศทางใหม่ (แก้ปัญหาเดินอัดกำแพงค้าง)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWandering = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}