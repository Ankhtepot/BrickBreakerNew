#pragma warning disable 0168

using Assets.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {
    [Header("GameplaySetups")]
    [SerializeField] int brickCount = 0;
    [SerializeField] int ballCount = 0;
    [SerializeField] int Lives = 3;
    [Header("PlayFieldSetups")]
    [SerializeField] public Vector3 basePaddleBallRelation;
    [SerializeField] bool checkForBricks = true;
    //[SerializeField] float SplashScreenShowUpDuration = 2f;
    private TextMeshProUGUI LivesText;
    //[SerializeField] float xMinKe;
    [Header("BallShakerProps")]
    [Range(0, 10)][SerializeField] float xMin = 3f;
    [Range(0, 10)][SerializeField] float xMax = 6f;
    [Range(0, 10)][SerializeField] float yMin = 0f;
    [Range(0, 10)][SerializeField] float yMax = 5f;

    //Caches
    SceneLoader sceneLoader;
    static GameSession instance = null;
    bool brickCheckCDIsOff = true;

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoad;
        sceneLoader = FindObjectOfType<SceneLoader>();
        if (instance != null && instance != this) {
            //print("Destroying duplicate GameSession");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (!sceneLoader.currentSceneIsLevel()) {
            if (LivesText) LivesText.enabled = false;
        }
        UpdateLivesText();
    }

    private void Update() {
        ScreenShake();
        while (checkForBricks && brickCheckCDIsOff) {
            StartCoroutine(checkBrickCount());
        }
    }

    private void ScreenShake() {
        
        if (Input.GetButtonDown("Jump")) {
            //print("Jump button pressed");
            Animator shakeAnimation = FindObjectOfType<Camera>().GetComponent<Animator>();
            if (shakeAnimation) {
                shakeAnimation.SetTrigger("ShakeCamera");
                foreach (Ball ball in FindObjectsOfType<Ball>()) {
                    ball.GetComponent<Rigidbody2D>().velocity +=
                        new Vector2(UnityEngine.Random.Range(xMin, xMax),
                        UnityEngine.Random.Range(yMin, yMax));
                }
            }
        }
    }

    private IEnumerator checkBrickCount() {
        brickCheckCDIsOff = false;
        yield return new WaitForSeconds(2f);
        //print("Update: Checking if all bricks are gone.");
        CheckBrickCount();
        brickCheckCDIsOff = true;
    }

    public void AddBall() {
        ballCount = BallAmount();
    }

    public void RetractBall() {
        ballCount = BallAmount();
        if (ballCount <= 1) RetractLife();
    }

    public int BallAmount() {
        return FindObjectsOfType<Ball>().Length;
    }

    private void CheckBrickCount() {
        if (CountBricks() <= 0) {
            //Debug.Log("All bricks gone, loading next screen");
            IBoss boss = FindObjectOfType<Boss>() as IBoss;
            if(boss!=null) {
                boss.StartEncounter();
            } else {
                NextLevel();
            }
        }
    }

    public void NextLevel() {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader) {
            if(sceneLoader.currentSceneIsLevel()) sceneLoader.LoadScene();
        } else print("GameSession: SceneLoader not found");
    }

    private int CountBricks() {
        //print("In count bricks");
        int counter = 0;
        foreach (Brick brick in FindObjectsOfType<Brick>()) {
            if (brick.tag == "Brick") counter++;
        }
        brickCount = counter;
        //print("Brick counter: " + counter);
        return counter;
    }

    public void AddLife() {
        Lives++;
        UpdateLivesText();
        Animator paddle = FindObjectOfType<LifeAdjustment>().GetComponent<Animator>();
        if (paddle) paddle.SetTrigger("Plus");
    }

    public void RetractLife() {
        if (Lives <= 0) {
            FindObjectOfType<SceneLoader>().LoadGameOverScene();
        } else {
            Lives--;
            Animator paddle = FindObjectOfType<LifeAdjustment>().GetComponent<Animator>();
            if (paddle) paddle.SetTrigger("Minus");
            UpdateLivesText();
        }
        //Debug.Log("GameSession: RetractLife: afterRetract, Lives = " + Lives);
    }

    public void UpdateLivesText() {
        if (LivesText) LivesText.text = Lives.ToString();
    }    

    private void OnSceneLoad(Scene loadedScene, LoadSceneMode mode) {
        
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}
