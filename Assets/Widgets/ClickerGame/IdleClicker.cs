using UnityEngine;
using TMPro;

public class IdleClicker : MonoBehaviour
{
    public int power = 1;
    public int cost = 5;
    public int level = 0;
    public float interval = 5;
    public string buttonName;

    private float curTime = 0;
    private ClickerManager gm;

    [SerializeField] private TextMeshProUGUI nameText;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
    }

    private void OnValidate() {
        nameText.text = buttonName;
    }

    private void Update() {
        if (level > 0) {
            curTime += Time.deltaTime;
            if (curTime >= interval) {
                gm.UpdateCounter(power);
                curTime = 0;
            }
        }
    }

    public void Upgrade() {
        if (gm.counter >= cost) {
            level++;

            if (level > 1) {
                power++;
            }

            gm.UpdateCounter(-cost);
            cost = (int)(cost * 1.2);
        }
    }
}
