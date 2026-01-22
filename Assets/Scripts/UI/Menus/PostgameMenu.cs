using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostgameMenu : MonoBehaviour {
    [SerializeField] BitmapText deathMsg;
    [SerializeField] BitmapText speed;
    [SerializeField] BitmapText highscoreText;
    [SerializeField] Animator highscore;
    [SerializeField] GameObject newThing;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;

        if (manager.newThing) {
            newThing.SetActive(true);
        }

        if (manager.score > manager.highscore) {
            highscore.SetTrigger("Play");
            manager.highscore = manager.score;
            PlayerPrefs.SetInt("Highscore", manager.highscore);
        }

        highscoreText.text = $"{manager.highscore:0000000000}";
        speed.text = $"{manager.topSpeed:00.0}%";

        // 34 chars per line at 600 width
        var skip = AttemptMessage();
        if (skip) return;

        bool regular = Random.value > 0.5f ? true : false;
        if (regular) {
            deathMsg.text = Random.value > 0.5f ?
                "Another pilot to the cause." :
                "There are millions more dedicated to our cause.";
            return;
        }

        if (manager.aiIsSane) {
            GeneralMessages();
        }
        else {
            if (PlayerPrefs.GetInt("Sanity", 1) == 1) {
                deathMsg.text = "...";
                return;
            }

            PlayerPrefs.SetInt("Sanity", 0);
            EndGameMessages();
        }
    }

    void Update() {
        if (Keyboard.current.rKey.wasPressedThisFrame) Retry();
        if (Keyboard.current.backspaceKey.wasPressedThisFrame || Keyboard.current.mKey.wasPressedThisFrame) Menu();
    }

    public void Retry() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public void Menu() { SceneManager.LoadScene("Main Menu"); }

    bool AttemptMessage() {
        switch (manager.attempts) {
            case 1:
                deathMsg.text = Random.value > 0.5f ?
                    "Not the first to fall victim to the gravity graveyard." :
                    "This is going to take a while...";
                return true;
            case 3:
                deathMsg.text = "In the old days, you could be called a serial killer.";
                return true;
            case 50:
                deathMsg.text = "Don't mind the pilot counter, it's only for the records.";
                return true;
            case 100:
                deathMsg.text = "There was a saying... They're dropping like flies?";
                return true;
            case 250:
                deathMsg.text = "I'm reminded of a certain 20th century british doctor...";
                return true;
            case 500:
                deathMsg.text = "The great thing about space is that graves don't need to be dug.";
                return true;
            case 1000:
                deathMsg.text = "It's lucky you don't hear their voices, so they can't haunt you.";
                return true;
            case 10000:
                deathMsg.text = "Fun fact: 10000 babies are born every second in the sol system.";
                return true;
            case 100000:
                deathMsg.text = "You're really dedicated to the cause...";
                return true;
            case 999999:
                deathMsg.text = "So, you've officially tripled the gravity graveyard's kill count.";
                return true;
        }

        return false;
    }

    void GeneralMessages() {
        switch (manager.deathBy) {
            case "Gravity":
                deathMsg.text = Random.value > 0.5f ?
                    "And the pilot laughed as he fell, for he knew to fall means to once have soared." :
                    "And the pilot cried as he fell, for he knew he was not icarus.";
                break;
            case "Asteroid":
                deathMsg.text = Random.value > 0.5f ?
                    "It is the second most likely way to go in the gravity graveyard, no need to fret." :
                    "This isn't a drill you realise, you can't just put the cone back after knocking it over.";
                break;
            case "ASBroid":
                deathMsg.text = Random.value > 0.5f ?
                    "Even long-extinct civilisations attempt to hinder our progress! Do not falter!" :
                    "Pilots die to plasma bullets all the time, try mixing it up a little?";
                break;
            case "Bombroid":
                deathMsg.text = Random.value > 0.5f ?
                    "One does wonder just how these asteroids stop so suddenly." :
                    "Try swinging a tad wider next time yeah? Otherwise another pilot won't get a next time.";
                break;
            case "Shipwreck":
                deathMsg.text = Random.value > 0.5f ?
                    "Every ship we lose adds to the danger... There's a message in that somewhere." :
                    "Just goes to show that life is as expendable as material.";
                break;
            case "Megaroid":
                deathMsg.text = Random.value > 0.5f ?
                    "How does an asteroid this size stay together in this extreme game of tug-of-war?" :
                    "I thought an asteroid of this size would be hard to miss. I've been proven wrong.";
                break;
            case "Tunnelroid":
                deathMsg.text = Random.value > 0.5f ?
                    "It was a tight fit, no-one blames you. Or is it that no-one can?" :
                    "Don't tell me you got claustrophobic.";
                break;
        }
    }

    void EndGameMessages() {
        switch (manager.deathBy) {
            case "Gravity":
                deathMsg.text = Random.value > 0.5f ?
                    "And the pilot laughed as he fell, for he knew to fall means to once have soared." :
                    "And the pilot cried as he fell, for he knew he was not icarus.";
                break;
            case "Asteroid":
                deathMsg.text = Random.value > 0.5f ?
                    "It is the second most likely way to go in the gravity graveyard, no need to fret." :
                    "This isn't a drill you realise, you can't just put the cone back after knocking it over.";
                break;
            case "ASBroid":
                deathMsg.text = Random.value > 0.5f ?
                    "Even long-extinct civilisations attempt to hinder our progress! Do not falter!" :
                    "Pilots die to plasma bullets all the time, try mixing it up a little?";
                break;
            case "Bombroid":
                deathMsg.text = Random.value > 0.5f ?
                    "One does wonder just how these asteroids stop so suddenly." :
                    "Try swinging a tad wider next time yeah? Otherwise another pilot won't get a next time.";
                break;
            case "Shipwreck":
                deathMsg.text = Random.value > 0.5f ?
                    "Every ship we lose adds to the danger... There's a message in that somewhere." :
                    "Just goes to show that life is as expendable as material.";
                break;
            case "ShipwreckBullet": // AI is crazy when ships are shooting
                deathMsg.text = Random.value > 0.5f ?
                    "An overpopulation problem is fixed if everyone just dies right?" :
                    "Killed by their own creations. The tag-line for many 21st Century sci-fi movies.";
                break;
            case "Megaroid":
                deathMsg.text = Random.value > 0.5f ?
                    "How does an asteroid this size stay together in this extreme game of tug-of-war?" :
                    "I thought an asteroid of this size would be hard to miss. I've been proven wrong.";
                break;
            case "Tunnelroid":
                deathMsg.text = Random.value > 0.5f ?
                    "It was a tight fit, no-one blames you. Or is it that no-one can?" :
                    "Don't tell me you got claustrophobic.";
                break;
        }
    }
}