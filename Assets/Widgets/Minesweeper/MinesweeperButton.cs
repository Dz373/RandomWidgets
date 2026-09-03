using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MinesweeperButton : MonoBehaviour
{
    public Vector2Int pos;
    public bool revealed;
    public bool flagged;

    public Button button;
    public RightClickDetector rightClickButton;
    
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image img;

    public void InitializeButton(int r, int c) {
        pos = new Vector2Int(r, c);
        GetComponent<RectTransform>().localPosition = new Vector3(c*75, -r*75, 0);
    }

    public void RevealCell(int val) {
        if(val == -1) {
            img.color = Color.red;
            return;
        }

        if(val != 0)
            buttonText.text = val.ToString();

        img.color = Color.gray;
        revealed = true;
    }

    public void FlagCell() {
        if (flagged) {
            flagged = false;
            img.color = Color.white;
        }
        else {
            flagged = true;
            img.color = Color.green;
        }
    }
}
