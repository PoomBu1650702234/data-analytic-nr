using UnityEngine;

public class RingRotation : MonoBehaviour
{
    public int rotationSpeed = 100; // เปลี่ยนเป็น int
    public int targetAngle; // เปลี่ยนเป็น int
    public int tolerance = 5; // เปลี่ยนเป็น int

    private bool isActive = false; // ตรวจสอบว่าวงแหวนนี้ Active หรือไม่
    private bool isLocked = false;

    void Start()
    {
        targetAngle = Random.Range(0, 360); // ตั้งมุมเป้าหมายแบบสุ่มเป็นจำนวนเต็ม
        Debug.Log($"{gameObject.name} Target Angle: {targetAngle}");
    }

    void Update()
    {
        // ถ้าวงแหวนถูกล็อกหรือไม่ได้ Active ให้ข้ามการหมุน
        if (isLocked || !isActive) return;

        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        // ตรวจสอบการหมุนและล็อกทันทีเมื่อมุมตรงกับ targetAngle
        CheckIfAligned();
    }

    private void RotateLeft()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void RotateRight()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    public void SetActive(bool active)
    {
        isActive = active; // ตั้งค่าว่าจะหมุนวงแหวนนี้ได้หรือไม่
    }

    private void CheckIfAligned()
    {
        // ดึงมุมปัจจุบันจาก Transform
        float currentAngle = transform.eulerAngles.z;
        currentAngle = (currentAngle + 360) % 360; // ทำให้มุมอยู่ในช่วง 0-360

        // เปลี่ยนเป็น int เพื่อให้ตรงกับ targetAngle (ซึ่งเป็น int)
        int currentAngleInt = Mathf.RoundToInt(currentAngle);

        // ดีบักค่ามุม
        Debug.Log($"Current Angle: {currentAngleInt}, Target Angle: {targetAngle}");

        // ใช้ tolerance ในการตรวจสอบความแตกต่าง
        if (Mathf.Abs(currentAngleInt - targetAngle) <= tolerance)
        {
            Debug.Log($"{gameObject.name} is aligned and locked!");
            transform.eulerAngles = new Vector3(0, 0, targetAngle); // ล็อกมุมให้ตรง
            isLocked = true;

            // แจ้ง GameManager ว่าลำดับถัดไปพร้อมทำงาน
            GameManager.Instance.NextRing();
        }
    }
}
