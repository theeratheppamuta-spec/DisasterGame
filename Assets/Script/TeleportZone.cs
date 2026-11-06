using UnityEngine;
using System.Collections; // 🌟 อย่าลืมบรรทัดนี้นะครับ

public class TeleportZone : MonoBehaviour
{
    [Header("จุดหมายที่จะวาร์ปไป")]
    public Transform targetDestination;
    
    [Header("ระบบจัดการภารกิจ")]
    public ObjectiveManager objectiveManager;

    [Header("🌟 ลากกล้อง Cinemachine ของด่านนี้มาใส่ (เช่น CinemachineCamera(EQ))")]
    public GameObject virtualCameraToReset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportAndSnapCamera(other.gameObject));
        }
    }

    private IEnumerator TeleportAndSnapCamera(GameObject player)
    {
        // 1. ปิดตากล้อง VCam ชั่วคราว เพื่อล้างความจำ! (หยุดการสวิง)
        if (virtualCameraToReset != null)
        {
            virtualCameraToReset.SetActive(false);
        }

        // 2. ย้ายตัวละคร
        player.transform.position = targetDestination.position;

        // 3. เปลี่ยนภารกิจ
        if (objectiveManager != null)
        {
            objectiveManager.NextObjective();
        }

        // 4. รอ 1 เฟรม เพื่อให้ตัวละครยืนบนดาดฟ้าให้เรียบร้อยก่อน
        yield return new WaitForEndOfFrame();

        // 5. เปิดตากล้อง VCam ขึ้นมาใหม่! 
        // พอมันตื่นขึ้นมา มันจะลืมที่อยู่เก่า แล้วโฟกัสที่ตัวละครทันทีแบบตัดฉับครับ!
        if (virtualCameraToReset != null)
        {
            virtualCameraToReset.SetActive(true);
        }
    }
}