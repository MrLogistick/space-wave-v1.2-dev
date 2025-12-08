using UnityEngine;

public class ShipAbility : MonoBehaviour
{
    public bool ability;

    [SerializeField] int initialBullets;
    [SerializeField] int maxBullets;
    int bulletCount;

    [SerializeField] GameObject bullet;

    Transform shootPoint;

    void Start()
    {
        shootPoint = GetComponentInChildren<Transform>();
        bulletCount = initialBullets;
    }

    void Update()
    {
        if (bulletCount > 0 && ability)
        {
            ability = false;
            Instantiate(bullet, shootPoint.position, bullet.transform.rotation);
        }
    }
}