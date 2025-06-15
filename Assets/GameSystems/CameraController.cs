using System;
using System.Collections;
using Labyrinth.Eventsystem;
using Labyrinth.Level;
using Unity.Cinemachine;
using UnityEngine;

namespace Labyrinth.GameSystem {
    public class CameraController : MonoBehaviour {

        [SerializeField]
        CinemachineCamera cineCamera;
        [SerializeField, Range(0, 2)]
        float impactRumbleDuration = 1f;

        [SerializeField, Range(0, 2)]
        float gameOverRumbleDuration = 1f;

        CinemachineBasicMultiChannelPerlin perlinNoise;

        protected void OnValidate() {
            if (!cineCamera) {
                cineCamera = GetComponentInChildren<CinemachineCamera>();
            }
        }

        protected void OnEnable() {
            if (cineCamera) {
                perlinNoise = cineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            }
            perlinNoise.enabled = false;
            CameraEventManager.onImpact += ProcessImpact;
            GameStateEventManager.onGameOver += ProcessGameOver;
        }

        protected void OnDisable() {
            CameraEventManager.onImpact -= ProcessImpact;
            GameStateEventManager.onGameOver -= ProcessGameOver;
        }

        void ProcessImpact() {
            StartCoroutine(Rumble(impactRumbleDuration));
        }
        void ProcessGameOver(LevelSetup level) {
            StartCoroutine(Rumble(gameOverRumbleDuration));
        }

        IEnumerator Rumble(float duration) {
            perlinNoise.enabled = true;
            yield return new WaitForSecondsRealtime(duration);
            perlinNoise.enabled = false;
        }
    }
}