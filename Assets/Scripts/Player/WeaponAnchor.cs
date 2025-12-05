using UnityEngine;

public class WeaponAnchor : MonoBehaviour
{
    [SerializeField] Transform anchor;

    void Update()
    {
        transform.position = anchor.position;
    }
}