using UnityEngine;
using System.Collections;

public class FirePoint : MonoBehaviour
{
    [Header("ใส่ Prefab น้ำที่สร้างจาก Sprite Sheet (กรอบสีฟ้า)")]
    public GameObject waterSprayPrefab; 

    private bool isPlayerNearby = false;
    private bool isExtinguished = false;

    // เก็บอ้างอิงถึงตัว Player เอาไว้คำนวณทิศทางหันหน้าน้ำ
    private GameObject playerRef;

    void Update()
    {
        // ยืนใกล้กองไฟที่ยังไม่ดับ และกด E
        if (isPlayerNearby && !isExtinguished && Input.GetKeyDown(KeyCode.E))
        {
            TryExtinguishFire();
        }
    }

    void TryExtinguishFire()
    {
        PlayerInventory inv = FindFirstObjectByType<PlayerInventory>();
        FireManager manager = FindFirstObjectByType<FireManager>();

        // เช็คว่าระบบครบถ้วนไหม
        if (inv == null || manager == null || playerRef == null) return;

        // เช็คว่าในกระเป๋ามีถังดับเพลิงไหม (ดึงชื่อไอเทมจาก FireManager)
        if (inv.HasItem(manager.extinguisherName))
        {
            isExtinguished = true; // ล็อกไว้ไม่ให้กดซ้ำได้

            // ===============================================
            // 🌟 ระบบเสกน้ำและหมุนทิศทางไปหากองไฟ
            // ===============================================
            if (waterSprayPrefab != null)
            {
                // 1. คำนวณทิศทาง: จาก Player ไปหากองไฟ
                Vector3 direction = transform.position - playerRef.transform.position;
                
                // 2. คำนวณมุมหมุน: แปลงทิศทางเป็นองศา (แกน Z สำหรับเกม 2D)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                // 3. เสกน้ำขึ้นมาที่ตำแหน่งผู้เล่น + ให้ยื่นไปข้างหน้านิดนึง (Offset)
                Vector3 spawnOffset = direction.normalized * 0.2f; 
                Vector3 spawnPos = playerRef.transform.position + spawnOffset;
                
                GameObject currentWater = Instantiate(waterSprayPrefab, spawnPos, rotation);

                // 4. สั่งทำลายวัตถุน้ำทิ้ง หลังจากฉีดเสร็จ (0.5 วินาที)
                Destroy(currentWater, 0.5f); 
            }

            // สั่งเริ่มนับเวลาหน่วงให้ไฟดับ (เพื่อรอดูแอนิเมชั่นน้ำฉีด)
            StartCoroutine(PerformExtinguishSequence(manager));
        }
        else
        {
            Debug.Log("❌ ไม่มีถังดับเพลิง! เข้าใกล้ไม่ได้ ร้อน!");
        }
    }

    // ลำดับการดับไฟ (หน่วงเวลา)
    IEnumerator PerformExtinguishSequence(FireManager manager)
    {
        Debug.Log("💦 กำลังฉีดน้ำ...");
        
        // รอสักพักให้แอนิเมชั่นน้ำฉีดเสร็จก่อน 
        yield return new WaitForSeconds(0.5f); 

        Debug.Log("💦 ไฟดับแล้ว!");
        manager.ReportFireExtinguished(); // ตะโกนบอกหัวหน้า (FireManager)
        gameObject.SetActive(false); // ซ่อนรูปกองไฟทิ้งไป
    }

    // 🌟 เอาไว้ใช้ตอนกดปุ่ม Retry เกิดใหม่ (FireManager จะมาเรียกใช้)
    public void ResetFirePoint()
    {
        isExtinguished = false;
        isPlayerNearby = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // ถ้าเป็น Player เดินมาชน ให้เก็บค่า playerRef ไว้ใช้หมุนทิศทางน้ำ
        if (col.CompareTag("Player") && !isExtinguished)
        {
            isPlayerNearby = true;
            playerRef = col.gameObject; 
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // ถ้า Player เดินออกไป
        if (col.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}