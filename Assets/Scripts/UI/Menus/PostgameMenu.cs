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
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "Not the first to fall victim to   the gravity graveyard." :
                    "This is going to take a while...";
                skip = true;
                break;
        }

        if (skip) return;

        switch (manager.deathBy) {
            case "Gravity":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "And the pilot laughed as he fell, for he knew to fall means to once have soared." :
                    "Falling prolongs the pilot's agony you realise.";
                break;
            case "Asteroid":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "The pilots have faith in your     abilities guide, remember that." :
                    "Only the second most likely way to go in the gravity graveyard.";
                break;
            case "ASBroid":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "Even long-extinct civilisations   attempt to hinder our progress!  Do not falter!" :
                    "Pilots die to plasma bullets all  the time, try mixing it up a     little?";
                break;
            case "Bombroid":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "One does wonder just how these    asteroids stop so suddenly." :
                    "Try swinging a tad wider next timeyeah? Otherwise another pilot     won't get a next time.";
                break;
            case "Shipwreck":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "Every ship we lose adds to the    danger... There's a message in   that somewhere." :
                    "Just goes to show that life is as expendable as material.";
                break;
            case "ShipwreckBullet": // AI is crazy when ships are shooting
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "An overpopulation problem is fixedif everyone just dies right?" :
                    "Killed by their own creations. Thetag-line for many 21st Century   sci-fi movies.";
                break;
            case "Megaroid":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "How does an asteroid this size    stay together in this extreme    game of tug-of-war?" :
                    "I thought an asteroid of this sizewould be hard to miss. I've been proven wrong.";
                break;
            case "Tunnelroid":
                deathMsg.text = Random.value > 0.5f ?  //                               //
                    "It was a tight fit, no-one blames you. Or is it that no-one can?" :
                    "Don't tell me you got             claustrophobic.";
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