using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLogic : MonoBehaviour {

    public void ResetBall() {
        Ball ball = FindObjectOfType<Ball>();
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (ball && gameSession) {
            ball.CorrectPaddleCollision();
            gameSession.RetractLife();
        }
        
    }

}
