using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
    public ButtonParameters[] buttons;
    // Launch 0
    // Endless 1
    // Ships 2
    // Options 3
    // Quit 4

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

        if (Keyboard.current.lKey.wasPressedThisFrame) { Launch(); }
        if (Keyboard.current.eKey.wasPressedThisFrame) { Endless(); }
        if (Keyboard.current.sKey.wasPressedThisFrame) { Ships(); }
        if (Keyboard.current.oKey.wasPressedThisFrame) { Options(); }
        if (Keyboard.current.qKey.wasPressedThisFrame) { Quit(); }
    }

    public void Launch() {
        SceneManager.LoadScene("Gameplay");
    }

    public void Endless() {
        SceneManager.LoadScene("Gameplay");
    }

    public void Ships() {
        buttons[2].Reset();
        GetComponentInParent<MenuManager>().shipsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Options() {
        buttons[3].Reset();
        GetComponentInParent<MenuManager>().optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit() {
        if (buttons[4].exclamation.activeSelf) {
            buttons[4].exclamation.SetActive(false);
            Application.Quit();
        }
        else {
            buttons[4].exclamation.SetActive(true);
        }
    }
}