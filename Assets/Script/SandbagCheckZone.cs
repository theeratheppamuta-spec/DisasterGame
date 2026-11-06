using UnityEngine;

public class SandbagCheckZone : MonoBehaviour
{
    public bool isBlocked = false; 
    public string sandbagItemName = "SandBag"; 

    private bool isPlayerNearby = false; // 🌟 ตัวแปรเช็คว่ายืนใกล้ไหม (เหมือนตู้ทีวี)

    void Update()
    {
        // 🌟 1. เช็คว่าผู้เล่นยืนอยู่ใกล้ๆ และกดปุ่ม E ไหม?
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            InteractToPlaceSandbag();
        }
    }

    // 🌟 2. ระบบกล่องล่องหนสำหรับเช็คระยะ (เดินเข้า)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerNearby = true;
        }
    }

    // 🌟 3. ระบบกล่องล่องหนสำหรับเช็คระยะ (เดินออก)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerNearby = false;
        }
    }

    public void InteractToPlaceSandbag()
{
    if (isBlocked) return; 

    PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

    if (inventory != null)
    {
        // 🌟 เปลี่ยนมาใช้ฟังก์ชันนี้ มันจะวิ่งไปสั่งหักของและเสกถุงทรายออกมาจากตัวละครให้เองเลยครับ
        if (inventory.FindAndUseItem(sandbagItemName))
        {
            isBlocked = true; // เปลี่ยนสถานะประตูเป็นอุดแล้ว
            Debug.Log("🧱 ประตูรับทราบการวางถุงทรายสำเร็จ!");
        }
        else
        {
            Debug.Log("❌ ไม่มีถุงทรายในกระเป๋า!");
        }
        }
    }
}