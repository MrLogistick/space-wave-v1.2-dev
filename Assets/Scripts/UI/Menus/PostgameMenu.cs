using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostgameMenu : MonoBehaviour {
    [SerializeField] BitmapText deathMsg;
    [SerializeField] BitmapText speed;
    [SerializeField] BitmapText highscoreText;
    [SerializeField] Animator highscore;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;

        if (manager.score > manager.highscore) {
            highscore.SetTrigger("Play");
            manager.highscore = manager.score;
            PlayerPrefs.SetInt("Highscore", manager.highscore);
        }

        highscoreText.text = $"{manager.highscore:0000000000}";
        speed.text = $"{manager.topSpeed:00.0}%";

        // 34 chars per line at 600 width
        var skip = false;
        switch (manager.attempts) {
            case 1:
                deathMsg.text = Random.value > 0.5f ?
                    "Not the first to fall victim to   the gravity graveyard." :
                    "";
                skip = true;
                break;
        }

        if (skip) return;

        switch (manager.deathBy) {
            case "Gravity":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "Asteroid":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "ASBroid":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "Bombroid":
                deathMsg.text = Random.value > 0.5f ?
                    "Orange is not always the friendly colour." :
                    "Wrong wire! Too late...";
                break;
            case "Shipwreck":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "ShipwreckBullet":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "Megaroid":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
            case "Tunnelroid":
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
        }
    }

    void Update() {
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            Retry();
        }

        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.mKey.wasPressedThisFrame) {
            Menu();
        }
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        SceneManager.LoadScene("Main Menu");
    }
}