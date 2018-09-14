﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [System.Serializable]
    public enum PickupType {
        Glue, Laser, Enlarge, Life, Multiple, Fireball
    }

    [SerializeField] float fallSpeed = 0.1f;
    [SerializeField] PickupType pickupType;
    [SerializeField] AudioClip pickupSound;

    PickupManager pickupTable;

    // Use this for initialization
    void Start() {
        pickupTable = FindObjectOfType<PickupManager>();
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Paddle") {
            MusicPlayer SFXPlayer = FindObjectOfType<MusicPlayer>();
            if(SFXPlayer) SFXPlayer.PlayClip(pickupSound);
            pickupTable.ApplyEffect(pickupType);
            DestroyThis();
        }
        if (collision.gameObject.tag == "LoseCollider") DestroyThis();
    }

    private void DestroyThis() {
        //print("Destroying this pickup");
        Destroy(gameObject);
    }
}
