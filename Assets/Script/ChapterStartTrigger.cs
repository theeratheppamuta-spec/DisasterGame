using UnityEngine;

public class ChapterStartTrigger : MonoBehaviour
{
    [Header("ลาก Cutscene Manager ของบทนี้มาใส่ที่นี่")]
    public UniversalCutsceneManager cutsceneManagerToWakeUp;

    // 🌟 ตัวแปรใหม่ เอาไว้จำว่าเคยเหยียบไปแล้วหรือยัง
    private bool hasTriggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 🌟 เช็คว่าเป็น Player และยังไม่เคยเหยียบใช่ไหม?
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // ล็อคไว้ไม่ให้ทำงานซ้ำรัวๆ เวลาเดินผ่าน

            if (cutsceneManagerToWakeUp != null)
            {
                cutsceneManagerToWakeUp.gameObject.SetActive(true);
                cutsceneManagerToWakeUp.PlayIntroCutscene();
            }
        }
    }

    // 🌟 ฟังก์ชันใหม่: เอาไว้ให้ระบบอื่น (เช่นด่านไฟไหม้) สั่งล้างความจำตอนเกิดใหม่
    public void ResetTrigger()
    {
        hasTriggered = false; // ปลดล็อคให้เหยียบใหม่ได้
        Debug.Log("🔄 รีเซ็ตจุดเหยียบแล้ว พร้อมให้เดินมาเหยียบเริ่มด่านใหม่!");
    }
}