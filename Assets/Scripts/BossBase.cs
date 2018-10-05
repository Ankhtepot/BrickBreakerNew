using Assets.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour, IBoss, IPlayList {
    public float movingAnimationSpeed;
    public float attackSpeed;
    public float attackCDBase;
    public float attackCDVariation;
    [HideInInspector]
    public SoundSystem soundSystem;
    public AudioClip dyingSound;

    public int HealthPoints { get; set; }

    private void Start() {
        AssignSoundSystem();
    }

    public void AssignSoundSystem() {
        SoundSystem ss = FindObjectOfType<SoundSystem>();
        if (ss) this.soundSystem = ss;
    }

    public abstract void Dying();
    public abstract void OnArrival();
    public abstract void OnCollisionEnter2D();
    public abstract void OnDeath();
    public abstract void StartEncounter();

    public abstract SoundSystem.PlayListID GetPlayListID();
}
