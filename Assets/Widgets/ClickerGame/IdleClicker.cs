using UnityEngine;
using TMPro;

public class IdleClicker : IdleUpgrade {
    public int powerPerLevel = 1;
    public float interval = 5;
    public string buttonName;

    private float curTime = 0;
    private ClickerManager gm;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RectTransform progressBar;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
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
        if (gm.counter >= (int)cost) {
            level++;
            power += powerPerLevel;

            gm.UpdateCounter(-(int)cost);
            cost *= 1.2f;
        }
    }

    public float GetPowerPerSecond() {
        return power / interval;
    }

    public override void LoadLevel(int l) {
        level = l;
        power = l * powerPerLevel;
        cost *= Mathf.Pow(1.2f, l);
    }
}
