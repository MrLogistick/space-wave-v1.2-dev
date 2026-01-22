using System.Collections;
using UnityEngine;

public class Bombroid : AsteroidBehaviour {
    [SerializeField] float explosionDelay;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] Animator ticker;
    [SerializeField] CircleCollider2D trigger;
    float triggerRadius;
    bool softStop;
    bool activated;

    protected override void OptionalOnEnable() {
        triggerRadius = trigger.radius;
        trigger.enabled = true;
        softStop = false;
        activated = false;
        overrideSpeed = false;
        trigger.gameObject.tag = "Untagged";
    }

    protected override void OptionalCollisions(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.gameObject.CompareTag("Player") && !softStop) {
            if (!thisCol.isTrigger) return;
            softStop = true;
            StartCoroutine(Delay());
        }
        else if (!otherCol.isTrigger) {
            if (thisCol.isTrigger) return;
            softStop = false;
            trigger.enabled = false;
            Disable(true);
        }
    }

    protected override void OptionalUpdate() {
        ticker.speed *= manager.postGame ? manager.endMultiplier : 1f;
        var main = explosion.main;
        main.simulationSpeed *= manager.postGame ? manager.endMultiplier : 1f;

        if (explosion.isPlaying) {
            float t = explosion.time / main.duration;
            trigger.radius = Mathf.Lerp(0f, triggerRadius, t);
        }
        else {
            trigger.radius = triggerRadius;
        }

        if (overrideSpeed) speed = 0f;
    }

    IEnumerator Delay() {
        if (activated) yield break;
        activated = true;
        ticker.SetTrigger("Trigger");

        yield return CheckForSeconds(0.2f);
        if (!softStop) yield break;

        overrideSpeed = true;

        yield return CheckForSeconds(explosionDelay);
        if (!softStop) yield break;

        Explode();
    }

    void Explode() {
        trigger.gameObject.tag = "Weapon";
        explosion.Play();
        StartCoroutine(DisableTrigger());
        Disable(true);
    }

    IEnumerator DisableTrigger() {
        var main = explosion.main;
        yield return new WaitForSeconds(main.duration / main.simulationSpeed);
        trigger.enabled = false;
    }

    IEnumerator CheckForSeconds(float time) {
        var t = 0f;
        while (t < time) {
            if (!softStop) yield break;
            t += Time.deltaTime;
            yield return null;
        }
    }
}