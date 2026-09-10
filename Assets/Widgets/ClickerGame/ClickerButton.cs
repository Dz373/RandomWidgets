using UnityEngine;

public class ClickerButton : MonoBehaviour
{
    public int power = 1;
    public int cost = 5;
    public int level = 0;

    private ClickerManager gm;

    private void Start() {
        gm = FindFirstObjectByType<ClickerManager>();
    }

    private void OnMouseDown() {
        gm.UpdateCounter(power);
    }

    public void Upgrade() {
        if(gm.counter >= cost) {
            level++;
            power++;
            
            gm.UpdateCounter(-cost);

            cost = (int)(cost * 1.2);
        }
    }
}
