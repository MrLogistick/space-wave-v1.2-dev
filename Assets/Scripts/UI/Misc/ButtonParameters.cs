using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonParameters : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler {

    public bool newThing;
    public bool hovered;
    public bool selected;
    public GameObject pin;
    public GameObject excalmation;

    void Update() {
        if (!excalmation) return;
        excalmation.SetActive(newThing);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        hovered = false;
    }

    public void OnSelect(BaseEventData eventData) {
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData) {
        selected = false;
    }
}