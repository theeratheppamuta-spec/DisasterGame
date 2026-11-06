using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // ถ้าคนที่เดินมาชนคือ Player
        if (col.CompareTag("Player"))
        {
            FireManager manager = FindFirstObjectByType<FireManager>();
            
            // สั่งให้ด่านไฟไหม้เริ่มทำงานทันที!
            if (manager != null)
            {
                manager.StartFireEvent();
            }

            // ซ่อนกล่องพรมแดนนี้ทิ้งไปเลย จะได้ไม่ทำงานซ้ำตอนเดินกลับมาเหยียบ
            gameObject.SetActive(false); 
        }
    }
}