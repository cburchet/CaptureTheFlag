using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour {

    public SpawnPoint[] team1;
    public SpawnPoint[] team2;

    public List<GameObject> toSpawn;

    SpawnPoint ChooseSpawnTeam1()
    {
        return team1[Random.Range(1, team1.Length)];
    }

    SpawnPoint ChooseSpawnTeam2()
    {
        return team2[Random.Range(1, team2.Length)];
    }

    public void Respawn()
    {
        GameObject player = toSpawn[0];
        toSpawn.Remove(player);
        if (player.tag == "Player")
        {
            Transform toPlace = null;
            int count = 0;
            do
            {
                toPlace = ChooseSpawnTeam1().transform;
                count++;
                //prevent endless searches for position
                if (count > 5)
                {
                    break;
                }
            } while (!ChooseSpawnTeam1().isFilled);
            SetPosition(player, toPlace);
            if (player.name == "Player")
            {
                player.GetComponent<Movement>().enabled = true;
            }
            else
            {
                //readd ai movement
                player.GetComponent<BaseAI>().enabled = true;
            }
        }
        else
        {
            Transform toPlace = null;
            int count = 0;
            do
            {
                toPlace = ChooseSpawnTeam2().transform;
                count++;
                //prevent endless searches for position
                if (count > 5)
                {
                    break;
                }
            } while (!ChooseSpawnTeam2().isFilled);
            SetPosition(player, toPlace);

            player.GetComponent<BaseAI>().agent.enabled = true;
            //readd ai movement
        }
    }

    //set position and rotation of player
    void SetPosition(GameObject player, Transform transform)
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
}
