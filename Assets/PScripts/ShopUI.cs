using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinTextUI;

    private void Update()
    {
        if (Shop.instance != null && coinTextUI != null) 
        {
            int amount = Shop.instance.GetCoin();
            coinTextUI.text = amount.ToString();
        }
    }
}
