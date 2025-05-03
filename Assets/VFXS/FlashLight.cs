using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    public static FlashLight instance;
    private enum BatteryState 
    {
        Decresing,
        Refilling
    }
    [Header("Logics Settings")]

    [SerializeField] private float maxCapBattery = 10f;
    [SerializeField] private float battery = 10f;
    [SerializeField] private float batteryDrainRate = 0.1f;
    [SerializeField] private float batteryRechargeRate;

    [Header("Flashlight Visual Settings")]
    
    [SerializeField] private BatteryState batteryState;
    [SerializeField] private bool isFlashON;

    [SerializeField] int mode = 1;

    [SerializeField] GameObject flashLightMode1;
    [SerializeField] GameObject flashLightMode2;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isFlashON = false;
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && batteryState == BatteryState.Decresing) 
        {
            isFlashON = !isFlashON;
            // Play the audio if it's assigned
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (mode == 1) 
            { 
                mode = 2;
                batteryDrainRate *= 2;
            }
            else 
            {
                mode = 1;
                batteryDrainRate /= 2;
                
            }
        }
        FlashlightLookAtMouse();
        AdjustBattery();
    }
    private void FlashlightLookAtMouse()
    {
        Vector3 rawMousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(rawMousePos.x, rawMousePos.y, Camera.main.nearClipPlane));
        Vector3 dir = (worldMousePos - transform.position);
        dir.z = 0;
        transform.up = dir.normalized;
    }

    private void AdjustBattery()
    {
        if (batteryState == BatteryState.Decresing && isFlashON == true) 
        {
            battery -= batteryDrainRate * Time.deltaTime;
            if (battery <= 0)
            {
                batteryState = BatteryState.Refilling;
                isFlashON = false;
            }
        }
        if (batteryState == BatteryState.Refilling && isFlashON == false) 
        {
            battery += batteryRechargeRate * Time.deltaTime;
            if (battery >= maxCapBattery) 
            {
                battery = maxCapBattery;
                batteryState = BatteryState.Decresing;
            }

        }

        if (batteryState == BatteryState.Decresing && isFlashON == false)
        {
            battery += batteryRechargeRate * Time.deltaTime;
            if (battery >= maxCapBattery)
            {
                battery = maxCapBattery;
            }
        }

        // Open/off > Light > collider and Color filter at same time 
        if (isFlashON == false)
        {
            flashLightMode1.SetActive(false);
            flashLightMode2.SetActive(false);

        }
        else if (isFlashON == true) 
        {
            if (mode == 1) 
            {
                flashLightMode1.SetActive(true);
                flashLightMode2.SetActive(false);
            }
            if (mode == 2)
            {
                flashLightMode1.SetActive(false);
                flashLightMode2.SetActive(true);
            }

        }

    }

    public int GetMode() 
    {
        return mode;
    }

    public float GetMaxBattery() 
    {
        return maxCapBattery;
    }
    public float GetCurrentBattery()
    {
        return battery;
    }

    public string GetbatteryState() 
    {
        if (batteryState == BatteryState.Decresing) 
        {
            return "-";
        }
        if (batteryState == BatteryState.Refilling)
        {
            return "+";
        }
        return null;

    }

    public void Upgrade() 
    {
        maxCapBattery *= 3;
        batteryDrainRate = 0.5f;
        batteryRechargeRate *= 3;
    }

}
