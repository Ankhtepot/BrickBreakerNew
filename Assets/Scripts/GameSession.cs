using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

	[SerializeField] int brickCount = 0;
    [SerializeField] int ballCount = 0;
    [SerializeField] int Lives = 3;
    [SerializeField] public Vector3 basePaddleBallRelation;
    [SerializeField] bool checkForBricks = true;
    

    static GameSession instance = null;
    [SerializeField] TextMeshProUGUI LivesText;
    bool brickCheckCDIsOff = true;
    int sceneCount;

    List<string> notLevelScenes = new List<string>{"Start Screen","Win Screen","Game Over" };

    private void Start() {
        if (instance != null && instance != this) {
            //print("Destroying duplicate GameSession");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        sceneCount = SceneManager.sceneCount;
        Scene activeScene = SceneManager.GetActiveScene();
        if (!currentSceneIsLevel()) {
            if (LivesText) LivesText.enabled = false;
        }
        UpdateLivesText();
    }

    private void Update() {
        while(checkForBricks && brickCheckCDIsOff) {
            StartCoroutine(checkBrickCount());
        }
    }

    private IEnumerator checkBrickCount() {
        brickCheckCDIsOff = false;
        yield return new WaitForSeconds(2f);
        print("Update: Checking if all bricks are gone.");
        NextLevelIfBricksAreAllGone();
        brickCheckCDIsOff = true;
    }

    public void AddBall() {
        ballCount = BallAmount();
    }

    public void RetractBall() {
        ballCount = BallAmount();
        if(ballCount <= 1) RetractLife();
    }

    public int BallAmount() {
        return FindObjectsOfType<Ball>().Length;
    }

    private void NextLevelIfBricksAreAllGone() {
        if(CountBricks()<=0) {
            Debug.Log("All bricks gone, loading next screen");
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            if(currentSceneIsLevel()) sceneLoader.LoadNextScene();
        }
    }

    public void AddLife() {
        Lives++;
        UpdateLivesText();
    }

    public void RetractLife() {
        if (Lives <= 0) {
            FindObjectOfType<SceneLoader>().LoadGameOverScene();
        } else {
            Lives--;
            UpdateLivesText();
        }
        //Debug.Log("GameSession: RetractLife: afterRetract, Lives = " + Lives);
    }

    public void UpdateLivesText() {
        if (LivesText) LivesText.text = Lives.ToString();
    }

    private int CountBricks() {
        //print("In count bricks");
        int counter = 0;
        foreach(Brick brick in FindObjectsOfType<Brick>()) {
            if (brick.tag == "Brick") counter++;
        }
        brickCount = counter;
        //print("Brick counter: " + counter);
        return counter;
    }

    private bool currentSceneIsLevel() {
        if (notLevelScenes.Contains(SceneManager.GetActiveScene().name)) return false;
        return true;
    }
}
