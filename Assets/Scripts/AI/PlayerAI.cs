using UnityEngine;
using System.Collections;

public class PlayerAI : BaseAI {

	// Use this for initialization
	void Start ()
    {
        enemyTag = "Enemy";
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update(); 
	}

}
