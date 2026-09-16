using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;

public class ClickerManager : MonoBehaviour
{
    public int counter = 0;
    public ClickerSaveFile save;
    public Dictionary<string, float> powerPerSecond = new Dictionary<string, float>();
    private IdleUpgrade[] upgrades;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI counterText;

    private string savePath;
    private void Start() {
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");

        //int totalSeconds = CaculateOfflineTime();
        upgrades = GetComponentsInChildren<IdleUpgrade>();

        LoadGame();
    }

    private int CalculateIdleGain() {
        float pps = 0;
        foreach (string key in powerPerSecond.Keys) {
            pps += powerPerSecond[key];
        }

        return (int)(pps * CalculateOfflineTime());
    }

    public void UpdateCounter(int amount) {
        counter += amount;
        counterText.text = counter.ToString();
    }

    public void SaveGame() {
        save.quitTime = DateTime.UtcNow.ToString();
        save.counter = counter;

        for (int i = 0; i < upgrades.Length; i++) {
            save.upgradeLevels[i] = upgrades[i].level;
        }

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadGame() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            save = JsonUtility.FromJson<ClickerSaveFile>(json);
        }
        else
            save = NewGame();

        for (int i = 0; i < upgrades.Length; i++) {
            upgrades[i].LoadLevel(save.upgradeLevels[i]);
        }

        UpdateCounter(save.counter);
    }

    public ClickerSaveFile NewGame() {
        ClickerSaveFile gameData = new ClickerSaveFile();
        gameData.upgradeLevels = new int[upgrades.Length];

        return gameData;
    }

    private int CalculateOfflineTime() {
        if (save == null) 
            return 0;

        if (DateTime.TryParse(save.quitTime, out DateTime lastQuitTime)) {
            TimeSpan ts = DateTime.UtcNow - lastQuitTime;
            
            return (int)ts.TotalSeconds;
        }

        return 0;
    }
}

[Serializable]
public class ClickerSaveFile {
    public string quitTime;
    public int[] upgradeLevels;
    public int counter = 0;
}