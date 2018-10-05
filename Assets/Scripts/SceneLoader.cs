using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField] int sceneToReturnTo = 0;
    [SerializeField] int sceneToBeLoaded = -1;
    [SerializeField] float SplScrFadeDelay = 2f;

    //Caches
    Animator splashScreen;
    List<string> notLevelScenes = new List<string> { "Start Screen", "Win Screen", "Game Over" };

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Start() {
        splashScreen = FindObjectOfType<SplashScreen>().GetComponent<Animator>();
    }

    public void LoadScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneToBeLoaded = ++currentSceneIndex;
        FetchLevel();
    }

    public void LoadScene(int sceneNr) {
        sceneToBeLoaded = sceneNr;
        FetchLevel();
    }

    public void FetchLevel() {
        if (splashScreen) {
            splashScreen.SetTrigger("ShowUp");
            splashScreen.SetBool("Fade", false);
            //Animation triggers FetchNextLevel
        } else {
            print("SceneLoader: splashScreen not found");
        }
        Debug.Log("Loading Screen index: " + (sceneToBeLoaded));
        int STBL = sceneToBeLoaded;
        sceneToBeLoaded = -1;
        SceneManager.LoadScene(STBL);
    }

    public void LoadFirstScene() {
        LoadScene(0);
    }

    public void QuitApplication() {
        Application.Quit();
        print("Request to quit received");
    }

    public void LoadGameOverScene() {
        int gameOverSceneNr = sceneIndexFromName("Game Over");
        LoadScene(gameOverSceneNr);
    }

    public void ManageCreditsSceneView() {
        Scene currentScreen = SceneManager.GetActiveScene();
        int creditsSceneNr = sceneIndexFromName("Credits Scene");
        print("SceneLoader/ManageCreditsSceneView: creditsSceneNr: " + creditsSceneNr);
        if (currentScreen.name != "Credits Scene") {
            sceneToReturnTo = currentScreen.buildIndex;
            LoadScene(creditsSceneNr);
        } else if(currentScreen.name == "Credits Scene")
            LoadScene(sceneToReturnTo);
    }

    public bool currentSceneIsLevel() {
        if (notLevelScenes.Contains(SceneManager.GetActiveScene().name)) return false;
        return true;
    }

    void OnSceneLoad(Scene loadedScene, LoadSceneMode mode) {
        if (!splashScreen) splashScreen = FindObjectOfType<SplashScreen>().GetComponent<Animator>();
        if (splashScreen) {
            StartCoroutine(DelaySplScrFade());
        } else print("SceneLoader/OnSceneLoad: No splashScreen found");
    }

    IEnumerator DelaySplScrFade() {
        if (currentSceneIsLevel()) {
            yield return new WaitForSeconds(SplScrFadeDelay);
        }
        //print("SceneLoader/OnSceneLoad: Setting Fade to true");
        splashScreen.SetBool("Fade", true);
    }

    private string NameFromIndex(int BuildIndex) { //@Author:  Iamsodarncool/UnityAnswers
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    private int sceneIndexFromName(string sceneName) {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string testedScreen = NameFromIndex(i);
            //print("sceneIndexFromName: i: " + i + " sceneName = " + testedScreen);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}
