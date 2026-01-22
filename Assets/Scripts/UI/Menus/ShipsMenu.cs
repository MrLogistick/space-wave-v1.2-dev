using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShipsMenu : MonoBehaviour {
    public ButtonParameters[] buttons;
    MenuManager manager;

    void Start() {
        manager = GetComponentInParent<MenuManager>();
    }

    void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update() {
        foreach (ButtonParameters bp in buttons) {
            if (bp.selected) {
                bp.pin.SetActive(true);
            }
            else if (bp.hovered) {
                bp.pin.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);

                if (bp.exclamation) { bp.newThing = false; }
            }
            else if (manager.currentShip.ToString() == bp.name) {
                bp.pin.SetActive(true);
            }
            else {
                bp.pin.SetActive(false);
            }
        }

        if (Keyboard.current.tKey.wasPressedThisFrame && buttons[0].isActiveAndEnabled) { Athena(); }
        if (Keyboard.current.hKey.wasPressedThisFrame && buttons[1].isActiveAndEnabled) { Hermes(); }
        if (Keyboard.current.zKey.wasPressedThisFrame && buttons[2].isActiveAndEnabled) { Zeus(); }
        if (Keyboard.current.eKey.wasPressedThisFrame && buttons[3].isActiveAndEnabled) { Hephaetsus(); }
        if (Keyboard.current.aKey.wasPressedThisFrame && buttons[4].isActiveAndEnabled) { Ares(); }
        if (Keyboard.current.pKey.wasPressedThisFrame && buttons[5].isActiveAndEnabled) { Poseidon(); }
        if (Keyboard.current.rKey.wasPressedThisFrame && buttons[6].isActiveAndEnabled) { Aphrodite(); }
        if (Keyboard.current.dKey.wasPressedThisFrame && buttons[7].isActiveAndEnabled) { Dionysus(); }
        if (Keyboard.current.mKey.wasPressedThisFrame && buttons[8].isActiveAndEnabled) { Artemis(); }
        if (Keyboard.current.bKey.wasPressedThisFrame && buttons[9].isActiveAndEnabled) { Back(); }
    }

    public void Athena() { PlayerPrefs.SetInt("CurrentShip", 0); }
    public void Hermes() { PlayerPrefs.SetInt("CurrentShip", 1); }
    public void Zeus() { PlayerPrefs.SetInt("CurrentShip", 2); }
    public void Hephaetsus() { PlayerPrefs.SetInt("CurrentShip", 3); }
    public void Ares() { PlayerPrefs.SetInt("CurrentShip", 4); }
    public void Poseidon() { PlayerPrefs.SetInt("CurrentShip", 5); }
    public void Aphrodite() { PlayerPrefs.SetInt("CurrentShip", 6); }
    public void Dionysus() { PlayerPrefs.SetInt("CurrentShip", 7); }
    public void Artemis() { PlayerPrefs.SetInt("CurrentShip", 8); }

    public void Back() {
        buttons[9].Reset();
        manager.titleScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}