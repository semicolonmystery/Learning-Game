using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BownlingManager : MonoBehaviour {
    [SerializeField] GameObject ball;
    [SerializeField] GameObject pinsPrefab;
    [SerializeField] Vector3 pinsPosition;
    [SerializeField] float despawnAfterDownSeconds;

    private Vector3 ballStart;
    private GameObject pins;
    private bool[] pinsUp = new bool[10];
    private bool reloadInvoked = false;

    void Start() {
        ballStart = ball.transform.position;
        pins = Instantiate(pinsPrefab, pinsPosition, Quaternion.identity);
        ResetPinsUp();
    }

    void Update() {
        if (reloadInvoked) return;
        for (int i = 0; i < 10; i++) {
            if (!pinsUp[i]) {
                StartCoroutine(Reload());
                reloadInvoked = true;
                break;
            }
        }
    }

    private void ResetPinsUp() {
        for (int i = 0; i < 10; i++) pinsUp[i] = true;
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(despawnAfterDownSeconds);
        
        ResetPinsUp();
        ball.transform.position = ballStart;
        if (pins != null) Destroy(pins);
        pins = Instantiate(pinsPrefab, pinsPosition, Quaternion.identity);
        reloadInvoked = false;
    }

    public void PinDown(int i) {
        pinsUp[i] = false;
        Debug.Log("Pin Down " + i);
    }
    public bool IsDown(int i) {
        return !pinsUp[i];
    }
}
