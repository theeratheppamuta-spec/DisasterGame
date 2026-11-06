using UnityEngine;

public class BuildingExitTrigger : MonoBehaviour
{
    [Header("ลาก UniversalCutsceneManager ที่ต้องการใช้งานมาใส่")]
    public UniversalCutsceneManager cutsceneManager;

    [Header("ใส่ชื่อด่านให้ตรงกับ Chapter ID ใน Manager นั้นๆ")]
    public string levelNameTag = "EarthQuake"; // พิมพ์ค่าที่ต้องการใน Inspector ได้เลย

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
            {
                // 🌟 ส่งค่า levelNameTag เข้าไปให้ Manager ตรวจสอบ
                // Manager จะเช็คเองว่าชื่อตรงกับ chapterID ไหม ถ้าตรงถึงจะรันคัตซีน
                cutsceneManager.PlayFinalCutscene(levelNameTag);
                
                // ปิด Collider เพื่อไม่ให้ทำงานซ้ำ
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}