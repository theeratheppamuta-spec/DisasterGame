using UnityEngine;
using UnityEngine.Events; 
using TMPro; 

public class BunkerDoor : MonoBehaviour
{
    [Header("ระบบ Cutscene")]
    public UniversalCutsceneManager cutsceneManager;

    [Header("ระบบเช็คของ (พิมพ์ชื่อไอเทมที่ต้องการให้ครบ)")]
    // 🌟 เปลี่ยนจากการใส่เลข เป็นการใส่ชื่อไอเทมแทน เพื่อความแม่นยำ
    public string[] requiredItems = { "Flashlight", "Water", "Food", "Radio", "FirstAid" }; 

    [Header("เหตุการณ์เมื่อเก็บของไม่ครบ (ตาย)")]
    public string reasonMessage = "คุณรอดชีวิตไม่ได้ เพราะเก็บของเตรียมพร้อมไม่ครบ!"; 
    public TextMeshProUGUI deathReasonText; 
    public UnityEvent onDeath; 

    private bool isPlayerNearby = false;

    void Update()
    {
        // เมื่อผู้เล่นอยู่ใกล้ และกดปุ่ม E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // 🌟 ให้ฟังก์ชันไปค้นกระเป๋าว่ามีของครบไหม
            if (CheckHasAllItems()) 
            {
                // ✅ ของครบ: รอดชีวิต!
                if (cutsceneManager != null)
                {
                    cutsceneManager.PlayFinalCutscene("Storm"); 
                    Debug.Log("🚪 ประตูเปิด: รอดชีวิต!");
                }
            }
            else
            {
                // 💀 ของไม่ครบ: เปลี่ยนข้อความแล้วสั่งตาย!
                Debug.Log("💀 เก็บของไม่ครบ! ผู้เล่นตาย!");
                
                if (deathReasonText != null)
                {
                    deathReasonText.text = reasonMessage;
                }
                
                onDeath.Invoke(); 
            }
        }
    }

    // 🌟 ฟังก์ชันค้นกระเป๋า (เหมือนใน StormSystem เป๊ะ)
    bool CheckHasAllItems()
    {
        PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
        if (inv == null) return false;

        foreach (string item in requiredItems)
        {
            // ถ้าค้นแล้วเจอว่า "ขาด" ไอเทมชิ้นไหนชิ้นนึง จะตีกลับว่าไม่ครบ (false) ทันที
            if (!inv.HasItem(item)) return false; 
        }
        
        // ถ้าค้นจนจบแล้วไม่ขาดเลย แปลว่าครบ (true)
        return true; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNearby = false;
    }
}