using UnityEngine;
using System.Collections;

public class TrapPositionPlacer : MonoBehaviour {

    public GameObject trapPosition;

    public void SetPosition()
    {
        Instantiate(trapPosition, transform.position, Quaternion.identity);
    }
}
