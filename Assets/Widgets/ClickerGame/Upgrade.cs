using UnityEngine;
using TMPro;

public class IdleUpgrade : MonoBehaviour
{
    public int level = 0;
    public float cost = 5;
    public int power = 0;
    
    public string buttonName;
    public TextMeshProUGUI nameText;

    public virtual void Upgrade() {

    }

    public virtual void LoadLevel(int l) {

    }

    private void OnValidate() {
        nameText.text = buttonName;
    }
}
