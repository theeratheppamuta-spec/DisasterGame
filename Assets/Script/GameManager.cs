using UnityEngine;
using Unity.Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;         
    public GameObject deathCanvas;    
    public GameObject dialogManager;  
    public GameObject winScreen;

    [Header("Death Screen UI")]
    public TextMeshProUGUI deathReasonText;

    [Header("Cameras for each level")]
    public GameObject camEarthquake;
    public GameObject camFlood;
    public GameObject camStorm;
    public GameObject camFire;

    public GameObject[] allItemsInScene;

    // 🌟 [ระบบใหม่] ใส่จุดเกิดของด่านย่อยทั้งหมดเรียงตามลำดับ 
    // (Element 0 = จุดเริ่มเกมแรกสุด, Element 1 = ชั้น 2, Element 2 = ชั้น 3, ...)
    [Header("Sub-Level Spawns (เรียงลำดับด่านย่อย)")]
    public Transform[] earthquakeSpawns;
    public Transform[] floodSpawns;
    public Transform[] stormSpawns;
    public Transform[] fireSpawns;

    [Header("Warp Points to Reset")]
    public GameObject elevatorWarp;

    public GameObject[] cutscenesToReset;
    
    [Header("ระบบภารกิจของแต่ละด่าน")]
    public ObjectiveManager[] allObjectiveManagers; // ลาก ObjectiveManager ทุกด่านมาใส่ช่องนี้

    // ตัวแปรจำว่าตอนนี้กำลังเล่นด่านย่อยที่เท่าไหร่ (เริ่มที่ 0 เสมอ)
    private int currentSubLevelIndex = 0;

    private void Start()
    {
        string target = SceneUIManager.SelectedTargetLevel;
        if (!string.IsNullOrEmpty(target))
        {
            WarpPlayerToSelectedLevel(target);
        }
    }

    private void WarpPlayerToSelectedLevel(string levelName)
    {
        // รีเซ็ตด่านย่อยกลับไปนับศูนย์ใหม่ทุกครั้งที่เริ่มด่านหลักใหม่
        currentSubLevelIndex = 0; 
        
        if (camEarthquake != null) camEarthquake.SetActive(false);
        if (camFlood != null) camFlood.SetActive(false);
        if (camStorm != null) camStorm.SetActive(false);
        if (camFire != null) camFire.SetActive(false);

        // ดึงจุดเกิดจุดแรก (Index 0) มาใช้วาร์ปตอนเริ่มเกม
        if (levelName == "Earthquake")
        {
            if (earthquakeSpawns.Length > 0) player.transform.position = earthquakeSpawns[0].position;
            if (camEarthquake != null) camEarthquake.SetActive(true); 
        }
        else if (levelName == "Flood")
        {
            if (floodSpawns.Length > 0) player.transform.position = floodSpawns[0].position;
            if (camFlood != null) camFlood.SetActive(true); 
        }
        else if (levelName == "Storm")
        {
            if (stormSpawns.Length > 0) player.transform.position = stormSpawns[0].position;
            if (camStorm != null) camStorm.SetActive(true); 
        }
        else if (levelName == "Fire")
        {
            if (fireSpawns.Length > 0) player.transform.position = fireSpawns[0].position;
            if (camFire != null) camFire.SetActive(true); 
        }
    }

    public void PlayerDied(string reason = "คุณเสียชีวิต!")
    {
        if (deathReasonText != null) deathReasonText.text = reason; 
        deathCanvas.SetActive(true);   
        player.SetActive(false);       
        if(dialogManager != null) dialogManager.SetActive(false); 

        UniversalCutsceneManager cutscene = FindFirstObjectByType<UniversalCutsceneManager>();
        if (cutscene != null) cutscene.StopAllCoroutines();
    }

    public void RetryGame()
    {
        // 1. ให้เวลาในเกมกลับมาเดินปกติ (เผื่อโดนหยุดไว้ตอนตาย)
        Time.timeScale = 1f; 

        // 2. สั่งโหลดหน้าจอ Scene นี้ใหม่ตั้งแต่ต้น! ล้างไพ่ใหม่หมด
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void BackToMainMenu()
    {
        SceneUIManager.SelectedTargetLevel = ""; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene Selection"); 
    }

    // 🌟 [ฟังก์ชันครอบจักรวาล] สำหรับผูกกับปุ่ม NEXT
    public void GoToNextSubLevel()
    {
        if (winScreen != null) winScreen.SetActive(false);

        string mainLevel = SceneUIManager.SelectedTargetLevel;
        Transform nextSpawn = null;

        // เช็คว่าอยู่ด่านไหน และมีจุดเกิดด่านย่อยถัดไปรองรับไว้ไหม
        if (mainLevel == "Earthquake" && currentSubLevelIndex + 1 < earthquakeSpawns.Length)
        {
            currentSubLevelIndex++;
            nextSpawn = earthquakeSpawns[currentSubLevelIndex];
        }
        else if (mainLevel == "Flood" && currentSubLevelIndex + 1 < floodSpawns.Length)
        {
            currentSubLevelIndex++;
            nextSpawn = floodSpawns[currentSubLevelIndex];
        }
        else if (mainLevel == "Storm" && currentSubLevelIndex + 1 < stormSpawns.Length)
        {
            currentSubLevelIndex++;
            nextSpawn = stormSpawns[currentSubLevelIndex];
        }
        else if (mainLevel == "Fire" && currentSubLevelIndex + 1 < fireSpawns.Length)
        {
            currentSubLevelIndex++;
            nextSpawn = fireSpawns[currentSubLevelIndex];
        }

        // ถ้ายืนยันว่ามีด่านย่อยให้ลุยต่อ -> วาร์ปไปเลย!
        if (nextSpawn != null)
        {
            player.transform.position = nextSpawn.position;
            
            // 💡 ถ้าต้องการเคลียร์ของในกระเป๋าทุกครั้งที่ขึ้นด่านย่อยใหม่ ให้เอาคอมเมนต์สองบรรทัดล่างออกครับ
             PlayerInventory inventory = player.GetComponent<PlayerInventory>();
             if (inventory != null) inventory.ResetInventory();
        }
        else
        {
            // ถ้าด่านย่อยหมดแล้ว (ชนะของจริง) -> กลับหน้าเมนู
            BackToMainMenu();
        }
    }
}