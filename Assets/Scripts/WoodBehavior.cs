using UnityEngine;
using System.Collections;

public class WoodBehavior : MonoBehaviour {
    public ResourceManager resourceManager;

    GameObject position;

    public void SwitchPositions()
    {
        position = resourceManager.NewPosition();
        resourceManager.AddPosition(position);
        transform.position = position.transform.position;
        transform.rotation = position.transform.rotation;
    }
}
