using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.Eventsystem {
    public class GameStateEventManager : MonoBehaviour {
        public delegate void GoalReachedHandler();
        public static event GoalReachedHandler onGoalReached;

        public static void InvokeGoalReached() {
            onGoalReached?.Invoke();
        }

        public delegate void StartNewLevelHandler();
        public static event StartNewLevelHandler onStartNextLevel;

        public static void InvokeStartNextLevel() {
            onStartNextLevel?.Invoke();
        }

        public delegate void GameWonHandler(LevelSetup level);
        public static event GameWonHandler onGameWon;

        public static void InvokeGameWon(LevelSetup level) {
            onGameWon?.Invoke(level);
        }

        public delegate void GameOverHandler(LevelSetup level);
        public static event GameOverHandler onGameOver;

        public static void InvokeGameOver(LevelSetup level) {
            onGameOver?.Invoke(level);
        }

        public delegate void HitDangerHandler();
        public static event HitDangerHandler onHitDanger;
        public static void InvokeHitDanger() {
            onHitDanger?.Invoke();
        }
    }
}