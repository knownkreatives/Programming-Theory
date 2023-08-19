using System.Collections;
using UnityEngine;

public class MovementPowerup : Powerup { // INHERITANCE
    [SerializeField][Min(0)] float moveFactor = 2;

    public override IEnumerator Power() { // POLYMORPHISM
        GetComponent<MeshRenderer>().enabled = false;
        GameManager.Instance.SetPowerupType(PowerupType.Movement); // ABSTRACTION
        GameManager.Instance.UpdateGameSpeed(moveFactor);
        GameManager.Instance.SetIndicatorVisibility(true);
        yield return new WaitForSeconds(moveFactor);
        GameManager.Instance.ResetGameSpeed();
        GameManager.Instance.SetIndicatorVisibility(false);
        GameManager.Instance.SetPowerupType(PowerupType.None); // ABSTRACTION
        Destroy(gameObject);
    }
}
