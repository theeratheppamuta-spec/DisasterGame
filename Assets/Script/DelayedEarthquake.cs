using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class DelayedEarthquake : MonoBehaviour
{
    public CinemachineCamera vCam;
    public float delayBeforeShake = 10f; 
    public float shakeDuration = 10f;    
    public float shakeIntensity = 3f;    

    private CinemachineBasicMultiChannelPerlin noise;
    private bool hasTriggered = false; // 🌟 เพิ่มตัวแปรเช็คว่าเคยสั่นไปหรือยัง จะได้ไม่สั่นซ้ำซ้อน

    void Start()
    {
        // ตอนเริ่มเกม แค่ดึงระบบกล้องมาเตรียมไว้เฉยๆ (ยังไม่ต้องสั่งสั่น)
        if (vCam != null)
        {
            noise = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    // 🌟 ฟังก์ชันนี้จะทำงานก็ต่อเมื่อตัวละครเดินมาชนกล่อง Checkpoint เท่านั้น!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // ล็อกกุญแจทันที ป้องกันเดินเหยียบซ้ำ
            
            if (noise != null)
            {
                // สั่งเริ่มนับถอยหลังและสั่นกล้อง
                StartCoroutine(EarthquakeRoutine());
            }
            else
            {
                Debug.LogError("หาระบบสั่น (Noise) ในกล้องไม่เจอครับ! อย่าลืมไปเพิ่มใน Inspector นะ");
            }
        }
    }

    IEnumerator EarthquakeRoutine()
    {
        // 1. รอ 10 วินาทีแรก
        Debug.Log("เหยียบ Checkpoint แล้ว! รอ " + delayBeforeShake + " วินาทีก่อนสั่น...");
        yield return new WaitForSeconds(delayBeforeShake);

        Debug.Log("แผ่นดินไหวเริ่มแล้ว!");

        // 2. สั่งสั่น
        noise.AmplitudeGain = shakeIntensity;
        noise.FrequencyGain = shakeIntensity;

        // 3. สั่นค้างไว้ 10 วินาที
        yield return new WaitForSeconds(shakeDuration);

        // 4. หยุดสั่น
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;

        Debug.Log("แผ่นดินไหวสงบลง");
    }
}