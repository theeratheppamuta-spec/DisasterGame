using UnityEngine;
using Unity.Cinemachine;
using System.Collections; 
using TMPro; 

public class TVController : MonoBehaviour
{
    public CinemachineCamera vCam;
    public float shakeIntensity = 3f;

    [Header("--- ตั้งค่าเวลาภัยพิบัติ ---")]
    public float disasterDuration = 20f; 

    [Header("ระบบเช็คความตาย")]
    public string requiredItemName = "Hat";

    [Header("ระบบคัทซีนด่าน")]
    public UniversalCutsceneManager cutsceneManager;

    [Header("ระบบแสดงผลเวลาบนหน้าจอ (UI)")]
    public TextMeshProUGUI timerText; 

    // 🌟 สิ่งที่เพิ่มเข้ามาใหม่: ช่องลากรูปปุ่ม E หรือข้อความบอกทาง
    [Header("--- ระบบแจ้งเตือนปุ่มกด (Interact Prompt) ---")]
    public GameObject interactPrompt; 

    private CinemachineBasicMultiChannelPerlin noise;
    private float timer;
    private bool isPlayerNearby = false;
    private bool hasTriggeredTV = false; 

    void Start()
    {
        if (vCam != null) {
            noise = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        if (timerText != null) {
            timerText.text = "";
        }

        // 🌟 เริ่มเกมมาให้ซ่อนปุ่ม E ไว้ก่อนเสมอ
        if (interactPrompt != null) {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !hasTriggeredTV)
        {
            if (cutsceneManager != null)
            {
                hasTriggeredTV = true; 
                
                // 🌟 พอผู้เล่นกดเปิดแล้ว ให้ซ่อนปุ่ม E ไปทันที จะได้ไม่รกสายตาระหว่างคัทซีน
                if (interactPrompt != null) interactPrompt.SetActive(false); 

                cutsceneManager.PlayIntroCutscene(); 
            }
            else
            {
                StartShake(); 
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timerText != null) {
                timerText.text = "แผ่นดินไหวรุนแรง อีก: " + Mathf.CeilToInt(timer).ToString() + " วินาที";
            }

            if (timer <= 0 && noise != null)
            {
                noise.AmplitudeGain = 0;
                Debug.Log("แผ่นดินไหวสงบลง...");

                if (timerText != null) {
                    timerText.text = "";
                }
                
                CheckPlayerInventoryForSurvival(); 
            }
        }
    }

    public void StartShake()
    {
        if (noise != null)
        {
            noise.AmplitudeGain = shakeIntensity;
            timer = disasterDuration; 
            Debug.Log("เกิดแผ่นดินไหว! เวลาทั้งหมด: " + disasterDuration + " วินาที");
        }
    }

    private void CheckPlayerInventoryForSurvival()
    {
        PlayerInventory[] inventories = FindObjectsByType<PlayerInventory>(FindObjectsSortMode.None);
        
        if (inventories.Length > 0)
        {
            PlayerInventory inventory = inventories[0];
            
            if (!inventory.HasItem(requiredItemName))
            {
                Debug.Log("ซวยล่ะ! กล้องหยุดสั่นแล้วแต่ไม่มีหมวก -> ส่งไปหน้าตาย!");
                
                GameManager[] managers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
                if (managers.Length > 0)
                {
                    managers[0].PlayerDied("ไม่มีหมวก ตาย!!");
                }
            }
            else
            {
                Debug.Log("รอดตาย! มีหมวกนิรภัยอยู่ในกระเป๋า");
                
                if (cutsceneManager != null)
                {
                    cutsceneManager.PlayEndingCutscene();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 🌟 เดินเข้าใกล้ทีวี ให้เปิดปุ่ม E โชว์ขึ้นมา (แต่ต้องยังไม่เคยกดเปิดทีวีนะ)
        if (other.CompareTag("Player")) {
            isPlayerNearby = true;
            
            if (interactPrompt != null && !hasTriggeredTV) {
                interactPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 🌟 เดินห่างออกมา ให้ซ่อนปุ่ม E ไป
        if (other.CompareTag("Player")) {
            isPlayerNearby = false;
            
            if (interactPrompt != null) {
                interactPrompt.SetActive(false);
            }
        }
    }

    public void ResetTV()
   {
    hasTriggeredTV = false; // ปลดล็อกให้สามารถกลับมากด E ที่ทีวีได้อีกครั้ง
    timer = 0; // รีเซ็ตเวลานับถอยหลังแผ่นดินไหว
    
    if (timerText != null) timerText.text = ""; // ล้างข้อความเวลาบนหน้าจอ
    if (interactPrompt != null) interactPrompt.SetActive(false); // ซ่อนปุ่ม [E] ไว้ก่อน
    
    // สั่งให้กล้องหยุดสั่นทันที (ถ้ามันยังสั่นค้างอยู่ตอนที่ตาย)
    if (noise != null) {
        noise.AmplitudeGain = 0; 
    }
   }
}