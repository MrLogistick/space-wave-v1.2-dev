using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float initialGameSpeed;
    public float endMultiplier;
    public float gameSpeed { private set; get; }

    public float gameTime;
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
        Instantiate(postGameMenu);
    }
}