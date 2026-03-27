using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;    // ระยะเวลาห่างระหว่างนัด (ยิ่งน้อยยิ่งรัว)
    private float nextFireTime = 0f; // ตัวแปรเก็บเวลาที่จะยิงนัดถัดไปได้

    void Update()
    {
        // 1. ทำให้หมุนตามเมาส์ (ถ้ายังไม่มีโค้ดนี้ให้ใส่ไว้ด้วยครับ)
        RotateTowardsMouse();

        // 2. เปลี่ยนจาก GetButtonDown เป็น GetButton (เช็คการกดค้าง)
        // และเช็คว่าเวลาปัจจุบัน (Time.time) เกินเวลาที่จะยิงนัดถัดไปหรือยัง
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            
            // 3. ตั้งเวลาสำหรับนัดถัดไป
            // เวลาปัจจุบัน + ระยะหน่วง (Delay)
            nextFireTime = Time.time + fireRate; 
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        // หมุนเฉพาะ FirePoint หรือตัวปืน (แนะนำให้หมุนที่ปืน)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        
        // ทำลายกระสุนทิ้งเพื่อประหยัด RAM
        Destroy(bullet, 2f);
    }
}