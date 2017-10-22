using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

    public string enemyTag;
    public Flag otherFlag;

    //set transform at beginning of game when placed
    public Vector3 startPosition;
    public Quaternion startRotation;
    public bool isCarried = false;

    //handles the score of the game
    public ScoreController score;
	
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flag" && isCarried)
        {
            if (enemyTag == "Enemy")
            {
                score.EnemyScore++;
                score.UpdateScore();
            }
            else
            {
                score.PlayerScore++;
                score.UpdateScore();
            }
            DropFlag();
            
        }
        else if (other.gameObject.tag == enemyTag && !isCarried)
        {
            //player picks up
            isCarried = true;
            FlagManager flagManager = other.gameObject.GetComponent<FlagManager>();
            this.transform.position = flagManager.flagPos.position;
            flagManager.flagCarrying = this;
            flagManager.isCarrying = true;
            this.transform.parent = other.gameObject.transform;
            //attach to empty gameobject flagposition to carry flag
        }
    }

    public void DropFlag()
    {
        FlagManager flagManager = transform.parent.GetComponent<FlagManager>();
        flagManager.isCarrying = false;
        flagManager.flagCarrying = null;
        transform.parent = null;
        transform.position = startPosition;
        transform.rotation = startRotation;
        isCarried = false;
    }
}
