using NUnit.Framework;
using UnityEngine;

namespace Labyrinth.Character {
    public class CharacterController : MonoBehaviour {
        [SerializeField]
        InputManager inputManager;
        [SerializeField]
        Rigidbody2D attachedRigidbody;


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

        protected void OnTriggerEnter2D(Collider2D collision) {
            float distance = attachedRigidbody.Distance(collision).distance;
            // Remove overlap (stop at wall)
            transform.position += new Vector3(0, distance, 0);
            // Bounce back
            currentSpeed *= -1;
            Move();
        }
    }
}