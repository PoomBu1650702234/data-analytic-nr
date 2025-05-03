
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Transactions;
using CustomEvent = Unity.Services.Analytics.CustomEvent;


public class Shop : MonoBehaviour
{
    [SerializeField] private int coins;
    [SerializeField] private GameObject uiButton;
    private PlayerController player;

    [SerializeField]private string previousScene;
    [SerializeField] private string currentScene;
    [SerializeField] private int revenue; //Per stage
    [SerializeField] private int cost; //Per stage

    [SerializeField] private float timer = 0f;
    public Item[] items;

    public static Shop instance;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    
    private void Start()
    {
        Initialize();
        player = PlayerController.instance;
        if (player == null)
        {
            print("Player not found");
        }
        else 
        {
            print("Player found");
        }
        foreach (var item in items)
        {
            item.buyButton.onClick.AddListener(() => item.Buy(player));
        }

        UpdateUILogic();
        string sceneName = SceneManager.GetActiveScene().name;
        currentScene = sceneName;
       
        if (sceneName == "MapSetup1" || sceneName == "MapSetup2" || sceneName == "MapSetup3")
        {
            uiButton.SetActive(true);
        }
        else 
        {
            uiButton.SetActive(false);
        }
    }

    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MapSetup1" || sceneName == "MapSetup2" || sceneName == "MapSetup3")
        {
            timer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        if (currentScene != scene.name) 
        {
            previousScene = currentScene;
            currentScene = scene.name;
        }
        
        if (scene.name == "MapSetup1" || scene.name == "MapSetup2" || scene.name == "MapSetup3")
        {
            timer = 0f;
            uiButton.SetActive(true);
        }
        else
        {
            uiButton.SetActive(false);
        }
        if (scene.name == "VictoryScreen") 
        {
            int burnrate = cost - revenue;
            Debug.Log($"Burn Rate Map : {previousScene} = {burnrate}");
            SendBurnRateData(previousScene, burnrate);
            SendSessionData(previousScene, timer);
            timer = 0f;
        }

        revenue = 0;
        cost = 0;
        
        ResetShopForNewStage();

    }

    private void ResetShopForNewStage()
    {
        player = PlayerController.instance;
        if (player == null)
        {
            print("Player not found");
        }
        else
        {
            print("Player found");
        }
        foreach (var item in items)
        {
            item.purchased = false;
            if (item.buyButton != null) 
            {
                item.buyButton.interactable = true;
            }
        }
    }


    public int GetCoin()
    {
        return coins;
    }

    public void SetCoin(int amount) 
    {
        if (coins > amount)
        {
            //coin decrese
            cost += math.abs(coins - amount);
            Debug.Log($"Cost = {cost}");
        }
        else if(coins < amount)
        {
            //coin increse
            revenue += math.abs(coins - amount);
            Debug.Log($"revenue = {revenue}");
        }
        coins = amount;
    }

    public void UpdateUILogic()
    {
        foreach (var item in items)
        {
            //Change that u can't interact with UI when it was purchased
            item.buyButton.interactable = !item.purchased;
        }
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    private void SendBurnRateData(string mapName, int brunRate)
    {
        CustomEvent exampleEvent = new CustomEvent("burnRateCalculation")
        {
            {"mapName",mapName},{"burnRate",brunRate}
        };

        AnalyticsService.Instance.RecordEvent(exampleEvent);
        Debug.Log($"record signal send {mapName},{brunRate}");
    }
    private void SendSessionData(string mapName, float timer)
    {
        CustomEvent exampleEvent = new CustomEvent("stageCompleteTimer")
        {
            {"mapName",mapName},{"stageTimer",timer}
        };

        AnalyticsService.Instance.RecordEvent(exampleEvent);
        Debug.Log($"record signal send {mapName},{timer}");
    }
}
