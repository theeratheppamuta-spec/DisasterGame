using UnityEngine;
using System.Collections; // 🌟 อย่าลืมบรรทัดนี้ ต้องใช้สำหรับระบบหน่วงเวลา (Coroutine)

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim; 

    // ==========================================
    // 🌟 [เพิ่มใหม่] ตัวแปรสำหรับระบบสตันท์
    // ==========================================
    [Header("ระบบสตันท์")]
    public bool isStunned = false;
    private SpriteRenderer sr; // เอาไว้สั่งเปลี่ยนสีตัวละครตอนโดนชน

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        sr = GetComponent<SpriteRenderer>(); // ดึงตัวจัดการภาพมาเตรียมไว้
    }

    void Update() {
        // 🌟 ดักไว้ตรงนี้! ถ้าติดสตันท์อยู่ ให้หยุดรับค่าปุ่มกด
        if (isStunned) 
        {
            // บังคับให้ความเร็วเป็น 0 เพื่อให้แอนิเมชันกลับไปท่ายืนนิ่งๆ (Idle) ทันที
            anim.SetFloat("Speed", 0);
            movement = Vector2.zero; 
            return; // 🌟 คำสั่งนี้จะเตะออกจากฟังก์ชัน Update ข้ามการเดินด้านล่างไปเลย
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate() {
        // 🌟 ต้องหยุดการคำนวณฟิสิกส์ด้วยตอนติดสตันท์ ไม่งั้นตัวละครจะยังไถลได้
        if (isStunned) return; 

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // ==========================================
    // 🌟 [เพิ่มใหม่] ฟังก์ชันสั่งสตันท์ (ย้ายมาจาก PlayerStun)
    // ==========================================
    public void StunPlayer(float stunDuration)
    {
        if (!isStunned) 
        {
            StartCoroutine(StunRoutine(stunDuration));
        }
    }

    IEnumerator StunRoutine(float time)
    {
        isStunned = true;
        if (sr != null) sr.color = Color.red; // ตัวแดงแว้บนึงให้รู้ว่าเจ็บ

        yield return new WaitForSeconds(time); // รอมึนงงตามเวลาที่กำหนด

        isStunned = false;
        if (sr != null) sr.color = Color.white; // กลับมาสีปกติ
    }
}