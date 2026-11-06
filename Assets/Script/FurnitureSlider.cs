using UnityEngine;
using System.Collections;

public class FurnitureSlider : MonoBehaviour
{
    // 🌟 [ระบบใหม่] ล็อคการทำงานด้วยชื่อด่าน
    [Header("ตั้งค่าด่านที่ให้ทำงาน")]
    public string expectedStageName = "Storm"; // พิมพ์ชื่อด่านที่ต้องการให้มันทำงาน (เช่น Storm)

    [Header("ตั้งค่าจุดไถล")]
    public Transform pointA; 
    public Transform pointB; 
    
    [Header("ตั้งค่าความเร็วลม")]
    public float slideSpeed = 5f; 
    public float waitTime = 1f;   

    [Header("ล็อคขอบหน้าจอ (กันทะลุฉาก)")]
    public bool keepInScreen = true; 
    public float paddingX = 1.5f;    

    private Vector3 targetPos;
    private bool isWaiting = false;

    void Start()
    {
        if (pointB != null) targetPos = pointB.position;
    }

    void Update()
    {
        // 🌟 1. เช็คชื่อด่าน! ถ้าผู้เล่นไม่ได้อยู่ด่านนี้ ให้หยุดทำงาน (ต้นไม้จะนิ่ง และไม่วาร์ปข้ามด่าน)
        if (SceneUIManager.SelectedTargetLevel != expectedStageName) return;

        // 2. ถ้ากำลังรอลมเปลี่ยนทิศ หรือไม่ได้ตั้งจุดหมายไว้ ก็ไม่ต้องทำอะไร
        if (isWaiting || pointA == null || pointB == null) return;

        // 3. เริ่มไถล!
        transform.position = Vector3.MoveTowards(transform.position, targetPos, slideSpeed * Time.deltaTime);
        bool hitScreenEdge = false;

        // 4. ระบบล็อคขอบจอ (ทำงานแค่ในด่านของตัวเองเท่านั้น)
        if (keepInScreen && Camera.main != null)
        {
            Vector3 pos = transform.position;
            Vector3 screenMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 screenMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

            if (pos.x <= screenMin.x + paddingX || pos.x >= screenMax.x - paddingX)
            {
                pos.x = Mathf.Clamp(pos.x, screenMin.x + paddingX, screenMax.x - paddingX);
                transform.position = pos;
                hitScreenEdge = true; 
            }
        }

        if (Vector3.Distance(transform.position, targetPos) < 0.1f || hitScreenEdge)
        {
            StartCoroutine(WaitAndSwitchDirection());
        }
    }

    IEnumerator WaitAndSwitchDirection()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime); 
        targetPos = (targetPos == pointA.position) ? pointB.position : pointA.position;
        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null) player.StunPlayer(1.5f); 
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (!isWaiting) StartCoroutine(WaitAndSwitchDirection());
        }
    }
}