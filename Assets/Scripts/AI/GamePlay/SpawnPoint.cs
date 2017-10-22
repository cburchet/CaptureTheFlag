using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public bool isFilled;

	// Use this for initialization
	void Start () {
        isFilled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        isFilled = true;
    }

    void OnTriggerExit(Collider other)
    {
        isFilled = false;
    }
}
