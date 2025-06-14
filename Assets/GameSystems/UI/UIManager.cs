using System.Collections;
using Labyrinth.Eventsystem;
using Labyrinth.GameSystems.UI;
using UnityEngine;

namespace Labyrinth.GameSystem {
    public class UIManager : MonoBehaviour {

        [SerializeField]
        Canvas levelFinishedCanvas;
        [SerializeField]
        Canvas winCanvas;
        [SerializeField, UnityEngine.Range(0, 10)]
        float levelFinishScreenDuration = 5;
        [SerializeField, UnityEngine.Range(0, 10)]
        float gameWinScreenDuration = 5;

        [SerializeField]
        MenuButton menuButton;

        bool alreadyFinishedLevel;
        bool alreadyFinishedGame;

        protected void OnEnable() {
            GameStateEventManager.onGoalReached += LevelFinish;
            GameStateEventManager.onGameWon += ShowGameWin;
        }

        protected void OnDisable() {
            GameStateEventManager.onGoalReached -= LevelFinish;
            GameStateEventManager.onGameWon -= ShowGameWin;
        }

        protected void Start() {
            levelFinishedCanvas.gameObject.SetActive(false);
            winCanvas.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(false);
        }

        void LevelFinish() {
            // Show level finish UI
            if (!alreadyFinishedLevel) {
                alreadyFinishedLevel = true;
                StartCoroutine(ShowLevelFinishScreenForSeconds(levelFinishScreenDuration));
            }
        }

        IEnumerator ShowLevelFinishScreenForSeconds(float levelFinishScreenDuration) {
            levelFinishedCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(levelFinishScreenDuration);
            levelFinishedCanvas.gameObject.SetActive(false);

            // Next level can be started!
            GameStateEventManager.InvokeStartNextLevel();
            alreadyFinishedLevel = false;
        }

        void ShowGameWin() {
            if (!alreadyFinishedGame) {
                alreadyFinishedGame = true;
                StartCoroutine(ShowGameWinScreen(gameWinScreenDuration));
            }
        }

        IEnumerator ShowGameWinScreen(float gameWinScreenDuration) {
            winCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(gameWinScreenDuration);

            // Show button for restart or main menu
            menuButton.gameObject.SetActive(true);
        }
    }
}