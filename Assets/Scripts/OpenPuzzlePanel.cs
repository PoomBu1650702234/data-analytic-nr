using UnityEngine;

public class OpenPuzzlePanel : MonoBehaviour
{
    public GameObject puzzlePanel; // Drag and drop the Panel here in the Inspector.

    void Start()
    {
        // ซ่อน Panel เมื่อเริ่มต้น
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        // แสดง Panel เมื่อคลิกที่ Object
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
        }
    }
}

