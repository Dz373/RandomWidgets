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

    public void SetButton(TextMeshProUGUI output) {
        button.onClick.AddListener(() => ButtonPress(output));
    }

    private void ButtonPress(TextMeshProUGUI output) {
        print(input);
    }
}
