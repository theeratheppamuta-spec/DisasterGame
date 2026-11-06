using UnityEngine;
using TMPro; 
using System.Collections; 
using UnityEngine.Events; 

public class UniversalCutsceneManager : MonoBehaviour
{
    [Header("--- ตั้งค่าด่านที่ดูแล (ต้องตรงกับตอนเลือกด่าน) ---")]
    public string chapterID; // เช่น ใส่ว่า "Earthquake" หรือ "Fire"
    [Header("--- ตั้งค่าฉากเปิด (Intro) ---")]
    public string expectedLevelName = "LevelName"; 
    public string chapterTextToShow = "บทที่ 1";
    [TextArea(3, 5)] public string[] introMessages; 
    public UnityEvent onIntroEnd; 

    [Header("--- ตั้งค่าฉากจบในห้อง (Ending) ---")]
    [TextArea(3, 5)] public string[] endingMessages; 
    public UnityEvent onEndingEnd; 

    // 🌟 ส่วนที่เพิ่มเข้ามาใหม่: สำหรับฉากพูดทิ้งท้ายเมื่อถึงหน้าตึกสำเร็จ
    [Header("--- ตั้งค่าฉากสำเร็จด่านที่หน้าตึก (Final Clear) ---")]
    [TextArea(3, 5)] public string[] finalMessages; 
    public UnityEvent onFinalEnd; // เอาไว้สั่งขึ้นหน้าจอ Victory UI หรือโหลดกลับหน้าเมนูหลัก

    [Header("UI หน้าจอ (ใช้ร่วมกัน)")]
    public GameObject chapterPanel;
    public TextMeshProUGUI chapterText; 
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    void Start()
    {
        Time.timeScale = 1f;
        if (SceneUIManager.SelectedTargetLevel == expectedLevelName)
        {
            StartCoroutine(PlayChapterTitleOnly());
        }
    }

    IEnumerator PlayChapterTitleOnly()
    {
        if(chapterText != null) chapterText.text = chapterTextToShow;
        chapterPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        chapterPanel.SetActive(false);
    }

    public void PlayIntroCutscene()
    {
        StartCoroutine(PlayIntroDialogue());
    }

    IEnumerator PlayIntroDialogue()
    {
        yield return StartCoroutine(DisplayDialogRoutine(introMessages));
        onIntroEnd.Invoke(); 
    }

    public void PlayEndingCutscene()
    {
        StartCoroutine(PlayEnding());
    }

    IEnumerator PlayEnding()
    {
        yield return StartCoroutine(DisplayDialogRoutine(endingMessages));
        onEndingEnd.Invoke(); 
    }

    // 🌟 ฟังก์ชันใหม่: สำหรับให้สคริปต์ที่หน้าตึกโทรมาสั่งรันตอนผู้เล่นวิ่งมาถึง
     public void PlayFinalCutscene(string levelName)
    {
        // ถ้าชื่อด่านที่ส่งมา ไม่ตรงกับที่ตั้งไว้ในตัวนี้ ให้หยุดทำงานทันที!
        if (levelName != chapterID) return;

        StartCoroutine(PlayFinal());
    }


    IEnumerator PlayFinal()
    {
        yield return StartCoroutine(DisplayDialogRoutine(finalMessages));
        onFinalEnd.Invoke(); 
    }
   
    IEnumerator DisplayDialogRoutine(string[] messages)
    {
        if (messages != null && messages.Length > 0)
        {
            dialogPanel.SetActive(true);
            for (int i = 0; i < messages.Length; i++)
            {
                dialogText.text = messages[i];
                yield return null; 
                while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0))
                {
                    yield return null;
                }
            }
        }
        dialogPanel.SetActive(false);
    }

    // 🌟 เพิ่มฟังก์ชันนี้สำหรับให้ระบบด่านต่างๆ สั่งลบความจำได้
    public void ResetCutsceneState()
    {
        Debug.Log("🔄 ล้างความจำ Cutscene Manager เรียบร้อย พร้อมคุยใหม่!");
    }
}