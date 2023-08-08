using UnityEngine;

public class CarObstacle : Obstacle { // INHERITANCE
    [SerializeField][Min(0.1f)] float speed;

    protected override void moveObstacle() { // POLYMORPHISM
        if (!GameManager.Instance.GetGameState()) { // ABSTRACTION
            transform.Translate(Vector3.forward * Time.deltaTime * speed * GameManager.Instance.GetGameSpeed()); // ABSTRACTION

            if (transform.position.z < -20) {
                Destroy(gameObject);
            }
        }
    }
}
