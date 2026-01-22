using UnityEngine;

public class Dock : MonoBehaviour {
    [SerializeField] float movement;
    float time = 1f;
    Vector2 initialPos;
    Vector2 startPos;
    Vector2 targetPos;

    bool rightDockVisible = true;
    [SerializeField] Vector3 leftDock;
    [SerializeField] Vector3 rightDock;
    [SerializeField] float rotSpeed;
    int dir;

    MenuManager.ShipType currentShip;
    MenuManager.ShipType previousShip;
    [SerializeField] GameObject[] docks;

    bool flipping;
    bool instant;
    bool first = true;

    RectTransform rt;

    MenuManager manager;

    void Start() {
        rt = GetComponent<RectTransform>();
        manager = GetComponentInParent<MenuManager>();
        initialPos = rt.anchoredPosition;

        currentShip = manager.currentShip;
        if (currentShip == MenuManager.ShipType.Athena) NewShip();
    }

    void Update()
    {
        if (time >= 1f)
        {
            time = 0f;
            startPos = rt.anchoredPosition;
            targetPos = initialPos + Random.insideUnitCircle * movement;
        }
        else
        {
            time += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, time);
        }

        currentShip = manager.currentShip;
        if (previousShip != currentShip) NewShip();

        if (flipping) {
            float targetZ = dir > 0 ? 180f : 0f;
            float z;

            if (instant) {
                z = targetZ;
            }
            else {
                z = Mathf.MoveTowardsAngle(rt.localEulerAngles.z, targetZ, rotSpeed * Time.deltaTime);
            }

            rt.localEulerAngles = new Vector3(0f, 0f, z);

            if (Mathf.Approximately(rt.localEulerAngles.z, targetZ)) {
                flipping = false;
                instant = false;
            }
        }
    }

    void NewShip() {
        RectTransform currentDock = null;
        RectTransform previousDock = null;

        for (int i = 0; i < docks.Length; i++) {
            docks[i].SetActive(false);

            if (docks[i].name == currentShip.ToString())
            {
                docks[i].SetActive(true);
                currentDock = docks[i].GetComponent<RectTransform>();
            }
            else if (docks[i].name == previousShip.ToString())
            {
                docks[i].SetActive(true);
                previousDock = docks[i].GetComponent<RectTransform>();
            }
        }

        if (!previousDock) {
            previousDock = docks[1].GetComponent<RectTransform>();
        }

        rightDockVisible = !rightDockVisible;
        if (rightDockVisible) {
            FlipDocks(currentDock, previousDock);
            dir = 1;
        }
        else {
            FlipDocks(previousDock, currentDock);
            dir = -1;
        }

        flipping = true;
        previousShip = currentShip;

        if (first) {
            instant = true;
            first = false;
        }
    }

    void FlipDocks(RectTransform left, RectTransform right) {
        left.anchoredPosition = new Vector2(leftDock.x, leftDock.y);
        left.localEulerAngles = new Vector3(0f, 0f, leftDock.z);

        right.anchoredPosition = new Vector2(rightDock.x, rightDock.y);
        right.localEulerAngles = new Vector3(0f, 0f, rightDock.z);
    }
}