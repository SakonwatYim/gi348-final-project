using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Offset ปกติ")]
    public Vector3 offset = new Vector3(0, 1.5f, -10);

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f;   // ระยะมองไปข้างหน้า
    public float lookAheadSpeed = 3f;      // ความเร็วในการเปลี่ยนทิศ

    [Header("Smooth")]
    public float smoothSpeed = 5f;

    private Vector3 currentLookAhead;

    void LateUpdate()
    {
        if (target == null) return;

        // หาทิศการเคลื่อนที่ (velocity)
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        Vector2 velocity = rb != null ? rb.linearVelocity : Vector2.zero;

        // ถ้ากำลังเคลื่อนที่ → มองไปข้างหน้า
        Vector3 targetLookAhead = Vector3.zero;

        if (velocity.magnitude > 0.1f)
        {
            targetLookAhead = velocity.normalized * lookAheadDistance;
        }

        // ทำให้เลื่อนนุ่ม
        currentLookAhead = Vector3.Lerp(currentLookAhead, targetLookAhead, lookAheadSpeed * Time.deltaTime);

        // ตำแหน่งสุดท้าย
        Vector3 desiredPosition = target.position + offset + currentLookAhead;

        // Smooth กล้อง
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}