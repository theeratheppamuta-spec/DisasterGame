using UnityEngine;

public class LifeJacketZone : MonoBehaviour
{
    [Header("ชื่อไอเทมที่ต้องการ")]
    public string requiredItem = "Jacket"; // พิมพ์ชื่อเสื้อชูชีพให้ตรงกับในระบบ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory[] inventories = FindObjectsByType<PlayerInventory>(FindObjectsSortMode.None);
            if (inventories.Length > 0)
            {
                // ถ้าค้นกระเป๋าแล้วไม่มีเสื้อชูชีพ
                if (!inventories[0].HasItem(requiredItem))
                {
                    Debug.Log("ซวยแล้ว! เดินมาป้ายเตือนแต่ไม่มีเสื้อชูชีพ -> จมน้ำตาย!");
                    FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0].PlayerDied("ไม่มีเสื้อชูชีพ จมน้ำตาย!");
                }
                else
                {
                    Debug.Log("รอดตัวไป! ใส่เสื้อชูชีพมาพร้อมลุยน้ำ");
                }
            }
        }
    }
}