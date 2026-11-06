using UnityEngine;

public class ElectricBreaker : MonoBehaviour
{
    public GameObject breakerPanel; // ลาก BreakerPanel มาใส่ในช่องนี้
    private bool isPlayerNearby = false;
    private bool isPowerOff = false;

    void Update()
    {
        // ถ้า Adam อยู่ใกล้ตู้ และกดปุ่ม E (หรือปุ่มที่คุณตั้งไว้)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !breakerPanel.activeSelf)
        {
            OpenPanel();
        }
    }

    void OpenPanel()
    {
        breakerPanel.SetActive(true);
        Time.timeScale = 0f; // หยุดเกมชั่วคราวขณะเลือก (ถ้าต้องการ)
        // ถ้าคุณมีสคริปต์ควบคุมตัวละคร อย่าลืมสั่งปิดการเคลื่อนที่ของ Adam ด้วยนะครับ
    }

    // ฟังก์ชันสำหรับปุ่ม "ตัดไฟ"
    public void OnCutPower()
    {
        isPowerOff = true;
        Debug.Log("ตัดไฟเรียบร้อยแล้ว! ปลอดภัยจากไฟดูด");
        ClosePanel();
        // เพิ่ม Logic อื่นๆ เช่น ปิดไฟในฉาก หรือเปลี่ยนสีตู้ไฟ
    }

    // ฟังก์ชันสำหรับปุ่ม "ไม่ตัดไฟ"
    // ฟังก์ชันสำหรับปุ่ม "ไม่ตัดไฟ"
    public void OnIgnore()
    {
        Debug.Log("คุณไม่ตัดไฟ ระวังอันตรายเมื่อน้ำท่วม!");
        ClosePanel(); // คืนค่าเวลาให้กลับมาเดินปกติก่อน (สำคัญมาก)

        // 🌟 เพิ่มคำสั่งตายตรงนี้! 🌟
        Debug.Log("💀 ตาย! ปฏิเสธการตัดไฟ ทำให้น้ำท่วมถึงปลั๊กและโดนไฟดูด!");
        GameManager manager = FindFirstObjectByType<GameManager>();
        if (manager != null) manager.PlayerDied("\nท่านเสียชีวิตเนื่องจากไม่ตัดไฟ");
    }
    public void ClosePanel()
    {
        breakerPanel.SetActive(false);
        Time.timeScale = 1f; // ให้เกมเดินต่อ
    }

    // เช็กว่า Adam เดินมาหน้าตู้หรือยัง
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}