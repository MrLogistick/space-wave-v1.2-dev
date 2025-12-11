using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float initialGameSpeed;
    float gameSpeed;

    float gameTime;

    public static GameManager instance {private set; get;}

    void Awake() {
        instance = this;
    }

    void Start() {
        gameSpeed = initialGameSpeed;
    }

    void Update() {
        gameTime += Time.deltaTime;
    }

    public void AlterGameSpeedBy(float value) {
        gameSpeed += value;
    }

    public void TriggerPostGame(string deathBy) {
        
    }
}