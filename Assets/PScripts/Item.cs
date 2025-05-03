using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int price;
    public Button buyButton;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI ItemNameText;
    public bool purchased = false;

    private void Start()
    {
        priceText.text = price.ToString();
        ItemNameText.text = itemName;

    }
    public void Buy(PlayerController player)
    {
        if (player == null)
        {
            Debug.Log("Cannot buy item: Player is missing or has been destroyed.");
            return;
        }

        int tempCoin = Shop.instance.GetCoin();
        if (purchased) return;

        if (tempCoin >= price)
        {
            Shop.instance.SetCoin(tempCoin - price);
            ApplyUpgrade(player);
            purchased = true;
            buyButton.interactable = false;
            Shop.instance.UpdateUILogic();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void ApplyUpgrade(PlayerController player)
    {
        
        Debug.Log($"{itemName} upgrade applied!");
        if (itemName == "Respawn Same Point") 
        {
            player.RespawnUpgrade();
        }
        if (itemName == "Faster Walk Speed")
        {
            player.SpeedUpgrade();
        }
        if (itemName == "Jump Higher")
        {
            player.JumpUpgrade();
        }
        if (itemName == "Better Flashlight")
        {
            FlashLight.instance.Upgrade();
        }
    }
}
