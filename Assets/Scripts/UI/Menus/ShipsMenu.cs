using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ShipsMenu : MonoBehaviour {
    [SerializeField]  ButtonParameters[] buttons;
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
            }
            else {
                bp.pin.SetActive(false);
            }
        }

        if (Keyboard.current.tKey.wasPressedThisFrame) { Athena(); }
        if (Keyboard.current.hKey.wasPressedThisFrame) { Hermes(); }
        if (Keyboard.current.zKey.wasPressedThisFrame) { Zeus(); }
        if (Keyboard.current.eKey.wasPressedThisFrame) { Hephaetsus(); }
        if (Keyboard.current.aKey.wasPressedThisFrame) { Ares(); }
        if (Keyboard.current.pKey.wasPressedThisFrame) { Poseidon(); }
        if (Keyboard.current.rKey.wasPressedThisFrame) { Aphrodite(); }
        if (Keyboard.current.dKey.wasPressedThisFrame) { Dionysus(); }
        if (Keyboard.current.mKey.wasPressedThisFrame) { Artemis(); }
        if (Keyboard.current.bKey.wasPressedThisFrame) { Back(); }
    }

    public void Athena() { manager.currentShip = MenuManager.ShipType.Athena; }
    public void Hermes() { manager.currentShip = MenuManager.ShipType.Hermes; }
    public void Zeus() { manager.currentShip = MenuManager.ShipType.Zeus; }
    public void Hephaetsus() { manager.currentShip = MenuManager.ShipType.Hephaetsus; }
    public void Ares() { manager.currentShip = MenuManager.ShipType.Ares; }
    public void Poseidon() { manager.currentShip = MenuManager.ShipType.Poseidon; }
    public void Aphrodite() { manager.currentShip = MenuManager.ShipType.Aphrodite; }
    public void Dionysus() { manager.currentShip = MenuManager.ShipType.Dionysus; }
    public void Artemis() { manager.currentShip = MenuManager.ShipType.Artemis; }

    public void Back() {
        manager.titleScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}