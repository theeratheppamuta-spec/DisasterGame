using UnityEngine;

public class BunkerItem : MonoBehaviour
{
    [Header("ตั้งชื่อไอเทมชิ้นนี้ให้ตรงกับในระบบประตู/พายุ")]
    public string itemName = "Flashlight"; // เช่น พิมพ์ว่า Flashlight, Water, Food

    [Header("รูปภาพไอเทมที่จะไปโชว์ในกระเป๋า")] // 🌟 เพิ่มช่องนี้เข้ามา!
    public Sprite itemIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ถ้าคนที่เดินมาชนคือ Player
        if (collision.CompareTag("Player"))
        {
            // ไปค้นหาว่ากระเป๋าของผู้เล่นอยู่ที่ไหน
            PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
            
            if (inv != null)
            {
                // 🌟 แก้ไข: ส่งทั้ง "ชื่อ" และ "รูปภาพ" เข้าไปให้กระเป๋า
                inv.AddItem(itemName, itemIcon); 
                Debug.Log($"🎒 เก็บไอเทม: {itemName} เข้ากระเป๋าแล้ว!");
            }
            
            // ปิดการมองเห็นไอเทม (เหมือนเก็บเข้ากระเป๋าไปแล้ว)
            gameObject.SetActive(false); 
        }
    }
}