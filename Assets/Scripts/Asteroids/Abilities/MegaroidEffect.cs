using UnityEngine;

public class MegaroidEffect : AsteroidAbility {
    [SerializeField] int maxHits;
    int hits;

    [SerializeField] Vector2 spawnVariability;
    [SerializeField] ParticleSystem hitEffect;

    public override void Fire(ObstacleMovement main) {
        var spawner = GetComponentInParent<AsteroidSpawner>();

        var rand = Random.Range(spawnVariability.x, spawnVariability.y);
        for (int i = 0; i < rand; i++) {
            Vector2 pos = Random.insideUnitCircle * main.radius;
            var roid = Random.Range(0, spawner.generalRoids.Length);
            
            GameObject roidInstance = Instantiate(spawner.generalRoids[roid], transform.parent);
            roidInstance.transform.position = (Vector2)transform.position + pos;
            roidInstance.GetComponent<ObstacleMovement>().camLeft = main.camLeft;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Weapon")) {
            Destroy(other.gameObject);
            hits++;

            if (hits >= maxHits) {
                GetComponent<ObstacleMovement>().Disable(true);
            }
            else {
                hitEffect.Play();
            }
        }
    }
}