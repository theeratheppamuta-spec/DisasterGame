using UnityEngine;
using System.Collections;
using TMPro; // 🌟 เพิ่มบรรทัดนี้เพื่อให้รู้จัก UI Text

public class StormSystem : MonoBehaviour
{
    public enum StormTrigger {
        StartStage1_CollectItems, 
        StartStage2_DoorTimer,    
        Action_FixDoor,           
        Action_ListenRadio,       
        Check_BunkerExit          
    }

    [Header("เลือกหน้าที่ของกล่องนี้")]
    public StormTrigger triggerType;

    [Header("ตั้งค่าด่านพายุ")]
    public float stage1CollectTime = 30f;
    public string[] requiredItems;
    public float doorFixTimeLimit = 30f; 

    [Header("UI แสดงเวลา (ใส่ TextMeshPro)")]
    public TextMeshProUGUI timerText; // 🌟 ช่องใหม่สำหรับลากตัวหนังสือบอกเวลามาใส่

    public static bool isDoorFixed = false;
    public static bool isRadioSafe = false;
    private static bool isStage1Started = false;
    private static bool isStage2Started = false;

    private bool isPlayerNearby = false;

    // 🌟 เพิ่มฟังก์ชันนี้เข้าไป เพื่อล้างสมองตัวแปร Static ทุกครั้งที่เกิดใหม่
    void Awake()
    {
        isDoorFixed = false;
        isRadioSafe = false;
        isStage1Started = false;
        isStage2Started = false;
    }

    void Start()
    {
        // ซ่อนเวลาไว้ก่อนตอนเริ่มเกม
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (triggerType == StormTrigger.Action_FixDoor)
            {
                isDoorFixed = true;
                Debug.Log("🔨 ซ่อมประตูบังเกอร์สำเร็จ! น้ำไม่เข้าแล้ว");
            }
            else if (triggerType == StormTrigger.Action_ListenRadio)
            {
                isRadioSafe = true;
                Debug.Log("📻 วิทยุแจ้งว่า: พายุสงบแล้ว สามารถออกจากบังเกอร์ได้!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (triggerType == StormTrigger.StartStage1_CollectItems && !isStage1Started)
            {
                isStage1Started = true;
                StartCoroutine(Stage1Timer());
            }
            else if (triggerType == StormTrigger.StartStage2_DoorTimer && !isStage2Started)
            {
                isStage2Started = true;
                StartCoroutine(Stage2DoorTimer());
            }
            else if (triggerType == StormTrigger.Check_BunkerExit)
            {
                if (!isRadioSafe)
                {
                    Debug.Log("💀 ตาย! พายุยังไม่สงบ ออกจากบังเกอร์โดนลมพัดปลิว!");
                    TriggerDeath("ออกจากบังเกอร์โดนลมพัดปลิว");
                }
                else
                {
                    Debug.Log("🎉 ผ่านด่านพายุอย่างปลอดภัย!");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) isPlayerNearby = false;
    }

    // 🌟 ฟังก์ชันตัวช่วย: แอบสแกนกระเป๋าแบบเงียบๆ ว่าครบหรือยัง
    bool CheckHasAllItems()
    {
        PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
        if (inv == null) return false;

        foreach (string item in requiredItems)
        {
            // ถ้าค้นกระเป๋าแล้วไม่เจอไอเทมชื่อนี้...
            if (!inv.HasItem(item)) 
            {
                // 🌟 เพิ่มบรรทัดนี้ลงไป เพื่อให้มันฟ้องออก Console ว่ามันหาคำว่าอะไรไม่เจอ!
                Debug.Log($"❌ ระบบหยุดเวลาไม่ได้ เพราะหาไอเทมชื่อ: [{item}] ไม่เจอในกระเป๋า!");
                return false; 
            }
        }
        return true;
    }

    // 🌟 ปรับปรุง: เปลี่ยนจากรอเฉยๆ เป็นนับถอยหลังขึ้นจอ (ระบบเดิม 100%)
    IEnumerator Stage1Timer()
    {
        float timeLeft = stage1CollectTime;
        if (timerText != null) timerText.gameObject.SetActive(true);

        if (CheckHasAllItems())
            {
                if (timerText != null) timerText.gameObject.SetActive(false);
                Debug.Log("✅ เก็บของพายุครบ รอดตายบทที่ 1! (หยุดเวลาทันที)");
                yield break; // << คำสั่งนี้แหละที่ทำลายตัวนับเวลาทิ้งทันที
            }

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timerText != null) timerText.text = "เวลาหาของ: " + Mathf.CeilToInt(timeLeft).ToString() + " วินาที";
            yield return null; // รอเฟรมถัดไป
        }

        if (timerText != null) timerText.gameObject.SetActive(false);

        // โค้ดเช็คของอันเดิมของคุณ
        PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
        if (inv != null)
        {
            foreach (string item in requiredItems)
            {
                if (!inv.HasItem(item))
                {
                    Debug.Log("💀 ตาย! เก็บของไม่ครบ (ขาด: " + item + ")");
                    TriggerDeath("ท่านเก็บของไม่ครบ");
                    yield break;
                }
            }
        }
        Debug.Log("✅ เก็บของพายุครบ รอดตายบทที่ 1!");
    }

    // 🌟 ปรับปรุง: เปลี่ยนเป็นนับถอยหลังขึ้นจอ
    IEnumerator Stage2DoorTimer()
    {
        float timeLeft = doorFixTimeLimit;
        if (timerText != null) timerText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            // เช็คว่าถ้าซ่อมประตูเสร็จแล้ว ให้หยุดเวลาและซ่อนข้อความเลย
            if (isDoorFixed) 
            {
                if (timerText != null) timerText.gameObject.SetActive(false);
                yield break; 
            }

            timeLeft -= Time.deltaTime;
            if (timerText != null) timerText.text = "รีบซ่อมประตู!: " + Mathf.CeilToInt(timeLeft).ToString() + " วินาที";
            yield return null;
        }

        if (timerText != null) timerText.gameObject.SetActive(false);

        // โค้ดเช็คความตายอันเดิมของคุณ
        if (!isDoorFixed)
        {
            Debug.Log("💀 ตาย! ซ่อมประตูไม่ทัน น้ำทะลักเข้าบังเกอร์!");
            TriggerDeath("ซ่อมประตูไม่ทัน ตาย!!");
        }
    }

    public void ResetStorm()
    {
        isDoorFixed = false;
        isRadioSafe = false;
        isStage1Started = false;
        isStage2Started = false;
        if (timerText != null) timerText.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    void TriggerDeath(string reason) { GameManager m = FindFirstObjectByType<GameManager>(); if(m!=null) m.PlayerDied(reason); }
}