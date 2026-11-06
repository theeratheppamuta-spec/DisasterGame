using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [Header("ตั้งค่าจุดเกิดเหตุ")]
    // 🌟 เปลี่ยนจาก GameObject ธรรมดา เป็น Array [] เพื่อใส่ของได้หลายชิ้น!
    public GameObject[] debrisPrefabs; 
    public float minSpawnTime = 1f; 
    public float maxSpawnTime = 3f; 

    private float timer;

    void Start()
    {
        SetRandomTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            SpawnDebris();
            SetRandomTimer(); 
        }
    }

    void SpawnDebris()
    {
        // เช็คก่อนว่าเราได้ลากของมาใส่ในช่องแล้วจริงๆ ใช่ไหม
        if (debrisPrefabs.Length > 0)
        {
            // 🌟 สุ่มตัวเลขตั้งแต่ 0 ถึงจำนวนชิ้นของที่เรามี
            int randomIndex = Random.Range(0, debrisPrefabs.Length);
            
            // เลือกของชิ้นนั้นออกมาปา!
            GameObject selectedDebris = debrisPrefabs[randomIndex];
            Instantiate(selectedDebris, transform.position, Quaternion.identity);
        }
    }

    void SetRandomTimer()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}