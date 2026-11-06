using UnityEngine;
using System.Collections; // 🌟 ต้องมีบรรทัดนี้ด้วยสำหรับการทำแอนิเมชัน (Coroutine)

public class TimerZone : MonoBehaviour
{
    [Header("ตั้งค่าหน้าที่ของกล่องนี้")]
    public bool isStartZone = true; 
    public float timeLimit = 30f;   

    // ==========================================
    // 🌊 [เพิ่มใหม่] ตั้งค่าเอฟเฟกต์คลื่นสึนามิ (ใส่เฉพาะกล่อง Start Zone)
    // ==========================================
    [Header("เอฟเฟกต์คลื่นสึนามิ (เฉพาะจุดเริ่ม)")]
    public Transform giantTsunamiWave;  // แผ่นรูปน้ำตกยักษ์
    public Transform waveStartPos;      // จุดที่คลื่นซุ่มรอ (นอกจอ)
    public Transform waveEndPos;        // จุดที่คลื่นซัดเข้ามาทับ (กลางจอ)
    public float waveCrashSpeed = 3f;   // ความเร็วคลื่นซัด

    // ตัวแปรส่วนกลางสำหรับนับเวลา
    public static float currentTime;
    public static bool isTimerRunning = false;
    
    // 🌟 ตัวแปรใหม่กันบั๊ก เพื่อไม่ให้โค้ดตายมันเรียกซ้ำรัวๆ ตอนเวลาติดลบ
    private bool isDead = false; 

    private void Start()
    {
        // 🌟 ตอนเริ่มเกม สั่งให้คลื่นไปแอบรอนอกจอก่อนเลย
        if (isStartZone && giantTsunamiWave != null && waveStartPos != null)
        {
            giantTsunamiWave.position = waveStartPos.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เพิ่มเงื่อนไข !isDead เพื่อไม่ให้รับค่าใหม่ถ้าตายไปแล้ว
        if (collision.CompareTag("Player") && !isDead) 
        {
            if (isStartZone)
            {
                if (!isTimerRunning) 
                {
                    currentTime = timeLimit;
                    isTimerRunning = true;
                    Debug.Log("🌊 สึนามิกำลังมา! มีเวลา " + timeLimit + " วินาที หนีขึ้นดาดฟ้าด่วน!");
                }
            }
            else
            {
                isTimerRunning = false;
                Debug.Log("🎉 ถึงดาดฟ้าแล้ว! รอดตายจากสึนามิอย่างหวุดหวิด!");
            }
        }
    }

    private void Update()
    {
        // เพิ่ม !isDead เข้าไปเช็คด้วย
        if (isStartZone && isTimerRunning && SceneUIManager.SelectedTargetLevel == "Earthquake" && !isDead)
        {
            currentTime -= Time.deltaTime;
            
            if (currentTime <= 0)
            {
                isTimerRunning = false;
                isDead = true; // ล็อกไว้เลย กันโค้ดรันซ้ำ
                Debug.Log("หมดเวลา! น้ำท่วมมิด หนีไม่ทัน -> เรียกคลื่นยักษ์!");
                
                // 🌟 เปลี่ยนจากการตายทันที เป็นเรียกแอนิเมชันคลื่นซัดแทน
                StartCoroutine(TsunamiDeathSequence());
            }
        }
    }

    // ==========================================
    // 🌟 โค้ดแอนิเมชันคลื่นซัด (เพิ่มใหม่)
    // ==========================================
    IEnumerator TsunamiDeathSequence()
    {
        // 1. แอนิเมชันคลื่นพุ่งเข้ามา
        if (giantTsunamiWave != null && waveStartPos != null && waveEndPos != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * waveCrashSpeed;
                giantTsunamiWave.position = Vector3.Lerp(waveStartPos.position, waveEndPos.position, t);
                yield return null;
            }
            
            // รอให้คลื่นทับจอสักแป๊บ (0.5 วินาที) ให้ตกใจเล่น
            yield return new WaitForSeconds(0.5f);
        }

        // 2. ค่อยเรียกหน้าต่าง Game Over
        FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0].PlayerDied("หมดเวลา หนีไม่ทัน!\nคลื่นสึนามิซัดคุณเสียชีวิต");
    }
}