using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class BaseAI : MonoBehaviour, IDamageable {

    public int wood;
    public int metal;
    public int rope;

    public TrapDismantle dismantler;
    public bool canDisable = false;

    public int health;
    public RespawnManager respawnManager;

    public Flag enemyFlag;
    public Flag myFlag;
    protected FlagManager flagManager;

    protected Camera cam;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public string enemyTag;
    //layer for seeing resources, characters, traps
    protected LayerMask sightLayer = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | 1;

    public BaseTrap[] traps;
	// Use this for initialization
	void Awake ()
    {
        cam = GetComponent<Camera>();
        metal = 100;
        wood = 100;
        rope = 250;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        health = 100;
        flagManager = GetComponent<FlagManager>();
        dismantler = GetComponent<TrapDismantle>();
	}


    protected virtual void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            Collider[] collidersInRange;
            cam.enabled = true;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            cam.enabled = false;
            //removed due to no combat
           // BaseAI enemyFound = CheckForEnemies(collidersInRange, planes);
            if (!agent.hasPath)
            {
                 if (flagManager.isCarrying)
                {
                    agent.destination = myFlag.transform.position;
                }
                else
                {
                    agent.destination = enemyFlag.transform.position;
                }
            }
           // if (enemyFound) removed for no combat
            if (false)
            {
                //insert combat if needed
            }
            else
            {
                collidersInRange = Physics.OverlapSphere(transform.position, 100f, 1 << 10);
                BaseTrap trapFound = CheckForTraps(collidersInRange, planes);
                if (trapFound)
                {
                    int chanceToSee = Random.Range(0, 101);
                    if (chanceToSee <= 50)
                    {
                        trapFound.wasSeen = true;
                        trapFound.Invoke("ResetSeen", 5f);
                        canDisable = false;
                    }
                    else
                    {
                        trapFound.wasSeen = true;
                        if (dismantler.AICanDisable(this, trapFound))
                        {
                            agent.destination = trapFound.transform.position;
                            canDisable = true;
                        }
                        else
                        {
                            trapFound.obstacle.enabled = true;
                            canDisable = false;
                        }
                        trapFound.Invoke("ResetSeen", 5f);
                    }
                }
                else
                {
                    collidersInRange = Physics.OverlapSphere(transform.position, 100f, 1 << 8);
                    GameObject resourceFound = CheckForResources(collidersInRange, planes);
                    if (resourceFound)
                    {
                        //move to resourceFound and collect
                        agent.destination = resourceFound.transform.position;
                        resourceFound = null;
                    }
                    else
                    {
                        if (!flagManager.isCarrying)
                        {
                            agent.destination = enemyFlag.transform.position;
                        }
                        collidersInRange = Physics.OverlapSphere(transform.position, 100f, 1 << 11);
                        RaycastHit hit;
                        GameObject trapPlaceFound = CheckForSpot(collidersInRange, planes, out hit);
                        if (trapPlaceFound)
                        {
                            if (trapPlaceFound.tag == "pitPosition")
                            {
                                traps[0].SetTrap(hit, enemyTag);
                            }
                            else
                            {
                                RaycastHit normal;
                                Physics.Raycast(hit.point, hit.normal, out normal, 15f);
                                traps[1].SetTrap(hit, normal, enemyTag);
                            }
                            Destroy(trapPlaceFound);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //trap and resource checking
        if (other.gameObject.layer == 8)
        {
            if (other.gameObject.tag == "wood")
            {
                wood += 5;
                other.gameObject.GetComponent<WoodBehavior>().SwitchPositions();
            }
            else if (other.gameObject.tag == "metal")
            {
                metal += 5;
                other.gameObject.GetComponent<MetalBehavior>().SwitchPositions();
            }
            else if (other.gameObject.tag == "rope")
            {
                rope += 5;
                other.gameObject.GetComponent<RopeBehavior>().SwitchPositions();
            }
        }
    }
	
    //unused
    protected BaseAI CheckForEnemies(Collider[] collidersInRange, Plane[] planes)
    {
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            if (collidersInRange[i].gameObject.layer == 9)
            {
                if (GeometryUtility.TestPlanesAABB(planes, collidersInRange[i].bounds))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(cam.transform.position,
                        collidersInRange[i].GetComponent<Camera>().transform.position - cam.transform.position, out hit, 100, sightLayer))
                    {
                        if (hit.collider.gameObject.layer == 9)
                        {
                            return collidersInRange[i].GetComponent<BaseAI>();
                        }
                    }
                }
            }
        }
        return null;
    }

    protected GameObject CheckForSpot(Collider[] collidersInRange, Plane[] planes, out RaycastHit hitReturn)
    {
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            if (GeometryUtility.TestPlanesAABB(planes, collidersInRange[i].bounds))
            {
                if (Physics.Raycast(cam.transform.position,
                    collidersInRange[i].transform.position - cam.transform.position, out hit, 100, sightLayer))
                {
                    hitReturn = hit;
                    if (hit.collider.gameObject.layer == 11)
                    {
                        return collidersInRange[i].gameObject;
                    }
                }
            }
        }
        hitReturn = hit;
        return null;
    }
    //check for traps within player sight
    protected BaseTrap CheckForTraps(Collider[] collidersInRange, Plane[] planes)
    {
        //check each collider in range of player
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            //check if collider is within AI's camera's view frustum
            if (GeometryUtility.TestPlanesAABB(planes, collidersInRange[i].bounds))
            {
                //check for anything in between AI and trap
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position,
                    collidersInRange[i].transform.position - cam.transform.position, out hit, 100, sightLayer))
                {
                    if (hit.collider.gameObject.layer == 10)
                    {
                        BaseTrap trap = collidersInRange[i].GetComponent<BaseTrap>();
                        return trap;
                    }

                }
            }
        }
        return null;
    }

    protected GameObject CheckForResources(Collider[] collidersInRange, Plane[] planes)
    {
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            if (GeometryUtility.TestPlanesAABB(planes, collidersInRange[i].bounds))
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position,
                    collidersInRange[i].transform.position - cam.transform.position, out hit, 100, sightLayer))
                {
                    if (hit.collider.gameObject.layer == 8)
                    {
                        return collidersInRange[i].gameObject;
                    }
                }
            }
        }
        return null;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //respawn
        health = 100;
        respawnManager.toSpawn.Add(this.gameObject);
        respawnManager.Invoke("Respawn", 5.0f);
        agent.enabled = false;
        if (flagManager.isCarrying)
        {
            flagManager.flagCarrying.DropFlag();
        }
    }

}
