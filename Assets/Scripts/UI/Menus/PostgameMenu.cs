using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostgameMenu : MonoBehaviour {
    [SerializeField] BitmapText deathMsg;
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

        switch (manager.attempts) {
            default:
                break;
            case 1:
                deathMsg.text = Random.value > 0.5f ?
                    "" :
                    "";
                break;
        }

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
            case "Shipwreck":
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