using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject openingPanel;
    public GameObject selectionPanel;
    // 🌟 ตัวแปร static สำหรับเก็บชื่อด่านที่เลือกเพื่อส่งข้ามซีน
    public static string SelectedTargetLevel;

    // ฟังก์ชันเดิมของคุณ KG (เผื่อมีปุ่มอื่นดึงไปใช้)
    public void LoadSceneOpen()
    {
        SceneManager.LoadScene("PlayMap");
    }

    // 🌟 ฟังก์ชันใหม่สำหรับให้ 4 ปุ่มภัยพิบัติเรียกใช้งาน
    public void SelectLevelAndLoad(string levelName)
    {
        SelectedTargetLevel = levelName; // บันทึกชื่อด่านที่ผู้เล่นกด
        Debug.Log("กำลังบันทึกด่าน: " + levelName + " และกำลังโหลดซีน open");
        SceneManager.LoadScene("PlayMap");   // โหลดเข้าซีน open ด่านรวม
    }

    void Start()
    {
    Debug.Log("SceneUIManager Start รัน");
    openingPanel.SetActive(true);
    selectionPanel.SetActive(false);  // ต้องซ่อนอันนี้ก่อน
}
    public void OnPlayButtonClick()
    {
        Debug.Log("OnPlayButtonClick ถูกเรียก!");
        openingPanel.SetActive(false);
        selectionPanel.SetActive(true);
    }
       

    public void OnBackButtonClick()
    {
        selectionPanel.SetActive(false);
        openingPanel.SetActive(true);
    }
public void LoadQuizScene()
{
    SceneManager.LoadScene("QuizScene"); 
}
}