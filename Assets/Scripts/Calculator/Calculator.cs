using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private Transform buttons;

    private void Awake() {
        foreach (InputButton b in buttons.GetComponentsInChildren<InputButton>()) {
            b.SetButton(textOutput);
        }
    }

}
