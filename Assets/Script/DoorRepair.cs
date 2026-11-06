using UnityEngine;

public class DoorRepair : MonoBehaviour
{
    public Sprite repairedDoorSprite; // รูปประตูที่ซ่อมแล้ว
    public GameObject repairHint;     // เครื่องหมาย ! บนหัวประตู (ถ้ามี)
    
    private bool isPlayerNearby = false;
    private bool isRepaired = false;

    void Update()
    {
        // เดินมาชนประตูแล้วกด E
        if (isPlayerNearby && !isRepaired && Input.GetKeyDown(KeyCode.E))
        {
            CheckToolBox();
        }
    }

    void CheckToolBox()
{
    // ค้นหา PlayerInventory ในฉาก
    PlayerInventory playerInv = Object.FindAnyObjectByType<PlayerInventory>();

    // เช็กว่าเจอ Inventory และมี ToolBox ไหม
    if (playerInv != null && playerInv.HasItem("ToolBox"))
    {
        DoRepair();
    }
    else
    {
        Debug.Log("ไม่มี ToolBox ซ่อมไม่ได้นะ!");
    }
}

    void DoRepair()
    {
        isRepaired = true;
        
        // 1. เปลี่ยนภาพประตู
        GetComponent<SpriteRenderer>().sprite = repairedDoorSprite;
        
        // 2. ปิดคำเตือน
        if (repairHint != null) repairHint.SetActive(false);

        PlayerInventory playerInv = Object.FindAnyObjectByType<PlayerInventory>();
        if (playerInv != null)
       {
        playerInv.RemoveItem("ToolBox"); // สั่งให้ลบ ToolBox ออกจากกระเป๋า

        // 🌟 [เพิ่มโค้ดส่วนนี้] สั่งให้ระบบเริ่มนับถอยหลังเปิดวิทยุในตัวอัตโนมัติ
            StartCoroutine(TriggerRadioWithDelay(playerInv));
       }

       StormSystem.isDoorFixed = true;

        Debug.Log("ใช้ ToolBox ซ่อมประตูเรียบร้อย!");

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRepaired)
        {
            isPlayerNearby = true;
            if (repairHint != null) repairHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (repairHint != null) repairHint.SetActive(false);
        }
    }

    // 🌟 [เพิ่ม Coroutine นี้เข้าไปท้ายสคริปต์]
    System.Collections.IEnumerator TriggerRadioWithDelay(PlayerInventory playerInv)
    {
        yield return new WaitForSeconds(1f); // หน่วงเวลา 1 วินาทีให้ผู้เล่นหายใจแป๊บนึง

        // เช็คว่าในกระเป๋ามีไอเทมชื่อ "Radio" หรือไม่
        if (playerInv != null && playerInv.HasItem("Radio"))
        {
            RadioController radio = Object.FindAnyObjectByType<RadioController>();
            if (radio != null)
            {
                radio.TriggerAutoBroadcast(); // เรียกวิทยุดังทันที!
            }
        }
        else
        {
            Debug.Log("📻 วิทยุไม่ดัง เพราะผู้เล่นไม่ได้หยิบวิทยุมาจากบทแรก!");
        }
    } 
}