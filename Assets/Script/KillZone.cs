using UnityEngine;

public class KillZone : MonoBehaviour
{
    // สร้างช่องให้ลาก GameManager มาใส่จากหน้าต่าง Inspector ทันที
    public GameManager gameManager; 

    // 🌟 1. เพิ่มตัวแปรสำหรับตั้งข้อความตาย (ใส่ค่าเริ่มต้นไว้เผื่อลืมพิมพ์)
    // [TextArea] จะช่วยให้ช่องพิมพ์ใน Unity กว้างขึ้นและกด Enter ขึ้นบรรทัดใหม่ได้ครับ
    [Header("ข้อความสาเหตุการตาย")]
    [TextArea]
    public string deathReason = "ทางเลือกที่ผิดพลาด!\nคุณเสียชีวิต";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("ชนกล่องจำลองทางเลือกที่ผิด -> ส่งไปฉากตาย");
            
            // เรียกใช้งานได้โดยตรงเลย ไม่ต้องสั่งสแกนหาในด่านให้เหนื่อยเครื่อง
            if (gameManager != null)
            {
                gameManager.PlayerDied(deathReason);
            }
        }
    }
}