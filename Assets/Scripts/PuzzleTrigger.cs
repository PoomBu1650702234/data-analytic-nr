/*using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    public Vector3 puzzleSpawnOffset = new Vector3(0, 2, 0); // ตำแหน่งเกิดของ Puzzle

    private void OnMouseDown()
    {
        if (!GameManager.Instance.IsPuzzleActive())
        {
            Vector3 spawnPosition = transform.position + puzzleSpawnOffset;
            GameManager.Instance.OpenPuzzle(spawnPosition);
        }
    }
}*/
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Key pressed!");  // แสดงเมื่อกดปุ่ม E
            GameManager.Instance.OpenPuzzle(transform.position); // เปิด Puzzle ผ่าน GameManager
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่า Player เข้าใกล้หรือไม่
        {
            isPlayerNearby = true;
            Debug.Log("Player เข้าใกล้ PuzzleTrigger");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player ออกจากระยะ PuzzleTrigger");
        }
    }
}
