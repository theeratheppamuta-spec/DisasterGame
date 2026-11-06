using UnityEngine;
using UnityEngine.Events; //
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // เพิ่มบรรทัดนี้เพื่อใช้ Text โชว์ตัวเลข

public class PlayerInventory : MonoBehaviour
{
    public List<Image> uiSlots; 
    public TextMeshProUGUI[] stackTexts; // ลาก Text ในแต่ละ Slot มาใส่ในนี้
    public GameObject sandbagPrefab; 
    
    private int[] slotCounts = new int[6]; 
    private string[] slotItemNames = new string[6]; // เก็บชื่อเพื่อเช็ก Stack

    public void AddItem(string name, Sprite sprite)
{
    // 1. วนลูปหาว่ามีของชื่อนี้อยู่แล้วไหม เพื่อ Stack
    for (int i = 0; i < uiSlots.Count; i++)
    {
        if (slotItemNames[i] == name) 
        {
            // แก้ตรงนี้: ถ้าเป็นถุงทราย ให้บวก 3 อัน
            int amountToAdd = (name == "SandBag") ? 3 : 1; 
            slotCounts[i] += amountToAdd;
            
            UpdateSlotUI(i);
            return;
        }
    }

    // 2. ถ้ายังไม่มี ให้หาช่องว่างใหม่
    for (int i = 0; i < uiSlots.Count; i++)
    {
        if (uiSlots[i].sprite == null)
        {
            uiSlots[i].sprite = sprite;
            uiSlots[i].color = Color.white;
            slotItemNames[i] = name;
            
            // แก้ตรงนี้ด้วย: ตอนเริ่มเก็บครั้งแรก ให้เริ่มที่ 3 อัน
            slotCounts[i] = (name == "SandBag") ? 3 : 1; 
            
            UpdateSlotUI(i);
            return;
        }
    }
}

public void UseItem(int slotIndex)
{
    if (slotIndex < uiSlots.Count && slotCounts[slotIndex] > 0)
    {
        string name = slotItemNames[slotIndex];

        if (name == "SandBag")
        {
            // 1. หาความสูง/ต่ำ ของการวาง (เหมือนเดิม)
            float yOffset = -0.5f; 
            
            // 2. เช็กทิศทางที่ตัวละครหันหน้า (เช็กจาก Scale แกน X)
            // ถ้า localScale.x เป็นบวก คือหันขวา, ถ้าเป็นลบ คือหันซ้าย
            float faceDirection = transform.localScale.x > 0 ? 1f : -1f;

            // 3. กำหนดระยะห่างด้านหน้า (เช่น ห่างจากตัว 0.8 หน่วย)
            float xOffset = 0.8f * faceDirection;

            // 4. คำนวณตำแหน่งสุดท้าย
            Vector3 spawnPos = transform.position + new Vector3(xOffset, yOffset, 0);

            // 5. เสกถุงทรายออกมา
            GameObject newBag = Instantiate(sandbagPrefab, spawnPos, Quaternion.identity);
            
            if(newBag.GetComponent<SpriteRenderer>() != null) {
                newBag.GetComponent<SpriteRenderer>().sortingOrder = 15;
            }
        }

        // ... ส่วนลดจำนวนไอเทมด้านล่างเหมือนเดิม ...
        slotCounts[slotIndex]--;
        if (slotCounts[slotIndex] <= 0)
        {
            uiSlots[slotIndex].sprite = null;
            uiSlots[slotIndex].color = Color.clear;
            slotItemNames[slotIndex] = "";
        }
        UpdateSlotUI(slotIndex);
    }
}
    void UpdateSlotUI(int index)
    {
        // อัปเดตตัวเลขบนหน้าจอ (ถ้ามีใส่ Text ไว้)
        if (stackTexts.Length > index && stackTexts[index] != null)
        {
            stackTexts[index].text = slotCounts[index] > 0 ? slotCounts[index].ToString() : "";
        }
    }
    // เพิ่มฟังก์ชันนี้ลงไปท้ายสคริปต์ PlayerInventory.cs
    public bool HasItem(string itemName)
    {
    // วนลูปเช็กชื่อไอเทมในทุกช่องของกระเป๋า
     for (int i = 0; i < slotItemNames.Length; i++)
      {
        if (slotItemNames[i] == itemName)
        {
            return true; // เจอไอเทมชื่อที่ต้องการ
        }
      }
       return false; // วนหาจนทั่วแล้วไม่เจอ
    }
    // เพิ่มฟังก์ชันนี้ลงใน PlayerInventory.cs
    // เอาไปวางทับฟังก์ชัน RemoveItem เดิมได้เลยครับ
    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < slotItemNames.Length; i++)
        {
            if (slotItemNames[i] == itemName)
            {
                // 🌟 หักไอเทมออกแค่ 1 ชิ้น (ไม่ล้างบางแล้ว!)
                slotCounts[i]--; 

                // 🌟 เช็คว่าถ้าหักจนเหลือ 0 ชิ้น ค่อยเคลียร์ภาพและข้อมูลในช่องนั้นทิ้ง
                if (slotCounts[i] <= 0)
                {
                    slotItemNames[i] = "";
                    slotCounts[i] = 0;
                    
                    if (uiSlots[i] != null)
                    {
                        uiSlots[i].sprite = null;
                        uiSlots[i].color = Color.clear;
                    }
                }

                // อัปเดตตัวเลขบนจอให้ถูกต้อง
                UpdateSlotUI(i);
                
                Debug.Log(itemName + " หักจากกระเป๋า 1 ชิ้น (เหลือ: " + slotCounts[i] + ")");
                return;
            }
        }
    
    }
    

    // เพิ่มฟังก์ชันนี้เข้าไปใน PlayerInventory นะครับ
    // ลบอันเก่าออกแล้ววางก้อนนี้แทนที่ลงไปท้ายสคริปต์ PlayerInventory.cs ครับ
    public void ResetInventory()
    {
      // วนลูปเคลียร์ค่าทุกช่องในกระเป๋า (ระบบคุณเซ็ตไว้ 6 ช่องตามรูปที่ 1)
      for (int i = 0; i < slotItemNames.Length; i++)
       {
        // 1. ล้างข้อมูลชื่อและจำนวนในระบบกลับเป็นค่าว่างและ 0
        slotItemNames[i] = "";
        slotCounts[i] = 0;

        // 2. ล้างรูปภาพไอเทมบนหน้าจอ UI ให้หายไป (ปรับเป็นโปร่งแสง)
        if (uiSlots[i] != null)
        {
            uiSlots[i].sprite = null;
            uiSlots[i].color = Color.clear; // ทำให้ช่องใส มองไม่เห็นรูปเก่า
        }
        // 3. สั่งอัปเดตตัวเลขจำนวนไอเทมบนจอให้หายไปด้วย
          UpdateSlotUI(i);
       }
          Debug.Log("ล้างไอเทมออกจากช่องกระเป๋าและ UI ทุกช่องเรียบร้อย!");
    }

    // 🌟 วางฟังก์ชันนี้ไว้ใน PlayerInventory.cs 
    public bool FindAndUseItem(string itemName)
    {
        for (int i = 0; i < slotItemNames.Length; i++)
        {
            if (slotItemNames[i] == itemName && slotCounts[i] > 0)
            {
                // เจอถุงทรายในช่องที่ i แล้ว สั่งให้ฟังก์ชันเสกของทำงานทันที!
                UseItem(i); 
                return true; 
            }
        }
        return false; 
    }
}