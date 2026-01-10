using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject titleScreen;
    public GameObject shipsMenu;
    public GameObject optionsMenu;
    TitleScreen titleScript;
    ShipsMenu shipsScript;
    OptionsMenu optionsScript;

    public ShipType currentShip = ShipType.Athena;
    ShipType[] allShips;
    public enum ShipType {
        Athena,
        Hermes,
        Zeus,
        Hephaetsus,
        Ares,
        Poseidon,
        Aphrodite,
        Dionysus,
        Artemis
    }

    GameObject persistantObject;

    void Awake() {
        allShips = (ShipType[])System.Enum.GetValues(typeof(ShipType));
        persistantObject = GameObject.FindGameObjectWithTag("Persistant");
    }

    void Start() {
        titleScript = titleScreen.GetComponent<TitleScreen>();
        shipsScript = shipsMenu.GetComponent<ShipsMenu>();
        optionsScript = optionsMenu.GetComponent<OptionsMenu>();

        PlayerPrefs.SetInt("Athena_Unlocked", 2);
        currentShip = (ShipType)PlayerPrefs.GetInt("PreviousShip", 0);

        ShipsMenuExclamation();
        OptionsMenuExclamation();
        TitleScreenExclamation();
    }

    void Update() {
        var music = persistantObject.GetComponent<AudioSource>();
        music.volume = (float)PlayerPrefs.GetInt("Music", 8) / 10;
    }

    void TitleScreenExclamation() {
        
        if (PlayerPrefs.GetInt("Complete", 0) > 0) {
            if (PlayerPrefs.GetInt("Complete", 0) == 2)
                return;
            
            PlayerPrefs.SetInt("Complete", 2);
            titleScript.buttons[1].gameObject.SetActive(true);
            titleScript.buttons[1].newThing = true;
        }
        else {
            titleScript.buttons[1].gameObject.SetActive(false);
        }

        // foreach (var button in shipsScript.buttons) {
        //     if (button.newThing) {
        //         titleScript.buttons[2].newThing = true;
        //         return;
        //     }
        // }
        
        titleScript.buttons[2].newThing = shipsScript.buttons.Any(b => b.newThing);
        titleScript.buttons[3].newThing = optionsScript.buttons[6].newThing;
    }

    void ShipsMenuExclamation() {

        foreach (ShipType ship in allShips) {
            if (PlayerPrefs.GetInt($"{ship}_Unlocked", 0) == 0) {
                shipsScript.buttons[(int)ship].gameObject.SetActive(false);
                continue;
            }

            if (PlayerPrefs.GetInt($"{ship}_Unlocked", 0) == 2)
                continue;

            PlayerPrefs.SetInt($"{ship}_Unlocked", 2);
            shipsScript.buttons[(int)ship].gameObject.SetActive(true);
            shipsScript.buttons[(int)ship].newThing = true;
        }
    }

    void OptionsMenuExclamation() {
        if (PlayerPrefs.GetInt("Complete", 0) > 0) {
            if (PlayerPrefs.GetInt("Complete", 0) == 2) return;

            PlayerPrefs.SetInt("Complete", 2);
            optionsScript.buttons[6].gameObject.SetActive(true);
            optionsScript.buttons[6].newThing = true;
        }
        else {
            optionsScript.buttons[6].gameObject.SetActive(false);
        }
    }
}