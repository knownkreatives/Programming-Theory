using UnityEngine;

public class Obstacle : MonoBehaviour {
    void Update() {
        moveObstacle();    
    }

    protected virtual void moveObstacle() { // POLYMORPHISM
        if (!GameManager.Instance.GetGameState()) { // ABSTRACTION
            transform.Translate(Vector3.back * 5 * Time.deltaTime * GameManager.Instance.GetGameSpeed(), Space.World); // ABSTRACTION

            if (transform.position.z < -20) {
                Destroy(gameObject);
            }
        }
    }
}
