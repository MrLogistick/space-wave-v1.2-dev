using UnityEngine;
using UnityEngine.UI;

public abstract class ShipAbility : MonoBehaviour {
    public bool activated;

    public GameObject ability;
    public Transform shootPoint;

    public int maxCount;
    public int initialCount;
    [HideInInspector] public int currentCount;

    public int overflowScore;
    int overflowCount;

    public GameObject visual;
    [HideInInspector] public Image fill;

    [HideInInspector] public GameManager manager;

    public WeaponType weapon;
    public enum WeaponType {
        Shockwave,
        Shield
    }

    protected virtual void Start() {
        manager = GameManager.instance;

        Transform instance = Instantiate(visual, GameObject.FindGameObjectWithTag("MainCanvas").transform).transform;
        fill = instance.GetChild(0).GetComponent<Image>();

        currentCount = initialCount;

        OptionalStart();
    }
    protected virtual void OptionalStart() { }

    protected virtual void Update() {
        fill.fillAmount = currentCount / (float)maxCount;

        if (activated) {
            activated = false;

            if (currentCount <= 0) return;
            currentCount--;

            Fire();
        }

        OptionalUpdate();
    }
    protected virtual void OptionalUpdate() { }

    protected virtual void Fire() {
        Instantiate(ability, shootPoint.position, ability.transform.rotation);
    }

    public virtual void ChangeCountBy(int value) {
        if (currentCount >= maxCount) {
            manager.AlterScoreBy(overflowScore);
            overflowCount++;

            if (overflowCount < 2 && weapon != WeaponType.Shockwave) return;

            if (PlayerPrefs.GetInt("Zeus_Unlocked", 0) == 0) {
                PlayerPrefs.SetInt("Zeus_Unlocked", 1);
                manager.newThing = true;
            }
        }
        else {
            currentCount += value;
        }
    }
}