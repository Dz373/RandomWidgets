using UnityEngine;

public class ClickerButton : MonoBehaviour
{
    public int power = 1;

    private void OnMouseDown() {
        FindFirstObjectByType<ClickerManager>().UpdateCounter(power);
    }
}
