using UnityEngine;

public class Speedring : AsteroidBehaviour {
    [SerializeField] float gameSpeedJump;
    [SerializeField] int scoreIncrease;
    [SerializeField] ParticleSystem passEffect;

    protected override void OptionalCollisions(Collider2D thisCol, Collider2D otherCol, int index) {
        if (!otherCol.gameObject.CompareTag("Player")) return;

        manager.AlterGameSpeedBy(gameSpeedJump);
        manager.AlterScoreBy(scoreIncrease);
        passEffect.Play();
    }

    protected override void OptionalUpdate() {
        if (!manager.postGame) return;

        var main = passEffect.main;
        main.simulationSpeed *= manager.endMultiplier;
    }
}