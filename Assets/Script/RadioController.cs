using UnityEngine;

public class RadioController : MonoBehaviour
{
    [Header("ใส่หน้าต่าง UI ทั้ง 2 หน้า")]
    public GameObject choicePanel;  
    public GameObject messagePanel; 

    [Header("ใส่จุดวาร์ปที่ต้องการให้โผล่มาตอนฟังจบ")]
    public GameObject warpPoint; // 🌟 เพิ่มตัวแปรนี้เข้ามาครับ!

    // 🌟 เพิ่มฟังก์ชัน Start() บรรทัดนี้เข้าไปครับ
    void Start()
    {
        // 1. สั่งปิดจุดวาร์ปทันทีที่เริ่มเกม ผู้เล่นจะได้เผลอไปเหยียบไม่ได้
        if (warpPoint != null)
        {
            warpPoint.SetActive(false); 
        }
    }

    public void TriggerAutoBroadcast()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    public void OnChooseListen()
    {
        choicePanel.SetActive(false); 
        if (messagePanel != null) messagePanel.SetActive(true); 
    }

    public void OnChooseIgnore()
    {
        choicePanel.SetActive(false);
        Time.timeScale = 1f; 
        
        GameManager manager = FindFirstObjectByType<GameManager>();
        if (manager != null) manager.PlayerDied("เดินออกไปก่อนฟังประกาศ ตาย!!");
    }

    public void OnAcknowledgeMessage()
    {
        messagePanel.SetActive(false);
        Time.timeScale = 1f; 
        
        StormSystem.isRadioSafe = true; 

        // 🌟 สั่งเปิดใช้งานจุดวาร์ปให้ผู้เล่นเดินไปเหยียบได้!
        if (warpPoint != null)
        {
            warpPoint.SetActive(true);
            Debug.Log("✨ จุดวาร์ปกลับขึ้นข้างบนเปิดใช้งานแล้ว!");
        }
    }
}