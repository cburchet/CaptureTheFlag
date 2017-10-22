using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDamageable {

    //health
    public int health;

    public Text woodText;
    public Text metalText;
    public Text ropeText;

    //beginning of resource system
    public int wood;
    public int metal;
    public int rope;
    LayerMask resourceLayer = 1 << 8;

    public RespawnManager respawnManager;
    public Camera cam;

	// Use this for initialization
	void Start ()
    {
        wood = 100;
        metal = 100;
        rope = 250;
        health = 100;
        woodText.text = "Wood: " + wood;
        metalText.text = "Metal: " + metal;
        ropeText.text = "Rope: " + rope;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //player hits E raycast forward to check for resources
	    if (Input.GetButtonDown("Interact"))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hitInfo;
            //if hit add to resource count
            if (Physics.Raycast(ray, out hitInfo, 100f, resourceLayer))
            {
                if (hitInfo.collider.tag == "wood")
                {
                    wood += 5;
                    woodText.text = "Wood: " + wood;
                    hitInfo.collider.gameObject.GetComponent<WoodBehavior>().SwitchPositions();
                }
                else if (hitInfo.collider.tag == "metal")
                {
                    metal += 5;
                    metalText.text = "Metal: " + metal;
                    hitInfo.collider.gameObject.GetComponent<MetalBehavior>().SwitchPositions();
                }
                else if (hitInfo.collider.tag == "rope")
                {
                    rope += 5;
                    ropeText.text = "Rope: " + rope;
                    hitInfo.collider.gameObject.GetComponent<RopeBehavior>().SwitchPositions();
                }
            }
        }
	}

    public void RemoveResources(int woodCost, int ropeCost, int metalCost)
    {
        wood -= woodCost;
        rope -= ropeCost;
        metal -= metalCost;
        woodText.text = "Wood: " + wood;
        metalText.text = "Metal: " + metal;
        ropeText.text = "Rope: " + rope;
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
        //play death animation
        //respawn
        health = 100;
        respawnManager.toSpawn.Add(this.gameObject);
        respawnManager.Invoke("Respawn", 5.0f);
        GetComponent<Movement>().enabled = false;
        FlagManager flagManager = GetComponent<FlagManager>();
        if (flagManager && flagManager.flagCarrying)
        {
            flagManager.flagCarrying.DropFlag();
        }
    }
}
