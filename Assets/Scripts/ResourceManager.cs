using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    // Resource variables
    public int wood;
    public int stone;
    public int gold;
    public int defence;
    public int raiders;

    public int employed;
    public int unemployed ;
    public int population;

    // UI Texts
    public Text woodText;
    public Text stoneText;
    public Text goldText;
    public Text populationText;
    public Text defenceText;
    public Text raidersText;
    public Text unemployedText;
    public Text employedText;
    //public Text differenceText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        PreventNegativeResources();
        UpdateUI();
    }


    // Method to calculate employed and unemployed
    void PreventNegativeResources()
    {
        if (unemployed < 0) unemployed = 0; // Prevent negative unemployed
    }
    // Method to update UI text
    void UpdateUI()
    {
        woodText.text = "Wood: " + wood;
        stoneText.text = "Stone: " + stone;
        goldText.text = "Gold: " + gold;
        populationText.text = "Population: " + population;
        defenceText.text = "" + defence;
        raidersText.text = "" + raiders;
        employedText.text = "Employed: " + (employed);
        unemployedText.text = "Unemployed: " + (population - employed);

        //differenceText.text = "Difference: " + (employed - unemployed);
    }

    // Methods to manipulate resources (linked to buttons)
    public void AddWood() { wood += 5; }
    public void AddStone() { stone += 5; }
    public void AddGold() { gold += 5; }
    public void AddDefence() { defence += 1; }
    public void AddRaiders() { raiders += 1; }
    public void AddPopulation() { population += 1; }
    public void AddEmployed() { employed += 1; }

    //public void AddRaiders() { raiders += 1; }

    //public void AddUnemployed() { unemployed += 1; } // Directly increase unemployed

    public void RemoveWood() { if (wood > 0) wood -= 10; }
    public void RemoveStone() { if (stone > 0) stone -= 10; }
    public void RemoveGold() { if (gold > 0) gold -= 10; }
    public void RemoveDefence() { if (defence > 0) defence -= 1; }
    public void RemoveRaiders() { if (raiders > 0) raiders -= 1; }
    public void RemovePopulation() { if (population > 0) population -= 1; }
    public void RemoveEmployed() { if (employed > 0) employed -= 1; }

    //public void RemoveUnemployed() { if (unemployed > 0) unemployed -= 1; }
}
