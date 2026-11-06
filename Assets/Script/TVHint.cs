using UnityEngine;

public class TVHint : MonoBehaviour
{
    [Header("UI และเครื่องหมาย")]
    public GameObject exclamationMark; // เครื่องหมาย ! (เด้งดึ๋งบนทีวี)

    [Header("ระบบ Cutscene")]
    public UniversalCutsceneManager cutsceneManager; // กล่องคัตซีนด่านพายุ

    private bool isPlayerNearby = false; 

    void Start()
    {
        // เริ่มเกมมาให้เปิดเครื่องหมาย ! ไว้ตลอดเวลา
        if (exclamationMark != null) exclamationMark.SetActive(true);
    }

    void Update()
    {
        // โค้ดขยับเครื่องหมาย ! ขึ้นลง
        if (exclamationMark != null && exclamationMark.activeSelf)
        {
            float newY = 0.14f + Mathf.Sin(Time.time * 3f) * 0.1f;
            exclamationMark.transform.localPosition = new Vector3(0, newY, 0);
        }

        // 🌟 ดักจับการกดปุ่ม E เพื่อเล่น Cutscene เมื่ออยู่ใกล้ทีวี
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (cutsceneManager != null)
            {
                // สั่งเล่น Ending Message ของด่านพายุ
                cutsceneManager.PlayEndingCutscene(); 
                
                Debug.Log("📺 TV: เล่น Cutscene ด่านพายุแล้ว!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // บอกว่าผู้เล่นเข้ามาใกล้แล้ว

            // ซ่อนเครื่องหมาย ! เมื่อเดินมาใกล้
            if (exclamationMark != null) exclamationMark.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // บอกว่าผู้เล่นเดินออกไปแล้ว

            // เปิดเครื่องหมาย ! กลับมาเมื่อเดินห่างออกไป
            if (exclamationMark != null) exclamationMark.SetActive(true);
        }
    }
}