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

        public delegate void GameWonHandler();
        public static event GameWonHandler onGameWon;

        public static void InvokeGameWon() {
            onGameWon?.Invoke();
        }
    }
}