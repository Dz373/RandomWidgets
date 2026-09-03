using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RightClickDetector : MonoBehaviour, IPointerClickHandler {
    public UnityEvent onRightClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            onRightClick.Invoke();
        }
    }
}