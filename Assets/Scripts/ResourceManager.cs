using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for SceneManager

public class ResourceManager : MonoBehaviour
{
    // Resource variables
    public int wood;
    public int stone;
    public int gold;

    public int employed;
    public int unemployed;
    public int population;

    // UI Texts
    public Text woodText;
    public Text stoneText;
    public Text goldText;
    public Text populationText;
    public Text unemployedText;
    public Text employedText;

    private static ResourceManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void OnEnable()
    {
        // Register the event listener for scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unregister the event listener when this object is disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        LoadResources(); // Load resources when the game starts
        FindUIElements(); // Find UI elements dynamically
        UpdateUI();
    }

    void Update()
    {
        PreventNegativeResources();
        UpdateUI();
    }

    // Method to prevent negative resources
    void PreventNegativeResources()
    {
        if (unemployed < 0) unemployed = 0; // Prevent negative unemployed
    }

    // Method to find UI elements dynamically
    void FindUIElements()
    {
        woodText = GameObject.Find("WoodText")?.GetComponent<Text>();
        stoneText = GameObject.Find("StoneText")?.GetComponent<Text>();
        goldText = GameObject.Find("GoldText")?.GetComponent<Text>();
        populationText = GameObject.Find("PopulationText")?.GetComponent<Text>();
        unemployedText = GameObject.Find("UnemployedText")?.GetComponent<Text>();
        employedText = GameObject.Find("EmployedText")?.GetComponent<Text>();
    }

    // Method to update UI text
    void UpdateUI()
    {
        if (woodText != null) woodText.text = "Wood: " + wood;
        if (stoneText != null) stoneText.text = "Stone: " + stone;
        if (goldText != null) goldText.text = "Gold: " + gold;
        if (populationText != null) populationText.text = "Population: " + population;
        if (employedText != null) employedText.text = "Employed: " + employed;
        if (unemployedText != null) unemployedText.text = "Unemployed: " + (population - employed);
    }

    // Methods to manipulate resources (linked to buttons)
    public void AddWood() { wood += 5; SaveResources(); }
    public void AddStone() { stone += 5; SaveResources(); }
    public void AddGold() { gold += 5; SaveResources(); }
    public void AddPopulation() { population += 1; SaveResources(); }
    public void AddEmployed() { employed += 1; SaveResources(); }

    public void RemoveWood() { if (wood > 0) wood -= 10; SaveResources(); }
    public void RemoveStone() { if (stone > 0) stone -= 10; SaveResources(); }
    public void RemoveGold() { if (gold > 0) gold -= 10; SaveResources(); }
    public void RemovePopulation() { if (population > 0) population -= 1; SaveResources(); }
    public void RemoveEmployed() { if (employed > 0) employed -= 1; SaveResources(); }

    // Method to save resources
    void SaveResources()
    {
        PlayerPrefs.SetInt("Wood", wood);
        PlayerPrefs.SetInt("Stone", stone);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Population", population);
        PlayerPrefs.SetInt("Employed", employed);
        PlayerPrefs.Save(); // Save changes to disk
    }

    // Method to load resources
    void LoadResources()
    {
        wood = PlayerPrefs.GetInt("Wood", 0); // Default to 0 if no data
        stone = PlayerPrefs.GetInt("Stone", 0);
        gold = PlayerPrefs.GetInt("Gold", 0);
        population = PlayerPrefs.GetInt("Population", 0);
        employed = PlayerPrefs.GetInt("Employed", 0);
    }

    // Called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements(); // Reinitialize UI references when a new scene is loaded
        UpdateUI();
    }
}