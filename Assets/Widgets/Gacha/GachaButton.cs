using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GachaButton : MonoBehaviour
{
    public List<string> commonItems = new List<string>();
    public List<string> rareItems = new List<string>();
    public List<string> superrareItems = new List<string>();
    public List<string> legendaryItems = new List<string>();
    
    public Dictionary<float, string> dropChance = new Dictionary<float, string>();

    public int testAmount;
    public Dictionary<string, int> testDrops = new Dictionary<string, int>();

    public TextMeshProUGUI resultText;

    private void Start() {
        dropChance.Add(0.01f, "Legendary");
        dropChance.Add(0.15f, "SuperRare");
        dropChance.Add(0.30f, "Rare");
        dropChance.Add(0.54f, "Common");
    }

    private void TestRolls() {
        testDrops.Clear();

        for (int i = 0; i < testAmount; i++) {
            string pull = GachaRoll();
            if (testDrops.ContainsKey(pull))
                testDrops[pull] += 1;
            else
                testDrops.Add(pull, 1);
        }

        foreach (string key in testDrops.Keys) {
            Debug.Log(key + ": " + testDrops[key]);
        }
    }

    private void OnMouseDown() {
        TestRolls();
    }

    private string GachaRoll() {
        float val = Random.Range(0f, 1f);
        float totalRoll = 0;
        foreach (float chance in dropChance.Keys) {
            totalRoll += chance;
            if (val < totalRoll) {
                val = chance;
                break;
            }
        }

        List<string> items = null;
        switch (dropChance[val]) {
            case "Legendary":
                items = legendaryItems;
                break;

            case "SuperRare":
                items = superrareItems;
                break;

            case "Rare":
                items = rareItems;
                break;

            case "Common":
                items = commonItems;
                break;
        }

        return items[(int)Random.Range(0, items.Count)];
    }
}
