using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;

public class ClickerManager : MonoBehaviour
{
    public int counter = 0;

    public Dictionary<string, float> powerPerSecond = new Dictionary<string, float>();

    [SerializeField] private TextMeshProUGUI counterText;

    private void Start() {
        //int totalSeconds = CaculateOfflineTime();
        print(CalculateIdleGain());
    }

    private int CalculateIdleGain() {
        float total = 0;
        foreach (string key in powerPerSecond.Keys) {
            total += powerPerSecond[key];
        }
        
        return (int)total;
    }

    public void UpdateCounter(int amount) {
        counter += amount;
        counterText.text = counter.ToString();
    }

    public void SaveGame() {
        PlayerPrefs.SetString("QuitTime", DateTime.UtcNow.ToString());
        
        PlayerPrefs.Save();
    }

    private int CaculateOfflineTime() {
        if (!PlayerPrefs.HasKey("QuitTime")) 
            return 0;

        string quitTime = PlayerPrefs.GetString("QuitTime");

        if (DateTime.TryParse(quitTime, out DateTime lastQuitTime)) {
            TimeSpan ts = DateTime.UtcNow - lastQuitTime;
            
            return (int)ts.TotalSeconds;
        }

        return 0;
    }
}