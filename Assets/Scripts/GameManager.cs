using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Transform playerSpawnPoint; // จุดเกิดของผู้เล่น
    [SerializeField] private GameObject playerPrefab; // Prefab ของ Player
    private GameObject player; // ตัวอ้างอิงถึง Player

    public RingRotation currentActiveRing = null; // วงแหวนที่ถูกเลือกในขณะนี้
    private int ringIndex = 0; // ลำดับของวงแหวนที่กำลังจะหมุน
    [SerializeField] private RingRotation[] rings; // ลำดับวงแหวน (Outer > Middle > Inner)
    [SerializeField] private GameObject puzzlePrefab; // Prefab ของ Puzzle
    [SerializeField] private GameObject puzzlePanel; // อ้างอิงถึง PuzzlePanel ที่อยู่นอก Prefab
    [SerializeField] private int puzzleKeyCount = 0; // จำนวนไอเท็มที่เก็บได้
    [SerializeField] private int requiredPuzzleKeys = 15; // จำนวนไอเท็มที่ต้องเก็บก่อนเปิด Puzzle
    [SerializeField] private GameObject closeButton;
    //[SerializeField] private Animator puzzleCompleteAnimator; // ใส่ Animator
    private GameObject currentPuzzle; // เก็บ Puzzle ที่ถูกสร้าง
    private bool isPuzzleActive = false;
    public bool isPuzzleCompleted = false; // ตัวแปรตรวจสอบสถานะการทำงานของ Puzzle


    [SerializeField] private WorldToColorEffect effect;
    [SerializeField] private string puzzleCompleteNextSceneName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.LogError("PlayerPrefab or SpawnPoint is not assigned in the Inspector!");
        }
        if (rings == null || rings.Length == 0)
        {
            Debug.LogError("Rings have not been assigned in the inspector.");
            return;  // หยุดการทำงานหากไม่มีการกำหนดค่า
        }

        if (rings.Length != 3)
        {
            Debug.LogError("Expected exactly 3 rings. Please assign the rings correctly in the Inspector.");
            return;
        }

        // ตรวจสอบให้แน่ใจว่ามีวงแหวน
        if (rings.Length == 3)
        {
            ActivateRing(0); // เริ่มจากวงแหวนแรก
        }
    }
    public void SpawnPlayer()
    {
        // ลบ Player เก่าก่อน (ถ้ามี)
        if (player != null)
        {
            Destroy(player);
        }

        // สร้าง Player ที่ตำแหน่ง Spawn
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        Debug.Log("Spawned Player at: " + playerSpawnPoint.position);
    }
    public void ActivateRing(int index)
    {
        if (rings == null || index < 0 || index >= rings.Length)
        {
            Debug.LogError("Rings are not assigned correctly or index is out of bounds.");
            return; // หยุดหากวงแหวนไม่ถูกกำหนดค่า
        }

        // ตรวจสอบให้แน่ใจว่า currentActiveRing ไม่เป็น null
        if (rings[index] != null)
        {
            ringIndex = index;
            currentActiveRing = rings[ringIndex];
            currentActiveRing.SetActive(true); // เปิดวงแหวนที่กำหนด
        }
        else
        {
            Debug.LogError("Ring at index " + index + " is null.");
        }
    }

    public void NextRing()
    {
        if (ringIndex + 1 < rings.Length)
        {
            currentActiveRing.SetActive(false); // ปิดการใช้งานวงแหวนปัจจุบัน
            ActivateRing(ringIndex + 1);       // เปลี่ยนไปวงแหวนถัดไป
        }
        else
        {
            Debug.Log("All rings aligned! Puzzle solved!");
            CompletePuzzle(); // เรียกใช้งานเมื่อ Puzzle สำเร็จ
        }
    }

    // เปิด Puzzle และสร้างวงแหวน
    public void OpenPuzzle(Vector3 position)
    {
        if (!CanOpenPuzzle()) // ถ้ายังเก็บไอเท็มไม่ครบ
        {
            Debug.Log("You need more Puzzle Keys! (" + puzzleKeyCount + "/" + requiredPuzzleKeys + ")");
            return; // หยุดการทำงาน
        }
        Debug.Log("OpenPuzzle called!");  // เพิ่มการแสดงผลเพื่อเช็คว่าเข้ามาถึงฟังก์ชันนี้หรือไม่
        if (!isPuzzleActive && !isPuzzleCompleted)
        {
            Vector3 puzzlePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
            puzzlePosition.z = 0f;  // เนื่องจากคุณเล่นใน 2D ค่าของ z ควรเป็น 0

            currentPuzzle = Instantiate(puzzlePrefab, puzzlePosition, Quaternion.identity);
            rings = currentPuzzle.GetComponentsInChildren<RingRotation>(); // ดึง RingRotation ทั้งหมดจาก Puzzle ที่สร้าง

            if (rings == null || rings.Length == 0)
            {
                Debug.LogError("No rings found in the puzzle prefab!");
                return;
            }

            isPuzzleActive = true;
            //puzzlePanel.SetActive(true);  // แสดง PuzzlePanel
            //closeButton.SetActive(true);  // แสดงปุ่ม Close
            ActivateRing(0);  // เริ่มจากวงแหวนแรก
            DisablePlayerControl();  // ปิดการควบคุม Player ขณะ Puzzle เปิด
        }
    }


    // เมื่อ Puzzle ผ่าน
    public void CompletePuzzle()
    {
        /*Debug.Log("CompletePuzzle() called!");
        if (puzzleCompleteAnimator != null)
        {
            //Debug.Log("Playing Animation");
            puzzleCompleteAnimator.SetTrigger("Play");
        }
        Invoke(nameof(ClosePuzzle), 2f);*/
        
        isPuzzleCompleted = true;
        // Play Color Filter Effect
        if (effect != null) 
        {
            effect.WorldEffectActivate();
        }
        Invoke(nameof(ChangeScene), 10f);
    }

    private void DisablePlayerControl()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
            player.State = PlayerState.Idle;
        }
    }


    private void EnablePlayerControl()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = true;
        }
    }

    // ปิด Puzzle
    public void ClosePuzzle()
    {
        if (isPuzzleActive && currentPuzzle != null)
        {
            Destroy(currentPuzzle); // ลบ Puzzle
            isPuzzleActive = false;
            //puzzlePanel.SetActive(false); // ซ่อน Puzzle Panel
            //closeButton.SetActive(false); // ซ่อนปุ่ม Close
            EnablePlayerControl(); // เปิดการควบคุม Player กลับมา
        }
    }

    // ฟังก์ชันเช็คว่า Puzzle กำลังเปิดอยู่หรือไม่
    public bool IsPuzzleActive()
    {
        return isPuzzleActive;
    }
    // ฟังก์ชันเพิ่มจำนวนไอเท็ม
    public void CollectPuzzleKey()
    {
        puzzleKeyCount++;
        Debug.Log("Collected Puzzle Key! (" + puzzleKeyCount + "/" + requiredPuzzleKeys + ")");

        if (puzzleKeyCount >= requiredPuzzleKeys)
        {
            Debug.Log("You have enough keys to open the puzzle!");
        }
    }

    // ฟังก์ชันตรวจสอบว่าเปิด Puzzle ได้หรือไม่
    public bool CanOpenPuzzle()
    {
        return puzzleKeyCount >= requiredPuzzleKeys;
    }

    public int GetPuzzleKeys() 
    {
        return puzzleKeyCount;
    }
    public int GetReqKey()
    {
        return requiredPuzzleKeys;
    }

    public void ChangeScene() 
    {
        SceneManager.LoadScene(puzzleCompleteNextSceneName);
    }
}
