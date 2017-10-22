using UnityEngine;
using System.Collections;

public class TrapDismantle : MonoBehaviour {



	// Update is called once per frame
	public bool PlayerCanDisable(PlayerStats stats, BaseTrap trap)
    {
        if (stats.wood >= trap.woodDismantleCost 
            && stats.metal >= trap.metalDismantleCost 
            && stats.rope >= trap.ropeDismantleCost)
        {
            return true;
        }
        return false;
    }

    public bool AICanDisable(BaseAI ai, BaseTrap trap)
    {
        if (ai.wood >= trap.woodDismantleCost
            && ai.metal >= trap.metalDismantleCost
            && ai.rope >= trap.metalDismantleCost)
        {
            if (ai.tag == trap.EnemyTag)
            {
                return true;
            }
        }
        return false;
    }

    public void DisableTrap(GameObject trap)
    {
        //for (int i = 0; i < trap.transform.childCount; i++)
        //{
        //    TrapPositionPlacer placer = trap.transform.GetChild(i).gameObject.GetComponent<TrapPositionPlacer>();
        //    if (placer)
        //    {
        //        placer.SetPosition();
        //    }
        //    Destroy(trap.transform.GetChild(i).gameObject);
        //    i--;
        //}
        PitFall pit = trap.GetComponent<PitFall>();
        TripWire wire = trap.GetComponent<TripWire>();
        if (pit)
        {
            pit.SetPositioner();
        }
        else if (wire)
        {
            wire.firstSwitch.GetComponent<TrapPositionPlacer>().SetPosition();
        }
        Destroy(trap);
    }
}
