using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField] int sceneToReturnTo;

    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Loading nextScreen index= " + (currentSceneIndex+1));
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

	public void LoadFirstScene() {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication() {
        Application.Quit();
        print("Request to quit received");
    }

    public void LoadGameOverScene() {
        SceneManager.LoadScene("Game Over");
    }

    public void ManageCreditsSceneView() {
        SceneManager.LoadScene("Credits scene");
    }
}
