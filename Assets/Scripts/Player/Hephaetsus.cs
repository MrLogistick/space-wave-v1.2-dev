using UnityEngine;

public class Hephaetsus : ShipAbility {
    [SerializeField] float generationRate;
    float time;

    [SerializeField] ParticleSystem generationEffect;

    protected override void OptionalUpdate() {
        time += Time.deltaTime;

        if (time < generationRate) return;
        ChangeCountBy(1);
        generationEffect.Play();
        time = 0f;
    }
}