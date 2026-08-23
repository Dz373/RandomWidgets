using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;

public class ClickerManager : MonoBehaviour
{
    public int counter = 0;

    [SerializeField] private TextMeshProUGUI counterText;

    private void Start() {
        Debug.Log(CaculateOfflineTime().ToString());
    }

    public void UpdateCounter(int amount) {
        counter += amount;
        counterText.text = counter.ToString();
    }

    public void SaveGame() {
        PlayerPrefs.SetString("QuitTime", DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    private double CaculateOfflineTime() {
        if (!PlayerPrefs.HasKey("QuitTime")) 
            return 0;

        string quitTime = PlayerPrefs.GetString("QuitTime");

        if (DateTime.TryParse(quitTime, out DateTime lastQuitTime)) {
            TimeSpan ts = DateTime.UtcNow - lastQuitTime;
            
            return ts.TotalSeconds;
        }

        return 0;
    }
}