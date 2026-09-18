using UnityEngine;

public class ClickerButton : IdleUpgrade {

    private ClickerManager gm;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
    }

    public void ButtonPress() {
        gm.UpdateCounter(power);
    }

    public override void Upgrade() {
        if(gm.counter >= (int)cost) {
            level++;
            power++;
            
            gm.UpdateCounter(-(int)cost);

            cost *= 1.2f;
        }
    }

    public override void LoadLevel(int l) {
        level = l;
        power = l;
        cost *= Mathf.Pow(1.2f, l-1);
    }
}
