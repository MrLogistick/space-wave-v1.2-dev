using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantObject : MonoBehaviour {
    static PersistantObject instance;

    [SerializeField] GameObject player;
    [SerializeField] ShipData[] ships;
    public int selectedShip;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        Screen.fullScreenMode = PlayerPrefs.GetInt("Fullscreen", 1) == 1
            ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    void GameStart() {
        var playerInstance = Instantiate(player, player.transform.position, player.transform.rotation);
        playerInstance.GetComponent<PlayerController>().data = ships[selectedShip];
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Gameplay") GameStart();
    }
}