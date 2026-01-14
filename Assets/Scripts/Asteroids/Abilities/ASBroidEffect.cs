using UnityEngine;

public class ASBroidEffect : AsteroidAbility {
    [SerializeField] float shotRate;
    float time;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shotPoint;

    public override void Fire(ObstacleMovement main) {
        time += 0.1f;

        if (time >= shotRate) {
            Instantiate(bullet, shotPoint.position, transform.rotation, transform);
            time = 0f;
        }
    }
}