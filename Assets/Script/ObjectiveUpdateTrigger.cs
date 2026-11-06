using UnityEngine;

public class ObjectiveUpdateTrigger : MonoBehaviour
{
    [Header("ลาก Objective Manager มาใส่ช่องนี้")]
    public ObjectiveManager objectiveManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. เรดาร์ตรวจจับการชน (เช็คว่ากล่องทำงานไหม)
        Debug.Log("💡 มีของมาชนกล่องนี้ชื่อ: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // 2. เรดาร์ตรวจจับ Tag (เช็คว่ารับรู้ว่าเป็นผู้เล่นไหม)
            Debug.Log("🚶‍♂️ คนที่ชนคือ Player! เตรียมเปลี่ยนภารกิจ...");

            if (objectiveManager != null)
            {
                // 3. สั่งเปลี่ยนภารกิจ
                objectiveManager.NextObjective();
                Debug.Log("✅ สั่ง NextObjective แล้ว! กล่องกำลังจะหายไป");
                
                GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                // 4. เรดาร์แจ้งเตือนสายไฟหลุด!
                Debug.LogError("❌ พัง! คุณลืมลาก Objective Manager มาใส่ในกล่อง Trigger ครับ!");
            }
        }
    }
}