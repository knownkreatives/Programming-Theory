using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class MoveObject : MonoBehaviour {
    float groundSpeed = 5;
    protected Rigidbody rb;

    protected virtual void Start() { // POLYMORPHISM
        rb = GetComponent<Rigidbody>();
    } 

    void FixedUpdate() {
        Move(); // ABSTRACTION
    }

    protected virtual void Update() {
        if (transform.position.z < -20) {
            Destroy(gameObject);
        }
    }

    protected virtual void Move() { // POLYMORPHISM
        if (!GameManager.Instance.GetGameState()) { // ABSTRACTION
            rb.MovePosition(transform.position + Vector3.back * groundSpeed * Time.deltaTime * GameManager.Instance.GetGameSpeed()); // ABSTRACTION
        }
    }
}
