using System.Collections;
using UnityEngine;

public class Bombroid : AsteroidBehaviour {
    [SerializeField] float explosionDelay;
    [SerializeField] Animator ticker;
    [SerializeField] CircleCollider2D trigger;
    float triggerRadius;
    bool softStop;
    bool activated;
    bool complete;

    protected override void OptionalOnEnable() {
        triggerRadius = trigger.radius;
        softStop = false;
        activated = false;
        complete = false;
        overrideSpeed = false;
        trigger.gameObject.tag = "Untagged";
    }

    protected override void OptionalCollisions(Collider2D thisCol, Collider2D otherCol, int index) {
        if (otherCol.gameObject.CompareTag("Player") && !softStop) {
            if (!thisCol.isTrigger) return;
            softStop = true;
            StartCoroutine(Delay());
        }

        if (!softStop) {
            Explode();
        }
    }

    protected override void OptionalUpdate() {
        ticker.speed *= manager.postGame ? manager.endMultiplier : 1f;
        var main = particles[1].main;
        main.simulationSpeed *= manager.postGame ? manager.endMultiplier : 1f;

        trigger.enabled = !complete ? true : false;

        if (trigger.gameObject.tag == "Weapon") {
            float t = particles[1].time / main.duration;
            trigger.radius = Mathf.Lerp(0f, triggerRadius, t);
        }
        else {
            trigger.radius = triggerRadius;
        }

        if (overrideSpeed) speed *= 0.8f * Time.deltaTime;

        if (trigger.enabled == false) {
            pool.ReturnToPool(gameObject);
        }
    }

    IEnumerator Delay() {
        if (activated) yield break;
        activated = true;

        yield return CheckForSeconds(0.2f);
        if (!softStop) yield break;

        overrideSpeed = true;
        ticker.SetTrigger("Trigger");

        yield return CheckForSeconds(explosionDelay);
        if (!softStop) yield break;

        Explode();
    }

    void Explode() {
        Disable(true);
        trigger.gameObject.tag = "Weapon";
        StartCoroutine(DisableTrigger());
    }

    IEnumerator DisableTrigger() {
        var main = particles[1].main;
        yield return new WaitForSeconds(main.duration);
        complete = true;
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