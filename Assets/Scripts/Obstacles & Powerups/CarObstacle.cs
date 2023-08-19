using UnityEngine;

public class CarObstacle : Obstacle { // INHERITANCE
    [SerializeField][Min(5)] float speed;

    protected override void Move() { // POLYMORPHISM
        if (!GameManager.Instance.GetGameState()) { // ABSTRACTION
            rb.MovePosition(transform.position + Vector3.back * speed * Time.deltaTime * GameManager.Instance.GetGameSpeed()); // ABSTRACTION

            if (transform.position.z < -20) {
                Destroy(gameObject);
            }
        }
    }
}
