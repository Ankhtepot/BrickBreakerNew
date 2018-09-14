using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    //private void Start() {
    //    Scene[] scenes = SceneManager.GetAllScenes();
    //    for (int i = 0; i < scenes.Length; i++) {
    //        print("Scene " + i + ": " + scenes[i].name + "\n");
    //    }
    //}

    public void LoadNextScene() {
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Loading nextScreen index= " + (currentSceneIndex+1));
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
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }
}
