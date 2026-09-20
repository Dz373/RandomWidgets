using UnityEngine;

public class ClickerButton : IdleUpgrade {

    private ClickerManager gm;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
    }

    public void ButtonPress() {
        gm.UpdateCounter(power*multiplier);
    }

    public override void Upgrade() {
        if(gm.counter >= (int)cost) {
            level++;
            power++;
            multiplier = (int)(level / 25) + 1;
            
            gm.UpdateCounter(-(int)cost);

            cost *= 1.2f;

            nameText.text = buttonName + " Lvl: " + level;
        }
    }

    public override void LoadLevel(int l) {
        level = l;
        power = l;
        multiplier = (int)(l / 25) + 1;
        cost *= Mathf.Pow(1.2f, l-1);

        if(level > 0)
            nameText.text = buttonName + " Lvl: " + level;
    }
}
