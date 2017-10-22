using UnityEngine;
using System.Collections;
using System;

public class TripWire : BaseTrap {

    public GameObject firstSwitch;
    public GameObject secondSwitch;
    public GameObject wire;

	// Use this for initialization
	void Start ()
    {
        woodCost = 4;
        ropeCost = 10;
        metalCost = 4;
        woodDismantleCost = 4;
        ropeDismantleCost = 10;
        metalDismantleCost = 4;
        damage = 100;
    }

    public override void TriggerTrap(GameObject enemy)
    {
        //enemy that triggered trap is player
        if (enemy.name == "Player")
        {
            //deal damage and prevent movement
            if (enemy.tag == EnemyTag)
            {
                enemy.GetComponent<PlayerStats>().TakeDamage(damage);
            }
        }
        //ai triggered trap
        else
        {
            BaseAI triggeringEnemy = enemy.GetComponent<BaseAI>();
            if (triggeringEnemy.gameObject.tag == EnemyTag)
            {
                if (!triggeringEnemy.canDisable)
                {
                    if (triggeringEnemy.tag == EnemyTag)
                    {
                        triggeringEnemy.TakeDamage(damage);
                        Debug.Log("enemy triggered");
                    }
                }
                else
                {
                    triggeringEnemy.dismantler.DisableTrap(this.gameObject);
                }
            }
            //deal damage and prevent movement for ai
        }
        firstSwitch.GetComponent<TrapPositionPlacer>().SetPosition();
        Destroy(this.gameObject);
    }

    public override void SetTrap(RaycastHit hit, RaycastHit normal, String tag)
    {
        //add both endSwitches and wire
        TripWire trap = (TripWire)Instantiate(this, (hit.point + normal.point) / 2, Quaternion.Euler(hit.normal));
        trap.EnemyTag = tag;
        trap.firstSwitch.transform.position = hit.point;
        trap.firstSwitch.transform.rotation = Quaternion.Euler(hit.normal);
        trap.secondSwitch.transform.position = normal.point;
        trap.secondSwitch.transform.rotation = Quaternion.Euler(normal.normal);
        trap.wire.transform.position = trap.transform.position;
        trap.wire.transform.rotation = Quaternion.Euler(hit.normal);
        if (hit.normal.x != 0)
        {
            trap.wire.transform.localScale = new Vector3((normal.point - hit.point).magnitude, .001f, .001f);
        }
        if (hit.normal.z != 0)
        {
            trap.wire.transform.localScale = new Vector3(.001f, .001f, (normal.point - hit.point).magnitude);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == EnemyTag)
        {
            TriggerTrap(other.gameObject);
        }
    }
}
