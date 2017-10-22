using UnityEngine;
using System.Collections;

public class TrapPlacement : MonoBehaviour {

    public TripWire wireTrap;
    public PitFall pit;

    PlayerStats stat;
    public Camera cam;
    TrapDismantle dismantle;

    void Start()
    {
        stat = GetComponent<PlayerStats>();
        dismantle = GetComponent<TrapDismantle>();
    }
	
    bool CheckResources(BaseTrap trap)
    {
        if (stat.wood >= trap.woodCost)
        {
            if (stat.rope >= trap.ropeCost)
            {
                if (stat.metal >= trap.metalCost)
                {
                    return true;
                }
            }
        }
        return false;
    }

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            RaycastHit normalHit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider)
                {
                    Ray normalCheck = new Ray(hit.point, hit.normal);
                    Debug.DrawRay(hit.point, hit.normal, Color.blue);
                    if (Physics.Raycast(normalCheck, out normalHit, 15f))
                    {
                        if (normalHit.collider)
                        {
                            if (CheckResources(wireTrap))
                            {
                                wireTrap.SetTrap(hit, normalHit, "Enemy");
                                stat.RemoveResources(wireTrap.woodCost, wireTrap.ropeCost, wireTrap.metalCost);
                            }
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider)
                {
                    if (CheckResources(pit))
                    {
                        pit.SetTrap(hit, "Enemy");
                        stat.RemoveResources(pit.woodCost, pit.ropeCost, pit.metalCost);
                    }
                }
            }
        }
        else if (Input.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            if (Physics.SphereCast(cam.transform.position,1f,cam.transform.forward, out hit, 5f, 1<<10))
            {
                if (hit.collider.gameObject.tag == "Trap")
                {
                    BaseTrap trap = hit.collider.gameObject.GetComponent<BaseTrap>();
                    if (trap.EnemyTag == this.gameObject.tag)
                    {
                        if (dismantle.PlayerCanDisable(stat, trap))
                        {
                            dismantle.DisableTrap(trap.gameObject);
                            stat.RemoveResources(trap.woodDismantleCost, trap.ropeDismantleCost, trap.metalDismantleCost);
                        }
                    }
                }
            }
        }
	}
}
