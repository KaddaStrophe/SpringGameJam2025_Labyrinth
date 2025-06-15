using Labyrinth.Eventsystem;
using Labyrinth.GameSystem;
using NUnit.Framework;
using UnityEngine;

namespace Labyrinth.Character {
    public class CharacterMover : MonoBehaviour {
        [SerializeField]
        InputManager inputManager;

        [SerializeField]
        GameStateManager gameStateManager;
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

        bool canMove = true;

        protected void OnValidate() {
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
            }
        }
        protected void OnEnable() {
            transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
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
                // TODO: Screenshake
                if (gameStateManager.isPermaDeath) {
                    canMove = false;
                    currentSpeed = 0;
                    currentRotateSpeed = 0;
                    gameStateManager.StartGameOver();
                    transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                } else {
                    ResetPlayerPosition(lastStartPos, lastStartRotation);
                }
            } else {    // Collision with walls
                ProcessCollisionWithWalls(collision);
                
            }
        }

        protected void OnTriggerStay2D(Collider2D collision) {
            if (!collision.gameObject.CompareTag(goalLayer) && !collision.gameObject.CompareTag(dangerLayer)) {
                ProcessCollisionWithWalls(collision);
            }
        }

        void ProcessCollisionWithWalls(Collider2D collision) {
            var distance = attachedRigidbody.Distance(collision);
            // Remove overlap (stop at wall)
            transform.position += new Vector3(distance.normal.x, distance.normal.y, 0) * distance.distance;
            // Bounce back
            currentSpeed *= -1;
            Move();
        }

        void TryStopping() {
            if (!canMove) {
                return;
            }
            if (inputManager.isStopping && !inputManager.isTurning) {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f); // limit to zero
                Move();
            }
        }
        void TryTurning() {
            if (!canMove) {
                return;
            }
            if (inputManager.isTurning) {
                currentRotateSpeed += rotateAcceleration * Time.deltaTime;
                currentRotateSpeed = Mathf.Min(currentRotateSpeed, fullRotateSpeed); // limit to full speed
                transform.Rotate(transform.forward, currentRotateSpeed * 100 * Time.deltaTime, Space.Self);

                // Decelerate forward movement
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f); // limit to zero
                Move();
            }
        }

        void TryMoving() {
            if (!canMove) {
                return;
            }
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
            if (!canMove) {
                return;
            }
            transform.Translate(currentSpeed * Time.deltaTime * transform.up, Space.World);
        }

        public void ResetPlayerPosition(Vector2 startPos, Vector2 lookAt) {
            lastStartPos = startPos;
            lastStartRotation = lookAt;
            currentRotateSpeed = 0;
            currentSpeed = 0;
            var rotation = Quaternion.LookRotation(Vector3.forward, lookAt);
            transform.SetPositionAndRotation(startPos, rotation);
        }
    }
}