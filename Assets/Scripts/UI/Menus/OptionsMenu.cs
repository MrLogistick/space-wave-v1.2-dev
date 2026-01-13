using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour {
    public ButtonParameters[] buttons;
    // Music 0
    // SFX 1
    // Tutorial 2
    // Erase Data 3
    // Static Start 4
    // Fullscreen 5
    // Terminate Companion 6
    // Back 7

    Color selected = new Color32(255, 70, 0, 255);
    Color normal = Color.white;

    [SerializeField] Sprite toggleOn;
    [SerializeField] Sprite toggleOff;

    BitmapText musicVol;
    BitmapText sfxVol;

    MenuManager manager;

    void Start() {
        musicVol = buttons[0].GetComponentInChildren<BitmapText>();
        sfxVol = buttons[1].GetComponentInChildren<BitmapText>();

        manager = GetComponentInParent<MenuManager>();
    }

    void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update() {
        musicVol.text = $": {PlayerPrefs.GetInt("Music", 8):00}";
        sfxVol.text = $": {PlayerPrefs.GetInt("SFX", 6):00}";

        foreach (ButtonParameters bp in buttons) {
            var image = bp.pin.GetComponent<Image>();

            if (bp.toggled) {
                bp.pin.SetActive(true);
                image.sprite = toggleOn;
            }
            else if (bp.toggleable) {
                bp.pin.SetActive(true);
                image.sprite = toggleOff;
            }

            if (bp.selected) {
                bp.pin.SetActive(true);
                image.color = selected;
            }
            else if (bp.hovered) {
                bp.pin.SetActive(true);
                image.color = selected;

                EventSystem.current.SetSelectedGameObject(null);

                if (bp.exclamation) { bp.newThing = false; }
            }
            else {
                if (bp.toggleable) {
                    image.color = normal;
                }
                else {
                    bp.pin.SetActive(false);
                }
            }
        }

        if (Keyboard.current.mKey.wasPressedThisFrame) { Music(); }
        if (Keyboard.current.xKey.wasPressedThisFrame) { SFX(); }
        if (Keyboard.current.sKey.wasPressedThisFrame) { StaticStart(0); }
        if (Keyboard.current.tKey.wasPressedThisFrame) { Tutorial(0); }
        if (Keyboard.current.eKey.wasPressedThisFrame) { EraseData(); }
        if (Keyboard.current.fKey.wasPressedThisFrame) { Fullscreen(0); }
        if (Keyboard.current.cKey.wasPressedThisFrame) { TerminateCompanion(0); }
        if (Keyboard.current.bKey.wasPressedThisFrame) { Back(); }
    }

    public void Music() {
        var currentVol = PlayerPrefs.GetInt("Music", 8);

        if (currentVol == 10) {
            PlayerPrefs.SetInt("Music", 0);
        }
        else {
            PlayerPrefs.SetInt("Music", currentVol + 1);
        }
    }

    public  void SFX() {
        var currentVol = PlayerPrefs.GetInt("SFX", 6);

        if (currentVol == 10) {
            PlayerPrefs.SetInt("SFX", 0);
        }
        else {
            PlayerPrefs.SetInt("SFX", currentVol + 1);
        }
    }

    public void Tutorial(int specific) { // Toggleable
        var b = buttons[2];
        if (specific == 0) {
            b.toggled = !b.toggled;
        }
        else {
            b.toggled = specific > 1 ? true : false;
        }

        PlayerPrefs.SetInt("Tutorial", b.toggled ? 1 : 0);
    }

    public void EraseData() {
        if (buttons[3].exclamation.activeSelf) {
            buttons[3].exclamation.SetActive(false);
            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetInt("Athena_Unlocked", 2);
            Tutorial(2);
            StaticStart(1);
            TerminateCompanion(1);
        }
        else {
            buttons[3].exclamation.SetActive(true);
        }
    }

    public void StaticStart(int specific) { // Toggleable
        var b = buttons[4];
        if (specific == 0) {
            b.toggled = !b.toggled;
        }
        else {
            b.toggled = specific > 1 ? true : false;
        }

        PlayerPrefs.SetInt("StaticStart", b.toggled ? 1 : 0);
    }

    public void Fullscreen(int specific) { // Toggleable
        var b = buttons[5];
        if (specific == 0) {
            b.toggled = !b.toggled;
        }
        else {
            b.toggled = specific > 1 ? true : false;
        }

        PlayerPrefs.SetInt("Fullscreen", b.toggled ? 1 : 0);
    }

    public void TerminateCompanion(int specific) { // Toggleable
        var b = buttons[6];
        if (specific == 0) {
            b.toggled = !b.toggled;
        }
        else {
            b.toggled = specific > 1 ? true : false;
        }

        PlayerPrefs.SetInt("Companion", b.toggled ? 1 : 0);
    }

    public void Back() {
        buttons[7].Reset();
        manager.titleScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}