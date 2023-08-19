using UnityEngine;

public class PlayerControler : MonoBehaviour {
    void FixedUpdate() {
        GameManager.Instance.SetTranformToLane(transform); // ABSTRACTION
        GameManager.Instance.MovePlayer(); // ABSTRACTION
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Obstacle")) {
            Destroy(other.gameObject);
            GameManager.Instance.LoseLive(1); // ABSTRACTION
            Debug.Log("Agh");
        }
        else if (other.gameObject.CompareTag("Powerup")) {
            StartCoroutine(other.GetComponent<Powerup>().Power()); // ABSTRACTION
            Debug.Log("Powerup");
        }
        else if (other.gameObject.CompareTag("Point")) {
            Destroy(other.gameObject);
            GameManager.Instance.AddScore(1);
            Debug.Log("Score");
        }
    }
}
