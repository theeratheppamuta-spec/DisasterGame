using UnityEngine;
using System.Collections;
using TMPro; // 🌟 ต้องเพิ่มบรรทัดนี้เพื่อจัดการ UI ตัวหนังสือ

public class FireManager : MonoBehaviour
{
    [Header("ลาก ObjectiveManager ของด่านไฟมาใส่")]
    public ObjectiveManager fireObjectiveManager;

    [Header("จุดเหยียบเริ่มด่าน (เพื่อรีเซ็ตตอนตาย)")]
    public ChapterStartTrigger fireStartTrigger;
    [Header("ตั้งค่าด่านไฟไหม้ บทที่ 1")]
    public float timeLimit = 30f; 
    public string extinguisherName = "Extinguisher"; 
    public GameObject warpPoint; 

    [Header("ตั้งค่าไฟลาม (Cinematic)")]
    public float spreadDelay = 0.2f; 

    [Header("ตั้งค่า UI เวลา")]
    public TextMeshProUGUI timerText; // 🌟 ช่องสำหรับลากข้อความเวลามาใส่
    
    [HideInInspector] public bool isPlayerInFireStage = false; 

    private FirePoint[] allFires; 
    private int totalFires;
    private int firesExtinguished = 0;
    private bool isStage1Clear = false;
    

    // 🌟 ตัวแปรสำหรับคุมเวลา
    private float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        allFires = Object.FindObjectsByType<FirePoint>(FindObjectsSortMode.None);
        totalFires = allFires.Length;

        foreach (FirePoint fire in allFires)
        {
            fire.gameObject.SetActive(false); 
        }

        if (warpPoint != null) warpPoint.SetActive(false); 

        // 🌟 ซ่อน UI เวลาไว้ก่อนตอนเริ่มเกม
        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 🌟 ระบบนับเวลาถอยหลังแบบเรียลไทม์
        if (isTimerRunning && !isStage1Clear)
        {
            currentTime -= Time.deltaTime; // ลดเวลาลงตามเฟรมเรท

            // อัปเดตตัวเลขบนหน้าจอ
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // โชว์ฟอร์แมต 00:30
            }

            // เมื่อเวลาหมด
            if (currentTime <= 0)
            {
                isTimerRunning = false;
                currentTime = 0;
                
                if (timerText != null) timerText.text = "00:00";
                
                Debug.Log("💀 หมดเวลา! ดับไฟไม่ทัน โดนไฟครอกตาย!");
                GameManager manager = FindFirstObjectByType<GameManager>();
                if(manager != null) manager.PlayerDied("\nท่านเสียชีวิตจากไฟครอก");
            }
        }
    }

    public void StartFireEvent()
    {
        if (!isPlayerInFireStage) 
        {
            isPlayerInFireStage = true; 
            StartCoroutine(SpawnFiresGradually());
        }
    }

    IEnumerator SpawnFiresGradually()
    {
        Debug.Log("🔥 เข้าสู่ด่านไฟไหม้! ไฟเริ่มลามแล้ว!");
        foreach (FirePoint fire in allFires)
        {
            fire.gameObject.SetActive(true);
            yield return new WaitForSeconds(spreadDelay); 
        }
        
        Debug.Log("⏰ ลามครบแล้ว เริ่มจับเวลา!");
        
        // 🌟 ตั้งค่าเวลาและเปิดโชว์ UI
        currentTime = timeLimit;
        isTimerRunning = true;
        if (timerText != null) timerText.gameObject.SetActive(true);
    }

    public void ReportFireExtinguished()
    {
        firesExtinguished++;
        // 🌟 เพิ่มเงื่อนไขนี้: ถ้าดับครบแล้ว ให้สั่งเปลี่ยนภารกิจ!
        if (firesExtinguished >= totalFires)
        {
            if (fireObjectiveManager != null)
            {
                fireObjectiveManager.NextObjective();
                Debug.Log("🔥 ไฟถูกดับหมดแล้ว! เปลี่ยนภารกิจแล้วครับ");
            }
        if (firesExtinguished >= totalFires)
           {
            isStage1Clear = true;
            isTimerRunning = false; // 🌟 หยุดเวลา

            // 🌟 ซ่อนเวลาเมื่อชนะด่าน
            if (timerText != null) timerText.gameObject.SetActive(false);

            Debug.Log("🎉 ดับไฟครบทุกจุดแล้ว รอดตายจากบทที่ 1!");

            if (warpPoint != null) warpPoint.SetActive(true);

            PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
            if (inv != null) inv.RemoveItem(extinguisherName);
           }
        }
    }

    public void ResetFire()
    {
        StopAllCoroutines(); 
        isStage1Clear = false;
        firesExtinguished = 0;
        

        // 🌟 สั่งล้างความจำจุดเหยียบ ให้ผู้เล่นเดินมาเริ่มด่านใหม่ได้
        if (fireStartTrigger != null)
        {
            fireStartTrigger.ResetTrigger();
        }
        
        // 🌟 รีเซ็ตและซ่อนเวลา
        isTimerRunning = false;
        if (timerText != null) timerText.gameObject.SetActive(false);

        if (warpPoint != null) warpPoint.SetActive(false);

        foreach (FirePoint fire in allFires)
        {
            if (fire != null)
            {
                fire.gameObject.SetActive(true); 
                fire.ResetFirePoint();           
                fire.gameObject.SetActive(false); 
            }
        }

        if (isPlayerInFireStage)
        {
            StartCoroutine(SpawnFiresGradually());
        }
    }
}