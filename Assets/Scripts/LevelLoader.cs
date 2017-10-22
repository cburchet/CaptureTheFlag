using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {


	public void OnClick(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
