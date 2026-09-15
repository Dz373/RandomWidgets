using UnityEngine;

public class ClickerButton : IdleUpgrade {
    public int power = 1;
    public int cost = 5;

    private ClickerManager gm;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
    }

    public void ButtonPress() {
        gm.UpdateCounter(power);
    }

    public override void Upgrade() {
        if(gm.counter >= cost) {
            level++;
            power++;
            
            gm.UpdateCounter(-cost);

            cost = (int)(cost * 1.2);
        }
    }
}
