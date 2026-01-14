using UnityEngine;

public class SpeedringEffect : AsteroidAbility {
    [SerializeField] float gameSpeedJump;
    [SerializeField] int scoreIncrease;
    [SerializeField] ParticleSystem effect;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;
    }

    public override void Fire(ObstacleMovement main) {
        // On Player Collision
        manager.AlterGameSpeedBy(gameSpeedJump);
        manager.AlterScoreBy(scoreIncrease);
        effect.Play();
    }

    void Update() {
        if (manager.postGame) {
            var main = effect.main;
            main.simulationSpeed *= manager.endMultiplier;
        }
    }
}