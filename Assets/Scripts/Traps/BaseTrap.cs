using UnityEngine;
using System.Collections;

public abstract class BaseTrap : MonoBehaviour {

    //cost to build
    public int woodCost;
    public int metalCost;
    public int ropeCost;

    //cost to dismantle
    public int woodDismantleCost;
    public int metalDismantleCost;
    public int ropeDismantleCost;

    public UnityEngine.AI.NavMeshObstacle obstacle;

    public bool wasSeen;

    public int damage;

    public string EnemyTag;

    public abstract void TriggerTrap(GameObject enemy);

    public virtual void SetTrap(RaycastHit hit, RaycastHit normal, string tag)
    { }

    public virtual void SetTrap(RaycastHit hit, string tag)
    { }

    void OnEnable()
    {
        obstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        wasSeen = false;
    }

    public void ResetSeen()
    {
        wasSeen = false;
        obstacle.enabled = false;
    }
}
