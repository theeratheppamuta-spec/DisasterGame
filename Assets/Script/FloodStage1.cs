using UnityEngine;
using System.Collections;
using TMPro;

public class FloodStage1 : MonoBehaviour
{
    [Header("ตั้งค่าเวลาด่านแรก")]
    public float searchTimeLimit = 30f;
    public string[] requiredItems; 
    
    [Header("อ้างอิงจุดตรวจถุงทราย")]
    public SandbagCheckZone[] sandbagZones;

    // 🌟 [พิมพ์เพิ่มตรงนี้] กล่องคัตซีนที่จะให้เล่นตอนชนะด่าน
    [Header("Cutscene ตอนอุดครบทุกจุด (Final Message)")] 

    // 🌟 เปลี่ยนจาก GameObject เป็นรับสคริปต์ UniversalCutsceneManager โดยตรง
    [Header("Cutscene หลักของด่านนี้")]
    public UniversalCutsceneManager mainCutscene;
    
    // 🌟 [เพิ่มใหม่] ช่องสำหรับใส่จุดอุดประตู เพื่อให้น้ำหยุดไหล
    [Header("จุดอุดประตูหลัก (ทำให้น้ำหยุดไหล)")]
    public SandbagCheckZone mainDoorZone; 

    [Header("ตั้งค่าแอนิเมชันน้ำทะลัก")]
    public Transform waterOverlay;      
    public Transform doorPosition;      
    public Transform roomCenterPosition; 
    
    [Header("ขนาดของน้ำ (X, Y, Z)")]
    public Vector3 startScale = new Vector3(0.1f, 0.1f, 1f); 
    public Vector3 fullScale = new Vector3(20f, 20f, 1f);
   
    // 🌟 [พิมพ์เพิ่มตรงนี้] กล่องรับสายไฟจากหน้าจอ UI
    [Header("UI แสดงเวลา")]
    public TextMeshProUGUI timerText; 
   
    private float currentTimer;
    private bool isSearching = true;
    private bool hasStarted = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasStarted)
        {
            hasStarted = true;
            currentTimer = searchTimeLimit;
            Debug.Log("น้ำเริ่มทะลักจากประตู! มีเวลา " + searchTimeLimit + " วินาที!");
            // 🌟 [พิมพ์เพิ่มตรงนี้] เปิดให้ตัวเลขแสดงบนจอ
            if (timerText != null) timerText.gameObject.SetActive(true);
            
            if (waterOverlay != null && doorPosition != null)
            {
                waterOverlay.position = doorPosition.position;
                waterOverlay.localScale = startScale;
            }
            
            StartCoroutine(StartFloodLoop());
        }
    }

    IEnumerator StartFloodLoop()
    {
        float elapsedTime = 0f; 
        float currentLerpValue = 0f; // 🌟 ตัวแปรเก็บระดับน้ำปัจจุบัน

        while (currentTimer > 0)
        {
            // 1. เวลานับถอยหลังเสมอ (เพื่อบังคับให้ผู้เล่นต้องหาของให้ทันด้วย)
            currentTimer -= Time.deltaTime;
            // 🌟 [พิมพ์เพิ่มตรงนี้] เอาเวลาที่เหลือไปแปลงเป็น นาที:วินาที แล้วโชว์บนจอ
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(currentTimer / 60);
                int seconds = Mathf.FloorToInt(currentTimer % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

           // 🌟 [พิมพ์เพิ่มใหม่ตรงนี้] 3. ระบบสแกนว่า "อุดครบทุกจุด" หรือยัง?
            bool isAllBlocked = true; // สมมติว่าอุดครบไว้ก่อน
            
            // เช็คประตูหลักก่อนเลย
            if (mainDoorZone != null && !mainDoorZone.isBlocked) isAllBlocked = false;
            
            // เช็คจุดอื่นๆ (หน้าต่าง/รอยรั่วอื่นๆ ถ้ามีใน Array)
            foreach (SandbagCheckZone zone in sandbagZones)
            {
                if (zone != null && !zone.isBlocked) isAllBlocked = false;
            }

            // 🌟 4. ถ้าทุกจุดถูกอุดครบแล้ว -> สั่งชนะทันที!
            // 🌟 4. ถ้าทุกจุดถูกอุดครบแล้ว
            if (isAllBlocked)
            {
                Debug.Log("🎉 อุดรอยรั่วครบแล้ว! ส่งไม้ต่อให้เล่น Final Message");
                
                // 🌟 สั่งไปที่สคริปต์ Cutscene ให้เล่นข้อความหมวด Final
                if (mainCutscene != null)
                {
                    
                    mainCutscene.PlayFinalCutscene("Flood"); 
                }

                // ซ่อนตัวเลขเวลาทิ้งไป
                if (timerText != null) timerText.gameObject.SetActive(false);

                // เบรค! จบการทำงานของระบบน้ำ
                yield break; 
            }

            // 3. 🌟 ถ้ายั้งไม่ได้อุดประตู ให้น้ำไหลเพิ่มขึ้นเรื่อยๆ
            if (!isAllBlocked)
            {
                elapsedTime += Time.deltaTime;
                currentLerpValue = elapsedTime / searchTimeLimit; 
            }
            // (ถ้า isDoorBlocked เป็น true ค่า currentLerpValue จะนิ่ง ทำให้น้ำหยุดขยายตัวทันที!)

            // 4. อัปเดตตำแหน่งและขนาดของน้ำ
            if (waterOverlay != null && doorPosition != null && roomCenterPosition != null)
            {
                waterOverlay.position = Vector3.Lerp(doorPosition.position, roomCenterPosition.position, currentLerpValue);
                waterOverlay.localScale = Vector3.Lerp(startScale, fullScale, currentLerpValue);
            }

            yield return null;
        }

        isSearching = false;
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();
        
        if (inventory != null)
        {
            foreach (string item in requiredItems)
            {
                if (!inventory.HasItem(item))
                {
                    TriggerDeath("เก็บของหนีไม่ทัน!\nคุณจมน้ำเสียชีวิต ");
                    yield break;
                }
            }
        }
        
        yield return new WaitForSeconds(1f); 

        foreach (SandbagCheckZone zone in sandbagZones)
        {
            if (zone != null && !zone.isBlocked)
            {
                TriggerDeath("น้ำทะลักเข้าบ้าน!\nคุณจมน้ำเสียชีวิต ");
                yield break;
            }
        }
    }

    void TriggerDeath(string reason)
    {
        GameManager manager = FindFirstObjectByType<GameManager>();
        if (manager != null) manager.PlayerDied(reason);
    }

    public void ResetFloodStage()
    {
        StopAllCoroutines(); 
        hasStarted = false;  
        

        if (sandbagZones != null)
        {
            foreach (SandbagCheckZone zone in sandbagZones)
            {
                if (zone != null) zone.isBlocked = false;
            }
        }

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("(Clone)")) Destroy(obj); 
        }

        if (waterOverlay != null && doorPosition != null)
        {
            waterOverlay.position = doorPosition.position;
            waterOverlay.localScale = startScale;
        }
    }
}