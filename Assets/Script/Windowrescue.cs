using UnityEngine;
using UnityEngine.Video; 
using System.Collections;
using UnityEngine.SceneManagement;

public class WindowRescue : MonoBehaviour
{
    public GameObject loadingPanel;
    public VideoPlayer videoPlayer;    
    public GameObject windowPanel; 

    [Header("Teleport Settings")]
    public GameObject player; 
    public Transform evacuationPoint; 

    [Header("Cutscene และ Win Screen")]
    // 🌟 เปลี่ยนจาก GameObject เป็น UniversalCutsceneManager ครับ
    public UniversalCutsceneManager acceptHelpCutscene;
    public GameObject winScreen;          // ลาก Win_screen มาใส่

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !windowPanel.activeSelf)
        {
            OpenPanel();
        }
    }

    void OpenPanel()
    {
        windowPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    // 🌟 2. แก้ปุ่ม "รับความช่วยเหลือ" -> ให้แสดงข้อความ Cutscene ก่อน!
    public void OnAcceptHelp()
    {
        windowPanel.SetActive(false);
        Time.timeScale = 1f;
        
        if (acceptHelpCutscene != null)
        {
            // 1. ปลุกกล่องคัตซีนให้ตื่นก่อน
            acceptHelpCutscene.gameObject.SetActive(true); 
            
            // 2. 🌟 เรียกชื่อฟังก์ชันให้ตรงเป๊ะๆ ตามสคริปต์ของคุณ KG
            acceptHelpCutscene.PlayEndingCutscene(); 
        }
        else
        {
            Debug.LogWarning("⚠️ ยังไม่ได้ใส่กล่อง Cutscene ในหน้าต่าง Inspector ครับ!");
        }
    }

    // 🌟 3. [เพิ่มใหม่] ฟังก์ชันนี้เอาไว้ให้ Cutscene เรียกตอน "อ่านข้อความจบ"
    public void StartVideoAndWin()
    {
        StartCoroutine(PlayVideoRoutine());
    }

    IEnumerator PlayVideoRoutine()
    {
        loadingPanel.SetActive(true);
        
        if (videoPlayer != null)
        {
            videoPlayer.Play(); 
            Debug.Log("เริ่มเล่นวิดีโอฉากโหลด...");
            yield return new WaitForSecondsRealtime(2f);
            videoPlayer.Stop();
        }

        if (player != null && evacuationPoint != null)
        {
            player.transform.position = evacuationPoint.position;
            Debug.Log("Adam ถูกย้ายไปที่ศูนย์อพยพแล้ว!");
        }

        Debug.Log("จบวิดีโอ เข้าสู่ฉากจบ");
        loadingPanel.SetActive(false);
        
        // 🌟 4. เรียกหน้าจอ Win Screen ขึ้นมา!
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }

    // ฟังก์ชันสำหรับปุ่ม "ยังไม่รับ"
    public void OnRefuseHelp()
    {
        Debug.Log("Adam เลือกที่จะสู้ต่อไป!");
        ClosePanel(); 

        Debug.Log("💀 ตาย! ปฏิเสธความช่วยเหลือจากเรือ ระดับน้ำท่วมสูงจนจมน้ำตาย!");
        GameManager manager = FindFirstObjectByType<GameManager>();
        if (manager != null) manager.PlayerDied("ตาย! ปฏิเสธความช่วยเหลือจากเรือ ระดับน้ำท่วมสูงจนจมน้ำตาย!");
    }

    public void ClosePanel()
    {
        windowPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}