using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonParameters : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler {

    public bool toggleable;
    public bool toggled;

    public bool newThing;
    public bool secondExclamation;

    public bool hovered;
    public bool selected;

    public GameObject pin;
    public GameObject exclamation;

    void Update() {
        if (!exclamation || secondExclamation) return;
        exclamation.SetActive(newThing);
    }

    public void Reset() {
        selected = false;
        hovered = false;
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