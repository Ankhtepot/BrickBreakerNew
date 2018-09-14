using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicOnOffControler : MonoBehaviour {

    /*  OnSceneLoad dosnt work to switch off sound in Ball so
     *  I will do check in Ball class for now....
     *  Sucks will some better solution with replacement of OnLevelLoad
     * */

    [SerializeField] Sprite MusicOnSprite;
    [SerializeField] Sprite MusicOffSprite;
    
    public bool MusicOn = true;
    AudioSource musicPlayer;
    static MusicOnOffControler instance = null;

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // void OnRuntimeMethodLoad() {
    //    // Add the delegate to be called when the scene is loaded, between Awake and Start.
    //    SceneManager.sceneLoaded += SceneLoaded;
    //}

    private void Start() {
        if (instance != null && instance != this) {
            print("Destroying duplicate MusicOnOff");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        musicPlayer = FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
        if(musicPlayer && !MusicOn) {
            musicPlayer.volume = 0;
        }
    }

    public void MusicOnOffToggle() {
        MusicOn = MusicOn ? false : true;
    }

    
}
