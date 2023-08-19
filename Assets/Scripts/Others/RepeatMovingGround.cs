using UnityEngine;

public class RepeatMovingGround : MoveObject { // INHERITANCE
    protected override void Update() { // POLYMORPHISM
        if (transform.position.z < -60) {
            transform.position = new Vector3(0, 0.25f, 0);
        }
    }
}
