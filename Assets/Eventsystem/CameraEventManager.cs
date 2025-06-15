using Labyrinth.Level;
using UnityEngine;

namespace Labyrinth.Eventsystem {
    public class CameraEventManager : MonoBehaviour {
        public delegate void ImpactHandler();
        public static event ImpactHandler onImpact;

        public static void InvokeImpact() {
            onImpact?.Invoke();
        }
    }
}