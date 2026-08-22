using UnityEngine;
using TMPro;

public class ClickerManager : MonoBehaviour
{
    public int counter = 0;

    [SerializeField] private TextMeshProUGUI counterText;

    public void UpdateCounter(int amount) {
        counter += amount;
        counterText.text = counter.ToString();
    }
}
