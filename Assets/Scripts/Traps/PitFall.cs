using UnityEngine;
using System.Collections;
using System;

public class PitFall : BaseTrap {
    public GameObject trapPosition;

	// Use this for initialization
	void Start ()
    {
        woodCost = 4;
        ropeCost = 4;
        metalCost = 10;
        woodDismantleCost = 4;
        ropeDismantleCost = 4;
        metalDismantleCost = 10;
        damage = 110;
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
                    triggeringEnemy.TakeDamage(damage);
                }
                else
                {
                    triggeringEnemy.dismantler.DisableTrap(this.gameObject);
                }
            }
            //deal damage and prevent movement for ai
        }
        SetPositioner();
        Destroy(this.gameObject);
    }

    public override void SetTrap(RaycastHit hit, string tag)
    {
        PitFall go = (PitFall)Instantiate(this, hit.point, Quaternion.Euler(hit.normal));
        go.EnemyTag = tag;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == EnemyTag )
        {
            TriggerTrap(other.gameObject);
        }
    }

    public void SetPositioner()
    {
        Instantiate(trapPosition, transform.position, Quaternion.identity);
    }
}
