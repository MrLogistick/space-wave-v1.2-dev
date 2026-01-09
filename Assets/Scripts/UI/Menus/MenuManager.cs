using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject titleScreen;
    public GameObject shipsMenu;
    public GameObject optionsMenu;
    
    public List<string> unlockedShips = new List<string>();
    public ShipType currentShip;
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
}