using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move
}
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool respawnPass = false;

    [Header("Player Health")]
    [SerializeField] private int maxHealth = 1; // จำนวนพลังชีวิตสูงสุด
    private int currentHealth; // พลังชีวิตปัจจุบัน
    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField]private float jumpForce = 10f;
    [SerializeField]private int maxJumps = 2; // จำนวนครั้งที่กระโดดได้

    [Header("PlayerState")]
    [SerializeField] private PlayerState state;
    public PlayerState State { get { return state; } set { state = value; } }

    [Header("Ground Check")]
    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    [SerializeField]private int jumpCount; // ตัวแปรนับจำนวนครั้งที่กระโดด

    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // กำหนดให้ HP เริ่มต้นเต็ม
        
    }

    private void Update()
    {
        
        // กระโดดเมื่อกด Space และอยู่บนพื้น
       if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            
            Jump();
        }


    }
    

    private void FixedUpdate()
    {
        
        // การเคลื่อนไหวในแกน X และ Y
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // กำหนดสถานะตัวละคร
        if (moveInput != 0)
        {
            state = PlayerState.Move;
            FlipCharacter(moveInput); // เรียกใช้ฟังก์ชันหมุนตัวละคร
            
        }
        else
        {
            state = PlayerState.Idle;
        }
        
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
    
        if (isGrounded)
        {
            jumpCount = 0;  // รีเซ็ตเมื่อสัมผัสพื้น
        }
    }
   private void FlipCharacter(float moveDirection)
    {
        // หากทิศทางการเคลื่อนที่เปลี่ยนไปจากเดิม ให้หมุนตัวละคร
        if ((moveDirection > 0 && !isFacingRight) || (moveDirection < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight; // เปลี่ยนสถานะทิศทาง

            // หมุนตัวละครโดยการกลับแกน X
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++; // เพิ่มจำนวนครั้งที่กระโดด

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.jump);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงพื้นที่ตรวจสอบพื้น
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    public void TakeDamage()
    {
        currentHealth--; // ลดพลังชีวิตลง 1

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.death);
        }

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            //audioManager.PlaySFX(audioManager.death);
            Die(); // ถ้าพลังหมดให้ตาย
        }
    }
    // ฟังก์ชันให้ Player ตาย
    private void Die()
    {
        Debug.Log("Player Died! Respawning...");
        if (respawnPass == false)
        {
            transform.position = spawnPoint.position; // กลับไปที่จุด Spawn
        }
        else if (respawnPass == true) 
        {
            transform.position += new Vector3(0,20,0); 
        }
        
        currentHealth = maxHealth; // รีเซ็ต HP
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // ตรวจสอบว่าเป็นศัตรู
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
                collision.GetContacts(contacts);

                foreach (ContactPoint2D contact in contacts)
                {
                    if (contact.normal.y > 0.5f) // ตรวจสอบว่าชนจากด้านบน
                    {
                        enemy.TakeDamage(); // ศัตรูตาย
                        Jump(); // ให้ Player เด้งขึ้น
                        return; // จบฟังก์ชัน
                    }
                }

                // ถ้าชนจากด้านข้างหรือล่าง -> Player ตายแทน
                TakeDamage();
            }
        }
    }

    public void SpeedUpgrade()
    {
        moveSpeed *= 3;
    }
    public void JumpUpgrade()
    {
        jumpForce *= 1.5f;
    }
    public void RespawnUpgrade() 
    {
        respawnPass = true;
    }

}
