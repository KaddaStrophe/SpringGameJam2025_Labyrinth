using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.Eventsystem {
    public class HotkeyEventManager : MonoBehaviour {
        public delegate void SkipToLevelHandler(LevelSetup level);
        public static event SkipToLevelHandler onSkipToLevel;

        public static void InvokeSkipToLevel(LevelSetup level) {
            onSkipToLevel?.Invoke(level);
        }


        public delegate void TogglePermadeathHandler();
        public static event TogglePermadeathHandler onTogglePermadeath;

        public static void InvokeTogglePermadeath() {
            onTogglePermadeath?.Invoke();
        }
    }
}