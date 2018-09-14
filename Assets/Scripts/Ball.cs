using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [Header("physical setup")]
    [SerializeField] Paddle paddle1;
    [SerializeField] public float launchPower = 10f;
    [Header("Sounds setup")]
    [SerializeField] AudioClip[] bounceSounds;
    [SerializeField] AudioClip unbreakableSound;
    [SerializeField] AudioClip paddleSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip wallBounceSound;

    //states
    bool hasStarted = false;
    public Boolean isGlueApplied = false;
    /*must have this Vector3 because when GlueIsActive, PaddleBallRelation must change,
     * but need to keep basic relation for new ball or correction */
    Vector3 PaddleBallRelation;
    Vector3 basePaddleBallRelation;
    GameSession gameSession;
    MusicPlayer SFXPlayer;

    // Use this for initialization
    void Start() {
        gameSession = FindObjectOfType<GameSession>();
        SFXPlayer = FindObjectOfType<MusicPlayer>();
        if (SFXPlayer) print("Ball: SFXPlayer assigned");
        basePaddleBallRelation = gameSession.basePaddleBallRelation;
        PaddleBallRelation = basePaddleBallRelation;
        //print("PaddleBallRelation: " + PaddleBallRelation.ToString());
        gameSession.AddBall();
        if (this.tag == "Fireball") GetComponentInChildren<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update() {
        if (!hasStarted) {
            lockToPaddle();
            launchOnClick();
        }
    }

    private void launchOnClick() {
        if (Input.GetMouseButtonDown(0)) {
            hasStarted = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(PaddleBallRelation.x * launchPower, PaddleBallRelation.y * launchPower);
        }
    }

    public Vector2 GetPaddleBallRelation() {
        return transform.position - paddle1.transform.position;
    }

    private void lockToPaddle() {
        transform.position = paddle1.transform.position + PaddleBallRelation;
    }

    private bool isOtherBallPresent() {
        if (FindObjectsOfType<Ball>().Length > 1) return true;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        String collisionTag = collision.gameObject.tag;
        if (collisionTag == "Paddle" && isGlueApplied) LockingToPaddle(collision);
        PlaySFX(collision);
        TweakVelocity();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "LoseCollider") {
            //Debug.Log("Ball Triggerers LoseCollider");
            ManageLoseCollider();
        }
        if (collision.tag == "Corrector") CorrectPaddleCollision();
    }

    public void CorrectPaddleCollision() {
        //Debug.Log("Correcting Ball");
        hasStarted = false;
        PaddleBallRelation = basePaddleBallRelation;
        lockToPaddle();
    }

    private void TweakVelocity() {
        Vector2 tweak = new Vector2(UnityEngine.Random.Range(0f, 0.2f), UnityEngine.Random.Range(0f, 0.2f));
        GetComponent<Rigidbody2D>().velocity += tweak;
    }

    private void PlaySFX(Collision2D collision) {
        //Debug.Log("Ball collides with " + collision.gameObject.tag);
        if (SFXPlayer && SFXPlayer.GetVolume() != 0f) {
            switch (collision.gameObject.tag) {
                case "Unbreakable": SFXPlayer.PlayClip(unbreakableSound); break;
                case "Paddle": SFXPlayer.PlayClip(paddleSound); break;
                case "Wall": SFXPlayer.PlayClip(wallBounceSound); break;
                case "LoseColider": AudioSource.PlayClipAtPoint(loseSound,transform.position); break;
                default: SFXPlayer.PlayClip(bounceSounds[UnityEngine.Random.Range(0, bounceSounds.Length)]); break;
            } 
        }
    }

    private void ManageLoseCollider() {
        if (FindObjectsOfType<Ball>().Length > 1) Destroy(gameObject);
        else CorrectPaddleCollision();
        gameSession.RetractBall();
    }

    private void LockingToPaddle(Collision2D collision) {
        if (collision.rigidbody != null && collision.rigidbody.tag == "Paddle") {
            //Debug.Log("Locking ball to Paddle on Collision");
            PaddleBallRelation = GetPaddleBallRelation();
            lockToPaddle();
            hasStarted = false;
        }
    }

}
