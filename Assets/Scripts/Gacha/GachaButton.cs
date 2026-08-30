using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GachaButton : MonoBehaviour
{
    public int testAmount;
    public TextMeshProUGUI resultText;
    public Dictionary<float, string> dropChance = new Dictionary<float, string>();
    public Dictionary<string, int> testDrops = new Dictionary<string, int>();

    private void Start() {
        dropChance.Add(0.01f, "Legendary");
        dropChance.Add(0.15f, "Super Rare");
        dropChance.Add(0.30f, "Rare");
        dropChance.Add(0.54f, "Common");
    }

    private void TestRolls() {
        testDrops.Clear();
        testDrops.Add("Legendary", 0);
        testDrops.Add("Super Rare", 0);
        testDrops.Add("Rare", 0);
        testDrops.Add("Common", 0);

        for (int i = 0; i < testAmount; i++) {
            testDrops[GachaRoll()] += 1;
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

        return dropChance[val];
    }
}
