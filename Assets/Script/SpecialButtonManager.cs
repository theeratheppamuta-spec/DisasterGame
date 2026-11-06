using UnityEngine;

public class SpecialButtonManager : MonoBehaviour
{
    [Header("ลากปุ่ม Special มาใส่ในช่องนี้ เพื่อสั่งซ่อน/โชว์")]
    public GameObject specialButton;

    void Start()
    {
        // ทุกครั้งที่โหลดหน้าเมนู ให้เช็คว่าต้องโชว์ปุ่มไหม
        CheckUnlockCondition();
    }

    // 🌟 1. ฟังก์ชันเช็คว่าเคยกดครบ 4 ด่านหรือยัง
    public void CheckUnlockCondition()
    {
        // ไปดึงสมุดพกมาดูว่า ด่านไหนเคยกดเข้าแล้วบ้าง (ถ้าเคยกดจะเป็นเลข 1)
        int eq = PlayerPrefs.GetInt("Earthquake_Visited", 0);
        int fl = PlayerPrefs.GetInt("Flood_Visited", 0);
        int st = PlayerPrefs.GetInt("Storm_Visited", 0);
        int fi = PlayerPrefs.GetInt("Fire_Visited", 0);

        if (eq == 1 && fl == 1 && st == 1 && fi == 1)
        {
            // ถ้ากดครบ 4 ด่านแล้ว -> โชว์ปุ่ม Special!
            if (specialButton != null) specialButton.SetActive(true);
        }
        else
        {
            // ถ้ายังกดไม่ครบ -> ซ่อนปุ่มไว้ก่อน
            if (specialButton != null) specialButton.SetActive(false);
        }
    }

    // 🌟 2. ฟังก์ชันนี้จะเอาไปผูกกับ "ปุ่มด่านทั้ง 4"
    public void RecordLevelClick(string levelName)
    {
        // บันทึกความจำลงเครื่องว่า "เคยกดเข้าด่านนี้แล้ว"
        PlayerPrefs.SetInt(levelName + "_Visited", 1);
        PlayerPrefs.Save();
        
        Debug.Log($"📌 บันทึกประวัติ: คุณเคยกดเข้าด่าน [{levelName}] แล้ว!");
    }

    // 🌟 3. ปุ่มสูตรโกง: ล้างประวัติ (เอาไว้กดตอนทำเกมเผื่ออยากรีเซ็ต)
    public void ResetVisitData()
    {
        PlayerPrefs.DeleteKey("Earthquake_Visited");
        PlayerPrefs.DeleteKey("Flood_Visited");
        PlayerPrefs.DeleteKey("Storm_Visited");
        PlayerPrefs.DeleteKey("Fire_Visited");
        Debug.Log("🗑️ ล้างประวัติการกดเข้าด่านทั้งหมดแล้ว!");
        CheckUnlockCondition(); 
    }
}