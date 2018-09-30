using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class SoundSystem : MonoBehaviour {

    [SerializeField] float baseVolume = 0.1f;
    [SerializeField] bool muted = false;
    [SerializeField] AudioClip[] BrickSounds;
    [SerializeField] AudioClip[] AppleSounds;

    AudioSource audioSource;
   // static SoundSystem instance = null;

    public enum PlayListID { Brick, Apple }

    // Use this for initialization
    void Start() {
        //if (instance != null && instance != this) {
        //    print("Destroying duplicate MusicPlayer");
        //    Destroy(gameObject);
        //} else {
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //}
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = baseVolume;
        //print("MusicPlayer is now active");
    }

    public float GetVolume() {
        return baseVolume;
    }

    public void SetVolume(float newVolume) {
        audioSource.volume = newVolume;
    }

    private void onSceneLoaded(Scene loadedScene, LoadSceneMode mode) {
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    public void PlayClip(AudioClip clip) {
            if (audioSource) audioSource.PlayOneShot(clip); 
    }

    private void unMute() {
        if (audioSource) audioSource.volume = baseVolume;
    }

    public void musicOnOff() {
        //print("Toggling music on/off");
        if (muted) {
            audioSource.volume = baseVolume;
            muted = false;
        } else {
            audioSource.volume = 0f;
            muted = true;
        }
    }
    public void PlayRandomSoundFromList(PlayListID playList) {
        if (playList == PlayListID.Apple) PlayClip(AppleSounds[Random.Range(0, AppleSounds.Length)]);
        if (playList == PlayListID.Brick) PlayClip(BrickSounds[Random.Range(0, BrickSounds.Length)]);
    }
}
