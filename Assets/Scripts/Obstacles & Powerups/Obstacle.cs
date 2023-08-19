using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Obstacle : MoveObject { // INHERITANCE
    [SerializeField] GameObject centerOfMass;

    protected override void Start() { // POLYMORPHISM
        rb = GetComponent<Rigidbody>();

        if (centerOfMass != null) {
            rb.centerOfMass = centerOfMass.transform.position;
        }
    }
}
