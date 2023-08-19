using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [SerializeField] protected float duration = 5f;

    public virtual IEnumerator Power() { // POLYMORPHISM
        yield return null;
    }
}
