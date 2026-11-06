using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;   // ตั้งชื่อใน Inspector ว่า "Sandbag"
    public Sprite itemSprite; // ลากรูปถุงทรายใส่ในนี้

    // 🌟 เพิ่มช่องนี้เข้ามาเพื่อเชื่อมกับระบบลำดับภารกิจ
    [Header("ระบบภารกิจ (ใส่เฉพาะชิ้นที่เก็บแล้วให้ขยับภารกิจทันที เช่น หมวก)")]
    public ObjectiveManager objectiveManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็กว่าวัตถุที่มาชนคือ Player หรือไม่[cite: 3]
        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            
            if (inv != null && itemSprite != null)
            {
                // สั่ง AddItem เพียง "ครั้งเดียว"[cite: 3]
                inv.AddItem(itemName, itemSprite);

                // 🌟 จุดสำคัญ: ถ้าไอเทมชิ้นนี้มีการลาก ObjectiveManager มาใส่ ให้สั่งเปลี่ยนขั้นภารกิจทันที
                if (objectiveManager != null)
                {
                    objectiveManager.NextObjective();
                    Debug.Log("🎯 เก็บไอเทมภารกิจสำเร็จ! เปลี่ยนเป็นเป้าหมายขั้นถัดไป");
                }

                // เก็บเสร็จแล้วทำลายไอเทมบนพื้นทิ้ง[cite: 3]
                gameObject.SetActive(false); // เปลี่ยนเป็นซ่อนวัตถุแทนการทำลาย[cite: 3]
            }
        }
    }
}
