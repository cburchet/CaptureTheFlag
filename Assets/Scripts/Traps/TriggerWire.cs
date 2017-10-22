using UnityEngine;
using System.Collections;

public class TriggerWire : MonoBehaviour {

    public TripWire wireTrap;
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == wireTrap.EnemyTag)
        {
            wireTrap.TriggerTrap(other.gameObject);
        }
    }
}
