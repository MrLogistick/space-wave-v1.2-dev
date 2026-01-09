using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float initialGameSpeed;
    public float endMultiplier;
    public float gameSpeed { private set; get; }

    float gametime;
    public int score;
    public int highscore;
    public int attempts;

    public string deathBy;
    public bool postGame { private set; get; }

    [SerializeField] GameObject scoreTextPrefab;
    BitmapText scoreText;
    [SerializeField] GameObject postGameMenu;

    public static GameManager instance {private set; get;}

    void Awake() {
        instance = this;
    }

    void Start() {
        gameSpeed = initialGameSpeed;
        postGame = false;

        scoreText = Instantiate(
            scoreTextPrefab, 
            GameObject.FindGameObjectWithTag("MainCanvas").transform
        ).GetComponent<BitmapText>();
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        attempts = PlayerPrefs.GetInt("Attempts", 0) + 1;
        PlayerPrefs.SetInt("Attempts", attempts);
    }

    void Update() {
        scoreText.text = $"{score:0000000000}";

        if (postGame) {
            gameSpeed *= endMultiplier;
        }
        else {
            gametime += Time.deltaTime * gameSpeed;

            if (gametime >= 6f) {
                score += 1;
                gametime = 0f;
            }
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