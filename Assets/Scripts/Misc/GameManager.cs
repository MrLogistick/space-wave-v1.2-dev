using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float initialGameSpeed;
    public float endMultiplier;
    public float gameSpeed { private set; get; }

    public float gameTime;
    public float highscore;
    public int attempts;

    public string deathBy;
    public bool postGame { private set; get; }

    [SerializeField] GameObject postGameMenu;

    public static GameManager instance {private set; get;}

    void Awake() {
        instance = this;
    }

    void Start() {
        gameSpeed = initialGameSpeed;
        postGame = false;

        attempts = PlayerPrefs.GetInt("Attempts", 0) + 1;
        PlayerPrefs.SetInt("Attempts", attempts);
    }

    void Update() {
        if (postGame) {
            gameSpeed *= endMultiplier;
        }
        else {
            gameTime += Time.deltaTime;
        }
    }

    public void AlterGameSpeedBy(float value) {
        gameSpeed += value;
    }

    public void TriggerPostGame(string value) {
        postGame = true;
        deathBy = value;

        Instantiate(postGameMenu, GameObject.FindGameObjectWithTag("MainCanvas").transform);
    }
}