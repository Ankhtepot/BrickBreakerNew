using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	public void LoadNextScene() {
        SceneLoader SL = FindObjectOfType<SceneLoader>();
        if (SL) SL.LoadScene();
        else
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
