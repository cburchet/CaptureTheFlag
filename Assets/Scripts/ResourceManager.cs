using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {

    public List<GameObject> resourcePositions;

    public GameObject NewPosition()
    {
        GameObject last = resourcePositions[0];
        resourcePositions.RemoveAt(0);
        return last;
    }

    public void AddPosition(GameObject toAdd)
    {
        resourcePositions.Add(toAdd);
    }
}
