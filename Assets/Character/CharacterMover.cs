using Labyrinth.Eventsystem;
using NUnit.Framework;
using UnityEngine;

namespace Labyrinth.Character {
    public class CharacterMover : MonoBehaviour {
        [SerializeField]
        InputManager inputManager;
        [SerializeField]
        Rigidbody2D attachedRigidbody;
        [SerializeField]
        string goalLayer = "Goal";
        [SerializeField]
        string dangerLayer = "Danger";


        [SerializeField, UnityEngine.Range(0, 10)]
        int fullSpeed = 1;
        [SerializeField, UnityEngine.Range(0, 5)]
        float acceleration = 0;
        [SerializeField, UnityEngine.Range(0, 5)]
        float deceleration = 0;

        [SerializeField, UnityEngine.Range(0, 50)]
        int fullRotateSpeed = 1;
        [SerializeField, UnityEngine.Range(0, 5)]
        float rotateAcceleration = 0;
        [SerializeField, UnityEngine.Range(0, 5)]
        float rotateDeceleration = 0;


        [SerializeField]
        float currentSpeed = 0;
        [SerializeField]
        float currentRotateSpeed = 0;


        Vector2 lastStartPos;
        Vector2 lastStartRotation;

        protected void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }
        protected void Start() {
            if (!inputManager) {
                TryGetComponent(out inputManager);
            }
            Assert.True(inputManager);
        }

        protected void FixedUpdate() {
            TryStopping();
            TryTurning();
            TryMoving();
        }

        protected void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.CompareTag(goalLayer)) { // Collision with goal
                GameStateEventManager.InvokeGoalReached();
            } else if (collision.gameObject.CompareTag(dangerLayer)) { // Collision with danger
                Debug.Log("Danger!");
                ResetPlayerPosition(lastStartPos, lastStartRotation);
            } else {    // Collision with walls
                float distance = attachedRigidbody.Distance(collision).distance;
                // Remove overlap (stop at wall)
                transform.position += new Vector3(0, distance, 0);
                // Bounce back
                currentSpeed *= -1;
                Move();
            }
        }
        void TryStopping() {
            if (inputManager.isStopping && !inputManager.isTurning) {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f); // limit to zero
                Move();
            }
        }
        void TryTurning() {
            if (inputManager.isTurning) {
                currentRotateSpeed += rotateAcceleration * Time.deltaTime;
                currentRotateSpeed = Mathf.Min(currentRotateSpeed, fullRotateSpeed); // limit to full speed
                transform.Rotate(transform.forward, currentRotateSpeed * 100 * Time.deltaTime, Space.Self);
            }
        }

        void TryMoving() {
            if (!inputManager.isTurning && !inputManager.isStopping) {
                // Decelerate rotation
                currentRotateSpeed -= rotateDeceleration * Time.deltaTime;
                currentRotateSpeed = Mathf.Max(currentRotateSpeed, 0); // limit to full speed
                transform.Rotate(transform.forward, currentRotateSpeed * 100 * Time.deltaTime, Space.Self);

                // Increase movement speed
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, fullSpeed); // limit to full speed
                Move();
            }
        }

        void Move() {
            transform.Translate(currentSpeed * Time.deltaTime * transform.up, Space.World);
        }

        public void ResetPlayerPosition(Vector2 startPos, Vector2 lookAt) {
            lastStartPos = startPos;
            lastStartRotation = lookAt;
            currentRotateSpeed = 0;
            currentSpeed = 0;
            var rotation = Quaternion.LookRotation(Vector3.forward,lookAt);
            transform.SetPositionAndRotation(startPos, rotation);
        }
    }
}