using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class StormTimer : MonoBehaviour
{
    [Header("UI ตัวหนังสือบอกเวลา")]
    public TextMeshProUGUI timerText;

    [Header("คำสั่งที่จะให้ทำงานเมื่อเวลาหมด (เช่น สั่งให้ตาย)")]
    public UnityEvent onTimeUp;

    private float timeLeft;
    private bool isCounting = false;
    private string currentPrefix = "";

    void Update()
    {
        if (isCounting)
        {
            timeLeft -= Time.deltaTime;
            
            if (timeLeft > 0)
            {
                // อัปเดตตัวเลขบนหน้าจอ
                int seconds = Mathf.CeilToInt(timeLeft);
                timerText.text = currentPrefix + seconds.ToString() + " วินาที";
            }
            else
            {
                // หมดเวลา!
                isCounting = false;
                timerText.text = "หมดเวลา!";
                onTimeUp.Invoke(); // 🌟 สั่งรัน Event ที่ตั้งไว้ (เช่น สั่งตาย)
            }
        }
    }

    // 🟢 ฟังก์ชันสำหรับสั่ง "เริ่ม" จับเวลา
    // (เวลาเรียกใช้ใน Inspector ให้กรอกเวลาในช่องแรก และข้อความในช่องสอง)
    public void StartTimer(float timeInSeconds, string prefixMessage)
    {
        timeLeft = timeInSeconds;
        currentPrefix = prefixMessage;
        isCounting = true;
        timerText.gameObject.SetActive(true);
    }

    // 🔴 ฟังก์ชันสำหรับสั่ง "หยุด" จับเวลา (เช่น ทำภารกิจเสร็จก่อนเวลาหมด)
    public void StopTimer()
    {
        isCounting = false;
        timerText.gameObject.SetActive(false);
    }
}