using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] Mesh [] playerMeshes;

    void Start() {
        GetComponent<MeshFilter>().mesh = playerMeshes [Random.Range(0, playerMeshes.Length)];
    }

    void FixedUpdate() {
        GameManager.Instance.SetTranformToLane(transform);
    }

    float time;
    void LateUpdate() {
        if (!GameManager.Instance.GetGameState()) {
            time = Time.time;
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && CheckCooldown() && !GameManager.Instance.GetGameState()) { // ABSTRACTION
                GameManager.Instance.ChangeLaneNum(-1);
                UpdateCooldown();
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && CheckCooldown()) { // ABSTRACTION
                GameManager.Instance.ChangeLaneNum(1);
                UpdateCooldown(); // ABSTRACTION
            }
        }
    }

    float cooldownTime = 0.2f;
    float remaningTime = 0;

    void UpdateCooldown() {
        remaningTime = (time + cooldownTime);
    }

    bool CheckCooldown() {
        return remaningTime < time;
    }

    int trigg = 0;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Obstacle") && trigg == 0) {
            Destroy(other.gameObject);
            GameManager.Instance.LoseLive(1); // ABSTRACTION
            Debug.Log("Hit");
            trigg = 1;
        }
    }

    private void OnTriggerExit(Collider other) {
        trigg = 0;
    }
}
