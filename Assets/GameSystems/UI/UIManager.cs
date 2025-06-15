using System.Collections;
using Labyrinth.Eventsystem;
using Labyrinth.GameSystems.UI;
using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.GameSystem {
    public class UIManager : MonoBehaviour {

        [SerializeField]
        Canvas levelFinishedCanvas;
        [SerializeField]
        Canvas winCanvas;
        [SerializeField]
        Canvas gameOverCanvas;
        [SerializeField, UnityEngine.Range(0, 10)]
        float levelFinishScreenDuration = 5;
        [SerializeField, UnityEngine.Range(0, 10)]
        float waitForMenuButtonDuration = 3;

        [SerializeField]
        MenuButton winMenuButton;
        [SerializeField]
        MenuButton gameOverMenuButton;

        bool alreadyFinishedLevel;
        bool alreadyFinishedGame;
        bool alreadyGameOver;

        protected void OnEnable() {
            GameStateEventManager.onGoalReached += LevelFinish;
            GameStateEventManager.onGameWon += ShowGameWin;
            GameStateEventManager.onGameOver += GameOver;
        }

        protected void OnDisable() {
            GameStateEventManager.onGoalReached -= LevelFinish;
            GameStateEventManager.onGameWon -= ShowGameWin;
            GameStateEventManager.onGameOver -= GameOver;
        }

        protected void Start() {
            levelFinishedCanvas.gameObject.SetActive(false);
            winCanvas.gameObject.SetActive(false);
            winMenuButton.gameObject.SetActive(false);
            gameOverMenuButton.gameObject.SetActive(false);
        }

        void LevelFinish() {
            // Show level finish UI
            if (!alreadyFinishedLevel && !alreadyGameOver) {
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

        void ShowGameWin(LevelSetup level) {
            if (!alreadyFinishedGame) {
                alreadyFinishedGame = true;
                StartCoroutine(ShowGameScreen(waitForMenuButtonDuration));
            }
        }

        void GameOver(LevelSetup level) {
            alreadyGameOver = true;
            StartCoroutine(ShowGameOverScreen(level, waitForMenuButtonDuration));
        }

        IEnumerator ShowGameScreen(float gameWinScreenDuration) {
            winCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(gameWinScreenDuration);

            // Show button for restart or main menu
            winMenuButton.gameObject.SetActive(true);
        }
        IEnumerator ShowGameOverScreen(LevelSetup level, float gameWinScreenDuration) {
            yield return new WaitForSeconds(1);
            level.HideLevel();
            gameOverCanvas.gameObject.SetActive(true);
            GameStateEventManager.InvokeShowGameOverScreen();
            yield return new WaitForSeconds(gameWinScreenDuration);

            // Show button for restart or main menu
            gameOverMenuButton.gameObject.SetActive(true);
        }
    }
}