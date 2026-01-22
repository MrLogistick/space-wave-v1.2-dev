using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float initialGameSpeed;
    [SerializeField] float speedIncrease;
    [SerializeField] float maxSpeed;
    [SerializeField] float returnRate;

    [SerializeField] float gainTime;
    [SerializeField] float midGame;
    public float endGame;

    public bool aiIsSane = true;

    public float publicMaxSpeed { private set; get; }

    float gameSpeed;
    public float rawSpeed { private set; get; }
    public float topSpeed { private set; get; }

    public float endMultiplier;

    float gametime;
    float accumulativeScore = 0;
    public int score { private set; get; }
    public int highscore;
    public int attempts;

    public string deathBy;
    public bool postGame { private set; get; } = false;

    [SerializeField] GameObject scoreTextPrefab;
    BitmapText scoreText;
    [SerializeField] GameObject pilotCounter;
    BitmapText pilotText;

    [SerializeField] GameObject postGameMenu;
    [HideInInspector] public bool newThing;

    public static GameManager instance {private set; get;}

    void Awake() {
        instance = this;
    }

    void Start() {
        gameSpeed = initialGameSpeed;
        rawSpeed = initialGameSpeed;
        publicMaxSpeed = maxSpeed;

        aiIsSane = PlayerPrefs.GetInt("Sanity", 1) == 1 ? true : false;

        scoreText = Instantiate(
            scoreTextPrefab, 
            GameObject.FindGameObjectWithTag("MainCanvas").transform
        ).GetComponent<BitmapText>();
        pilotText = Instantiate(
            pilotCounter, 
            GameObject.FindGameObjectWithTag("MainCanvas").transform
        ).GetComponent<BitmapText>();


        highscore = PlayerPrefs.GetInt("Highscore", 0);

        attempts = PlayerPrefs.GetInt("Attempts", 0) + 1;
        PlayerPrefs.SetInt("Attempts", attempts);
    }

    void Update() {
        scoreText.text = $"{score:0000000000}";
        pilotText.text = $"Pilot {attempts:000000}";
        
        topSpeed = Mathf.Max(gameSpeed, topSpeed);

        if (topSpeed >= endGame) {
            aiIsSane = false;
        }
        
        if (postGame) {
            gameSpeed *= endMultiplier;
            rawSpeed *= endMultiplier;
        }
        else {
            gametime += Time.deltaTime;
            if (gametime >= 0.1f) {
                accumulativeScore += rawSpeed / 100;
                gametime = 0;
            }

            if (accumulativeScore >= 1f) {
                accumulativeScore = 0f;
                score += 1;
            }

            gameSpeed += Time.deltaTime * speedIncrease;
            float desiredSpeed;
            
            if (gameSpeed < midGame) {
                desiredSpeed = gameSpeed;
            }
            else {
                desiredSpeed = maxSpeed - (maxSpeed - midGame) * Mathf.Exp(-(gameSpeed - midGame) / gainTime);

                if (PlayerPrefs.GetInt("Hermes_Unlocked", 0) == 0) {
                    PlayerPrefs.SetInt("Hermes_Unlocked", 1);
                    newThing = true;
                }
            }

            if (rawSpeed > desiredSpeed) {
                rawSpeed -= Time.deltaTime * returnRate;
            } else {
                rawSpeed = Mathf.Min(desiredSpeed, maxSpeed);
            }
        }
    }

    public void AlterGameSpeedBy(float value) {
        gameSpeed += value;
        rawSpeed += value;
    }

    public void AlterScoreBy(int value) {
        score += value;

        if (score < 0) {
            score = 0;
        }
    }

    public void TriggerPostGame(string value) {
        postGame = true;
        deathBy = value;

        Instantiate(postGameMenu, GameObject.FindGameObjectWithTag("MainCanvas").transform);
    }
}