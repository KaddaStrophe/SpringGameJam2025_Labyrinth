using Labyrinth.Character;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Labyrinth.GameSystems.UI {
    public class MenuButton : MonoBehaviour {
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip buttonPressClip;
        [SerializeField]
        Transform startButton;
        [SerializeField]
        Transform quitButton;
        [SerializeField, UnityEngine.Range(0f, 10f)]
        float ringProgressSpeed = 1f;

        [SerializeField]
        UnityEvent OnHoldPerformed;
        [SerializeField]
        UnityEvent OnClickPerformed;

        Image quitButtonImage;
        InputActions inputActions;
        bool isHolding;

        protected void OnEnable() {
            quitButton.TryGetComponent(out quitButtonImage);
            Assert.IsTrue(quitButtonImage);
            quitButtonImage.fillAmount = 0;

            inputActions = new InputActions();
            inputActions.UI.Press.canceled += ProcessPressCancel;
            inputActions.UI.Hold.performed += PerformHolding;

            inputActions.Enable();
        }

        protected void OnDisable() {
            inputActions.Disable();
            inputActions.UI.Press.canceled -= ProcessPressCancel;
            inputActions.UI.Hold.performed -= PerformHolding;
        }

        protected void FixedUpdate() {
            ProgressRing();
        }

        void PerformHolding(InputAction.CallbackContext context) {
            isHolding = true;
        }

        void ProcessPressCancel(InputAction.CallbackContext context) {
            if (!isHolding) {
                OnClickPerformed?.Invoke();
                audioSource.PlayOneShot(buttonPressClip);
            } else {
                isHolding = false;
            }
        }

        void ProgressRing() {
            if (isHolding) {
                quitButtonImage.fillAmount += ringProgressSpeed * Time.deltaTime;
                quitButtonImage.fillAmount = Mathf.Min(quitButtonImage.fillAmount, 1);

            } else { // If progress is canceled -> slowly drain progress
                quitButtonImage.fillAmount -= ringProgressSpeed * Time.deltaTime;
                quitButtonImage.fillAmount = Mathf.Max(quitButtonImage.fillAmount, 0);
            }

            // If progress ring is full -> perform holding complete event
            if (quitButtonImage.fillAmount == 1) {
                OnHoldPerformed?.Invoke();
                audioSource.PlayOneShot(buttonPressClip);
            }
        }
    }
}