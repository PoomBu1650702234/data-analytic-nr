using TMPro;
using UnityEngine;

public class PuzzleCountUI : MonoBehaviour
{
    private TextMeshProUGUI stringTex;

    private void Start()
    {
        stringTex = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        stringTex.text = $"Hidden Key Found {GameManager.Instance.GetPuzzleKeys()} / {GameManager.Instance.GetReqKey()}";
    }
}
