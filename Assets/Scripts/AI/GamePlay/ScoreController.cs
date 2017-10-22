using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

    public int EnemyScore;
    public int PlayerScore;
    public int winScore = 3;
    public Text playerScoreText;
    public Text enemyScoreText;

    public Canvas endCanvas;
    public Text endText;

    public BaseAI[] allAI;
    public Movement playerMove;

    // Use this for initialization
    void Start () {
        EnemyScore = 0;
        PlayerScore = 0;
        playerScoreText.text = PlayerScore.ToString();
        enemyScoreText.text = EnemyScore.ToString();
    }

    public void UpdateScore()
    {
        //gui for team scores
        playerScoreText.text = PlayerScore.ToString();
        enemyScoreText.text = EnemyScore.ToString();
        if (EnemyScore == winScore)
        {
            DisableMovement();
            endText.text = "Defeat";
            endCanvas.enabled = true;
            Invoke("LoadMenu", 2f);
        }
        else if (PlayerScore == winScore)
        {
            DisableMovement();
            endText.text = "Victory";
            endCanvas.enabled = true;
            Invoke("LoadMenu", 2f);
        }
    }

    void DisableMovement()
    {
        for (int i = 0; i < allAI.Length; i++)
        {
            allAI[i].enabled = false;
        }
        playerMove.enabled = false;
    }

    void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
