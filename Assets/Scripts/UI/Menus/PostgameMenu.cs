using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostgameMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI deathMsg;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] Animator highscore;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;

        if (manager.highscore > manager.gameTime) {
            highscore.SetTrigger("Play");
        }

        float minutes = manager.highscore / 60;
        float seconds = manager.highscore % 60;
        highscoreText.text = $"{minutes:00}:{seconds:00}";

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
            // Menu();
        }
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        SceneManager.LoadScene("Main Menu");
    }
}