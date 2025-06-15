using System.Collections.Generic;
using System.Linq;
using Labyrinth.Character;
using Labyrinth.Eventsystem;
using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.GameSystem {
    public class GameStateManager : MonoBehaviour {
        [SerializeField]
        public bool isPermaDeath;

        [SerializeField]
        GameObject levelContainer;

        [SerializeField]
        CharacterMover player;

        [SerializeField]
        ParticleSystem winParticles;

        [Header("Debug")]
        [SerializeField]
        LevelSetup[] levels;

        [SerializeField]
        int currentLevel = 0;

        protected void OnValidate() {
            if (!player) {
                player = FindObjectsByType<CharacterMover>(FindObjectsSortMode.None).FirstOrDefault();
            }
        }
        protected void OnEnable() {
            GameStateEventManager.onStartNextLevel += LoadNextLevel;
            HotkeyEventManager.onSkipToLevel += SkipToLevel;
            HotkeyEventManager.onTogglePermadeath += TogglePermadeath;

            if (!player) {
                player = FindObjectsByType<CharacterMover>(FindObjectsSortMode.None).FirstOrDefault();
            }

            int i = 0;
            var levelsList = new List<LevelSetup>();
            foreach (Transform child in levelContainer.transform) {
                if (child.gameObject.activeSelf && child.TryGetComponent<LevelSetup>(out var level)) {
                    levelsList.Add(level);
                    i++;
                }
            }
            levels = new LevelSetup[i];
            int j = 0;
            foreach (var level in levelsList) {
                levels[j] = level;
                j++;
            }
        }

        protected void OnDisable() {
            GameStateEventManager.onStartNextLevel -= LoadNextLevel;
            HotkeyEventManager.onSkipToLevel -= SkipToLevel;
            HotkeyEventManager.onTogglePermadeath -= TogglePermadeath;
        }

        protected void Start() {
            foreach (var level in levels) {
                level.HideLevel();
            }
            var firstLevel = levels[currentLevel];
            player.ResetPlayerPosition(firstLevel.playerStartPos, firstLevel.playerStartRotation);
            firstLevel.StartLevel();
        }

        void LoadNextLevel() {
            if (currentLevel >= 0) {
                levels[currentLevel].HideLevel();
            }
            if (currentLevel < levels.Length - 1) {
                currentLevel++;
                var level = levels[currentLevel];
                player.ResetPlayerPosition(level.playerStartPos, level.playerStartRotation);
                level.StartLevel();
            } else {
                WinGame();
            }
        }

        void WinGame() {
            GameStateEventManager.InvokeGameWon(levels[currentLevel]);
            winParticles.Play();
        }

        void TogglePermadeath() {
            isPermaDeath = !isPermaDeath;
        }

        void SkipToLevel(LevelSetup level) {
            levels[currentLevel].HideLevel();
            currentLevel = levels.ToList().FindIndex(l => l == level) - 1;
            LoadNextLevel();
        }

        public void StartGameOver() {
            GameStateEventManager.InvokeGameOver(levels[currentLevel]);
        }
    }
}