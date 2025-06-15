using System;
using Labyrinth.Eventsystem;
using Labyrinth.Level;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip bounceClip;
    [SerializeField]
    AudioClip hitClip;
    [SerializeField]
    AudioClip winClip;

    protected void OnEnable() {
        CameraEventManager.onImpact += PlayImpactClip;
        GameStateEventManager.onGameWon += PlayWinFanfare;
        GameStateEventManager.onHitDanger += PlayHitClip;
    }

    void PlayHitClip() {
        audioSource.PlayOneShot(hitClip);
    }

    void PlayImpactClip() {
        audioSource.PlayOneShot(bounceClip);
    }
    void PlayWinFanfare(LevelSetup level) {
        audioSource.PlayOneShot(winClip);
    }
}
