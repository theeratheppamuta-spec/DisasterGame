using UnityEngine;

public class DebrisMover : MonoBehaviour
{
    [Header("ตั้งค่าการบิน")]
    public float speed = 5f;
    public Vector3 moveDirection = Vector3.down; // พุ่งลงล่าง (จากหน้าต่างเข้าห้อง)
    public float lifetime = 4f; // ทำลายตัวเองหลัง 4 วินาที (กันขยะล้นแมพจนคอมค้าง)

    void Start()
    {
        // เริ่มนับเวลาถอยหลังทำลายตัวเองทันทีที่เกิดมา
        Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        // สั่งให้พุ่งไปในทิศทางที่กำหนดตลอดเวลา
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("💥 โดนเศษของกระแทก!");
            
            // 🌟 เปลี่ยนจาก PlayerStun เป็น PlayerMovement ตรงนี้ครับ! 🌟
            PlayerMovement movementScript = collision.GetComponent<PlayerMovement>();
            if (movementScript != null) movementScript.StunPlayer(1.5f);
            
            Destroy(gameObject); 
        }
        else if (collision.CompareTag("Wall"))
        {
             Destroy(gameObject); 
        }
    }
}