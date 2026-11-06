using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [Header("ชื่อด่านที่ Manager ตัวนี้ดูแล")]
    public string myLevelName; // ใส่ว่า "Earthquake" หรือ "Fire"
    [Header("UI แสดงข้อความภารกิจ")]
    public TextMeshProUGUI objectiveText;

    [Header("รายการภารกิจเรียงตามลำดับขั้น")]
    public string[] objectiveList;

    private int currentStep = 0;

    void Start()
   {
    // 🌟 ถ้าชื่อด่านไม่ตรงกับที่ตั้งไว้ ก็ให้หยุดทำงานทันที ไม่ต้องแสดงข้อความ
    if (SceneUIManager.SelectedTargetLevel != myLevelName)
    {
        this.enabled = false; 
        return;
    }
    UpdateObjectiveUI();
   }

    // 🌟 ฟังก์ชันสำหรับสั่งให้เปลี่ยนไปภารกิจขั้นถัดไป
    public void NextObjective()
    {
        currentStep++;
        UpdateObjectiveUI();
    }

    // ฟังก์ชันภายในสำหรับอัปเดตตัวหนังสือบนหน้าจอ
    private void UpdateObjectiveUI()
    {
        if (objectiveList != null && currentStep < objectiveList.Length)
        {
            objectiveText.text = "เป้าหมาย: " + objectiveList[currentStep];
        }
        else
        {
            objectiveText.text = "ภารกิจทั้งหมดสำเร็จแล้ว!";
        }
    }

    public void ResetObjective()
  {
    currentStep = 0; // ถอยกลับไปภารกิจแรกสุด
    UpdateObjectiveUI(); // อัปเดตตัวหนังสือบนจอให้กลับเป็น "สำรวจทีวี..."
  }  
}