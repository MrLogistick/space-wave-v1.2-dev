using UnityEngine.UI;
using UnityEngine;

public class ShipAbility : MonoBehaviour
{
    public bool activated;

    [SerializeField] int initialBullets;
    [SerializeField] int maxBullets;
    int bulletCount;

    [SerializeField] GameObject bullet;

    [SerializeField] Transform shootPoint;
    [SerializeField] Transform secondShootPoint;
    Transform currentPoint;

    [SerializeField] GameObject visual;
    Image fill;

    [SerializeField] ShipType currentShip;
    enum ShipType {
        Athena,
        Hermes,
        Zeus,
        Hephaetsus
    }

    void Start() {
        bulletCount = initialBullets;

        if (visual) {
            Transform liveVisual = Instantiate(visual, GameObject.FindGameObjectWithTag("MainCanvas").transform).transform;
            fill = liveVisual.GetChild(0).GetComponent<Image>();
        }
    }

    void Update() {
        switch (currentShip) {
            case ShipType.Athena: // script in weapon
                currentPoint = shootPoint;
                break;
            case ShipType.Hermes: // script in ship
                currentPoint = (transform.rotation.z >= -45f)
                    ? secondShootPoint : shootPoint;
                break;
        }

        if (activated) {
            activated = false;

            if (bulletCount <= 0) return;
            
            bulletCount--;
            Instantiate(bullet, currentPoint.position, bullet.transform.rotation);
        }

        fill.fillAmount = bulletCount / (float)maxBullets;
    }

    public void ChangeCountBy(int value) {
        if (bulletCount == maxBullets) return;
        bulletCount += value;
    }
}