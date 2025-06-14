using System.Linq;
using Labyrinth.Character;
using Labyrinth.Eventsystem;
using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.GameSystem {
    public class GameStateManager : MonoBehaviour {
        [SerializeField]
        GameObject levelContainer;

        [SerializeField]
        CharacterMover player;

        [SerializeField]
        UIManager uiManager;

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

            if (!player) {
                player = FindObjectsByType<CharacterMover>(FindObjectsSortMode.None).FirstOrDefault();
            }

            levels = new LevelSetup[levelContainer.transform.childCount];
            int i = 0;
            foreach (Transform child in levelContainer.transform) {
                if (child.gameObject.activeSelf && child.TryGetComponent<LevelSetup>(out var level)) {
                    levels[i] = level;
                    i++;
                }
            }
        }

        protected void OnDisable() {
            GameStateEventManager.onStartNextLevel -= LoadNextLevel;
        }

        protected void Start() {
            foreach (Transform child in levelContainer.transform) {
                if (child.TryGetComponent<LevelSetup>(out var level)) {
                    level.HideLevel();
                }
            }
            levels[currentLevel].StartLevel();
        }

        void LoadNextLevel() {
            levels[currentLevel].HideLevel();
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
            GameStateEventManager.InvokeGameWon();
        }
    }
}