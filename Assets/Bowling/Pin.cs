using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer = 0;
    [SerializeField] private int pinNumber = 0;

    private BownlingManager bownlingManager;
    private bool settled = false;

    void Start() {
        bownlingManager = FindObjectOfType<BownlingManager>();
        StartCoroutine(Settle());
    }

    void Update() {
        if (!settled) return;
        if (!IsTouchingGround() && !bownlingManager.IsDown(pinNumber))
            bownlingManager.PinDown(pinNumber);
    }

    public bool IsTouchingGround() {
        Vector3 bottomCapsulePoint = transform.position - (Vector3.up * (transform.localScale.y - 0.01f));
        bool isGrounded = Physics.Raycast(bottomCapsulePoint, Vector3.down, groundCheckDistance + 0.01f, groundLayer);
        return isGrounded;
    }

    private IEnumerator Settle() {
        yield return new WaitForSeconds(2f);
        settled = true;
        
    }
}
