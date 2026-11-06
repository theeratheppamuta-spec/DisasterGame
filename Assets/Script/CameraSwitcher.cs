using UnityEngine;
using Unity.Cinemachine; // สำหรับ Cinemachine เวอร์ชั่นใหม่

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera targetCamera; // ลากกล้องของโซนนี้มาใส่

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เมื่อเดินเข้าโซน ให้เพิ่ม Priority เพื่อให้กล้องนี้ทำงาน
            targetCamera.Priority = 20;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เมื่อเดินออกจากโซน ให้ลด Priority ลง เพื่อให้กล้องเดิมกลับมาทำงาน
            targetCamera.Priority = 5;
        }
    }
}