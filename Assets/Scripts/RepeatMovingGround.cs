using UnityEngine;

public class RepeatMovingGround : MonoBehaviour {

    void LateUpdate() {
        if (!GameManager.Instance.GetGameState()) {
            transform.Translate(Vector3.back * 5 * Time.deltaTime * GameManager.Instance.GetGameSpeed(), Space.World);

            if (transform.position.z < -60) {
                transform.position = new Vector3(0, 0.25f, 0);
            }
        }
    }
}
