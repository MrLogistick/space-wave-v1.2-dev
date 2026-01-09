using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
    [SerializeField]  ButtonParameters[] buttons;

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

        if (Keyboard.current.lKey.wasPressedThisFrame) {
            Launch();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame) {
            Endless();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame) {
            Ships();
        }

        if (Keyboard.current.oKey.wasPressedThisFrame) {
            Options();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame) {
            Quit();
        }
    }

    public void Launch() {
        SceneManager.LoadScene("Gameplay");
    }

    public void Endless() {
        SceneManager.LoadScene("Gameplay");
    }

    public void Ships() {
        GetComponentInParent<MenuManager>().shipsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Options() {
        GetComponentInParent<MenuManager>().optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }
}