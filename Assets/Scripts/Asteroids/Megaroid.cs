using UnityEngine;

public class Megaroid : AsteroidBehaviour {
    [SerializeField] int maxHits;
    int hits = 0;

    [SerializeField] float radius;

    [SerializeField] Vector2 spawnVariability;
    [SerializeField] ParticleSystem hitEffect;

    protected override void OptionalOnEnable() {
        hits = 0;
    }

    protected override void OptionalDisable(bool explode) {
        if (!explode) return;

        var spawner = GetComponentInParent<AsteroidSpawner>();

        var rand = Random.Range(spawnVariability.x, spawnVariability.y);
        for (int i = 0; i < rand; i++) {
            Vector2 pos = Random.insideUnitCircle * radius;
            var roid = Random.Range(0, spawner.generalRoids.Length);
            
            GameObject roidInstance = spawner.generalRoids[i].GetComponent<ObjectPool>().GetFromPool();
            roidInstance.transform.position = (Vector2)transform.position + pos;
            roidInstance.GetComponent<AsteroidBehaviour>().pool = 
                spawner.generalRoids[i].GetComponent<ObjectPool>();
        }
    }

    protected override void HandleCollision(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.gameObject.CompareTag("Weapon")) {
            hits++;

            if (otherCol.GetComponent<SafeForPlayer>()) {
                otherCol.GetComponent<SpriteRenderer>().enabled = false;
                manager.AlterScoreBy(scoreOnDestroy);
            }
            else {
                otherCol.gameObject.SetActive(false);
            }

            DeathCheck();
        }

        if (otherCol.GetComponent<Megaroid>()) {
            hits++;
            
            DeathCheck();
        }
    }

    void DeathCheck() {
        if (hits >= maxHits) {
            Disable(true);
        }
        else {
            hitEffect.Play();
        }
    }
}