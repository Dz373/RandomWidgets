using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public string input;
    public Button button;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void OnValidate() {
        buttonText.text = input;
    }
}
