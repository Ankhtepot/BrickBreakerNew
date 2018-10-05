using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	void triggerNextLevel() {
        SceneLoader SL = FindObjectOfType<SceneLoader>();
        if (SL) SL.FetchLevel();
        else print("SplashScreen/triggerNextLevel: GameSession not found.");
    }
}
