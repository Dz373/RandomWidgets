using UnityEngine;
using TMPro;

public class IdleClicker : IdleUpgrade {
    public int power = 0;
    public int cost = 5;
    public int powerPerLevel = 1;
    public float interval = 5;
    public string buttonName;

    private float curTime = 0;
    private ClickerManager gm;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RectTransform progressBar;

    private void Awake() {
        gm = FindFirstObjectByType<ClickerManager>();
        gm.powerPerSecond.Add(buttonName, GetPowerPerSecond());
    }

    private void OnValidate() {
        nameText.text = buttonName;
    }

    private void Update() {
        if (level > 0) {
            curTime += Time.deltaTime;
            progressBar.localScale = new Vector3(curTime/interval, 1, 1);

            if (curTime >= interval) {
                gm.UpdateCounter(power);
                curTime = 0;
            }
        }
    }

    public override void Upgrade() {
        if (gm.counter >= cost) {
            level++;
            power += powerPerLevel;

            gm.UpdateCounter(-cost);
            cost = (int)(cost * 1.2);
        }
    }

    public float GetPowerPerSecond() {
        return power / interval;
    }

    public override void LoadLevel(int l) {
        level = l;
        power = l * powerPerLevel;
        cost = (int)(cost * Mathf.Pow(1.25f, l));
    }
}
