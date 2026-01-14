using System.Collections;
using UnityEngine;

public class BombroidEffect : AsteroidAbility {
    [SerializeField] float explosionDelay;
    [SerializeField] ParticleSystem lethalExplosion;
    [SerializeField] CircleCollider2D trigger;
    [SerializeField] Animator ticker;
    [SerializeField] GameObject tickerObj;

    public bool active = true;
    bool softStop = false;

    [SerializeField] float triggerRadius;
    float time;

    GameManager manager;

    void Start() {
        manager = GameManager.instance;
    }

    void Update() {
        ticker.speed *= manager.postGame ? manager.endMultiplier : 1f;
        var main = lethalExplosion.main;
        main.simulationSpeed *= manager.postGame ? manager.endMultiplier : 1f;
        
        if (!tickerObj.activeSelf) {
            if (time < main.duration) time += Time.deltaTime;
            float t = time / main.duration;
            trigger.radius = Mathf.Lerp(0f, triggerRadius, t);
        }
        else {
            time = 0f;
            trigger.radius = triggerRadius;
        }
    }

    public override void Fire(ObstacleMovement main) {
        StartCoroutine(Delay(main));
    }

    IEnumerator Delay(ObstacleMovement main) {
        yield return new WaitForSeconds(0.2f);

        if (softStop) {
            main.overrideSpeed = true;
            main.speed = 0f;
            ticker.SetTrigger("Trigger");

            yield return new WaitForSeconds(explosionDelay);
        }

        Explode();

        var explosion = lethalExplosion.main;
        yield return new WaitForSeconds(explosion.duration / explosion.simulationSpeed);

        trigger.enabled = false;
    }

    void Explode() {
        lethalExplosion.Play();
        trigger.gameObject.tag = "Weapon";
        tickerObj.SetActive(false);

        active = false;
        GetComponent<ObstacleMovement>().Disable(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !softStop) {
            softStop = true;
            Fire(GetComponent<ObstacleMovement>());
        }
    }
}