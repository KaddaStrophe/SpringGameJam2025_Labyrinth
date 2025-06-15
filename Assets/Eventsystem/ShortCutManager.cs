using Labyrinth.Character;
using Labyrinth.Level;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Labyrinth.Eventsystem {
    public class ShortCutManager : MonoBehaviour {
        [SerializeField]
        bool activateHotkeys = true;
        [SerializeField]
        LevelSetup f1Skip;
        [SerializeField]
        LevelSetup f2Skip;
        [SerializeField]
        LevelSetup f3Skip;

        InputActions inputActions;
        protected void OnEnable() {
            inputActions = new InputActions();
            inputActions.Hotkey.Skip_1.canceled += SkipToFirstLevel;
            inputActions.Hotkey.Skip_2.canceled += SkipToSecondLevel;
            inputActions.Hotkey.Skip_3.canceled += SkipToThirdLevel;
            inputActions.Hotkey.Permadeath.canceled += TogglePermadeath;
            inputActions.Hotkey.MainMenu.canceled += LoadMainMenu;
            if (activateHotkeys) {
                inputActions.Enable();
            }
        }

        protected void OnDisable() {
            inputActions.Disable();
            inputActions.Hotkey.Skip_1.canceled -= SkipToFirstLevel;
            inputActions.Hotkey.Skip_2.canceled -= SkipToSecondLevel;
            inputActions.Hotkey.Skip_3.canceled -= SkipToThirdLevel;
            inputActions.Hotkey.Permadeath.canceled -= TogglePermadeath;
            inputActions.Hotkey.MainMenu.canceled -= LoadMainMenu;
        }

        void SkipToFirstLevel(InputAction.CallbackContext context) {
            HotkeyEventManager.InvokeSkipToLevel(f1Skip);
        }
        void SkipToSecondLevel(InputAction.CallbackContext context) {
            HotkeyEventManager.InvokeSkipToLevel(f2Skip);
        }
        void SkipToThirdLevel(InputAction.CallbackContext context) {
            HotkeyEventManager.InvokeSkipToLevel(f3Skip);
        }
        void TogglePermadeath(InputAction.CallbackContext context) {
            HotkeyEventManager.InvokeTogglePermadeath();
        }

        void LoadMainMenu(InputAction.CallbackContext context) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}