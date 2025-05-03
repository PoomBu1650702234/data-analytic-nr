using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TapToPlay : MonoBehaviour
{
    async void Start()
    {
        // Initialize Unity Services
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Services initialized successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }
    }

    public void OnTapToPlay()
    {
        string uid;

        // สร้าง UID ถ้ายังไม่มี
        if (!PlayerPrefs.HasKey("uid"))
        {
            uid = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("uid", uid);
            PlayerPrefs.SetInt("login_count", 0);
        }
        else
        {
            uid = PlayerPrefs.GetString("uid");
        }

        // เพิ่มจำนวน login count
        int loginCount = PlayerPrefs.GetInt("login_count");
        loginCount++;
        PlayerPrefs.SetInt("login_count", loginCount);

        // ตรวจสอบว่า Analytics พร้อมใช้งาน
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            // ส่งข้อมูลเข้า Analytics
            CustomEvent customEvent = new CustomEvent("tap_to_play")
            {
                { "custom_uid", uid },
                { "login_count", loginCount },
                { "tap_time", System.DateTime.UtcNow.ToString("o") }
            };

            AnalyticsService.Instance.RecordEvent(customEvent);
            Debug.Log($"Player {uid} tapped to play. Total login count: {loginCount}");
        }
        else
        {
            Debug.LogWarning("Analytics not initialized. Event not sent.");
        }

        
        // ไปยัง scene หรือเริ่มเกมต่อ
        // ตัวอย่าง: SceneManager.LoadScene("GameScene");
    }
}