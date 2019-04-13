using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    public static SFX instance;

    private AudioSource sfx;

    public AudioClip dice;
    public AudioClip piece;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        sfx = GetComponent<AudioSource>();
    }

    public void PlayDiceSound() {
        Play(dice);
    }

    public void PlayPieceSound() {
        Play(piece);
    }

    public void Mute(bool b) {
        sfx.mute = b;
    }

    private void Play(AudioClip clip) {
        sfx.pitch = Random.Range(.9f, 1.1f);
        sfx.PlayOneShot(clip);
    }

}
