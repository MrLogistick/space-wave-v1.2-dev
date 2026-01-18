using UnityEngine;

public class Speedring : AsteroidBehaviour {
    [SerializeField] float gameSpeedJump;
    [SerializeField] int scoreIncrease;
    [SerializeField] ParticleSystem passEffect;

    protected override void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.gameObject.CompareTag("Weapon")) {
            if (otherCol.GetComponent<SafeForPlayer>()) manager.AlterScoreBy(scoreOnDestroy);
            Disable(true);
        }

        var otherRoid = otherCol.GetComponent<AsteroidBehaviour>();
        if (otherRoid != null) {
            if (otherRoid.roidStrength > roidStrength) {
                Disable(true);
            }
        }

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