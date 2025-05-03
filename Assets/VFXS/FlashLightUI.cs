using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightUI : MonoBehaviour
{
    [SerializeField] FlashLight flashLight;
    [SerializeField] Image barUI;
    [SerializeField] TextMeshProUGUI modeNameText;


    private void Update()
    {
        if (flashLight == null) 
        {
            return;
        }
        float t = flashLight.GetCurrentBattery()/flashLight.GetMaxBattery();
        string modeName = "...";
        if (flashLight.GetMode() == 1)
        {
            modeName = "Normal";
        }
        else 
        {
            modeName = "Overloaded";
        }
        modeNameText.text = $"Mode: {modeName}";
        barUI.fillAmount = t;

        if (flashLight.GetbatteryState() == "-") 
        {
            barUI.color = Color.green;
        }
        if (flashLight.GetbatteryState() == "+")
        {
            barUI.color = Color.red;
        }
    }

}
