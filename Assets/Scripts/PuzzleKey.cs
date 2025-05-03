using Unity.VisualScripting;
using UnityEngine;

public class PuzzleKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // เมื่อ Player เดินชนไอเท็ม
        {
            if (gameObject.TryGetComponent<IHiddenObject>(out IHiddenObject hiddenObj))
            {
                Hidden hidden = hiddenObj as Hidden;
                if (hidden != null && hidden.Interactabe == true)
                {
                    GameManager.Instance.CollectPuzzleKey(); // บันทึกไอเท็มที่เก็บได้
                    Destroy(gameObject); // ลบไอเท็มออกจากเกม
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // เมื่อ Player เดินชนไอเท็ม
        {
            if (gameObject.TryGetComponent<IHiddenObject>(out IHiddenObject hiddenObj))
            {
                Hidden hidden = hiddenObj as Hidden;
                if (hidden != null && hidden.Interactabe == true)
                {
                    GameManager.Instance.CollectPuzzleKey(); // บันทึกไอเท็มที่เก็บได้
                    Destroy(gameObject); // ลบไอเท็มออกจากเกม
                }
            }
        }
    }
}


